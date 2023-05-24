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
    public class DepotController : ApiController
    {
        private  string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public DepotController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.Route("api/Depot/GetAllDeps")]
        public async Task<IEnumerable<depot>> GetAllDeps()
        {
            return await db.depot.ToListAsync();
        }

        [Authorize]
        [HttpGet]
        [System.Web.Http.Route("api/Depot/GetDepotsParUser")]
        public async Task<IEnumerable<depot>> GetDepotsParUser(string codeuser)
        {


            List<utilisateurpv> UsersPV = db.utilisateurpv.Where(pv => pv.codeuser == codeuser).ToList();
            var codePvList = UsersPV.Select(pv => pv.codepv).ToList(); 

            return await db.depot.Where(dep => codePvList.Contains(dep.codepv)).ToListAsync();

        }


        // GET: api/Depot/5
        [Authorize]
        [ResponseType(typeof(depot))]
        [System.Web.Http.Route("api/Depot/GetdepotAsync")]
        public async Task<IHttpActionResult> GetdepotAsync(string code, string codepv)
        {
            depot depot = await db.depot.Include(f => f.LignesDepot).Include(f => f.TMPLignesDepot).Where(f => f.Code == code).Where(f => f.codepv == codepv).FirstOrDefaultAsync();
            

            if (depot == null)
            {
                return NotFound();
            }

            return Ok(depot);
        }
        
        // PUT: api/Depot/5
        [ResponseType(typeof(void))]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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