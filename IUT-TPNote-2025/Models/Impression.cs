namespace IUT_TPNote_2025.Models
{
    public class Impression
    {
        public string Id { get; set; }
        public Imprimante ImprimanteUtilisee { get; set; }
        public string Demandeur { get; set; }
        public string NomDocument { get; set; }
        public int NbPages { get; set; }
        public DateTime DateSoumission { get; set; }
        public DateTime? DateFinImpression { get; set; }
        public TypeImpression Statut { get; set; }


        public Impression()
        {
            this.Id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            this.Statut = TypeImpression.EnAttente;
            this.DateSoumission = DateTime.Now;
        }
    }
}
