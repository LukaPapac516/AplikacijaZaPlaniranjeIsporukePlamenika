using System;
using System.Collections.Generic;

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
        public string OznakaNaloga { get; set; }
        public DateTime DatumOtvaranja { get; set; }
        public DateTime? DatumZatvaranja { get; set; }
        public StatusNaloga Status { get; set; }
        public string Opis { get; set; }
        public int ProjektId { get; set; }
        public Project Projekt { get; set; }
        public List<StavkaProizvodnje> Operacije { get; set; } = new List<StavkaProizvodnje>();
    }
}