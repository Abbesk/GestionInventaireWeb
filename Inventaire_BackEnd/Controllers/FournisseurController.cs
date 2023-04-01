using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Inventaire_BackEnd.Models;

namespace Inventaire_BackEnd.Controllers
{
    public class FournisseurController : ApiController
    {
        private somabeEntities db = new somabeEntities();

        // GET: api/Fournisseur
        public IQueryable<fournisseur> Getfournisseur()
        {
            return db.fournisseur;
        }

        // GET: api/Fournisseur/5
        [ResponseType(typeof(fournisseur))]
        public IHttpActionResult Getfournisseur(string id)
        {
            fournisseur fournisseur = db.fournisseur.Find(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            return Ok(fournisseur);
        }

        // PUT: api/Fournisseur/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putfournisseur(string id, fournisseur fournisseur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fournisseur.code)
            {
                return BadRequest();
            }

            db.Entry(fournisseur).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fournisseurExists(id))
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

        // POST: api/Fournisseur
        [ResponseType(typeof(fournisseur))]
        public IHttpActionResult Postfournisseur(fournisseur fournisseur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.fournisseur.Add(fournisseur);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (fournisseurExists(fournisseur.code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fournisseur.code }, fournisseur);
        }

        // DELETE: api/Fournisseur/5
        [ResponseType(typeof(fournisseur))]
        public IHttpActionResult Deletefournisseur(string id)
        {
            fournisseur fournisseur = db.fournisseur.Find(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            db.fournisseur.Remove(fournisseur);
            db.SaveChanges();

            return Ok(fournisseur);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool fournisseurExists(string id)
        {
            return db.fournisseur.Count(e => e.code == id) > 0;
        }
    }
}