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
    public class SocieteUserController : ApiController
    {
        private usererpEntities db = new usererpEntities();

        public IQueryable<usersoc> Getusersoc()
        {

            return db.usersoc;
        }
        [Authorize]
        [Route("api/SocieteUser/GetusersocParUser")]
        [HttpGet]
        public List<usersoc> GetusersocParUser(string codeuser)
        {
            List<usersoc> ListesUserSoc = db.usersoc.ToList();
            List<usersoc> ListeSocieteParUser = new List<usersoc>();
            foreach (usersoc u in ListesUserSoc)
            {
                if (u.CODEUSER == codeuser)
                {
                    ListeSocieteParUser.Add(u);
                }
            }
            return ListeSocieteParUser;
        }

        // GET: api/SocieteUser/5
        [ResponseType(typeof(usersoc))]
        public IHttpActionResult Getusersoc(string id)
        {
            usersoc usersoc = db.usersoc.Find(id);
            if (usersoc == null)
            {
                return NotFound();
            }

            return Ok(usersoc);
        }

        // PUT: api/SocieteUser/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusersoc(string id, usersoc usersoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usersoc.CODEUSER)
            {
                return BadRequest();
            }

            db.Entry(usersoc).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usersocExists(id))
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

        // POST: api/SocieteUser
        [ResponseType(typeof(usersoc))]
        public IHttpActionResult Postusersoc(usersoc usersoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usersoc.Add(usersoc);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (usersocExists(usersoc.CODEUSER))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usersoc.CODEUSER }, usersoc);
        }

        // DELETE: api/SocieteUser/5
        [ResponseType(typeof(usersoc))]
        public IHttpActionResult Deleteusersoc(string id)
        {
            usersoc usersoc = db.usersoc.Find(id);
            if (usersoc == null)
            {
                return NotFound();
            }

            db.usersoc.Remove(usersoc);
            db.SaveChanges();

            return Ok(usersoc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersocExists(string id)
        {
            return db.usersoc.Count(e => e.CODEUSER == id) > 0;
        }
    }
}