using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        [StringLength(1000)]
        public string Tekst { get; set; }
        public DateTime DatumUnosa { get; set; }
        [Required]
        public int AutorId { get; set; }
        public virtual Djelatnik Autor { get; set; }
        public PrioritetNapomene Prioritet { get; set; }
        public DateTime DatumOtvaranja { get; set; }
    }
}