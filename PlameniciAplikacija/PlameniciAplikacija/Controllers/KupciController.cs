using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlameniciAplikacija.Extensions;
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

        public IActionResult Index(string? search)
        {
            var kupci = _context.Kupci
                .Include(k => k.Projekti)
                .OrderBy(k => k.Naziv)
                .ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                kupci = kupci.Where(k =>
                    (k.Naziv != null && k.Naziv.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (k.Grad != null && k.Grad.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (k.KontaktOsoba != null && k.KontaktOsoba.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (k.Email != null && k.Email.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (k.Telefon != null && k.Telefon.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (k.OIB != null && k.OIB.Contains(term, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            ViewBag.Search = search;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexContent", kupci);
            }

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

        [HttpGet]
        public IActionResult Edit(int id)
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

        [HttpPost]
        public IActionResult Edit(int id, Kupac kupac)
        {
            var existingKupac = _context.Kupci.FirstOrDefault(k => k.Id == id);
            if (existingKupac == null)
            {
                TempData["ErrorMessage"] = "Kupac nije pronađen.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(kupac.Naziv))
            {
                ModelState.AddModelError(nameof(kupac.Naziv), "Naziv je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(kupac.OIB))
            {
                ModelState.AddModelError(nameof(kupac.OIB), "OIB je obavezan.");
            }

            kupac.DatumUnosa = kupac.DatumUnosa == default ? existingKupac.DatumUnosa : kupac.DatumUnosa;

            if (!ModelState.IsValid)
            {
                kupac.Projekti = existingKupac.Projekti;
                return View(kupac);
            }

            existingKupac.Naziv = kupac.Naziv;
            existingKupac.OIB = kupac.OIB;
            existingKupac.Adresa = kupac.Adresa;
            existingKupac.Grad = kupac.Grad;
            existingKupac.KontaktOsoba = kupac.KontaktOsoba;
            existingKupac.Email = kupac.Email;
            existingKupac.Telefon = kupac.Telefon;
            existingKupac.Tip = kupac.Tip;
            existingKupac.DatumUnosa = kupac.DatumUnosa;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kupac ažuriran.";
            return RedirectToAction("Index");
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
