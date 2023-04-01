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
    public class tmpLignesDepotController : ApiController
    {
        private somabeEntities db = new somabeEntities();

        // GET: api/tmpLignesDepot
        public IQueryable<tmplignedepot> Gettmplignedepot()
        {
            return db.tmplignedepot;
        }

        // GET: api/tmpLignesDepot/5
        [ResponseType(typeof(tmplignedepot))]
        public IHttpActionResult Gettmplignedepot(string id)
        {
            tmplignedepot tmplignedepot = db.tmplignedepot.Find(id);
            if (tmplignedepot == null)
            {
                return NotFound();
            }

            return Ok(tmplignedepot);
        }

        // PUT: api/tmpLignesDepot/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttmplignedepot(string id, tmplignedepot tmplignedepot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tmplignedepot.famille)
            {
                return BadRequest();
            }

            db.Entry(tmplignedepot).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tmplignedepotExists(id))
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

        // POST: api/tmpLignesDepot
        [ResponseType(typeof(tmplignedepot))]
        public IHttpActionResult Posttmplignedepot(tmplignedepot tmplignedepot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tmplignedepot.Add(tmplignedepot);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tmplignedepotExists(tmplignedepot.famille))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tmplignedepot.famille }, tmplignedepot);
        }

        // DELETE: api/tmpLignesDepot/5
        [ResponseType(typeof(tmplignedepot))]
        public IHttpActionResult Deletetmplignedepot(string id)
        {
            tmplignedepot tmplignedepot = db.tmplignedepot.Find(id);
            if (tmplignedepot == null)
            {
                return NotFound();
            }

            db.tmplignedepot.Remove(tmplignedepot);
            db.SaveChanges();

            return Ok(tmplignedepot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tmplignedepotExists(string id)
        {
            return db.tmplignedepot.Count(e => e.famille == id) > 0;
        }
    }
}