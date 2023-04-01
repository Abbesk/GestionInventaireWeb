using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class Inventaire
    {
        public string numinv { get; set; }
        
        public Nullable<System.DateTime> dateinv { get; set; }
        public Nullable<int> nbrcomptage { get; set; }
        public string commentaire { get; set; }
        public string cloture { get; set; }
        public Nullable<System.DateTime> datecloture { get; set; }
        public string utilisateur { get; set; }
        public string coderep { get; set; }
        public string rsrep { get; set; }
        public string usera { get; set; }
        public string userm { get; set; }
        public string users { get; set; }
        public Nullable<System.DateTime> DATEDMAJ { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        public string codedep { get; set; }
        public string libdep { get; set; }
        public string numcloture { get; set; }
        public Nullable<System.DateTime> dateimp { get; set; }
        public string typdec { get; set; }
        public string declaration { get; set; }
        public string typeapp { get; set; }
        public string zone { get; set; }
        public virtual Depot Depot { get; set; }
        public  virtual PointVente PointVente { get; set; }

        public List<LigneInventaire> LignesInventaire { get; set; }

    }
}