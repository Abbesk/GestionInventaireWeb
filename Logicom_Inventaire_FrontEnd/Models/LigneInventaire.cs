using System;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class LigneInventaire
    {
        public string NumInv { get; set; }
        public Nullable<System.DateTime> dateInv { get; set; }
        public string codeart { get; set; }
        public string desart { get; set; }
        public Nullable<double> qtes { get; set; }
        public Nullable<double> stockinv { get; set; }
        public Nullable<double> ecartinv { get; set; }
        public Nullable<double> PUART { get; set; }
        public Nullable<double> Remise { get; set; }
        public string frs { get; set; }
        public string famille { get; set; }
        public string libellefourn { get; set; }
        public Nullable<double> prixnet { get; set; }
        public string numbe { get; set; }
        public string nordre { get; set; }
        public string lieustock { get; set; }
        public Nullable<double> numordre { get; set; }
        public string codedep { get; set; }
        public string libdep { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        public Nullable<double> eecart { get; set; }
        public Nullable<double> secart { get; set; }
        public Nullable<System.DateTime> dateexp { get; set; }
        public Nullable<double> stockinvlot { get; set; }
        public double nligne { get; set; }

    }
}