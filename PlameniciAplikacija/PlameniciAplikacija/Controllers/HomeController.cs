using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Data;
using PlameniciAplikacija.Models;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static string? ValidateDateOrder(DateTime datumUnosa, DateTime? ocekivaniRok, DateTime? realniRok)
        {
            if (ocekivaniRok.HasValue && ocekivaniRok.Value.Date < datumUnosa.Date)
            {
                return "Očekivani rok isporuke mora biti jednak ili nakon datuma unosa.";
            }

            if (realniRok.HasValue && realniRok.Value.Date < datumUnosa.Date)
            {
                return "Realni rok isporuke mora biti jednak ili nakon datuma unosa.";
            }

            if (ocekivaniRok.HasValue && realniRok.HasValue && realniRok.Value.Date < ocekivaniRok.Value.Date)
            {
                return "Realni rok isporuke mora biti jednak ili nakon očekivanog roka.";
            }

            return null;
        }

        public IActionResult Index(string? search)
        {
            var projekti = AppData.Projekti.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                DateTime parsedDate;
                var isDateSearch = DateTime.TryParse(term, out parsedDate)
                    || DateTime.TryParseExact(term,
                        new[] { "d/M/yyyy", "dd/MM/yyyy", "M/d/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d.M.yyyy", "dd.MM.yyyy" },
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parsedDate)
                    || DateTime.TryParseExact(term,
                        new[] { "d/M/yyyy", "dd/MM/yyyy", "d.M.yyyy", "dd.MM.yyyy", "yyyy-MM-dd" },
                        new CultureInfo("hr-HR"),
                        DateTimeStyles.None,
                        out parsedDate);

                projekti = projekti.Where(p =>
                    (p.Naziv != null && p.Naziv.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Kupac != null && p.Kupac.Naziv != null && p.Kupac.Naziv.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (p.BrojProjekta != null && p.BrojProjekta.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (isDateSearch && p.DatumUnosa.Date == parsedDate.Date)
                );
            }
            return View(projekti);
        }

        [HttpPost]
        public IActionResult Update(int id, StatusTriStanja statusPripremeRada, StatusTriStanja statusLansiranja, StatusProizvodnje statusProizvodnje, DateTime? realniRokIsporuke, string? napomena, bool prioritet, PrioritetNapomene napomenaPrioritet)
        {
            var projekt = AppData.Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt == null)
            {
                TempData["ErrorMessage"] = "Projekt nije pronađen.";
                return RedirectToAction("Index");
            }

            var dateValidationError = ValidateDateOrder(projekt.DatumUnosa, projekt.OcekivaniRokIsporuke, realniRokIsporuke);
            if (dateValidationError != null)
            {
                TempData["ErrorMessage"] = dateValidationError;
                return RedirectToAction("Index");
            }

            projekt.StatusPripremeRada = statusPripremeRada;
            projekt.StatusLansiranja = statusLansiranja;
            projekt.StatusProizvodnje = statusProizvodnje;
            projekt.RealniRokIsporuke = realniRokIsporuke;
            projekt.Napomena = napomena ?? string.Empty;
            projekt.Prioritet = prioritet;
            projekt.NapomenaPrioritet = napomenaPrioritet;

            TempData["SuccessMessage"] = "Projekt ažuriran";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var projekt = AppData.Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt != null)
            {
                AppData.Projekti.Remove(projekt);
                TempData["SuccessMessage"] = "Projekt obrisan";
            }
            else
            {
                TempData["ErrorMessage"] = "Projekt nije pronađen.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateRadniNalog(int projektId, int nalogId, DateTime datumOtvaranja, DateTime? datumZatvaranja, StatusNaloga status, string? opis)
        {
            var projekt = AppData.Projekti.FirstOrDefault(p => p.Id == projektId);
            if (projekt == null)
            {
                TempData["ErrorMessage"] = "Projekt nije pronađen.";
                return RedirectToAction("Index");
            }

            var nalog = projekt.RadniNalozi?.FirstOrDefault(n => n.Id == nalogId);
            if (nalog == null)
            {
                TempData["ErrorMessage"] = "Radni nalog nije pronađen.";
                return RedirectToAction("Details", new { id = projektId });
            }

            if (datumZatvaranja.HasValue && datumZatvaranja.Value.Date < datumOtvaranja.Date)
            {
                TempData["ErrorMessage"] = "Datum zatvaranja mora biti jednak ili nakon datuma otvaranja.";
                return RedirectToAction("Details", new { id = projektId });
            }

            nalog.DatumOtvaranja = datumOtvaranja;
            nalog.DatumZatvaranja = datumZatvaranja;
            nalog.Status = status;
            nalog.Opis = opis ?? string.Empty;
            AppData.SyncProjectStatuses(projekt);

            TempData["SuccessMessage"] = "Radni nalog ažuriran.";
            return RedirectToAction("Details", new { id = projektId });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Kupci = AppData.Kupci;
            return View(new Project
            {
                DatumUnosa = DateTime.Today,
                StatusProizvodnje = StatusProizvodnje.NijeUProizvodnji,
                NapomenaPrioritet = PrioritetNapomene.Srednji
            });
        }

        [HttpPost]
        public IActionResult Create(Project projekt)
        {
            if (projekt.DatumUnosa == default)
            {
                // If date was left empty, use today's date and clear binder error.
                ModelState.Remove(nameof(projekt.DatumUnosa));
                projekt.DatumUnosa = DateTime.Today;
            }

            if (!projekt.KupacId.HasValue)
            {
                ModelState.AddModelError(nameof(projekt.KupacId), "Odaberite kupca.");
            }

            projekt.Kupac = AppData.Kupci.FirstOrDefault(k => k.Id == projekt.KupacId);
            if (projekt.Kupac == null)
            {
                ModelState.AddModelError(nameof(projekt.KupacId), "Odaberite kupca.");
            }

            var dateValidationError = ValidateDateOrder(projekt.DatumUnosa, projekt.OcekivaniRokIsporuke, projekt.RealniRokIsporuke);
            if (dateValidationError != null)
            {
                ModelState.AddModelError(string.Empty, dateValidationError);
            }

            if (ModelState.IsValid)
            {
                projekt.Id = AppData.Projekti.Count > 0 ? AppData.Projekti.Max(p => p.Id) + 1 : 1;
                projekt.Napomena ??= string.Empty;
                projekt.RadniNalozi = AppData.BuildStandardRadniNalozi(projekt);
                AppData.SyncProjectStatuses(projekt);
                AppData.Projekti.Add(projekt);
                if (projekt.Kupac != null && !projekt.Kupac.Projekti.Any(p => p.Id == projekt.Id))
                {
                    projekt.Kupac.Projekti.Add(projekt);
                }
                TempData["SuccessMessage"] = "Projekt stvoren";
                return RedirectToAction("Index");
            }

            ViewBag.Kupci = AppData.Kupci;
            return View(projekt);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projekt = AppData.Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt == null)
            {
                if (projekt.Kupac != null)
                {
                    projekt.Kupac.Projekti.RemoveAll(p => p.Id == projekt.Id);
                }
                TempData["ErrorMessage"] = "Projekt nije pronađen.";
                return RedirectToAction("Index");
            }

            return View(projekt);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
