using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Models;
using System.Diagnostics;
using System.Globalization;

namespace PlameniciAplikacija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static int _radniNalogCounter = 1;
        private static readonly string[] StandardneOperacije = new[]
        {
            "Priprema materijala",
            "Bravari i zavarivači",
            "Farbanje",
            "Ožičavanje",
            "Završna montaža"
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Dummy kupci
        private static List<Kupac> Kupci = new List<Kupac>
        {
            new Kupac { Id = 1, Naziv = "Bosch Thermotechnology", OIB = "28765432109", Adresa = "Gottlieb-Daimler-Str. 12", Grad = "Stuttgart", KontaktOsoba = "Karl Meyer", Email = "sourcing@bosch.com", Telefon = "+49 711 1234" },
            new Kupac { Id = 2, Naziv = "Mercedes-Benz AG", OIB = "71234567890", Adresa = "Mercedesstraße 120", Grad = "Stuttgart", KontaktOsoba = "Helena Weiss", Email = "procurement@mercedes.com", Telefon = "+49 711 3050" },
            new Kupac { Id = 3, Naziv = "Siemens Energy", OIB = "99887766554", Adresa = "Werner-von-Siemens-Str. 1", Grad = "Berlin", KontaktOsoba = "Lukas Hoffmann", Email = "energy@siemens.com", Telefon = "+49 30 386" },
            new Kupac { Id = 4, Naziv = "Airbus Defence & Space", OIB = "55443322110", Adresa = "Airbus-Allee 1", Grad = "Hamburg", KontaktOsoba = "Adele Renault", Email = "supplychain@airbus.com", Telefon = "+33 5 6710" },
            new Kupac { Id = 5, Naziv = "Tesla Energy Europe", OIB = "45612378901", Adresa = "1 Tesla Straße", Grad = "Berlin", KontaktOsoba = "Emily Carter", Email = "energy@tesla.com", Telefon = "+49 30 4039" },
            new Kupac { Id = 6, Naziv = "General Electric Power", OIB = "33221144556", Adresa = "420 Lexington Ave", Grad = "New York", KontaktOsoba = "Michael Brown", Email = "supply@ge.com", Telefon = "+1 212 902" },
            new Kupac { Id = 7, Naziv = "Rheinmetall Industrial", OIB = "60112233445", Adresa = "Rheinmetallplatz 1", Grad = "Düsseldorf", KontaktOsoba = "Petra König", Email = "supply@rheinmetall.com", Telefon = "+49 211 473" }
        };

        // Privremena lista projekata (kasnije zamijeniti bazom)
        private static List<Project> Projekti = SeedProjects();

        private static List<Project> SeedProjects()
        {
            var projekti = new List<Project>
            {
                new Project {
                    Id = 1,
                    BrojProjekta = "1017988",
                    Naziv = "TEMINOX MONO G140",
                    KupacId = 1,
                    Kupac = Kupci[0],
                    DatumUnosa = DateTime.Today.AddDays(-14),
                    StatusPripremeRada = StatusTriStanja.Startano,
                    StatusLansiranja = StatusTriStanja.NijeStartano,
                    StatusProizvodnje = StatusProizvodnje.BravariZavarivaci,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(18),
                    RealniRokIsporuke = null,
                    Napomena = "BOSCH premium konfiguracija",
                    Prioritet = true
                },
                new Project {
                    Id = 2,
                    BrojProjekta = "1018120",
                    Naziv = "ROTONOX 200 GL",
                    KupacId = 2,
                    Kupac = Kupci[1],
                    DatumUnosa = DateTime.Today.AddDays(-7),
                    StatusPripremeRada = StatusTriStanja.Startano,
                    StatusLansiranja = StatusTriStanja.Startano,
                    StatusProizvodnje = StatusProizvodnje.Farbanje,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(9),
                    RealniRokIsporuke = null,
                    Napomena = "Mercedes validacija GL varijante"
                },
                new Project {
                    Id = 3,
                    BrojProjekta = "1017560",
                    Naziv = "ATONOX 50 G",
                    KupacId = 3,
                    Kupac = Kupci[2],
                    DatumUnosa = DateTime.Today.AddDays(-21),
                    StatusPripremeRada = StatusTriStanja.Zavrseno,
                    StatusLansiranja = StatusTriStanja.Startano,
                    StatusProizvodnje = StatusProizvodnje.Elektricari,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(5),
                    RealniRokIsporuke = DateTime.Today.AddDays(3),
                    Napomena = "Siemens demo za gas-only postrojenje"
                },
                new Project {
                    Id = 4,
                    BrojProjekta = "1018725",
                    Naziv = "SSBL 400 L",
                    KupacId = 4,
                    Kupac = Kupci[3],
                    DatumUnosa = DateTime.Today.AddDays(-3),
                    StatusPripremeRada = StatusTriStanja.Startano,
                    StatusLansiranja = StatusTriStanja.NijeStartano,
                    StatusProizvodnje = StatusProizvodnje.NijeUProizvodnji,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(21),
                    RealniRokIsporuke = null,
                    Napomena = "Airbus laboratorijska L serija"
                },
                new Project {
                    Id = 5,
                    BrojProjekta = "1018455",
                    Naziv = "TEMINOX 70 GL",
                    KupacId = 5,
                    Kupac = Kupci[4],
                    DatumUnosa = DateTime.Today.AddDays(-11),
                    StatusPripremeRada = StatusTriStanja.Zavrseno,
                    StatusLansiranja = StatusTriStanja.Startano,
                    StatusProizvodnje = StatusProizvodnje.Montaza,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(4),
                    RealniRokIsporuke = DateTime.Today.AddDays(6),
                    Napomena = "Tesla zahtjev za GL dvo-medijsku verziju",
                    Prioritet = true
                },
                new Project {
                    Id = 6,
                    BrojProjekta = "1018105",
                    Naziv = "ROTONOX 280 G&L",
                    KupacId = 6,
                    Kupac = Kupci[5],
                    DatumUnosa = DateTime.Today.AddDays(-18),
                    StatusPripremeRada = StatusTriStanja.Zavrseno,
                    StatusLansiranja = StatusTriStanja.Zavrseno,
                    StatusProizvodnje = StatusProizvodnje.Gotovo,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(-2),
                    RealniRokIsporuke = DateTime.Today.AddDays(-1),
                    Napomena = "GE G&L test bench",
                    Prioritet = false
                },
                new Project {
                    Id = 7,
                    BrojProjekta = "1017300",
                    Naziv = "SSBL 700 L",
                    KupacId = 7,
                    Kupac = Kupci[6],
                    DatumUnosa = DateTime.Today.AddDays(-25),
                    StatusPripremeRada = StatusTriStanja.Zavrseno,
                    StatusLansiranja = StatusTriStanja.Zavrseno,
                    StatusProizvodnje = StatusProizvodnje.Elektricari,
                    OcekivaniRokIsporuke = DateTime.Today.AddDays(12),
                    RealniRokIsporuke = null,
                    Napomena = "Rheinmetall industrijski paket"
                }
            };

            foreach (var projekt in projekti)
            {
                projekt.RadniNalozi = BuildStandardRadniNalozi(projekt);
                SyncProjectStatuses(projekt);
            }

            return projekti;
        }

        private static List<RadniNalog> BuildStandardRadniNalozi(Project projekt)
        {
            var radniNalozi = new List<RadniNalog>();

            for (int i = 0; i < StandardneOperacije.Length; i++)
            {
                radniNalozi.Add(new RadniNalog
                {
                    Id = _radniNalogCounter++,
                    OznakaNaloga = $"{projekt.BrojProjekta}-{i + 1:00}",
                    DatumOtvaranja = projekt.DatumUnosa.AddDays(i),
                    Status = i == 0 ? StatusNaloga.U_Tijeku : StatusNaloga.Otvoren,
                    Opis = StandardneOperacije[i],
                    ProjektId = projekt.Id,
                    Projekt = projekt
                });
            }

            return radniNalozi;
        }

        private static StatusTriStanja MapTriState(StatusNaloga status)
        {
            return status switch
            {
                StatusNaloga.Otvoren => StatusTriStanja.NijeStartano,
                StatusNaloga.U_Tijeku => StatusTriStanja.Startano,
                StatusNaloga.Zatvoren => StatusTriStanja.Zavrseno,
                _ => StatusTriStanja.NijeStartano
            };
        }

        private static RadniNalog? FindNalog(Project projekt, params string[] keywords)
        {
            if (projekt.RadniNalozi == null)
            {
                return null;
            }

            var loweredKeywords = keywords.Select(k => k.ToLowerInvariant()).ToArray();

            return projekt.RadniNalozi.FirstOrDefault(n =>
            {
                var opis = n.Opis?.ToLowerInvariant() ?? string.Empty;
                return loweredKeywords.Any(k => opis.Contains(k));
            });
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

            var lansiranje = FindNalog(projekt, "bravari", "zavarivaci", "zavariva");
            if (lansiranje != null)
            {
                projekt.StatusLansiranja = MapTriState(lansiranje.Status);
            }

            UpdateProizvodnjaStatus(projekt);
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

            var bravari = FindNalog(projekt, "bravari", "zavarivaci", "zavariva");
            if (bravari != null && bravari.Status != StatusNaloga.Otvoren)
            {
                projekt.StatusProizvodnje = StatusProizvodnje.BravariZavarivaci;
                return;
            }

            projekt.StatusProizvodnje = StatusProizvodnje.NijeUProizvodnji;
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

        public IActionResult Index(string search)
        {
            var projekti = Projekti.AsEnumerable();

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
        public IActionResult Update(int id, StatusTriStanja statusPripremeRada, StatusTriStanja statusLansiranja, StatusProizvodnje statusProizvodnje, DateTime? realniRokIsporuke, string? napomena, bool prioritet)
        {
            var projekt = Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt == null)
            {
                TempData["SuccessMessage"] = "Projekt nije pronađen.";
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

            TempData["SuccessMessage"] = "Projekt ažuriran";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var projekt = Projekti.FirstOrDefault(p => p.Id == id);
            if (projekt != null)
            {
                Projekti.Remove(projekt);
                TempData["SuccessMessage"] = "Projekt obrisan";
            }
            else
            {
                TempData["SuccessMessage"] = "Projekt nije pronađen.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateRadniNalog(int projektId, int nalogId, DateTime datumOtvaranja, DateTime? datumZatvaranja, StatusNaloga status, string? opis)
        {
            var projekt = Projekti.FirstOrDefault(p => p.Id == projektId);
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

            TempData["SuccessMessage"] = "Radni nalog ažuriran.";
            return RedirectToAction("Details", new { id = projektId });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Kupci = Kupci;
            return View(new Project
            {
                DatumUnosa = DateTime.Today,
                StatusProizvodnje = StatusProizvodnje.NijeUProizvodnji
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

            projekt.Kupac = Kupci.FirstOrDefault(k => k.Id == projekt.KupacId);
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
                projekt.Id = Projekti.Count > 0 ? Projekti.Max(p => p.Id) + 1 : 1;
                projekt.Napomena ??= string.Empty;
                projekt.RadniNalozi = BuildStandardRadniNalozi(projekt);
                SyncProjectStatuses(projekt);
                Projekti.Add(projekt);
                TempData["SuccessMessage"] = "Projekt stvoren";
                return RedirectToAction("Index");
            }

            ViewBag.Kupci = Kupci;
            return View(projekt);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projekt = Projekti.FirstOrDefault(p => p.Id == id);
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
    }
}
