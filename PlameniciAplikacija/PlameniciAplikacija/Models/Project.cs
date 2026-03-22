using System;

namespace PlameniciAplikacija.Models
{

    public enum StatusTriStanja
    {
        NijeStartano,
        Startano,
        Zavrseno
    }

    public enum StatusProizvodnje
    {
        BravariZavarivaci,
        Farbanje,
        Montaza,
        Elektricari,
        Gotovo
    }

    public class Project
    {
        public int Id { get; set; }
        public string BrojProjekta { get; set; }
        public string Naziv { get; set; }
        public int KupacId { get; set; }
        public Kupac Kupac { get; set; }
        public DateTime DatumUnosa { get; set; }

        public StatusTriStanja StatusPripremeRada { get; set; }
        public StatusTriStanja StatusLansiranja { get; set; }
        public StatusProizvodnje StatusProizvodnje { get; set; }

        public DateTime? OcekivaniRokIsporuke { get; set; }
        public DateTime? RealniRokIsporuke { get; set; }
        public int? Kasnjenje
        {
            get
            {
                if (RealniRokIsporuke.HasValue && OcekivaniRokIsporuke.HasValue)
                {
                    return (RealniRokIsporuke.Value - OcekivaniRokIsporuke.Value).Days;
                }
                return null;
            }
        }
        public string Napomena { get; set; }

        public bool Prioritet { get; set; } // Ako je true, prikazati poseban stil

        public List<RadniNalog> RadniNalozi { get; set; } = new List<RadniNalog>();
        public List<StavkaProizvodnje> StavkeProizvodnje { get; set; } = new List<StavkaProizvodnje>();
        public List<Napomena> Napomene { get; set; } = new List<Napomena>();
        public List<FazaProjekta> FazeProjekta { get; set; } = new List<FazaProjekta>();

        public List<Djelatnik> Djelatnici { get; set; } = new List<Djelatnik>(); // N:M
    }
}
