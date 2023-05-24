using Logicom_Inventaire_FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Logicom_Inventaire_FrontEnd.Controllers
{
    public class InventaireController : Controller
    {
        string baseURL = "http://localhost:44328/api/";
        public async Task<ActionResult> LignesHistorique(string  id)
        {
            HistoriqueInventaire historique = new HistoriqueInventaire();
            if ( id!= null && id!="" )
            {
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Historique?id="+id);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    historique = JsonConvert.DeserializeObject<HistoriqueInventaire>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                

            } 
            }
            return View(historique);
        }
        public async Task<ActionResult> HistoriqueInventaire()
        {

            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            /////Charger point de vente 
            IList<HistoriqueInventaire> historiques = new List<HistoriqueInventaire>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Historique" );
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    historiques = JsonConvert.DeserializeObject<List<HistoriqueInventaire>>(results)
                    .OrderByDescending(d => d.datemaj)
                    .ToList();


                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewData.Model = historiques;

            }
            /////Charger point de vente 
            IList<UserPV> pointVentes = new List<UserPV>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("UserPV/GetUtilisateurpvs?codeuser=" + (string)Session["code"]);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    pointVentes = JsonConvert.DeserializeObject<List<UserPV>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewBag.pvs = pointVentes;

            }
            return View(); 
        }
            public async Task<ActionResult> Index()
        {

            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            /////Charger point de vente 
            IList<UserPV> pointVentes = new List<UserPV>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("UserPV/GetUtilisateurpvs?codeuser="+(string)Session["code"]);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    pointVentes = JsonConvert.DeserializeObject<List<UserPV>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewBag.pvs = pointVentes;

            }
            /////Charger la liste des depots 
            IList<Depot> deps = new List<Depot>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Depot/GetDepotsParUser?codeuser="+(string)Session["code"]);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    deps = JsonConvert.DeserializeObject<List<Depot>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewBag.deps = deps;

            }

            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "Inventaire/GetInventaires");
                client.DefaultRequestHeaders.Accept.Clear();
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire");
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
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
            /////Charger point de vente 
            IList<PointVente> pointVentes = new List<PointVente>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("PointVente");
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    pointVentes = JsonConvert.DeserializeObject<List<PointVente>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewBag.pvs = pointVentes;

            }
            /////Charger la liste des depots 
            IList<Depot> deps = new List<Depot>();
            using (var client = new HttpClient())
            {
                string token = (string)Session["token"];
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Depot");
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    deps = JsonConvert.DeserializeObject<List<Depot>>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }
                ViewBag.deps = deps;

            }
            IList<Inventaire> inventaires = new List<Inventaire>();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("Inventaire/InventairesNonClotures");
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
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
            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            if (inventaire.numinv == null)
            {
                IList<UserPV> pvs = new List<UserPV>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.GetAsync("UserPV?codeuser="+(string)Session["code"]);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        pvs = JsonConvert.DeserializeObject<List<UserPV>>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }
                    ViewBag.Code_Point_Vente = new SelectList(pvs, "codepv", "libpv");


                }

                
               string nouveauIndex="";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "Inventaire/NouveauIndex");
                    client.DefaultRequestHeaders.Accept.Clear();
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.GetAsync("");
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        nouveauIndex = JsonConvert.DeserializeObject<string>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }

                }
        
            Inventaire inv = new Inventaire
            {
                numinv = nouveauIndex,
                nbrcomptage = 1,
                commentaire = "Inventaire Physique",
                dateinv = DateTime.Now,
                cloture = "0",
                utilisateur = (string)Session["code"]

            };
                return View(inv);
            }



            else
            {
                using (var client = new HttpClient())
                {

                    var json = JsonConvert.SerializeObject(inventaire);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(baseURL + "Inventaire/Create");
                    client.DefaultRequestHeaders.Accept.Clear();
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.PostAsync("Inventaire", data);
                    if ((int)getData.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }
                    if (getData.IsSuccessStatusCode)
                    {
                        //var jsonReponse = await getData.Content.ReadAsStringAsync();
                        //ViewData.Model = (List<Inventaire>)TempData["lstInventaires"];
                        return RedirectToAction("InventairesNonClotures");

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Impossible de créer l'inventaire." +
                            "Veuiller Vérifier s'il y a déja un inventaire non cloturé pour le depot selectionné";
                        return View("ErreurCreation");
                    }
                }

            }
            

           
        }
        public async Task<ActionResult> SelectionnerArticles(string id, Inventaire model)
        {
            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            if (model.numinv != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/SelectionnerArticles?id=" + id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content).ConfigureAwait(false);
                    if ((int)response.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }



                    return RedirectToAction("Index");


                }

            }
            /////////
            Inventaire inventaire = new Inventaire();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Inventaire/GetInventaireById/?id=" + id);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaire = JsonConvert.DeserializeObject<Inventaire>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }



            return View(inventaire);
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
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content).ConfigureAwait(false);
                    if ((int)response.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }


                    return RedirectToAction("InventairesNonClotures");

                }

            }
            /////////
            Inventaire inventaire = new Inventaire();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Inventaire/GetInventaireById/?id="+id);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaire = JsonConvert.DeserializeObject<Inventaire>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }
            
            

            return View(inventaire);
        }

        public async Task<ActionResult> CloturerInventaire(string id, Inventaire model)
        {
            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            if (model.numinv != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/CloturerInventaire?id=" + id);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string token = (string)Session["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string jsonContent = JsonConvert.SerializeObject(model);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content).ConfigureAwait(false);
                    if ((int)response.StatusCode == 401)
                    {
                        return RedirectToAction("Authentifier", "Login");
                    }


                    return RedirectToAction("Index");



                }

            }
            /////////
            Inventaire inventaire = new Inventaire();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage getData = await client.GetAsync("Inventaire/GetInventaireById/?id=" + id);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    inventaire = JsonConvert.DeserializeObject<Inventaire>(results);

                }
                else
                {
                    Console.WriteLine("Erreur calling web api");
                }

            }



            return View(inventaire);
        }
        public async Task<ActionResult> AfficherLignes(string id)
        {
            if (!((string)Session["role"]).Equals("admin") && !((string)Session["role"]).Equals("Superviseur"))
            {
                return RedirectToAction("Authentifier", "Login");
            }
            Inventaire inventaire = new Inventaire();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:44328/api/Inventaire/GetInventaireById?id=" + id);
                client.DefaultRequestHeaders.Accept.Clear();
                string token = (string)Session["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync(client.BaseAddress);
                if ((int)getData.StatusCode == 401)
                {
                    return RedirectToAction("Authentifier", "Login");
                }
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