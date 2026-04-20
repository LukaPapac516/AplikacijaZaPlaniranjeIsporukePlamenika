using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        NijeUProizvodnji,
        BravariZavarivaci,
        Farbanje,
        Montaza,
        Elektricari,
        Gotovo
    }

    public class Project
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Broj projekta je obavezan")]
        [StringLength(50)]
        public string BrojProjekta { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan")]
        [StringLength(200)]
        public string Naziv { get; set; }
        public int? KupacId { get; set; }
        public virtual Kupac? Kupac { get; set; }
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
        [StringLength(500)]
        public string Napomena { get; set; } = string.Empty;
        public PrioritetNapomene NapomenaPrioritet { get; set; } = PrioritetNapomene.Srednji;

        public bool Prioritet { get; set; } // Ako je true, prikazati poseban stil

        public virtual ICollection<RadniNalog> RadniNalozi { get; set; } = new List<RadniNalog>();
        public virtual ICollection<StavkaProizvodnje> StavkeProizvodnje { get; set; } = new List<StavkaProizvodnje>();
        public virtual ICollection<Napomena> Napomene { get; set; } = new List<Napomena>();
        public virtual ICollection<FazaProjekta> FazeProjekta { get; set; } = new List<FazaProjekta>();

        public virtual ICollection<Djelatnik> Djelatnici { get; set; } = new List<Djelatnik>(); // N:M
    }
}
