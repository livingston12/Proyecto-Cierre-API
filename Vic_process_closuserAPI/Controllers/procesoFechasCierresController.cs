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
using Vic_process_closuserAPI.Models;

namespace Vic_process_closuserAPI.Controllers
{
    [RoutePrefix("procesoCierres")]
    public class procesoFechasCierresController : ApiController
    {
        private BUCDWEntities db = new BUCDWEntities();

        // GET: api/procesoFechasCierres
        [ Route("getall")]
        public IQueryable<procesoFechasCierre> GetprocesoFechasCierres()
        {
            return db.procesoFechasCierres;
        }

        //// GET: api/procesoFechasCierres/5
        //[ResponseType(typeof(procesoFechasCierre))]
        //public async Task<IHttpActionResult> GetprocesoFechasCierre(int id)
        //{
        //    procesoFechasCierre procesoFechasCierre = await db.procesoFechasCierres.FindAsync(id);
        //    if (procesoFechasCierre == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(procesoFechasCierre);
        //}

        // PUT: api/procesoFechasCierres/5
        [ResponseType(typeof(void)), Route("put")]
        public async Task<IHttpActionResult> PutprocesoFechasCierre(int id, procesoFechasCierre procesoFechasCierre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != procesoFechasCierre.ID)
            {
                return BadRequest();
            }

            db.Entry(procesoFechasCierre).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procesoFechasCierreExists(id))
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

        // POST: api/procesoFechasCierres
        [ResponseType(typeof(procesoFechasCierre)), Route("post")]
        public async Task<IHttpActionResult> PostprocesoFechasCierre(view_procesoCierres Vw_procesoCierre)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<procesoFechasCierre> Listcierres = new List<procesoFechasCierre>();
            List<ControlCierre> ListProcessNew = new List<ControlCierre>();
            List<ControlCierre> ListProcessExist = db.ControlCierres.ToList();

            foreach (var cierre in Vw_procesoCierre.listday_clouser)
            {               
                addNewProcessControl(Vw_procesoCierre, ListProcessExist,ref ListProcessNew);
                Listcierres.Add(new procesoFechasCierre()
                {
                    ID_process = Vw_procesoCierre.ID_process,
                    day_clouser = cierre,
                    Active = true
                });
            }            

            db.procesoFechasCierres.AddRange(Listcierres);
            if (ListProcessNew.Any())            
                db.ControlCierres.AddRange(ListProcessNew);

            await db.SaveChangesAsync();
            return Ok(Listcierres.Select(x=> new { x.ID,x.ID_process,x.day_clouser,x.Active }));

            //return CreatedAtRoute("DefaultApi", new { id = procesoFechasCierre.ID }, procesoFechasCierre);
        }

        private void addNewProcessControl(view_procesoCierres vw_procesoCierre, List<ControlCierre> ListProcessExist,ref List<ControlCierre> ListProcessNew)
        {
            string processName = db.Procesoes.Where(x => x.ProcesoID == vw_procesoCierre.ID_process).Select(x=>x.Nombre).FirstOrDefault();

            if (!ListProcessExist.Where(x=>x.processName == processName).Any())
            { // If dont exist in table process control insert it
                ListProcessNew.Add(new ControlCierre() {processName = processName, day_cierre = vw_procesoCierre.listday_clouser[0]});
            }
        }



        // DELETE: api/procesoFechasCierres/5
        [ResponseType(typeof(procesoFechasCierre)), Route("delete")]
        public async Task<IHttpActionResult> DeleteprocesoFechasCierre(int id)
        {
            procesoFechasCierre procesoFechasCierre = await db.procesoFechasCierres.FindAsync(id);
            if (procesoFechasCierre == null)
            {
                return NotFound();
            }

            db.procesoFechasCierres.Remove(procesoFechasCierre);
            await db.SaveChangesAsync();

            return Ok(procesoFechasCierre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool procesoFechasCierreExists(int id)
        {
            return db.procesoFechasCierres.Count(e => e.ID == id) > 0;
        }
    }
}