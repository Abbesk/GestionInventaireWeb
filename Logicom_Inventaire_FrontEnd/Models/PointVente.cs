using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class PointVente
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Adresse { get; set; }
        public string Responsable { get; set; }
        public Nullable<System.DateTime> Datec { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string typepv { get; set; }
        public string multidepot { get; set; }
        public ICollection<Depot> Depots { get; set; } 
    }
}