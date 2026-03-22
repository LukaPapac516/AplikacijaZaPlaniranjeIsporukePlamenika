using System;

namespace PlameniciAplikacija.Models
{
    public enum PrioritetNapomene
    {
        Nizak,
        Srednji,
        Visok
    }

    public class Napomena
    {
        public int Id { get; set; }
        public string Tekst { get; set; }
        public DateTime DatumUnosa { get; set; }
        public int AutorId { get; set; }
        public Djelatnik Autor { get; set; }
        public PrioritetNapomene Prioritet { get; set; }
        public DateTime DatumOtvaranja { get; set; }
    }
}