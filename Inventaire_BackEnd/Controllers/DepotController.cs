using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Inventaire_BackEnd.Models;

namespace Inventaire_BackEnd.Controllers
{
    public class DepotController : ApiController
    {
        private somabeEntities db = new somabeEntities();

        // GET: api/Depot
        public async Task<IEnumerable<depot>> Getdepot()
        {
            return await db.depot.Include(f => f.TMPLignesDepot).Include(f => f.LignesDepot).ToListAsync();

        }

        // GET: api/Depot/5
        [ResponseType(typeof(depot))]
        public async Task<IHttpActionResult> GetdepotAsync(string code, string codepv)
        {
            depot depot = await db.depot.Include(f => f.PointVente).Include(f => f.LignesDepot).Include(f => f.TMPLignesDepot).Where(f => f.Code == code).Where(f => f.codepv == codepv).FirstOrDefaultAsync();
            

            if (depot == null)
            {
                return NotFound();
            }

            return Ok(depot);
        }

        // PUT: api/Depot/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdepot(string id, depot depot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != depot.Code)
            {
                return BadRequest();
            }

            db.Entry(depot).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!depotExists(id))
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

        // POST: api/Depot
        [ResponseType(typeof(depot))]
        public IHttpActionResult Postdepot(depot depot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.depot.Add(depot);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (depotExists(depot.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = depot.Code }, depot);
        }

        // DELETE: api/Depot/5
        [ResponseType(typeof(depot))]
        public IHttpActionResult Deletedepot(string id)
        {
            depot depot = db.depot.Find(id);
            if (depot == null)
            {
                return NotFound();
            }

            db.depot.Remove(depot);
            db.SaveChanges();

            return Ok(depot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool depotExists(string id)
        {
            return db.depot.Count(e => e.Code == id) > 0;
        }
    }
}