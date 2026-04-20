using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlameniciAplikacija.Models
{
    public enum StatusNaloga
    {
        Otvoren,
        U_Tijeku,
        Zatvoren
    }

    public class RadniNalog
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string OznakaNaloga { get; set; }
        public DateTime DatumOtvaranja { get; set; }
        public DateTime? DatumZatvaranja { get; set; }
        public StatusNaloga Status { get; set; }
        [StringLength(500)]
        public string Opis { get; set; }
        [Required]
        public int ProjektId { get; set; }
        public virtual Project Projekt { get; set; }
        public virtual ICollection<StavkaProizvodnje> Operacije { get; set; } = new List<StavkaProizvodnje>();
    }
}