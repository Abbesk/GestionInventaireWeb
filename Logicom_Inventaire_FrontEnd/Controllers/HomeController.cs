using Logicom_Inventaire_FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Logicom_Inventaire_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        string baseURL = "http://localhost:44328/api/";

        public async Task<ActionResult> Index()
        {
            if ((string)Session["token"] != null)
            {
                IList<Inventaire> inventaires = new List<Inventaire>();
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.GetAsync("Inventaire/GetInventaires");
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);
                        //Moyenne d inventaires par semestre 
                        var moyenne = inventaires.GroupBy(i => new { Trimestre = (i.dateinv.Value.Month - 1) / 6, Annee = i.dateinv.Value.Year })
                         .Select(g => new { g.Key.Trimestre, g.Key.Annee, Moyenne = g.Count() })
                         .GroupBy(g => g.Trimestre)
                         .Select(g => new { Trimestre = g.Key, Moyenne = g.Average(x => x.Moyenne) })
                         .ToList();                         
                        ViewBag.Moyenne6Mois = (moyenne.Average(m => m.Moyenne)).ToString("0.0");
                        //Nombre d inventaires  
                        ViewBag.NbInvsOuverts = inventaires.Count(inv => inv.cloture == "0" && inv.dateinv.Value.Year == DateTime.Now.Year);
                        ViewBag.NbInvsClotures = inventaires.Count(inv => inv.cloture == "1" && inv.dateinv.Value.Year == DateTime.Now.Year);
                        ViewBag.NbInvsAnneeCourant = inventaires.Count(inv => inv.dateinv.Value.Year == DateTime.Now.Year);
                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }
                    
                    return View();

                }
            }
            else
            {
                return RedirectToAction("Authentifier", "Login");
            }
           
        }



        public async Task<ActionResult> Contact()
        {
            string selectedSoc = (string)Session["SelectedSoc"];
            if ((string)Session["token"] != "")
            {

                using (var client = new HttpClient())
                {
                    string token = (string)Session["token"];

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage getData = await client.GetAsync("SocieteERP/GetFonctionSociete?id=" + selectedSoc);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        ViewBag.fonction = JsonConvert.DeserializeObject<string>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }

                }
                using (var client = new HttpClient())
                {
                    string token = (string)Session["token"];

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage getData = await client.GetAsync("SocieteERP/GetAdresseSociete?id=" + selectedSoc);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        ViewBag.adresse = JsonConvert.DeserializeObject<string>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }

                }
                using (var client = new HttpClient())
                {
                    string token = (string)Session["token"];

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage getData = await client.GetAsync("SocieteERP/GetEmailSociete?id=" + selectedSoc);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        ViewBag.email = JsonConvert.DeserializeObject<string>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }

                }
                using (var client = new HttpClient())
                {
                    string token = (string)Session["token"];

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage getData = await client.GetAsync("SocieteERP/GetTelSociete?id=" + selectedSoc);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        ViewBag.tel = JsonConvert.DeserializeObject<string>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }

                }
                ViewBag.Message = "Your contact page.";
                return View();
            }
            return RedirectToAction("Index", "Login");




        }
    }
}