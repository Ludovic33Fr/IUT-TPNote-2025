using IUT_TPNote_2025.Models;

namespace IUT_TPNote_2025.Services
{
    public static class PrintJobManager
    {
        private static Queue<Imprimante> _imprimantesDisponibles = new Queue<Imprimante>(
            new Imprimante[] { Imprimante.Lasercouleur, Imprimante.JetEncreA3, Imprimante.LaserNB }
            );

        // Dictionnaire pour suivre les tâches en cours d'impression ou terminées, avec leur ID comme clé
        private static Dictionary<string, Impression> _tachesImpression = new Dictionary<string, Impression>();

        private static readonly object _lock = new object();


        public static int GetNombreImprimantesDisponibles()
        {
            return _imprimantesDisponibles.Count;
        }
        
        public static int GetNombreTachesEnAttente()
        {
            return _tachesImpression.Values.Where<Impression>(p => p.Statut.Equals(TypeImpression.EnAttente)).Count();
        }
        public static int GetNombreTachesEnCours()
        {
            return _tachesImpression.Values.Where<Impression>(p => p.Statut.Equals(TypeImpression.EnCours)).Count();
        }
        public static int GetNombreTachesTerminees()
        {
            return _tachesImpression.Values.Where<Impression>(p => p.Statut.Equals(TypeImpression.Imprimee)).Count();
        }


        public static List<Impression> GetAllTaches()
        {
            lock (_lock)
            {
                return _tachesImpression.Values.ToList();
            }
        }

        public static Impression GetTacheById(string id)
        {
            lock (_lock)
            {
                if (!string.IsNullOrEmpty(id) && _tachesImpression.ContainsKey(id))
                {
                    return _tachesImpression[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public static string SoumettreImpression(string demandeur, string nomDocument, int nbPages)
        {
            lock( _lock)
            {
                Impression maTacheImpression = new Impression()
                {
                    Demandeur = demandeur,
                    NomDocument = nomDocument,
                    NbPages = nbPages
                };

                _tachesImpression.Add(maTacheImpression.Id, maTacheImpression);

                return maTacheImpression.Id;
            }

        }

        public static bool AnnulerImpression(string id)
        {
            
            lock(_lock)
            {
                if (!string.IsNullOrEmpty(id) && _tachesImpression.ContainsKey(id))
                {
                    Impression maTacheImpression = _tachesImpression[id];
                    maTacheImpression.DateFinImpression = DateTime.Now;
                    maTacheImpression.Statut = TypeImpression.Annulee;

                    _imprimantesDisponibles.Enqueue(maTacheImpression.ImprimanteUtilisee);
                    return true;
                }

                return false;
            }
        }

        private static async Task SimulerImpression(string idTache)
        {
            Impression tacheImpression;

            lock (_lock)
            {
                tacheImpression = GetTacheById(idTache);
            }

            if (tacheImpression != null)
            {
                int nbWait = 1000 * tacheImpression.NbPages;
                await Task.Delay(nbWait); // simulation impression

                lock (_lock)
                {
                    tacheImpression.DateFinImpression = DateTime.Now;
                    tacheImpression.Statut = TypeImpression.Imprimee;

                    // Libérer l'imprimante
                    _imprimantesDisponibles.Enqueue(tacheImpression.ImprimanteUtilisee);
                }
            }
        }


        public static async Task StartPooling(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(200);

                Impression tache = null;

                lock (_lock)
                {
                    if (_imprimantesDisponibles.Count != 0 &&
                        _tachesImpression.Values.Any(p => p.Statut == TypeImpression.EnAttente))
                    {
                        // Sélectionne la tâche à imprimer
                        tache = _tachesImpression.Values
                                 .Where(p => p.Statut == TypeImpression.EnAttente)
                                 .FirstOrDefault();

                        // Réserve une imprimante et change le statut
                        if (tache != null)
                        {
                            var imprimante = _imprimantesDisponibles.Dequeue();
                            tache.Statut = TypeImpression.EnCours;
                            tache.ImprimanteUtilisee = imprimante;
                        }
                    }
                }

                // Lance l'impression en dehors du lock si une tâche a été réservée
                if (tache != null)
                {
                    _ = SimulerImpression(tache.Id); // Pas besoin d'attendre
                }
            }
        }
    }
}
