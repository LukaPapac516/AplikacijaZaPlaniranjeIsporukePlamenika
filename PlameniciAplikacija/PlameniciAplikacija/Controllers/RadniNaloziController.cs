using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;

namespace PlameniciAplikacija.Controllers
{
    public class RadniNaloziController : Controller
    {
        private readonly AppDbContext _context;

        public RadniNaloziController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string? search, StatusNaloga? status)
        {
            var query = _context.RadniNalozi
                .Include(r => r.Projekt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(r =>
                    r.OznakaNaloga.Contains(term) ||
                    r.Opis.Contains(term) ||
                    (r.Projekt != null && r.Projekt.Naziv.Contains(term)) ||
                    (r.Projekt != null && r.Projekt.BrojProjekta.Contains(term))
                );
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            var radniNalozi = query
                .OrderByDescending(r => r.DatumOtvaranja)
                .ToList();

            ViewBag.Search = search;
            ViewBag.SelectedStatus = status;
            ViewBag.Total = _context.RadniNalozi.Count();
            ViewBag.Active = _context.RadniNalozi.Count(r => r.Status == StatusNaloga.U_Tijeku);
            ViewBag.Open = _context.RadniNalozi.Count(r => r.Status == StatusNaloga.Otvoren);
            ViewBag.Closed = _context.RadniNalozi.Count(r => r.Status == StatusNaloga.Zatvoren);

            return View(radniNalozi);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nalog = _context.RadniNalozi.FirstOrDefault(r => r.Id == id);
            if (nalog == null)
            {
                TempData["ErrorMessage"] = "Radni nalog nije pronaden.";
                return RedirectToAction("Index");
            }

            PopulateProjects(nalog.ProjektId);
            return View(nalog);
        }

        [HttpPost]
        public IActionResult Edit(int id, RadniNalog updated)
        {
            var nalog = _context.RadniNalozi.FirstOrDefault(r => r.Id == id);
            if (nalog == null)
            {
                TempData["ErrorMessage"] = "Radni nalog nije pronaden.";
                return RedirectToAction("Index");
            }

            if (updated.DatumZatvaranja.HasValue && updated.DatumZatvaranja.Value.Date < updated.DatumOtvaranja.Date)
            {
                ModelState.AddModelError(nameof(updated.DatumZatvaranja), "Datum zatvaranja mora biti jednak ili nakon datuma otvaranja.");
            }

            if (!ModelState.IsValid)
            {
                updated.Id = id;
                PopulateProjects(updated.ProjektId);
                return View(updated);
            }

            nalog.OznakaNaloga = updated.OznakaNaloga;
            nalog.DatumOtvaranja = updated.DatumOtvaranja;
            nalog.DatumZatvaranja = updated.DatumZatvaranja;
            nalog.Status = updated.Status;
            nalog.Opis = updated.Opis;
            nalog.ProjektId = updated.ProjektId;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Radni nalog azuriran.";
            return RedirectToAction("Index");
        }

        private void PopulateProjects(int? selectedProjectId = null)
        {
            ViewBag.Projekti = _context.Projekti
                .OrderBy(p => p.BrojProjekta)
                .ThenBy(p => p.Naziv)
                .ToList();
            ViewBag.SelectedProjectId = selectedProjectId;
        }
    }
}
