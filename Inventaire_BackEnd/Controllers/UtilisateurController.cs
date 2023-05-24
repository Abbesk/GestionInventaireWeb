using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Inventaire_BackEnd.Models;
using Microsoft.IdentityModel.Tokens;

namespace Inventaire_BackEnd.Controllers
{
    public class UtilisateurController : ApiController
    {
        private readonly usererpEntities db = new usererpEntities();

        [System.Web.Http.HttpPost]
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
                    var session = Guid.NewGuid().ToString();
                    var expirationTime = DateTime.UtcNow.AddMinutes(30);

                    // Store the session ID in a cookie
                    var cookie = new HttpCookie("SessionID", session);
                    cookie.Expires = expirationTime;
                    HttpContext.Current.Response.Cookies.Add(cookie);

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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Api/Utilisateur/Logout")]
        public IHttpActionResult Logout()
        {
            var sessionID = HttpContext.Current.Request.Cookies["SessionID"]?.Value;
            if (sessionID != null)
            {
                // Remove the session ID from the cache
                HttpContext.Current.Cache.Remove(sessionID);

                // Expire the session ID cookie
                var cookie = new HttpCookie("SessionID");
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            return Ok();
        }

        /*public static bool ValidateToken(string token)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_Here")),
                        ValidateIssuer = true,
                        ValidIssuer = "Your_Issuer_Name",
                        ValidateAudience = true,
                        ValidAudience = "Your_Audience_Name",
                        ValidateLifetime = true
                    };

                    try
                    {
                        SecurityToken validatedToken;
                        var claims = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }*/


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Api/Utilisateur/ChoisirSociete")]
        public IHttpActionResult ChoisirSociete(string soc)
        {
            string s = "";
            if (soc != null)
            {
                soc = soc.ToLower();
                HttpContext.Current.Cache.Insert("SelectedSoc", soc);
                s = (string)HttpContext.Current.Cache["SelectedSoc"];
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