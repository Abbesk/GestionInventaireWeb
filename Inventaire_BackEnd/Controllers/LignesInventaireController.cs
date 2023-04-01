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
    public class LignesInventaireController : ApiController
    {
        private somabeEntities db = new somabeEntities();

        // GET: api/LignesInventaire
        public IQueryable<linv> Getlinv()
        {
            return db.linv.Include(f => f.Inventaire).Include(f => f.Inventaire.Depot);
        }

        // GET: api/LignesInventaire/5
        [ResponseType(typeof(linv))]
        public IHttpActionResult Getlinv(string id)
        {
            linv linv = db.linv.Find(id);
            if (linv == null)
            {
                return NotFound();
            }

            return Ok(linv);
        }

        // PUT: api/LignesInventaire/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlinv(string id, linv linv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != linv.NumInv)
            {
                return BadRequest();
            }

            db.Entry(linv).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!linvExists(id))
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

        // POST: api/LignesInventaire
        [ResponseType(typeof(linv))]
        public IHttpActionResult Postlinv(linv linv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.linv.Add(linv);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (linvExists(linv.NumInv))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = linv.NumInv }, linv);
        }

        // DELETE: api/LignesInventaire/5
        [ResponseType(typeof(linv))]
        public IHttpActionResult Deletelinv(string id)
        {
            linv linv = db.linv.Find(id);
            if (linv == null)
            {
                return NotFound();
            }

            db.linv.Remove(linv);
            db.SaveChanges();

            return Ok(linv);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool linvExists(string id)
        {
            return db.linv.Count(e => e.NumInv == id) > 0;
        }
    }
}