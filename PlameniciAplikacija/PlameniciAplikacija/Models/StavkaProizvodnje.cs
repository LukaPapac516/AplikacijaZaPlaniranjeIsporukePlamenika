using System;
using System.ComponentModel.DataAnnotations;

namespace PlameniciAplikacija.Models
{
    public enum TipOperacije
    {
        BravariZavarivaci,
        Planiranje,
        Elektricari,
        Farbanje,
        Montaza
    }

    public enum StatusStavke
    {
        NijePoceto,
        U_Tijeku,
        Zavrseno
    }

    public class StavkaProizvodnje
    {
        public int Id { get; set; }
        public TipOperacije Tip { get; set; }
        public DateTime? DatumPlaniranja { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public StatusStavke Status { get; set; }
        [StringLength(500)]
        public string Napomena { get; set; }
        
        // Foreign Keys
        [Required]
        public int ProjektId { get; set; }
        public virtual Project Projekt { get; set; }
        
        public int? RadniNalogId { get; set; }
        public virtual RadniNalog RadniNalog { get; set; }
    }
}