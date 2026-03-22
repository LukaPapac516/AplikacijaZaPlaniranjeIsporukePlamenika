using System;

namespace PlameniciAplikacija.Models
{
    public enum StatusFaze
    {
        NijePocelo,
        U_Tijeku,
        Zavrseno
    }

    public class FazaProjekta
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Redosljed { get; set; }
        public DateTime? PlaniraniPocetak { get; set; }
        public DateTime? PlaniraniZavrsetak { get; set; }
        public DateTime? StvarniPocetak { get; set; }
        public DateTime? StvarniZavrsetak { get; set; }
        public StatusFaze Status { get; set; }
        public string Napomena { get; set; }
        public DateTime DatumPlaniranja { get; set; }
    }
}