using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class LigneHistorique
    {
        public string nummaj { get; set; }
        public string CODEMVT { get; set; }
        public string typmvt { get; set; }
        public string CODEDEP { get; set; }
        public string LIBDEP { get; set; }
        public string CODEART { get; set; }
        public string DESART { get; set; }
        public Nullable<float> QTEART { get; set; }
        public string FAMILLE { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        
       
    }
}