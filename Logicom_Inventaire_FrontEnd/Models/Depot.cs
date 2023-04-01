using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class Depot
    {
        
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Adresse { get; set; }
        public string Responsable { get; set; }
        public DateTime? Datec { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string TYPED { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        public string inactif { get; set; }
        public int? sel { get; set; }
        public string SAISIQTENEG { get; set; }

        public  List <LigneDepot> LignesDepot { get; set; }
        public  List<tmpLigneDepot> TMPLignesDepot { get; set; }
        public List<Inventaire> InventaireCourant { get; set; }

        
    }
}