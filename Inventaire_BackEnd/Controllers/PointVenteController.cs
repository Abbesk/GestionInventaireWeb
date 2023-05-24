using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Inventaire_BackEnd.Models;

namespace Inventaire_BackEnd.Controllers
{
    public class PointVenteController : ApiController
    {
        private  string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public PointVenteController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }


        // GET: api/PointVente
        [System.Web.Http.Authorize]
        public async Task<IEnumerable<pointvente>> GetInventaires()
        {
            return await db.pointvente.ToListAsync();

        }
        // GET: api/PointVente/5
        [System.Web.Http.Authorize]
        [ResponseType(typeof(pointvente))]
        public IHttpActionResult Getpointvente(string id)
        {
            pointvente pointvente = db.pointvente.Where(f => f.Code == id).FirstOrDefault() ;
            if (pointvente == null)
            {
                return NotFound();
            }

            return Ok(pointvente);
        }

        // PUT: api/PointVente/5
        [System.Web.Http.Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpointvente(string id, pointvente pointvente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pointvente.Code)
            {
                return BadRequest();
            }

            db.Entry(pointvente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pointventeExists(id))
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

        // POST: api/PointVente
        [System.Web.Http.Authorize]
        [ResponseType(typeof(pointvente))]
        public IHttpActionResult Postpointvente(pointvente pointvente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.pointvente.Add(pointvente);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (pointventeExists(pointvente.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pointvente.Code }, pointvente);
        }

        // DELETE: api/PointVente/5
        [System.Web.Http.Authorize]
        [ResponseType(typeof(pointvente))]
        public IHttpActionResult Deletepointvente(string id)
        {
            pointvente pointvente = db.pointvente.Find(id);
            if (pointvente == null)
            {
                return NotFound();
            }

            db.pointvente.Remove(pointvente);
            db.SaveChanges();

            return Ok(pointvente);
        }
        [System.Web.Http.Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [System.Web.Http.Authorize]
        private bool pointventeExists(string id)
        {
            return db.pointvente.Count(e => e.Code == id) > 0;
        }
    }
}