using Microsoft.AspNetCore.Mvc;
using PlameniciAplikacija.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace PlameniciAplikacija.Controllers
{
    public class LookupController : Controller
    {
        private readonly AppDbContext _context;

        public LookupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Plamenici(string q)
        {
            // Prefer DB-stored burner types if available
            if (_context.VrstePlamenika != null && _context.VrstePlamenika.Any())
            {
                var query = _context.VrstePlamenika.AsQueryable();
                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim();
                    query = query.Where(x => EF.Functions.Like(x.Naziv, "%" + term + "%"));
                }

                var results = query
                    .OrderBy(x => x.Naziv)
                    .Select(x => new { id = x.Id, text = x.Naziv })
                    .ToList();

                return Json(results);
            }

            var items = new[] { "ROTONOX", "SSBL", "TEMINOX", "ATONOX" };
            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                items = items.Where(x => x.Contains(term, StringComparison.OrdinalIgnoreCase)).ToArray();
            }

            var result = items.Select(x => new { id = x, text = x });
            return Json(result);
        }

        [HttpGet]
        public IActionResult RadnaMjesta(string q)
        {
            var query = _context.Djelatnici
                .Where(d => d.RadnoMjesto != null)
                .Select(d => d.RadnoMjesto.Trim())
                .Distinct()
                .OrderBy(x => x)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(x => EF.Functions.Like(x, "%" + term + "%"));
            }

            var results = query
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new { id = x, text = x })
                .ToList();

            var fallbackRadnaMjesta = new[]
            {
                "Voditelj projekta",
                "Voditelj proizvodnje",
                "Voditelj kvalitete",
                "Projektant",
                "Elektro inženjer",
                "Procesni inženjer",
                "Tehničar",
                "Planer proizvodnje",
                "Planer materijala",
                "Koordinator logistike",
                "Koordinator instalacija",
                "Specijalist nabave",
                "Specijalist kvalitete",
                "Bravar montažer",
                "Mehaničar zavarivač",
                "Servisni tehničar",
                "PLC programer",
                "Administrativni referent",
                "Radnik montaže",
                "Elektro tehničar"
            };

            results = results
                .Select(x => x.text)
                .Concat(fallbackRadnaMjesta)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .Select(x => new { id = x, text = x })
                .ToList();

            return Json(results);
        }
        
        [HttpGet]
        public IActionResult KupciGradovi(string q)
        {
            var query = _context.Kupci
                .Where(k => k.Grad != null)
                .Select(k => k.Grad.Trim())
                .Distinct()
                .OrderBy(x => x)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(x => EF.Functions.Like(x, "%" + term + "%"));
            }

            var results = query
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new { id = x, text = x })
                .ToList();

            var fallbackGradovi = new[]
            {
                "Zagreb",
                "Split",
                "Rijeka",
                "Osijek",
                "Zadar",
                "Pula",
                "Slavonski Brod",
                "Karlovac",
                "Sisak",
                "Varaždin",
                "Dubrovnik",
                "Šibenik",
                "Bjelovar",
                "Čakovec",
                "Krapina",
                "Vukovar",
                "Makarska",
                "Poreč",
                "Velika Gorica",
                "Samobor"
            };

            results = results
                .Select(x => x.text)
                .Concat(fallbackGradovi)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .Select(x => new { id = x, text = x })
                .ToList();

            return Json(results);
        }
    }
}
