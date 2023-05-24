namespace Logicom_Inventaire_FrontEnd.Models
{
    public class Article
    {
        public string code { get; set; }
        public string libelle { get; set; }
        public string fourn { get; set; }
        public string libellefourn { get; set; }
        public string fam { get; set; }

        public double puht { get; set; }
        public Famille Famille { get; set; }
        public Fournisseur Fournisseur { get; set; }
    }
}