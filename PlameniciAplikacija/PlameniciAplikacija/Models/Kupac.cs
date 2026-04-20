using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlameniciAplikacija.Models
{
    public enum TipKupca
    {
        PravnaOsoba,
        FizickaOsoba,
        InozemniPartner
    }

    public class Kupac
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan")]
        [StringLength(150)]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "OIB je obavezan")]
        [StringLength(11)]
        public string OIB { get; set; }
        [StringLength(200)]
        public string Adresa { get; set; }
        [StringLength(100)]
        public string Grad { get; set; }
        [StringLength(100)]
        public string KontaktOsoba { get; set; }
        [EmailAddress(ErrorMessage = "Email mora biti validan")]
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Telefon { get; set; }
        public TipKupca Tip { get; set; }
        public DateTime DatumUnosa { get; set; }

        public virtual ICollection<Project> Projekti { get; set; } = new List<Project>();
    }
}