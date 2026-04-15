using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;
using System;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class KupciController : Controller
    {
        public IActionResult Index()
        {
            var kupci = AppData.Kupci.OrderBy(k => k.Naziv).ToList();
            return View(kupci);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var kupac = AppData.Kupci.FirstOrDefault(k => k.Id == id);
            if (kupac == null)
            {
                TempData["ErrorMessage"] = "Kupac nije pronađen.";
                return RedirectToAction("Index");
            }

            return View(kupac);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Kupac
            {
                DatumUnosa = DateTime.Today,
                Tip = TipKupca.PravnaOsoba
            });
        }

        [HttpPost]
        public IActionResult Create(Kupac kupac)
        {
            if (string.IsNullOrWhiteSpace(kupac.Naziv))
            {
                ModelState.AddModelError(nameof(kupac.Naziv), "Naziv je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kupac.OIB))
            {
                ModelState.AddModelError(nameof(kupac.OIB), "OIB je obavezan.");
            }

            kupac.DatumUnosa = kupac.DatumUnosa == default ? DateTime.Today : kupac.DatumUnosa;

            if (!ModelState.IsValid)
            {
                return View(kupac);
            }

            kupac.Id = AppData.Kupci.Any() ? AppData.Kupci.Max(k => k.Id) + 1 : 1;
            AppData.Kupci.Add(kupac);
            TempData["SuccessMessage"] = "Kupac dodan.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var kupac = AppData.Kupci.FirstOrDefault(k => k.Id == id);
            if (kupac == null)
            {
                TempData["ErrorMessage"] = "Kupac nije pronađen.";
                return RedirectToAction("Index");
            }

            foreach (var projekt in AppData.Projekti.Where(p => p.KupacId == id))
            {
                projekt.Kupac = null;
                projekt.KupacId = null;
            }

            kupac.Projekti.Clear();
            AppData.Kupci.Remove(kupac);
            TempData["SuccessMessage"] = "Kupac obrisan.";
            return RedirectToAction("Index");
        }
    }
}
