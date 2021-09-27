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
    [RoutePrefix("Logs")]
    public class LogsController : ApiController
    {
        private BUCDWEntities db = new BUCDWEntities();

        // GET: api/Logs
        [Route("getall")]
        public IQueryable<Log> GetLogs()
        {
            return db.Logs.OrderByDescending(x=>x.FechaEjecucion);
        }

        [ResponseType(typeof(view_Pie_values)), Route("getPie")]
        public async Task<IHttpActionResult> getPie(DateTime? date = null)
        {
            view_Pie_values pie = new view_Pie_values();
            try
            {
                var listLabels = await db.labelsPies.ToListAsync();
                var ListBdValues = db.sp_GetValuesPie(date).FirstOrDefault();

                List<int?> values = new List<int?>();

                foreach (var item in listLabels)
                {
                    switch (item.ID)
                    {
                        case 1:
                            values.Add(ListBdValues.TotalcorridoHoy);
                            break;
                        case 2:
                            values.Add(ListBdValues.TotalNocorrido);
                            break;
                        case 3:
                            values.Add(ListBdValues.TotalEnproceso);
                            break;
                        case 4:
                            values.Add(ListBdValues.Total_conErrores);
                            break;
                    }
                }

                pie.Labes = listLabels.Select(x => x.LabelName).ToArray();
                pie.Values = values.ToArray();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(pie);
        }

        // GET: api/Logs/5
        [ResponseType(typeof(Log)),Route("getbyid")]
        public async Task<IHttpActionResult> GetLog(int id)
        {
            Log log = await db.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // PUT: api/Logs/5
        //[ResponseType(typeof(void)), Route("put")]
        //public async Task<IHttpActionResult> PutLog(int id, Log log)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != log.LogsID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(log).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LogExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Logs
        //[ResponseType(typeof(Log)), Route("post")]
        //public async Task<IHttpActionResult> PostLog(Log log)
        //{
        //    try
        //    {
        //        if (log == null)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.Logs.Add(log);
        //        await db.SaveChangesAsync();

        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest(ModelState);

        //    }           
        //    return CreatedAtRoute("Default", new { id = log.LogsID }, log);
        //}


        [ResponseType(typeof(Log)), Route("postClouser")]
        public async Task<IHttpActionResult> PostLog(view_PostLog view_PostLog)
        {
            List<Log> ListLogs = new List<Log>();
            DateTime dateExecuteDT;
            try
            {
                dateExecuteDT = DateTime.Parse(view_PostLog.dateExecute);
                if (view_PostLog.dates_to_From == null || view_PostLog.processId == null || view_PostLog.dateExecute == null)                
                    return BadRequest(ModelState);                
                else if(view_PostLog.dates_to_From.Length != 2)
                    return BadRequest(ModelState);

                DateTime dateFrom, dateTo;
                Log Log = new Log();
               

                dateFrom = DateTime.Parse(view_PostLog.dates_to_From[0].ToString());
                dateTo = DateTime.Parse(view_PostLog.dates_to_From[1].ToString());
                var My_process = db.Procesoes;
                Guid obj = Guid.NewGuid();

                foreach (int item in view_PostLog.processId)
                {
                   var proceso =  My_process.Where(x => x.ProcesoID == item).FirstOrDefault();
                    if (validateLogExist(proceso, dateFrom, dateTo, dateExecuteDT))                    
                        continue;
                    
                    Log = new Log()
                    {
                        EstatusDelProceso = 0,
                        Estado = true,
                        Guid = obj,                       

                        ProcesoNombre = proceso.Nombre,
                        ProcesoGUID = proceso.GuidRelacional,
                        FechaDesde = dateFrom,
                        FechaHasta = dateTo,
                        FechaEjecucion = dateExecuteDT,
                        MensajeError = "Execute By App",
                        ReEjecutadoCodigo = "1",
                        FechaModificacion = DateTime.Now
                    };
                    ListLogs.Add(Log);
                }

                db.Logs.AddRange(ListLogs);
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);

            }
            return Ok(ListLogs);
            //return CreatedAtRoute("Default", new { id = log.LogsID }, log);
        }

        private bool validateLogExist(Proceso proceso, DateTime dateFrom, DateTime dateTo, DateTime dateExecute)
        {
            return db.Logs.Where(x => x.FechaDesde == dateFrom && x.FechaHasta == dateTo && x.FechaEjecucion == dateExecute && x.ProcesoGUID == proceso.GuidRelacional && x.EstatusDelProceso == 0).Any();
        }

        //// DELETE: api/Logs/5
        //[ResponseType(typeof(Log)), Route("delete")]
        //public async Task<IHttpActionResult> DeleteLog(int id)
        //{
        //    Log log = await db.Logs.FindAsync(id);
        //    if (log == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Logs.Remove(log);
        //    await db.SaveChangesAsync();

        //    return Ok(log);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogExists(int id)
        {
            return db.Logs.Count(e => e.LogsID == id) > 0;
        }
    }
}