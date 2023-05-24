using Inventaire_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Inventaire_BackEnd.Controllers
{
    public class HistoriqueController : ApiController { 

     private string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
    private string connectionString;
    private SocieteEntities db;

    public HistoriqueController()
    {
        connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
        db = new SocieteEntities(connectionString);
    }


    [HttpGet]
    [Authorize]
        public  async Task<IEnumerable<ehist_erp>> GetHistoriques()
        {

            return await db.ehist_erp.ToListAsync();

        }


        [Authorize]
    [ResponseType(typeof(ehist_erp))]
        public async Task<IHttpActionResult> GetHistorique(string id)
        {
            
            ehist_erp h = await db.ehist_erp.SingleAsync(f => f.NumMAJ == id); 
            db.Entry(h).Collection(f => f.Lignes)
                                        .Query()
                                        .Where(l => l.nummaj.Equals(h.NumMAJ))
                                        .Load();
            if (h == null)
        {
            return NotFound();
        }

        return Ok(h);
    }

 
    [Authorize]
    [ResponseType(typeof(void))]
    public IHttpActionResult PutHistorique(string id, ehist_erp ehist_erp)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != ehist_erp.NumMAJ)
        {
            return BadRequest();
        }

        db.Entry(ehist_erp).State = EntityState.Modified;

        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!!HistoriqueExists(id))
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

  
    [Authorize]
    [ResponseType(typeof(famille))]
    public IHttpActionResult PostHistorique(ehist_erp ehist_erp)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        db.ehist_erp.Add(ehist_erp);

        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            if (HistoriqueExists(ehist_erp.NumMAJ))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtRoute("DefaultApi", new { id = ehist_erp.NumMAJ }, ehist_erp);
    }

    // DELETE: api/Famille/5
    [Authorize]
    [ResponseType(typeof(ehist_erp))]
    public IHttpActionResult DeleteHistorique(string id)
    {
            ehist_erp ehist_erp = db.ehist_erp.Find(id);
        if (ehist_erp == null)
        {
            return NotFound();
        }

        db.ehist_erp.Remove(ehist_erp);
        db.SaveChanges();

        return Ok(ehist_erp);
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
    private bool HistoriqueExists(string id)
    {
        return db.ehist_erp.Count(e => e.NumMAJ == id) > 0;
    }
}
}