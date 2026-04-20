using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlameniciAplikacija.Models
{
    public enum TipDjelatnika
    {
        Inzenjer,
        Tehnicar,
        Radnik,
        Voditelj,
        Administracija
    }

    public class Djelatnik
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(100)]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(100)]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Radno mjesto je obavezno")]
        [StringLength(100)]
        public string RadnoMjesto { get; set; }
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Email mora biti validan")]
        [StringLength(150)]
        public string Email { get; set; }
        public TipDjelatnika Tip { get; set; }
        public DateTime DatumZaposlenja { get; set; }

        public virtual ICollection<Project> Projekti { get; set; } = new List<Project>(); // N:M
    }
}