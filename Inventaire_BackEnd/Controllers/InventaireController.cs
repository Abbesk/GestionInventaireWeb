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
using System.Web.Mvc;
using Inventaire_BackEnd.Models;
using Newtonsoft.Json;

namespace Inventaire_BackEnd.Controllers
{
    
    public class InventaireController : ApiController
    {
        private  string societyName = (string)HttpContext.Current.Cache["SelectedSoc"];
        private string connectionString;
        private SocieteEntities db;

        public InventaireController()
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["SocieteEntities"].ConnectionString, societyName);
            db = new SocieteEntities(connectionString);
        }


        
        [System.Web.Http.Authorize]
        public async Task <IEnumerable<invphysique>> GetInventaires()
        {
            
            return await db.invphysique.Include(f => f.Depot).Include(f => f.PointVente).ToListAsync();
                                                    
        }
            [System.Web.Http.Authorize]
            [System.Web.Http.HttpGet]
            [System.Web.Http.Route("api/Inventaire/InventairesParDate")]
        public async Task<IEnumerable<invphysique>> InventairesParDate(int date)
        {

            return await db.invphysique.Include(f => f.Depot).Include(f => f.PointVente).Where(f => f.dateinv.Value.Year==date).ToListAsync();

        }
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/InventairesNonClotures")]
        public async Task<IEnumerable<invphysique>> InventairesNonClotures()
        {
            
            return await db.invphysique.Where(f=>f.cloture=="0").ToListAsync();

        }
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/NbOuverts")]
        public async Task<int> NombreInventairesOuverts()
        {
            return await db.invphysique.Include(f => f.Depot)
                                        .Include(f => f.PointVente)
                                        .Where(f => f.cloture == "0")
                                        .CountAsync();
        }
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/DepotParCodePV")]
        public async Task<IEnumerable<depot>> DepotParCodePV(string code)
        {
            
            var depots = await db.depot.Include(f => f.TMPLignesDepot).Include(f => f.LignesDepot).ToListAsync();
            
            if (code != null)
            {
                depots = depots.Where(f => f.codepv == code).ToList();
                
            }
            return depots;
        }
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/NouveauIndex")]
        public Task<string> NouveauIndex()
        {
            
           
            if (db.invphysique.Count() == 0)
                            return Task.FromResult("0110000");
                        else
                        {
                            List<invphysique> inventaires = db.invphysique.ToList(); 
                
                
                
                                invphysique dernierInventaire = inventaires.OrderBy(f => f.numinv).LastOrDefault();
                                return Task.FromResult(("0" + (Convert.ToInt32(dernierInventaire.numinv) + 1).ToString()));

                
                        }

            
            
            
        }

      
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/GetInventaireById/")]
        [ResponseType(typeof(invphysique))]
        public async Task<IHttpActionResult> GetInventaireById(string id)
        {
            invphysique invphysique =  await db.invphysique.SingleAsync(f => f.numinv == id);
            db.Entry(invphysique.Depot).Collection(f => f.LignesDepot)
                                       .Query()
                                       .Include(f => f.Article)
                                      
                                       .Load();
            
            db.Entry(invphysique.Depot).Collection(f => f.TMPLignesDepot)
                                       .Query()
                                       .Where(tmp => tmp.numinv.Equals(invphysique.numinv) )
                                       .Load();
            db.Entry(invphysique).Collection(f => f.LignesInventaire)
                                       .Query()
                                       .Where(tmp => tmp.NumInv.Equals(invphysique.numinv))
                                       .Load();






            if (invphysique == null)
            {
                return NotFound();
            }

            return Ok(invphysique);
        }

        // PUT: api/Inventaire/5
        [System.Web.Http.Authorize]
        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInventaire(string id, invphysique invphysique)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invphysique.numinv)
            {
                return BadRequest();
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!invphysiqueExists(id))
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

        // --------------------------------Saisir Comptage physique--------------------------------------------//


        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/Inventaire/SaisirComptagePhysique/")]
        public IHttpActionResult SaisirComptagePhysique(string id, invphysique invphysique)
        {
            
            
            
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invphysique.numinv)
            {
                return BadRequest(); 
            }
            invphysique ExistingInventaire = db.invphysique.Find(id);
            ExistingInventaire.DATEDMAJ = DateTime.Now;
            //Saisir Comptage physique
            ehist_erp h = new ehist_erp()
            {
                NumMAJ = getNvIndexHis(),
                datemaj = DateTime.Now,
                heuremaj = DateTime.Now.ToString("HH:mm:ss"),
                codeapp = "01",
                libmaj = "Comptage",
                codeutil = (string)HttpContext.Current.Cache["codeuser"],
                nomutil = (string)HttpContext.Current.Cache["nomuser"],
                typemvt = "Inventaire",
                codemvt = invphysique.numinv,
                datemvt=invphysique.dateinv
                
            };
            db.ehist_erp.Add(h);
            if (invphysique.Depot.TMPLignesDepot != null && ExistingInventaire.cloture == "0")
            {
                for (int i = 0; i < ExistingInventaire.Depot.TMPLignesDepot.Count; i++)
                {     
                        ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).qteInventaire = invphysique.Depot.TMPLignesDepot.ElementAt(i).qteInventaire;
                        ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).commentaire = invphysique.Depot.TMPLignesDepot.ElementAt(i).commentaire;
                        db.Entry(ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i)).State = EntityState.Modified;
                    lhist_erp lh = new lhist_erp()
                    {
                        nummaj = h.NumMAJ,
                        CODEMVT = ExistingInventaire.numinv,
                        typmvt = h.typemvt,
                        CODEDEP = ExistingInventaire.codedep,
                        LIBDEP = ExistingInventaire.libdep,
                        CODEART = ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).codeart,
                        DESART= ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).desart,
                        FAMILLE= ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).famille,
                        QTEART= (float?)ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).qteInventaire,
                        codepv=ExistingInventaire.codepv,
                        libpv=ExistingInventaire.libpv,
                        
                    };
                    db.lhist_erp.Add(lh); 
                }
            }
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!invphysiqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode((HttpStatusCode)(int)HttpStatusCode.OK);
        }
        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/Inventaire/CloturerInventaire/")]
        public IHttpActionResult CloturerInventaire(string id, invphysique invphysique)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invphysique.numinv)
            {
                return BadRequest();
            }
            invphysique ExistingInventaire = db.invphysique.Find(id);
            int f = 0;
            //Cloturer l'inventaire
            if (invphysique.Depot.TMPLignesDepot != null && ExistingInventaire.cloture == "0")

            {
                foreach (tmplignedepot tmplignedepot in invphysique.Depot.TMPLignesDepot)
                {
                    linv linv = new linv()
                    {
                        NumInv = ExistingInventaire.numinv,
                        dateInv = ExistingInventaire.dateinv,
                        codeart = tmplignedepot.codeart,
                        desart = tmplignedepot.desart,
                        qtes = tmplignedepot.qteart,
                        stockinv = tmplignedepot.qteInventaire,
                        ecartinv = tmplignedepot.qteart - tmplignedepot.qteInventaire,
                        PUART = tmplignedepot.puht,

                        famille = tmplignedepot.famille,
                        libellefourn = tmplignedepot.libellefourn,

                        nordre = tmplignedepot.nordre,
                        lieustock = tmplignedepot.lieustock,
                        codedep = ExistingInventaire.codedep,
                        libdep = ExistingInventaire.libdep,
                        codepv = ExistingInventaire.codepv,
                        libpv = ExistingInventaire.libpv,
                        eecart = 0,
                        secart = 0,
                        nligne = f

                    };
                    f++;
                    
                   
                    db.linv.Add(linv);
                    db.SaveChangesAsync();
                    ExistingInventaire.cloture = "1";
                    ExistingInventaire.datecloture = DateTime.Now; 
                }
            }
            foreach (lignedepot ld in db.lignedepot.ToList())
            {
                foreach(tmplignedepot tmp in db.tmplignedepot.ToList())
                {
                    if(ld.codedep==tmp.codedep && ld.codepv==tmp.codepv && tmp.codeart==ld.codeart && ld.famille == tmp.famille)
                    {
                        ld.qteart = tmp.qteInventaire;
                    }
                }
                if (ld.codedep == ExistingInventaire.codedep && ld.codepv == ExistingInventaire.codepv && ld.isSelected == 1)
                {
                    ld.isSelected = 0;

                    db.Entry(ld).State = EntityState.Modified;
                }
            }
            foreach (tmplignedepot tmp in db.tmplignedepot.ToList())
            {
                if(tmp.numinv==ExistingInventaire.numinv)
                {
                    db.tmplignedepot.Remove(tmp);
                }
            }
            ehist_erp h = new ehist_erp()
            {
                NumMAJ = getNvIndexHis(),
                datemaj = DateTime.Now,
                heuremaj = DateTime.Now.ToString("HH:mm:ss"),
                codeapp = "01",
                libmaj = "Cloture",
                codeutil = (string)HttpContext.Current.Cache["codeuser"],
                nomutil = (string)HttpContext.Current.Cache["nomuser"],
                typemvt = "Inventaire",
                codemvt = invphysique.numinv,
                datemvt = invphysique.dateinv
            };
            db.ehist_erp.Add(h);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!invphysiqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode((HttpStatusCode)(int)HttpStatusCode.OK);
        }



        // POST: api/Inventaire
        [System.Web.Http.Authorize]
        [ResponseType(typeof(invphysique))]
        public IHttpActionResult Create( invphysique invphysique)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
           
            //Affectation du point de vente selon le code point de vente
            if (invphysique.codepv != null)
                invphysique.PointVente = db.pointvente.Find(invphysique.codepv);
            if (invphysique.PointVente != null)
            {
                invphysique.libpv =invphysique.PointVente.Libelle;
                invphysique.Depot = db.depot.FirstOrDefault(d => d.Code == invphysique.codedep && d.codepv == invphysique.codepv);

                invphysique.libdep = invphysique.Depot.Libelle;
            }

            bool possibilite = !db.invphysique
                .Where(inv => inv.cloture == "0" && inv.codedep == invphysique.codedep && invphysique.codepv == inv.codepv)
                .Any();
           
                if (!possibilite)
                {
                    return StatusCode((HttpStatusCode)(int)HttpStatusCode.Forbidden);
                }
            


            
                
                db.invphysique.Add(invphysique);
            ehist_erp h = new ehist_erp()
            {
                NumMAJ = getNvIndexHis(),
                datemaj = DateTime.Now,
                heuremaj = DateTime.Now.ToString("HH:mm:ss"),
                codeapp = "01",
                libmaj = "Ajout",
                codeutil = (string)HttpContext.Current.Cache["codeuser"],
                nomutil = (string)HttpContext.Current.Cache["nomuser"],
                typemvt = "Inventaire",
                codemvt = invphysique.numinv,
                datemvt = invphysique.dateinv
            };
            db.ehist_erp.Add(h);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (invphysiqueExists(invphysique.numinv) )
                {
                    return Conflict();
                }
                else if(invphysique.Depot.TMPLignesDepot.Count > 1)
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = invphysique.numinv }, invphysique);
        }
        [System.Web.Http.Route("api/")]
        public string getNvIndexHis()
        {
            if (db.ehist_erp.Count() == 0)

                return (DateTime.Now.ToString("yy") + "00001");
            else
            {
                List<ehist_erp> dfps = db.ehist_erp.ToList();

                string dernierAnnée = dfps.LastOrDefault().NumMAJ.Substring(0, 2);
                if (dernierAnnée == DateTime.Now.ToString("yy"))
                {
                    string dernierNumero = (Convert.ToInt32(dfps.LastOrDefault().NumMAJ.Substring(2, 5)) + 1).ToString();
                    if (dernierNumero.Length == 1)
                    {
                        return (DateTime.Now.ToString("yy") + "0000" + dernierNumero);
                    }
                    else
                    {
                        if (dernierNumero.Length == 2)
                        {
                            return (DateTime.Now.ToString("yy") + "000" + dernierNumero);
                        }
                        else
                        {
                            if (dernierNumero.Length == 3)
                            {
                                return (DateTime.Now.ToString("yy") + "00" + dernierNumero);
                            }
                            else
                            {
                                if (dernierNumero.Length == 4)
                                {
                                    return (DateTime.Now.ToString("yy") + "0" + dernierNumero);
                                }
                                else
                                {
                                    return (DateTime.Now.ToString("yy") + dernierNumero);
                                }
                            }
                        }
                    }

                }
                else
                {
                    return (DateTime.Now.ToString("yy") + "00001");
                }

            }
        }

        
        
       
        [System.Web.Http.Authorize]
        [ResponseType(typeof(invphysique))]
        public IHttpActionResult DeleteInventaire(string id)
        {
            invphysique invphysique = db.invphysique.Find(id);
            if (invphysique == null)
            {
                return NotFound();
            }

            db.invphysique.Remove(invphysique);
            db.SaveChanges();

            return Ok(invphysique);
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
        private bool invphysiqueExists(string id)
        {
            return db.invphysique.Count(e => e.numinv == id) > 0;
        }
        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/Inventaire/SelectionnerArticles")]
        public IHttpActionResult SelectionnerArticles(string id, invphysique invphysique)
        {

          


            invphysique ExistingInventaire = db.invphysique.Find(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invphysique.numinv)
            {
                return BadRequest();
            }
            if (invphysique.Depot.LignesDepot != null)
            {
                
                ehist_erp h = new ehist_erp()
                {
                    NumMAJ = getNvIndexHis(),
                    datemaj = DateTime.Now,
                    heuremaj = DateTime.Now.ToString("HH:mm:ss"),
                    codeapp = "01",
                    libmaj = "Sélection",
                    codeutil = (string)HttpContext.Current.Cache["codeuser"],
                    nomutil = (string)HttpContext.Current.Cache["nomuser"],
                    typemvt = "Inventaire",
                    codemvt = invphysique.numinv,
                    datemvt = invphysique.dateinv
                };
                db.ehist_erp.Add(h);
                db.SaveChangesAsync(); 
                // ExistingInventaire.DATEDMAJ = DateTime.Now;
                for (int i = 0; i < ExistingInventaire.Depot.LignesDepot.Count; i++)
                {
                ExistingInventaire.Depot.LignesDepot.ElementAt(i).isSelected = invphysique.Depot.LignesDepot.ToList()[i].isSelected;
                //ExistingInventaire.Depot.LignesDepot.ElementAt(1).isSelected = invphysique.Depot.LignesDepot.ToList()[1].isSelected;
                db.Entry(ExistingInventaire.Depot.LignesDepot.ElementAt(i)).State = EntityState.Modified;
                }
                ExistingInventaire.DATEDMAJ = DateTime.Now; 
                db.invphysique.Attach(ExistingInventaire);

                int j = 0;
                bool trouve = false;
                //Creation d'un tmp ligne depot pour chaque article (ligne depot) selectionne
                foreach (lignedepot ldp in ExistingInventaire.Depot.LignesDepot)
                {
                    foreach (tmplignedepot tmplignedepot in db.tmplignedepot.ToList())
                    {
                        if (ldp.isSelected == 0 && tmplignedepot.codeart == ldp.codeart && ldp.famille == tmplignedepot.famille && ldp.codedep == tmplignedepot.codedep && ldp.codepv == tmplignedepot.codepv && tmplignedepot.numinv == ExistingInventaire.numinv)
                        {
                            db.tmplignedepot.Remove(tmplignedepot);
                            db.SaveChangesAsync();
                        }


                    }

                    if (ldp.isSelected == 1)
                    {
                       
                        tmplignedepot tmp = new tmplignedepot()
                        {
                            nordre = j.ToString(),
                            codeart = ldp.codeart,
                            codedep = ldp.codedep,
                            famille = ldp.famille,
                            desart = ldp.desart,
                            lieustock = ldp.lieustock,
                            codepv = ldp.codepv,
                            commentaire = "",
                            Datderninv=ExistingInventaire.dateinv, 
                            
                            fourn = ldp.Article.fourn,
                            libellefourn = ldp.libfourn,
                            libelle = ldp.libelle,
                            puht = ldp.Article.puht,
                            qteart = ldp.qteart,
                            qteInventaire = 0,

                            numinv = ExistingInventaire.numinv
                        };
                        lhist_erp lh = new lhist_erp()
                        {
                            nummaj = h.NumMAJ,
                            CODEMVT = ExistingInventaire.numinv,
                            typmvt = h.typemvt,
                            CODEDEP = ExistingInventaire.codedep,
                            LIBDEP = ExistingInventaire.libdep,
                            CODEART = ldp.codeart,
                            DESART = ldp.desart,
                            FAMILLE = ldp.famille,
                            QTEART = (float?)ldp.qteart,
                            codepv = ExistingInventaire.codepv,
                            libpv = ExistingInventaire.libpv,

                        };
                        db.lhist_erp.Add(lh);

                        foreach (tmplignedepot tmplignedepot in db.tmplignedepot.ToList())
                        {

                            if (tmplignedepot.codeart == tmp.codeart && tmplignedepot.famille == tmp.famille && tmplignedepot.codedep == tmp.codedep && tmp.codepv == tmplignedepot.codepv)
                            {
                                trouve = true;
                                break;


                            }
                            else
                            {
                                trouve = false;
                            }
                        }
                       
                        
                        if (!trouve)
                        {
                            
                            db.tmplignedepot.Add(tmp);
                            db.SaveChangesAsync();
                        }
                    }




                }
            }
            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!invphysiqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode((HttpStatusCode)(int)HttpStatusCode.OK);
        }

    }
}