using System;
using System.Collections.Generic;
using System.Linq;
using PlameniciAplikacija.Models;

namespace PlameniciAplikacija.Data
{
    public static class AppData
    {
        private static int _radniNalogCounter = 1;
        private static readonly string[] StandardneOperacije =
        {
            "Priprema materijala",
            "Bravari i zavarivači",
            "Farbanje",
            "Ožičavanje",
            "Završna montaža"
        };

        public static List<Kupac> Kupci { get; } = SeedKupci();
        public static List<Project> Projekti { get; } = SeedProjects();
        public static List<Djelatnik> Djelatnici { get; } = SeedDjelatnici();

        private static List<Kupac> SeedKupci()
        {
            var today = DateTime.Today;

            return new List<Kupac>
            {
                new Kupac { Id = 1, Naziv = "Bosch Thermotechnology", OIB = "28765432109", Adresa = "Gottlieb-Daimler-Str. 12", Grad = "Stuttgart", KontaktOsoba = "Karl Meyer", Email = "sourcing@bosch.com", Telefon = "+49 711 1234", Tip = TipKupca.PravnaOsoba, DatumUnosa = today.AddDays(-240) },
                new Kupac { Id = 2, Naziv = "Mercedes-Benz AG", OIB = "71234567890", Adresa = "Mercedesstraße 120", Grad = "Stuttgart", KontaktOsoba = "Helena Weiss", Email = "procurement@mercedes.com", Telefon = "+49 711 3050", Tip = TipKupca.PravnaOsoba, DatumUnosa = today.AddDays(-210) },
                new Kupac { Id = 3, Naziv = "Siemens Energy", OIB = "99887766554", Adresa = "Werner-von-Siemens-Str. 1", Grad = "Berlin", KontaktOsoba = "Lukas Hoffmann", Email = "energy@siemens.com", Telefon = "+49 30 386", Tip = TipKupca.PravnaOsoba, DatumUnosa = today.AddDays(-190) },
                new Kupac { Id = 4, Naziv = "Airbus Defence & Space", OIB = "55443322110", Adresa = "Airbus-Allee 1", Grad = "Hamburg", KontaktOsoba = "Adele Renault", Email = "supplychain@airbus.com", Telefon = "+33 5 6710", Tip = TipKupca.InozemniPartner, DatumUnosa = today.AddDays(-160) },
                new Kupac { Id = 5, Naziv = "Tesla Energy Europe", OIB = "45612378901", Adresa = "1 Tesla Straße", Grad = "Berlin", KontaktOsoba = "Emily Carter", Email = "energy@tesla.com", Telefon = "+49 30 4039", Tip = TipKupca.InozemniPartner, DatumUnosa = today.AddDays(-130) },
                new Kupac { Id = 6, Naziv = "General Electric Power", OIB = "33221144556", Adresa = "420 Lexington Ave", Grad = "New York", KontaktOsoba = "Michael Brown", Email = "supply@ge.com", Telefon = "+1 212 902", Tip = TipKupca.InozemniPartner, DatumUnosa = today.AddDays(-110) },
                new Kupac { Id = 7, Naziv = "Rheinmetall Industrial", OIB = "60112233445", Adresa = "Rheinmetallplatz 1", Grad = "Düsseldorf", KontaktOsoba = "Petra König", Email = "supply@rheinmetall.com", Telefon = "+49 211 473", Tip = TipKupca.PravnaOsoba, DatumUnosa = today.AddDays(-95) }
            };
        }

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
                    Napomena = "BOSCH premium konfiguracija",
                    Prioritet = true,
                    NapomenaPrioritet = PrioritetNapomene.Visok
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
                    Napomena = "Mercedes validacija GL varijante",
                    NapomenaPrioritet = PrioritetNapomene.Srednji
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
                    Napomena = "Siemens demo za gas-only postrojenje",
                    NapomenaPrioritet = PrioritetNapomene.Nizak
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
                    Napomena = "Airbus laboratorijska L serija",
                    NapomenaPrioritet = PrioritetNapomene.Srednji
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
                    Prioritet = true,
                    NapomenaPrioritet = PrioritetNapomene.Visok
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
                    NapomenaPrioritet = PrioritetNapomene.Nizak
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
                    Napomena = "Rheinmetall industrijski paket",
                    NapomenaPrioritet = PrioritetNapomene.Srednji
                }
            };

            foreach (var projekt in projekti)
            {
                var kupac = Kupci.FirstOrDefault(k => k.Id == projekt.KupacId);
                if (kupac != null && !kupac.Projekti.Any(p => p.Id == projekt.Id))
                {
                    kupac.Projekti.Add(projekt);
                }

                projekt.RadniNalozi = BuildStandardRadniNalozi(projekt);
                SyncProjectStatuses(projekt);
            }

            return projekti;
        }

        private static List<Djelatnik> SeedDjelatnici()
        {
            var djelatnici = new List<Djelatnik>
            {
                new Djelatnik { Id = 1, Ime = "Marin", Prezime = "Kovač", RadnoMjesto = "Voditelj projekta", Email = "marin.kovac@plamenici.hr", Tip = TipDjelatnika.Voditelj, DatumZaposlenja = DateTime.Today.AddYears(-6) },
                new Djelatnik { Id = 2, Ime = "Sara", Prezime = "Horvat", RadnoMjesto = "Elektro inženjer", Email = "sara.horvat@plamenici.hr", Tip = TipDjelatnika.Inzenjer, DatumZaposlenja = DateTime.Today.AddYears(-3) },
                new Djelatnik { Id = 3, Ime = "Ivan", Prezime = "Barišić", RadnoMjesto = "Planer proizvodnje", Email = "ivan.barisic@plamenici.hr", Tip = TipDjelatnika.Tehnicar, DatumZaposlenja = DateTime.Today.AddYears(-2) },
                new Djelatnik { Id = 4, Ime = "Lea", Prezime = "Marić", RadnoMjesto = "Koordinator logistike", Email = "lea.maric@plamenici.hr", Tip = TipDjelatnika.Administracija, DatumZaposlenja = DateTime.Today.AddYears(-5).AddMonths(1) },
                new Djelatnik { Id = 5, Ime = "Filip", Prezime = "Novak", RadnoMjesto = "Voditelj kvalitete", Email = "filip.novak@plamenici.hr", Tip = TipDjelatnika.Voditelj, DatumZaposlenja = DateTime.Today.AddYears(-4) },
                new Djelatnik { Id = 6, Ime = "Kristina", Prezime = "Šimić", RadnoMjesto = "Procesni inženjer", Email = "kristina.simic@plamenici.hr", Tip = TipDjelatnika.Inzenjer, DatumZaposlenja = DateTime.Today.AddYears(-3).AddMonths(-4) },
                new Djelatnik { Id = 7, Ime = "Petar", Prezime = "Lukić", RadnoMjesto = "Specijalist nabave", Email = "petar.lukic@plamenici.hr", Tip = TipDjelatnika.Administracija, DatumZaposlenja = DateTime.Today.AddYears(-7) },
                new Djelatnik { Id = 8, Ime = "Anja", Prezime = "Petrović", RadnoMjesto = "Mehaničar zavarivač", Email = "anja.petrovic@plamenici.hr", Tip = TipDjelatnika.Radnik, DatumZaposlenja = DateTime.Today.AddYears(-2).AddMonths(-6) },
                new Djelatnik { Id = 9, Ime = "Luka", Prezime = "Tomić", RadnoMjesto = "Elektro tehničar", Email = "luka.tomic@plamenici.hr", Tip = TipDjelatnika.Tehnicar, DatumZaposlenja = DateTime.Today.AddYears(-1).AddMonths(-10) },
                new Djelatnik { Id = 10, Ime = "Martina", Prezime = "Grgić", RadnoMjesto = "Planer materijala", Email = "martina.grgic@plamenici.hr", Tip = TipDjelatnika.Administracija, DatumZaposlenja = DateTime.Today.AddYears(-8) },
                new Djelatnik { Id = 11, Ime = "Nikola", Prezime = "Jurić", RadnoMjesto = "Voditelj proizvodnje", Email = "nikola.juric@plamenici.hr", Tip = TipDjelatnika.Voditelj, DatumZaposlenja = DateTime.Today.AddYears(-9) },
                new Djelatnik { Id = 12, Ime = "Klara", Prezime = "Herceg", RadnoMjesto = "PLC programer", Email = "klara.herceg@plamenici.hr", Tip = TipDjelatnika.Inzenjer, DatumZaposlenja = DateTime.Today.AddYears(-4).AddMonths(-7) },
                new Djelatnik { Id = 13, Ime = "Domagoj", Prezime = "Bašić", RadnoMjesto = "Servisni tehničar", Email = "domagoj.basic@plamenici.hr", Tip = TipDjelatnika.Tehnicar, DatumZaposlenja = DateTime.Today.AddYears(-6).AddMonths(3) },
                new Djelatnik { Id = 14, Ime = "Iva", Prezime = "Radić", RadnoMjesto = "Specijalist kvalitete", Email = "iva.radic@plamenici.hr", Tip = TipDjelatnika.Inzenjer, DatumZaposlenja = DateTime.Today.AddYears(-5).AddMonths(-2) },
                new Djelatnik { Id = 15, Ime = "Marko", Prezime = "Blažević", RadnoMjesto = "Bravar montažer", Email = "marko.blazevic@plamenici.hr", Tip = TipDjelatnika.Radnik, DatumZaposlenja = DateTime.Today.AddYears(-3).AddMonths(-9) },
                new Djelatnik { Id = 16, Ime = "Tena", Prezime = "Knez", RadnoMjesto = "Industrijski dizajner", Email = "tena.knez@plamenici.hr", Tip = TipDjelatnika.Inzenjer, DatumZaposlenja = DateTime.Today.AddYears(-4).AddMonths(5) },
                new Djelatnik { Id = 17, Ime = "Dario", Prezime = "Mandić", RadnoMjesto = "Tehničar automatizacije", Email = "dario.mandic@plamenici.hr", Tip = TipDjelatnika.Tehnicar, DatumZaposlenja = DateTime.Today.AddYears(-2).AddMonths(2) },
                new Djelatnik { Id = 18, Ime = "Maja", Prezime = "Ivanković", RadnoMjesto = "Koordinator instalacija", Email = "maja.ivankovic@plamenici.hr", Tip = TipDjelatnika.Voditelj, DatumZaposlenja = DateTime.Today.AddYears(-6).AddMonths(-8) },
                new Djelatnik { Id = 19, Ime = "Stjepan", Prezime = "Orešković", RadnoMjesto = "Radnik montaže", Email = "stjepan.oreskovic@plamenici.hr", Tip = TipDjelatnika.Radnik, DatumZaposlenja = DateTime.Today.AddYears(-3).AddMonths(1) },
                new Djelatnik { Id = 20, Ime = "Andrea", Prezime = "Prskalo", RadnoMjesto = "Administrativni referent", Email = "andrea.prskalo@plamenici.hr", Tip = TipDjelatnika.Administracija, DatumZaposlenja = DateTime.Today.AddYears(-1).AddMonths(-4) }
            };

            AssignProjectsToEmployeeInternal(djelatnici[0], new[] { 1, 2, 5 });
            AssignProjectsToEmployeeInternal(djelatnici[1], new[] { 3, 6 });
            AssignProjectsToEmployeeInternal(djelatnici[2], new[] { 4, 7 });
            AssignProjectsToEmployeeInternal(djelatnici[3], new[] { 2, 4 });
            AssignProjectsToEmployeeInternal(djelatnici[4], new[] { 1, 5, 6 });
            AssignProjectsToEmployeeInternal(djelatnici[5], new[] { 3, 5 });
            AssignProjectsToEmployeeInternal(djelatnici[6], new[] { 1 });
            AssignProjectsToEmployeeInternal(djelatnici[7], new[] { 5, 7 });
            AssignProjectsToEmployeeInternal(djelatnici[8], new[] { 3, 4 });
            AssignProjectsToEmployeeInternal(djelatnici[9], new[] { 2 });
            AssignProjectsToEmployeeInternal(djelatnici[10], new[] { 1, 2, 3 });
            AssignProjectsToEmployeeInternal(djelatnici[11], new[] { 6, 7 });
            AssignProjectsToEmployeeInternal(djelatnici[12], new[] { 4 });
            AssignProjectsToEmployeeInternal(djelatnici[13], new[] { 5 });
            AssignProjectsToEmployeeInternal(djelatnici[14], new[] { 2, 6 });
            AssignProjectsToEmployeeInternal(djelatnici[15], new[] { 7 });
            AssignProjectsToEmployeeInternal(djelatnici[16], new[] { 3 });
            AssignProjectsToEmployeeInternal(djelatnici[17], new[] { 1, 4, 6 });
            AssignProjectsToEmployeeInternal(djelatnici[18], new[] { 5 });
            AssignProjectsToEmployeeInternal(djelatnici[19], new[] { 2, 3 });

            return djelatnici;
        }

        public static List<RadniNalog> BuildStandardRadniNalozi(Project projekt)
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

        public static void SyncProjectStatuses(Project projekt)
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

        public static void AssignProjectsToEmployee(Djelatnik djelatnik, IEnumerable<int> projectIds)
        {
            var targetIds = new HashSet<int>(projectIds ?? Enumerable.Empty<int>());
            RemoveEmployeeFromProjects(djelatnik);
            djelatnik.Projekti.Clear();

            foreach (var projekt in Projekti.Where(p => targetIds.Contains(p.Id)))
            {
                djelatnik.Projekti.Add(projekt);
                if (!projekt.Djelatnici.Any(d => d.Id == djelatnik.Id))
                {
                    projekt.Djelatnici.Add(djelatnik);
                }
            }
        }

        public static void RemoveEmployeeFromProjects(Djelatnik djelatnik)
        {
            foreach (var projekt in Projekti)
            {
                var toRemove = projekt.Djelatnici.Where(d => d.Id == djelatnik.Id).ToList();
                foreach (var item in toRemove)
                    projekt.Djelatnici.Remove(item);
            }

            djelatnik.Projekti.Clear();
        }

        private static void AssignProjectsToEmployeeInternal(Djelatnik djelatnik, IEnumerable<int> projectIds)
        {
            djelatnik.Projekti.Clear();

            foreach (var projekt in Projekti.Where(p => projectIds.Contains(p.Id)))
            {
                djelatnik.Projekti.Add(projekt);
                if (!projekt.Djelatnici.Any(d => d.Id == djelatnik.Id))
                {
                    projekt.Djelatnici.Add(djelatnik);
                }
            }
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
