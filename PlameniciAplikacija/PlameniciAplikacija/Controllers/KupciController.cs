using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;
using System;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class KupciController : Controller
    {
        private readonly AppDbContext _context;

        public KupciController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var kupci = _context.Kupci
                .Include(k => k.Projekti)
                .OrderBy(k => k.Naziv)
                .ToList();
            return View(kupci);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var kupac = _context.Kupci
                .Include(k => k.Projekti)
                .FirstOrDefault(k => k.Id == id);
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

            _context.Kupci.Add(kupac);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kupac dodan.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var kupac = _context.Kupci.FirstOrDefault(k => k.Id == id);
            if (kupac == null)
            {
                TempData["ErrorMessage"] = "Kupac nije pronađen.";
                return RedirectToAction("Index");
            }

            var projektiKupca = _context.Projekti.Where(p => p.KupacId == id).ToList();
            foreach (var projekt in projektiKupca)
            {
                projekt.Kupac = null;
                projekt.KupacId = null;
            }

            _context.Kupci.Remove(kupac);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kupac obrisan.";
            return RedirectToAction("Index");
        }
    }
}
