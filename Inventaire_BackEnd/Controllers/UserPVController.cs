using Inventaire_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Inventaire_BackEnd.Controllers
{
    public class UserPVController : ApiController
    {
        private string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public UserPVController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }


        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<utilisateurpv>> GetUtilisateurpvs(string codeuser)
        {
            return await db.utilisateurpv.Where(pv=>pv.codeuser==codeuser).ToListAsync();

        }
    }
}
