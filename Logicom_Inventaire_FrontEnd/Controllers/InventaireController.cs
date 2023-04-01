using Logicom_Inventaire_FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Logicom_Inventaire_FrontEnd.Controllers
{
    public class InventaireController : Controller
    {
        string baseURL = "http://localhost:44328/api/";
        



        public async Task<ActionResult> Index2()
        {
            DataTable dt = new DataTable();
            using (var inventaire = new HttpClient())
            {
                inventaire.BaseAddress = new Uri(baseURL);
                inventaire.DefaultRequestHeaders.Accept.Clear();
                inventaire.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await inventaire.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewData.Model = dt;
            }

            return View();
        }



        public async Task<ActionResult> Index()
        {
            IList<Inventaire> inventaires = new List<Inventaire>(); 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL+ "Inventaire");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewData.Model = inventaires;
                
            }

            return View();
        }
        public async Task<ActionResult> InventairesNonClotures()
        {
            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);
                    //TempData["lstInventaires"] = inventaires; 

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewData.Model = inventaires;
            }

            return View();
        }
        public async Task<ActionResult> Create(Inventaire inventaire)
        {




             IList<PointVente> pvs = new List<PointVente>(); 
             using (var client = new HttpClient())
             {
                 client.BaseAddress = new Uri(baseURL);
                 client.DefaultRequestHeaders.Accept.Clear();
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage getData = await client.GetAsync("PointVente");
                 if (getData.IsSuccessStatusCode)
                 {
                     string results = getData.Content.ReadAsStringAsync().Result;
                     pvs = JsonConvert.DeserializeObject<List<PointVente>>(results);

                 }
                 else
                 {
                     Console.WriteLine("Erreur calling web api");
                 }
                 ViewBag.Code_Point_Vente = new SelectList(pvs, "code", "libelle");

             }
             
             IList<Depot> depots = new List<Depot>();
             using (var client = new HttpClient())
             {
                 client.BaseAddress = new Uri(baseURL);
                 client.DefaultRequestHeaders.Accept.Clear();
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage getData = await client.GetAsync("Depot");
                 if (getData.IsSuccessStatusCode)
                 {
                     string results = getData.Content.ReadAsStringAsync().Result;
                     depots = JsonConvert.DeserializeObject<List<Depot>>(results);

                 }
                 else
                 {
                     Console.WriteLine("Erreur calling web api");
                 }

                 ViewBag.codeDepot = new SelectList(depots, "code", "libelle");
             }


             



             
            /////////
                IList<Inventaire> inventaires = new List<Inventaire>();
             using (var client = new HttpClient())
             {
                 client.BaseAddress = new Uri(baseURL);
                 client.DefaultRequestHeaders.Accept.Clear();
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage getData = await client.GetAsync("Inventaire");
                     if (getData.IsSuccessStatusCode)
                 {
                     string results = getData.Content.ReadAsStringAsync().Result;
                     inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);

                 }
                 else
                 {
                     Console.WriteLine("Erreur calling web api");
                 }

             }
            ////////



            if (inventaire.numinv != null)
            {
                using (var client = new HttpClient())
                {
                    
                    var json = JsonConvert.SerializeObject(inventaire);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(baseURL +"Inventaire");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.PostAsync("Inventaire",data);
                    if (getData.IsSuccessStatusCode)
                    {
                        //var jsonReponse = await getData.Content.ReadAsStringAsync();
                        //ViewData.Model = (List<Inventaire>)TempData["lstInventaires"];
                        return RedirectToAction("InventairesNonClotures") ;

                    }
                    else 
                    {
                        ViewBag.ErrorMessage = "Impossible de créer l'inventaire." +
                            "Veuiller Vérifier s'il y a déja un inventaire non cloturé pour le depot selectionné";
                        return View("ErreurCreation"); 
                    }
                }
                
            }
            string nouveauIndex; 
            if (inventaires.Count > 0)
            {
                Inventaire dernierInventaire = inventaires.OrderBy(f => f.numinv).LastOrDefault();
                 nouveauIndex = "0" + (Convert.ToInt32(dernierInventaire.numinv) + 1).ToString();
                
            }
            else
            {
                nouveauIndex = "0110000"; 
            }
            Inventaire inv = new Inventaire
            {
                numinv = nouveauIndex,
                nbrcomptage = 1,
                commentaire = "Inventaire Physique",
                dateinv = DateTime.Now,
                cloture = "0"
                
            };

            return View(inv); 
        }
        public async Task<ActionResult> SelectionnerArticles(string id, Inventaire model)
        {
            if (model.numinv != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/SelectionnerArticles?id="+id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync( client.BaseAddress,content).ConfigureAwait(false);

                    
                        
                        return RedirectToAction("Index");
                    
                   
                }
                
            }
            /////////
            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }
            ////////
            Inventaire inventaire = new Inventaire(); 
            foreach(Inventaire inv in inventaires)
            {
                if (inv.numinv == id)
                {
                    inventaire = inv;
                    break; 

                }
            }

            return View(inventaire) ;
        }







        public async Task<ActionResult> SaisirComptagePhysique(string id, Inventaire model)
        {
            if (model.numinv != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/SaisirComptagePhysique?id=" + id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content).ConfigureAwait(false);


                    return RedirectToAction("InventairesNonClotures");

                }

            }
            /////////
            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }
            ////////
            Inventaire inventaire = new Inventaire();
            foreach (Inventaire inv in inventaires)
            {
                if (inv.numinv == id)
                {
                    inventaire = inv;
                    break;

                }
            }

            return View(inventaire);
        }

        public async Task<ActionResult> CloturerInventaire(string id, Inventaire model)
        {
            if (model.numinv != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/CloturerInventaire?id=" + id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content).ConfigureAwait(false) ;

                    
                        return RedirectToAction("Index");
                    
                       
                    
                }

            }
            /////////
            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaires = JsonConvert.DeserializeObject<List<Inventaire>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }
            ////////
            Inventaire inventaire = new Inventaire();
            foreach (Inventaire inv in inventaires)
            {
                if (inv.numinv == id)
                {
                    inventaire = inv;
                    break;

                }
            }

            return View(inventaire);
        }
        public async Task<ActionResult> AfficherLignes(string id)
        {
            Inventaire inventaire = new Inventaire();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/GetInventaireById?id=" + id);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync(client.BaseAddress);
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaire = JsonConvert.DeserializeObject<Inventaire>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewData.Model = inventaire;

            }

            return View();
        }

    }
}