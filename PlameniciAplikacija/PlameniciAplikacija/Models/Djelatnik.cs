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
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string RadnoMjesto { get; set; }
        public string Email { get; set; }
        public TipDjelatnika Tip { get; set; }
        public DateTime DatumZaposlenja { get; set; }

        public List<Project> Projekti { get; set; } = new List<Project>(); // N:M
    }
}