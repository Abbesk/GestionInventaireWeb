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
using System.Web.Mvc;
using Inventaire_BackEnd.Models;


namespace Inventaire_BackEnd.Controllers
{
    
    public class InventaireController : ApiController
    {
        private somabeEntities db = new somabeEntities();

        // GET: api/Inventaire
        public async Task <IEnumerable<invphysique>> GetInventaires()
        {
            return await db.invphysique.Include(f => f.Depot).Include(f => f.PointVente).ToListAsync();
                                                    
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/InventairesNonClotures")]
        public async Task<IEnumerable<invphysique>> InventairesNonClotures()
        {
            return await db.invphysique.Include(f => f.Depot).Include(f => f.PointVente).Where(f=>f.cloture=="0").ToListAsync();

        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/DepotParCodePV")]
        public async Task<IEnumerable<depot>> DepotParCodePV(string code)
        {
            Console.WriteLine("Code parameter: " + code);
            var depots = await db.depot.Include(f => f.TMPLignesDepot).Include(f => f.LignesDepot).ToListAsync();
            Console.WriteLine("Total number of depots: " + depots.Count);
            if (code != null)
            {
                depots = depots.Where(f => f.codepv == code).ToList();
                Console.WriteLine("Number of depots with matching codepv: " + depots.Count);
            }
            return depots;
        }
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

        // GET: api/Inventaire/5

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Inventaire/GetInventaireById/")]
        [ResponseType(typeof(invphysique))]
        public async Task<IHttpActionResult> GetInventaireById(string id)
        {
            //invphysique invphysique = db.invphysique
            //.Include(f => f.PointVente).Include(f => f.Depot).ThenInclude(b=> b.LignesDepot).Where(f => f.numinv == id)
            // .FirstOrDefault();

            invphysique invphysique =  await db.invphysique.SingleAsync(f => f.numinv == id);
            db.Entry(invphysique.Depot).Collection(f => f.LignesDepot)
                                       .Query()
                                       .Include(f => f.Article)
                                      
                                       .Load();
            db.Entry(invphysique.PointVente).Collection(f => f.Depots)
                                       .Query()
                                       .Include(f => f.LignesDepot)
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
            if (invphysique.Depot.TMPLignesDepot != null && ExistingInventaire.cloture == "0")
            {
                for (int i = 0; i < ExistingInventaire.Depot.TMPLignesDepot.Count; i++)
                {     
                        ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).qteInventaire = invphysique.Depot.TMPLignesDepot.ElementAt(i).qteInventaire;
                        ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i).commentaire = invphysique.Depot.TMPLignesDepot.ElementAt(i).commentaire;
                        db.Entry(ExistingInventaire.Depot.TMPLignesDepot.ElementAt(i)).State = EntityState.Modified;
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

            return CreatedAtRoute("DefaultApi", new { id = invphysique.numinv }, invphysique);
        }
        [ResponseType(typeof(void))]
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

            return CreatedAtRoute("DefaultApi", new { id = invphysique.numinv }, invphysique);
        }



        // POST: api/Inventaire
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
                invphysique.libpv = db.pointvente.Find(invphysique.codepv).Libelle;
                invphysique.Depot = invphysique.PointVente.Depots.Where(f => f.Code == invphysique.codedep).FirstOrDefault();
                invphysique.libdep = invphysique.Depot.Libelle;
            }
            bool possibilite = true; 
            List<invphysique> lstInventaires = db.invphysique.ToList();
            if (lstInventaires != null)
            {
                foreach(invphysique inv in lstInventaires )
                {
                    if (inv.cloture=="0" && inv.codedep ==invphysique.codedep && invphysique.codepv == inv.codepv)
                    {
                    possibilite = false;
                    break;
                    }
                 }
            }
            if (!possibilite)
            {
                if (!possibilite)
                {
                    return StatusCode((HttpStatusCode)(int)HttpStatusCode.Forbidden);
                }
            }


            
                
                db.invphysique.Add(invphysique);
            
            

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

        // DELETE: api/Inventaire/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool invphysiqueExists(string id)
        {
            return db.invphysique.Count(e => e.numinv == id) > 0;
        }
        [ResponseType(typeof(void))]
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

            return CreatedAtRoute("DefaultApi", new { id = invphysique.numinv }, invphysique);
        }

    }
}