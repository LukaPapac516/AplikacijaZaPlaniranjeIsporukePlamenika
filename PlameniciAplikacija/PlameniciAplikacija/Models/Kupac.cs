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
        public string Naziv { get; set; }
        public string OIB { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public string KontaktOsoba { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public TipKupca Tip { get; set; }
        public DateTime DatumUnosa { get; set; }

        public List<Project> Projekti { get; set; } = new List<Project>();
    }
}