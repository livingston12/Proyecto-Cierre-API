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
    [RoutePrefix("procesos"), AllowAnonymous]
    public class ProcesosController : ApiController
    {
        private BUCDWEntities db = new BUCDWEntities();

        // GET: api/Procesos
        [Route("getall")]
        public IQueryable<Proceso> GetProcesoes()
        {

            return db.Procesoes;
        }

  
      

        // GET: api/Procesos/5

        [ResponseType(typeof(Proceso)), Route("getbyid")]
        public async Task<IHttpActionResult> getbyid(int id)
        {
            Proceso proceso = await db.Procesoes.FindAsync(id);
            if (proceso == null)
            {
                return NotFound();
            }

            return Ok(proceso);
        }

        // PUT: api/Procesos/5
        [ResponseType(typeof(void)), Route("put")]
        public async Task<IHttpActionResult> PutProceso(int id, Proceso proceso)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proceso.ProcesoID)
            {
                return BadRequest();
            }
            var currentProcess = await db.Procesoes.FindAsync(id);

            if (currentProcess == null)            
                return BadRequest();           

            try
            {
                //proceso.GuidRelacional = currentProcess.GuidRelacional;
                //proceso.Guid = currentProcess.Guid;
                //proceso.Estado = true;
                //proceso.FechaModificacion = DateTime.Now;
                currentProcess.PlanFechaEjecucionID = proceso.PlanFechaEjecucionID;
                currentProcess.Nombre = proceso.Nombre;
                currentProcess.Descripcion = proceso.Descripcion;
                //db.Entry(proceso).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProcesoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Procesos
        [ResponseType(typeof(Proceso)), Route("post")]
        public async Task<IHttpActionResult> PostProceso(Proceso proceso)
        {
            try
            {
                if (proceso == null)
                {
                    return BadRequest(ModelState);
                }

                // Set default values
                proceso.GuidRelacional = Guid.NewGuid();
                proceso.PermitirNotificaciones = false;
                proceso.PermitirRecordatorioCalendario = false;
                proceso.PermitirUltimoEstatusCalendario = false;
                proceso.RepetirProcesoDespuesFallo = false;
                proceso.MaximaRepeticionDeProceso = 0;
                proceso.FechaModificacion = DateTime.Now;
                proceso.Guid = Guid.NewGuid();
                proceso.Estado = true;
                db.Procesoes.Add(proceso);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

                return BadRequest(ModelState);
            }


            return CreatedAtRoute("Default", new { id = proceso.ProcesoID }, proceso);
        }

        // DELETE: api/Procesos/5
        [ResponseType(typeof(Proceso)), Route("delete")]
        public async Task<IHttpActionResult> DeleteProceso(int id)
        {
            Proceso proceso = await db.Procesoes.FindAsync(id);
            if (proceso == null)
            {
                return NotFound();
            }

            db.Procesoes.Remove(proceso);
            await db.SaveChangesAsync();

            return Ok(proceso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProcesoExists(int id)
        {
            return db.Procesoes.Count(e => e.ProcesoID == id) > 0;
        }
    }
}