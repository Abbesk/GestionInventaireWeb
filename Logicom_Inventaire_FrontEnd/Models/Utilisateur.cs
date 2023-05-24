using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logicom_Inventaire_FrontEnd.Models
{
    public class Utilisateur
    {
        public string codeuser { get; set; }
        public string nom { get; set; }
        public string motpasse { get; set; }
        public string type { get; set; }
        public string jnlactif { get; set; }
        public string prog1 { get; set; }
        public string prog2 { get; set; }
        public string prog3 { get; set; }
        public string prog4 { get; set; }
        public string bloque { get; set; }
        public string actif { get; set; }
        public string numvol { get; set; }
        public string socutil { get; set; }
        public string etatbcf { get; set; }
        public string etatbcc { get; set; }
        public string etatcl { get; set; }
        public string etatfr { get; set; }
        public string etatbe { get; set; }
        public string etatvente { get; set; }
        public string etatdemachat { get; set; }
        public string vente { get; set; }
        public string achat { get; set; }
        public string client { get; set; }
        public string frs { get; set; }
        public string bcc { get; set; }
        public string bcf { get; set; }
        public string stat { get; set; }
        public string acces { get; set; }
        public string afftot { get; set; }
        public string socutilclot { get; set; }
        public string directeur { get; set; }
        public string etatmarge { get; set; }
        public string exercice { get; set; }
        public string etatimport { get; set; }
        public string etatbestk { get; set; }
        public string Etatbcfstk { get; set; }
        public string etatfrstk { get; set; }
        public string etatimportstk { get; set; }
        public string etatbccstk { get; set; }
        public string etatimmo { get; set; }
        public string acceclot { get; set; }
        public string BL { get; set; }
        public string FC { get; set; }
        public string TICKCAIS { get; set; }
        public string GESTBE { get; set; }
        public string impbarre { get; set; }
        public string PARAM { get; set; }
        public string mp1 { get; set; }
        public string mp2 { get; set; }
        public string mp3 { get; set; }
        public string mp4 { get; set; }
        public string mp5 { get; set; }
        public Nullable<System.DateTime> dernacce { get; set; }
        public Nullable<int> nbrjr { get; set; }
        public string email { get; set; }
    }
}