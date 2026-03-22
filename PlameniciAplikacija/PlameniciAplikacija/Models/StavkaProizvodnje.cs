using System;

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
        public string Napomena { get; set; }
    }
}