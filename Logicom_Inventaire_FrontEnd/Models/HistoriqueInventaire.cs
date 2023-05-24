using System;
using System.Collections.Generic;


namespace Logicom_Inventaire_FrontEnd.Models
{
    public class HistoriqueInventaire
    {
        public string NumMAJ { get; set; }
        public DateTime? datemaj { get; set; }
        public string heuremaj { get; set; }
        public string codeapp { get; set; }
        public string libmaj { get; set; }
        public string codeutil { get; set; }
        public string nomutil { get; set; }
        public string codepv { get; set; }
        public string codemvt { get; set; }
        public DateTime? datemvt { get; set; }
        public string typemvt { get; set; }
        public double? MHT { get; set; }
        public double? MTTC { get; set; }
        public double? MTVA { get; set; }
        public double? MREMISE { get; set; }
        public double? MFODEC { get; set; }
        public double? MRETENUE { get; set; }
        public string numpiece { get; set; }
        public string TYPEREGL { get; set; }
        public string CODETRS { get; set; }
        public string LIBTRS { get; set; }
        public string TYPETRS { get; set; }
        public string etat { get; set; }
        public string numdoss { get; set; }
        public string typefac { get; set; }
        public Nullable<double> cours { get; set; }
        public string codesel { get; set; }
        public string typemvtimp { get; set; }
        public Nullable<System.DateTime> datepiece { get; set; }
        public Nullable<double> mtdev { get; set; }
        public string declaration { get; set; }
        public string typdec { get; set; }
        public Nullable<System.DateTime> dateentree { get; set; }
        public string dae { get; set; }
        public string pieceliee2 { get; set; }
        public string devise { get; set; }
        public string dureeajout { get; set; }
        public string dureemodif { get; set; }

        public List <LigneHistorique> Lignes { get; set; }
    }
}