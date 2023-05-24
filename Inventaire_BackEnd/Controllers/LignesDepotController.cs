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
    public class LignesDepotController : ApiController
    {
        private  string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public LignesDepotController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }


        // GET: api/LignesDepot
        [System.Web.Http.Authorize]
        public IQueryable<lignedepot> Getlignedepot()
        {
            return db.lignedepot;
        }

        // GET: api/LignesDepot/5
        [ResponseType(typeof(lignedepot))]
        [System.Web.Http.Authorize]
        public IHttpActionResult Getlignedepot(string codedep, string codeart,string famille)
        {
            lignedepot lignedepot = db.lignedepot

                .Where(f => f.famille == famille)
                .Where(f => f.codeart == codeart)
                .Where(f => f.codedep == codedep)
                .FirstOrDefault(); 
            if (lignedepot == null)
            {
                return NotFound();
            }

            return Ok(lignedepot);
        }

        // PUT: api/LignesDepot/5
        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize]
        public IHttpActionResult Putlignedepot(string id, lignedepot lignedepot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lignedepot.codedep)
            {
                return BadRequest();
            }

            db.Entry(lignedepot).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!lignedepotExists(id))
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

        // POST: api/LignesDepot
        [ResponseType(typeof(lignedepot))]
        [System.Web.Http.Authorize]
        public IHttpActionResult Postlignedepot(lignedepot lignedepot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.lignedepot.Add(lignedepot);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (lignedepotExists(lignedepot.codedep))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = lignedepot.codedep }, lignedepot);
        }

        // DELETE: api/LignesDepot/5
        [ResponseType(typeof(lignedepot))]
        [System.Web.Http.Authorize]
        public IHttpActionResult Deletelignedepot(string id)
        {
            lignedepot lignedepot = db.lignedepot.Find(id);
            if (lignedepot == null)
            {
                return NotFound();
            }

            db.lignedepot.Remove(lignedepot);
            db.SaveChanges();

            return Ok(lignedepot);
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
        private bool lignedepotExists(string id)
        {
            return db.lignedepot.Count(e => e.codedep == id) > 0;
        }
    }
}