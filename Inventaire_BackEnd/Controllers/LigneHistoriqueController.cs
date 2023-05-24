using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Inventaire_BackEnd.Controllers
{
    public class LigneHistoriqueController : ApiController
    {
        // GET: api/LigneHistorique
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LigneHistorique/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LigneHistorique
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LigneHistorique/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LigneHistorique/5
        public void Delete(int id)
        {
        }
    }
}
