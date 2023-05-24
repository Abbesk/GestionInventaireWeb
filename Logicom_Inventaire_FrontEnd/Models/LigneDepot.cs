using System;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class LigneDepot
    {
        public string codedep { get; set; }
        public string codeart { get; set; }
        public string libelle { get; set; }
        public string desart { get; set; }
        public string famille { get; set; }
        public double qteart { get; set; }
        public double stockinitial { get; set; }
        public double stockmin { get; set; }
        public double stockmax { get; set; }
        public string typearticle { get; set; }
        public double qteres { get; set; }
        public string numfourn { get; set; }
        public string libfourn { get; set; }
        public DateTime datderninv { get; set; }
        public string type { get; set; }
        public string artmouv { get; set; }
        public string lot { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        public string lieustock { get; set; }
        public string sousfamille { get; set; }
        public double qtereap { get; set; }
        public byte isSelected { get; set; }
        public string numInventaireCourant { get; set; }
        public Article article;

    }
}