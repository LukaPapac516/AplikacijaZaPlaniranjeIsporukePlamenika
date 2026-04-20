using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [StringLength(150)]
        public string Naziv { get; set; }
        [Required]
        public int Redosljed { get; set; }
        public DateTime? PlaniraniPocetak { get; set; }
        public DateTime? PlaniraniZavrsetak { get; set; }
        public DateTime? StvarniPocetak { get; set; }
        public DateTime? StvarniZavrsetak { get; set; }
        public StatusFaze Status { get; set; }
        [StringLength(500)]
        public string Napomena { get; set; }
        public DateTime DatumPlaniranja { get; set; }
        
        // Foreign Key
        [Required]
        public int ProjektId { get; set; }
        public virtual Project Projekt { get; set; }
    }
}