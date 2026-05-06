using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;
        private static readonly string[] StandardneOperacije =
        {
            "Priprema materijala",
            "Bravari i zavarivači",
            "Farbanje",
            "Ožičavanje",
            "Završna montaža"
        };

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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

        [HttpGet]
        public IActionResult Dashboard()
        {
            var projekti = _context.Projekti
                .Include(p => p.Kupac)
                .ToList();
            var today = DateTime.Today;

            ViewBag.TotalProjects = projekti.Count;
            ViewBag.PriorityProjects = projekti.Count(p => p.Prioritet);
            ViewBag.OverdueProjects = projekti.Count(p =>
                p.OcekivaniRokIsporuke.HasValue &&
                p.OcekivaniRokIsporuke.Value.Date < today &&
                p.StatusProizvodnje != StatusProizvodnje.Gotovo);
            ViewBag.ActiveCustomers = projekti
                .Where(p => p.KupacId.HasValue)
                .Select(p => p.KupacId!.Value)
                .Distinct()
                .Count();

            ViewBag.RiskProjects = projekti
                .Where(p => p.Prioritet || p.Kasnjenje > 0 || p.StatusProizvodnje == StatusProizvodnje.NijeUProizvodnji)
                .OrderByDescending(p => p.Prioritet)
                .ThenByDescending(p => p.Kasnjenje)
                .Take(6)
                .ToList();

            return View();
        }

        public IActionResult Index(string? search)
        {
            var projekti = _context.Projekti
                .Include(p => p.Kupac)
                .Include(p => p.Djelatnici)
                .AsEnumerable();

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
            var projekt = _context.Projekti.FirstOrDefault(p => p.Id == id);
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
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Projekt ažuriran";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var projekt = _context.Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt != null)
            {
                _context.Projekti.Remove(projekt);
                _context.SaveChanges();
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
            var projekt = _context.Projekti
                .Include(p => p.RadniNalozi)
                .FirstOrDefault(p => p.Id == projektId);
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
            SyncProjectStatuses(projekt);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Radni nalog ažuriran.";
            return RedirectToAction("Details", new { id = projektId });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Kupci = _context.Kupci
                .OrderBy(k => k.Naziv)
                .ToList();
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

            var kupacExists = projekt.KupacId.HasValue && _context.Kupci.Any(k => k.Id == projekt.KupacId.Value);
            if (!kupacExists)
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
                projekt.Napomena ??= string.Empty;
                _context.Projekti.Add(projekt);
                _context.SaveChanges();

                var radniNalozi = BuildStandardRadniNalozi(projekt);
                _context.RadniNalozi.AddRange(radniNalozi);
                projekt.RadniNalozi = radniNalozi;
                SyncProjectStatuses(projekt);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Projekt stvoren";
                return RedirectToAction("Index");
            }

            ViewBag.Kupci = _context.Kupci
                .OrderBy(k => k.Naziv)
                .ToList();
            return View(projekt);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projekt = _context.Projekti
                .Include(p => p.Kupac)
                .Include(p => p.RadniNalozi)
                .Include(p => p.StavkeProizvodnje)
                .FirstOrDefault(p => p.Id == id);

            if (projekt == null)
            {
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

        private static List<RadniNalog> BuildStandardRadniNalozi(Project projekt)
        {
            var radniNalozi = new List<RadniNalog>();

            for (int i = 0; i < StandardneOperacije.Length; i++)
            {
                radniNalozi.Add(new RadniNalog
                {
                    OznakaNaloga = $"{projekt.BrojProjekta}-{i + 1:00}",
                    DatumOtvaranja = projekt.DatumUnosa.AddDays(i),
                    Status = i == 0 ? StatusNaloga.U_Tijeku : StatusNaloga.Otvoren,
                    Opis = StandardneOperacije[i],
                    ProjektId = projekt.Id
                });
            }

            return radniNalozi;
        }

        private static void SyncProjectStatuses(Project projekt)
        {
            if (projekt.RadniNalozi == null || projekt.RadniNalozi.Count == 0)
            {
                return;
            }

            var priprema = FindNalog(projekt, "priprema");
            if (priprema != null)
            {
                projekt.StatusPripremeRada = MapTriState(priprema.Status);
            }

            var lansiranje = FindNalog(projekt, "bravari", "zavariva");
            if (lansiranje != null)
            {
                projekt.StatusLansiranja = MapTriState(lansiranje.Status);
            }

            UpdateProizvodnjaStatus(projekt);
        }

        private static StatusTriStanja MapTriState(StatusNaloga status) => status switch
        {
            StatusNaloga.Otvoren => StatusTriStanja.NijeStartano,
            StatusNaloga.U_Tijeku => StatusTriStanja.Startano,
            StatusNaloga.Zatvoren => StatusTriStanja.Zavrseno,
            _ => StatusTriStanja.NijeStartano
        };

        private static RadniNalog? FindNalog(Project projekt, params string[] keywords)
        {
            var loweredKeywords = keywords.Select(k => k.ToLowerInvariant()).ToArray();

            return projekt.RadniNalozi.FirstOrDefault(n =>
            {
                var opis = n.Opis?.ToLowerInvariant() ?? string.Empty;
                return loweredKeywords.Any(k => opis.Contains(k));
            });
        }

        private static void UpdateProizvodnjaStatus(Project projekt)
        {
            var montaza = FindNalog(projekt, "monta");
            if (montaza != null && montaza.Status == StatusNaloga.Zatvoren)
            {
                projekt.StatusProizvodnje = StatusProizvodnje.Gotovo;
                return;
            }

            var ozicavanje = FindNalog(projekt, "ozic", "ožič", "ozi");
            if (ozicavanje != null && ozicavanje.Status != StatusNaloga.Otvoren)
            {
                projekt.StatusProizvodnje = StatusProizvodnje.Elektricari;
                return;
            }

            var farbanje = FindNalog(projekt, "farban");
            if (farbanje != null && farbanje.Status != StatusNaloga.Otvoren)
            {
                projekt.StatusProizvodnje = StatusProizvodnje.Farbanje;
                return;
            }

            var bravari = FindNalog(projekt, "bravari", "zavariva");
            if (bravari != null && bravari.Status != StatusNaloga.Otvoren)
            {
                projekt.StatusProizvodnje = StatusProizvodnje.BravariZavarivaci;
                return;
            }

            projekt.StatusProizvodnje = StatusProizvodnje.NijeUProizvodnji;
        }
    }
}
