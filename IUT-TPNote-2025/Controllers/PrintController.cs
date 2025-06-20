using IUT_TPNote_2025.Models;
using IUT_TPNote_2025.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUT_TPNote_2025.Controllers
{
    public class PrintController : Controller
    {

        public PartialViewResult GetListeImpression()
        {
            var model = PrintJobManager.GetAllTaches();
            return PartialView("_ListeImpressionPartial", model);
        }


        // GET: PrintController
        public ActionResult Index()
        {
            var model = PrintJobManager.GetAllTaches();

            if (model == null || model.Count == 0)
                return RedirectToAction("PasImpression");

            return View(model);
        }

        // GET: PrintController
        public ActionResult PasImpression()
        {
            return View();
        }

        // GET: PrintController/Details/5
        public ActionResult Details(string id)
        {
            var model = PrintJobManager.GetTacheById(id);
            return View(model);
        }

        // GET: PrintController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrintController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Impression collection)
        {
            try
            {
                PrintJobManager.SoumettreImpression(collection.Demandeur, collection.NomDocument, collection.NbPages);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrintController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrintController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrintController/Delete/5
        public ActionResult Delete(string id)
        {
            var model = PrintJobManager.GetTacheById(id);
            return View(model);
        }

        // POST: PrintController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Impression collection)
        {
            try
            {
                PrintJobManager.AnnulerImpression(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
