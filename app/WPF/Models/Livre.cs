namespace BibliothequeWPF.Models
{
    public class Livre
    {
        public int id { get; set; }
        public string titre { get; set; }
        public string auteur { get; set; }
        public string isbn { get; set; }
        public int? exemplairesDisponibles { get; set; }
    }
} 