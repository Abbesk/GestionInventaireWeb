using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Inventaire_BackEnd.Models;
using Microsoft.IdentityModel.Tokens;

namespace Inventaire_BackEnd.Controllers
{
    public class UtilisateurController : ApiController
    {
        
        private usererpEntities db = new usererpEntities();
        

        [Authorize]
        [HttpGet]
        [Route("Api/Utilisateur/getRole")]
        public IHttpActionResult getRole()
        {
            return Ok((string)HttpContext.Current.Cache["role"]);
        }
        [HttpGet]
        public async Task<IEnumerable<utilisateur>> GetUsers()
        {

            return await db.utilisateur.ToListAsync()  ;

        }
        [HttpPost]
        public IHttpActionResult Login(utilisateur utilisateur)
        {
            using (var db = new usererpEntities())
            {
                utilisateur user = db.utilisateur.Find(utilisateur.codeuser);
                if (user == null || user.motpasse != utilisateur.motpasse)
                {
                    return Unauthorized();
                }
                else
                {
                    HttpContext.Current.Cache.Insert("role", user.type);
                    HttpContext.Current.Cache.Insert("codeuser", user.codeuser);
                    HttpContext.Current.Cache.Insert("nomuser", user.nom);
                    var session = Guid.NewGuid().ToString();
                    var expirationTime = DateTime.UtcNow.AddMinutes(600);
                    // Store the session ID in a cookie
                    var cookie = new HttpCookie("SessionID", session);
                    cookie.Expires = expirationTime;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    HttpContext.Current.Cache.Remove("SelectedSoc");
                    // Store the session ID and expiration time in a cache
                    HttpContext.Current.Cache[session] = expirationTime;
                    var token = GenerateToken(user);
                    
                    return Ok(token);
                }

            }

        }
        private string GenerateToken(utilisateur user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_Here"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.codeuser),
        new Claim(JwtRegisteredClaimNames.Name, user.nom),
        new Claim(ClaimTypes.Role, user.type),
        new Claim(JwtRegisteredClaimNames.Website,user.socutil)
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(600),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Api/Utilisateur/Logout")]
        public IHttpActionResult Logout()
        {
            var tokenCookie = HttpContext.Current.Request.Cookies["token"];
            if (tokenCookie != null)
            {
                // Expire the token cookie
                tokenCookie.Expires = DateTime.Now.AddDays(-1);
                tokenCookie.Value = string.Empty;
                tokenCookie.HttpOnly = true;
                tokenCookie.Secure = true;
                HttpContext.Current.Response.Cookies.Add(tokenCookie);
            }

            return Ok();
        }


        [HttpPost]
        [Route("Api/Utilisateur/ChoisirSociete")]
        public IHttpActionResult ChoisirSociete(string soc)
        {

            if (soc != null)
            {
                soc = soc.ToLower();
                HttpContext.Current.Cache.Remove("SelectedSoc");
                HttpContext.Current.Cache.Insert("SelectedSoc", soc);

                return Ok();
            }
            return BadRequest();


        }



        public bool IsValidUser(string username, string password)
        {
            using (var db = new usererpEntities())
            {
                utilisateur user = db.utilisateur.Find(username);
                if (user == null || user.motpasse != password)
                {
                    return false;
                }


                return true;
            }
        }
    }
    
}