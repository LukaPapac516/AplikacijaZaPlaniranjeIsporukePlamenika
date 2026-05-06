using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class DjelatniciController : Controller
    {
        private readonly AppDbContext _context;

        public DjelatniciController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var djelatnici = _context.Djelatnici
                .Include(d => d.Projekti)
                .OrderBy(d => d.Prezime)
                .ThenBy(d => d.Ime)
                .ToList();

            return View(djelatnici);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var djelatnik = _context.Djelatnici
                .Include(d => d.Projekti)
                .FirstOrDefault(d => d.Id == id);
            if (djelatnik == null)
            {
                TempData["ErrorMessage"] = "Djelatnik nije pronađen.";
                return RedirectToAction("Index");
            }

            return View(djelatnik);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateProjects();
            return View(new Djelatnik
            {
                DatumZaposlenja = DateTime.Today,
                Tip = TipDjelatnika.Inzenjer
            });
        }

        [HttpPost]
        public IActionResult Create(Djelatnik djelatnik, List<int>? projektiIds)
        {
            ValidateEmployee(djelatnik);
            djelatnik.DatumZaposlenja = djelatnik.DatumZaposlenja == default ? DateTime.Today : djelatnik.DatumZaposlenja;

            if (!ModelState.IsValid)
            {
                PopulateProjects(projektiIds ?? Enumerable.Empty<int>());
                return View(djelatnik);
            }

            var selectedIds = (projektiIds ?? Enumerable.Empty<int>()).ToHashSet();
            var selectedProjects = _context.Projekti
                .Where(p => selectedIds.Contains(p.Id))
                .ToList();
            foreach (var projekt in selectedProjects)
            {
                djelatnik.Projekti.Add(projekt);
            }

            _context.Djelatnici.Add(djelatnik);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Djelatnik dodan.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var djelatnik = _context.Djelatnici
                .Include(d => d.Projekti)
                .FirstOrDefault(d => d.Id == id);
            if (djelatnik == null)
            {
                TempData["ErrorMessage"] = "Djelatnik nije pronađen.";
                return RedirectToAction("Index");
            }

            PopulateProjects(djelatnik.Projekti.Select(p => p.Id));
            return View(djelatnik);
        }

        [HttpPost]
        public IActionResult Edit(int id, Djelatnik updated, List<int>? projektiIds)
        {
            var djelatnik = _context.Djelatnici
                .Include(d => d.Projekti)
                .FirstOrDefault(d => d.Id == id);
            if (djelatnik == null)
            {
                TempData["ErrorMessage"] = "Djelatnik nije pronađen.";
                return RedirectToAction("Index");
            }

            ValidateEmployee(updated);
            if (updated.DatumZaposlenja == default)
            {
                updated.DatumZaposlenja = djelatnik.DatumZaposlenja;
            }

            if (!ModelState.IsValid)
            {
                updated.Id = id;
                PopulateProjects(projektiIds ?? Enumerable.Empty<int>());
                return View(updated);
            }

            djelatnik.Ime = updated.Ime;
            djelatnik.Prezime = updated.Prezime;
            djelatnik.RadnoMjesto = updated.RadnoMjesto;
            djelatnik.Email = updated.Email;
            djelatnik.Tip = updated.Tip;
            djelatnik.DatumZaposlenja = updated.DatumZaposlenja;

            var selectedIds = (projektiIds ?? Enumerable.Empty<int>()).ToHashSet();
            var selectedProjects = _context.Projekti
                .Where(p => selectedIds.Contains(p.Id))
                .ToList();

            djelatnik.Projekti.Clear();
            foreach (var projekt in selectedProjects)
            {
                djelatnik.Projekti.Add(projekt);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Djelatnik ažuriran.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var djelatnik = _context.Djelatnici
                .Include(d => d.Projekti)
                .FirstOrDefault(d => d.Id == id);
            if (djelatnik == null)
            {
                TempData["ErrorMessage"] = "Djelatnik nije pronađen.";
                return RedirectToAction("Index");
            }

            djelatnik.Projekti.Clear();
            _context.Djelatnici.Remove(djelatnik);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Djelatnik obrisan.";
            return RedirectToAction("Index");
        }

        private void PopulateProjects(IEnumerable<int>? selectedProjectIds = null)
        {
            var selected = selectedProjectIds?.ToHashSet() ?? new HashSet<int>();
            ViewBag.Projekti = _context.Projekti
                .OrderBy(p => p.BrojProjekta)
                .Select(p => new Project
                {
                    Id = p.Id,
                    Naziv = p.Naziv,
                    BrojProjekta = p.BrojProjekta
                })
                .ToList();
            ViewBag.SelektiraniProjekti = selected;
        }

        private void ValidateEmployee(Djelatnik djelatnik)
        {
            if (string.IsNullOrWhiteSpace(djelatnik.Ime))
            {
                ModelState.AddModelError(nameof(djelatnik.Ime), "Ime je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(djelatnik.Prezime))
            {
                ModelState.AddModelError(nameof(djelatnik.Prezime), "Prezime je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(djelatnik.Email))
            {
                ModelState.AddModelError(nameof(djelatnik.Email), "Email je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(djelatnik.RadnoMjesto))
            {
                ModelState.AddModelError(nameof(djelatnik.RadnoMjesto), "Radno mjesto je obavezno.");
            }
        }
    }
}
