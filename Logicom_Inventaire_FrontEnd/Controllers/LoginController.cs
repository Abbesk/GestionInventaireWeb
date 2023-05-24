using Logicom_Inventaire_FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Logicom_Inventaire_FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public async Task<ActionResult> Index(Utilisateur user)
        {
            if (user.codeuser != null && user.motpasse != null)
            {


                // Create HttpClient instance to call the web API
                using (HttpClient client = new HttpClient())
                {
                    // Set the base address of the web API

                    // Set the content of the request body
                    var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    // Send a post request to the authenticate endpoint of the web API
                    HttpResponseMessage response = await client.PostAsync("http://localhost:44328/Api/Utilisateur/Login", content);

                    // If the response is successful, store the token in a cookie and redirect to the home page
                    if (response.IsSuccessStatusCode)
                    {
                        string token = (await response.Content.ReadAsStringAsync()).Trim('"');
                        Session["token"] = token;
                        HttpCookie cookie = new HttpCookie("token", token);
                        Response.Cookies.Add(cookie);

                        // Decode the token to retrieve the user name

                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadJwtToken(token);
                        string Role = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                        string id = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                        string name = jsonToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                        string Societe = jsonToken.Claims.FirstOrDefault(c => c.Type == "website")?.Value;

                        // Store the user name in the ViewBag for use in the navbar
                        Session["code"] = id;
                        Session["name"] = name;
                        Session["societe"] = Societe;
                        return RedirectToAction("ChoisirSociete", "Login");
                        //return RedirectToAction("Index", "Devis");
                    }
                    // If the response is not successful, display an error message
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid username or password";
                        return View(user);
                    }
                }
            }
            return View();
        }
        public async Task<ActionResult> ChoisirSociete(string soc)
        {
            if (soc == null)
            {
                IList<UserSoc> lstUserSoc = new List<UserSoc>();
                using (var client = new HttpClient())
                {
                    string token = (string)Session["token"];

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage getData = await client.GetAsync("http://localhost:44328/Api/SocieteUser/GetusersocParUser?codeuser=" + Session["code"]);
                    if (getData.IsSuccessStatusCode)
                    {
                        string results = getData.Content.ReadAsStringAsync().Result;
                        lstUserSoc = JsonConvert.DeserializeObject<List<UserSoc>>(results);

                    }
                    else
                    {
                        Console.WriteLine("Erreur calling web api");
                    }
                    ViewBag.lstSoc = lstUserSoc;

                }

                ViewData.Model = soc;
                return View();
            }
            Session["SelectedSoc"] = soc;
            using (HttpClient client = new HttpClient())
            {
                string token = (string)Session["token"];


                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(soc), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://localhost:44328/Api/Utilisateur/ChoisirSociete?soc=+" + soc, null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Devis");
                }
                else
                    return View();
            }

        }





        public async Task<ActionResult> Logout()
        {
            // Get the token from the cookie
            var token = Request.Cookies["token"]?.Value;

            // Create an HTTP client
            using (var client = new HttpClient())
            {


                Session["token"] = token;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync("http://localhost:44328/Api/Utilisateur/Logout", null);
                if (response.IsSuccessStatusCode)
                {
                    Response.Cookies.Remove("token");

                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return View();
                }
            }
        }



    }
}