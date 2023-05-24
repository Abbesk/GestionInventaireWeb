using System;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class tmpLigneDepot
    {
        public string famille { get; set; }
        public string codeart { get; set; }
        public string desart { get; set; }
        public Nullable<double> qteart { get; set; }
        public string lieustock { get; set; }
        public Nullable<System.DateTime> Datderninv { get; set; }
        public string codesel { get; set; }
        public string saisie { get; set; }
        public string LibUtil { get; set; }
        public string fourn { get; set; }
        public string libellefourn { get; set; }
        public double? numordre { get; set; }
        public string codedep { get; set; }
        public string libelle { get; set; }
        public string nordre { get; set; }
        public DateTime? dateexp { get; set; }
        public string numinv { get; set; }
        public double? puht { get; set; }
        public sbyte isSelected { get; set; }
        public string codepv { get; set; }
        public string commentaire { get; set; }
        public double? qteInventaire { get; set; }
        public Article Article { get; set; }
        

    }
}