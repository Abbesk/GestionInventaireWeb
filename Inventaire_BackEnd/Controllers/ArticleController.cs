using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Inventaire_BackEnd.Models;

namespace Inventaire_BackEnd.Controllers
{
    public class ArticleController : ApiController
    {
        private  string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public ArticleController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }

        // GET: api/Article
        [Authorize]
        public IQueryable<article> Getarticle()
        {
            return db.article;
        }

        // GET: api/Article/5
        [Authorize]
        [ResponseType(typeof(article))]
        public IHttpActionResult Getarticle(string code , string fam)
        {
            article article = db.article.Include(f => f.Famille).Where(f => f.Famille.code == fam).Where(f => f.code == code).Where(f => f.fam == fam).FirstOrDefault();
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Article/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putarticle(string id, article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.code)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!articleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Article
        [Authorize]
        [ResponseType(typeof(article))]
        public IHttpActionResult Postarticle(article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.article.Add(article);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (articleExists(article.code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = article.code }, article);
        }

        // DELETE: api/Article/5
        [Authorize]
        [ResponseType(typeof(article))]
        public IHttpActionResult Deletearticle(string id)
        {
            article article = db.article.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.article.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize]
        private bool articleExists(string id)
        {
            return db.article.Count(e => e.code == id) > 0;
        }
    }
}