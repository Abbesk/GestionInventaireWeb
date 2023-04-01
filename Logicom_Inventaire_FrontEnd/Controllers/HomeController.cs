using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logicom_Inventaire_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Logicom inventaire est un système de gestion des stocks en ligne permettant aux entreprises ou organisations de suivre et de contrôler les niveaux de stocks, les achats, les ventes et les retours de produits.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}