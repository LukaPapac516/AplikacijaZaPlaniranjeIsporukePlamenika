using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Models;
using System.Diagnostics;

namespace PlameniciAplikacija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Dummy kupci
        private static List<Kupac> Kupci = new List<Kupac>
        {
            new Kupac { Id = 1, Naziv = "Kupac 1", OIB = "12345678901", Adresa = "Adresa 1", Grad = "Grad 1", KontaktOsoba = "Ivan Ivić", Email = "kupac1@email.com", Telefon = "0911111111" },
            new Kupac { Id = 2, Naziv = "Kupac 2", OIB = "23456789012", Adresa = "Adresa 2", Grad = "Grad 2", KontaktOsoba = "Ana Anić", Email = "kupac2@email.com", Telefon = "0922222222" },
            new Kupac { Id = 3, Naziv = "Kupac 3", OIB = "34567890123", Adresa = "Adresa 3", Grad = "Grad 3", KontaktOsoba = "Marko Markić", Email = "kupac3@email.com", Telefon = "0933333333" }
        };

        // Privremena lista projekata (kasnije zamijeniti bazom)
        private static List<Project> Projekti = new List<Project>
        {
            new Project {
                Id = 1,
                BrojProjekta = "001",
                Naziv = "Projekt A",
                KupacId = 1,
                Kupac = Kupci[0],
                DatumUnosa = DateTime.Today.AddDays(-10),
                StatusPripremeRada = StatusTriStanja.NijeStartano,
                StatusLansiranja = StatusTriStanja.NijeStartano,
                StatusProizvodnje = StatusProizvodnje.BravariZavarivaci,
                OcekivaniRokIsporuke = DateTime.Today.AddDays(10),
                RealniRokIsporuke = null,
                Napomena = ""
            },
            new Project {
                Id = 2,
                BrojProjekta = "002",
                Naziv = "Projekt B",
                KupacId = 2,
                Kupac = Kupci[1],
                DatumUnosa = DateTime.Today.AddDays(-5),
                StatusPripremeRada = StatusTriStanja.Startano,
                StatusLansiranja = StatusTriStanja.NijeStartano,
                StatusProizvodnje = StatusProizvodnje.Farbanje,
                OcekivaniRokIsporuke = DateTime.Today.AddDays(5),
                RealniRokIsporuke = null,
                Napomena = "Hitno"
            },
            new Project {
                Id = 3,
                BrojProjekta = "003",
                Naziv = "Projekt C",
                KupacId = 3,
                Kupac = Kupci[2],
                DatumUnosa = DateTime.Today,
                StatusPripremeRada = StatusTriStanja.Zavrseno,
                StatusLansiranja = StatusTriStanja.Startano,
                StatusProizvodnje = StatusProizvodnje.Gotovo,
                OcekivaniRokIsporuke = DateTime.Today.AddDays(2),
                RealniRokIsporuke = DateTime.Today.AddDays(3),
                Napomena = ""
            }
        };

        public IActionResult Index(string search, DateTime? dateFrom, DateTime? dateTo, string statusPripreme, string statusLansiranja, string statusProizvodnje)
        {
            var projekti = Projekti.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                projekti = projekti.Where(p =>
                    (p.Naziv != null && p.Naziv.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (p.Kupac != null && p.Kupac.Naziv != null && p.Kupac.Naziv.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (p.BrojProjekta != null && p.BrojProjekta.Contains(search, StringComparison.OrdinalIgnoreCase))
                );
            }
            if (dateFrom.HasValue)
                projekti = projekti.Where(p => p.DatumUnosa >= dateFrom.Value);
            if (dateTo.HasValue)
                projekti = projekti.Where(p => p.DatumUnosa <= dateTo.Value);
            if (!string.IsNullOrWhiteSpace(statusPripreme) && Enum.TryParse<StatusTriStanja>(statusPripreme, out var parsedPriprema))
                projekti = projekti.Where(p => p.StatusPripremeRada == parsedPriprema);
            if (!string.IsNullOrWhiteSpace(statusLansiranja) && Enum.TryParse<StatusTriStanja>(statusLansiranja, out var parsedLansiranje))
                projekti = projekti.Where(p => p.StatusLansiranja == parsedLansiranje);
            if (!string.IsNullOrWhiteSpace(statusProizvodnje) && Enum.TryParse<StatusProizvodnje>(statusProizvodnje, out var parsedProizvodnja))
                projekti = projekti.Where(p => p.StatusProizvodnje == parsedProizvodnja);

            return View(projekti);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Kupci = Kupci;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project projekt)
        {
            projekt.Kupac = Kupci.FirstOrDefault(k => k.Id == projekt.KupacId);
            if (projekt.Kupac == null)
            {
                ModelState.AddModelError(nameof(projekt.KupacId), "Odaberite kupca.");
            }

            if (ModelState.IsValid)
            {
                projekt.Id = Projekti.Count > 0 ? Projekti.Max(p => p.Id) + 1 : 1;
                Projekti.Add(projekt);
                return RedirectToAction("Index");
            }

            ViewBag.Kupci = Kupci;
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
