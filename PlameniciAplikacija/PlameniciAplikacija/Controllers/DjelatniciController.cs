using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class DjelatniciController : Controller
    {
        public IActionResult Index()
        {
            var djelatnici = AppData.Djelatnici
                .OrderBy(d => d.Prezime)
                .ThenBy(d => d.Ime)
                .ToList();

            return View(djelatnici);
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

            djelatnik.Id = AppData.Djelatnici.Any() ? AppData.Djelatnici.Max(d => d.Id) + 1 : 1;
            AppData.Djelatnici.Add(djelatnik);
            AppData.AssignProjectsToEmployee(djelatnik, projektiIds ?? Enumerable.Empty<int>());
            TempData["SuccessMessage"] = "Djelatnik dodan.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var djelatnik = AppData.Djelatnici.FirstOrDefault(d => d.Id == id);
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
            var djelatnik = AppData.Djelatnici.FirstOrDefault(d => d.Id == id);
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

            AppData.AssignProjectsToEmployee(djelatnik, projektiIds ?? Enumerable.Empty<int>());
            TempData["SuccessMessage"] = "Djelatnik ažuriran.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var djelatnik = AppData.Djelatnici.FirstOrDefault(d => d.Id == id);
            if (djelatnik == null)
            {
                TempData["ErrorMessage"] = "Djelatnik nije pronađen.";
                return RedirectToAction("Index");
            }

            AppData.RemoveEmployeeFromProjects(djelatnik);
            AppData.Djelatnici.Remove(djelatnik);
            TempData["SuccessMessage"] = "Djelatnik obrisan.";
            return RedirectToAction("Index");
        }

        private void PopulateProjects(IEnumerable<int>? selectedProjectIds = null)
        {
            var selected = selectedProjectIds?.ToHashSet() ?? new HashSet<int>();
            ViewBag.Projekti = AppData.Projekti
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
