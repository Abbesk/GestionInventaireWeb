using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class ResponsablePV
    {
        public string codeuser { get; set; }
        public string nom { get; set; }
        public string codepv { get; set; }
        public string libpv { get; set; }
        public string pvdefaut { get; set; }
        public string afftot { get; set; }
        public string codedep { get; set; }
        public string societe_code { get; set; }
        public string libdep { get; set; }
    }
}