using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.Description;
using Vic_process_closuserAPI.Models;

namespace Vic_process_closuserAPI.Controllers
{
    [RoutePrefix("jobs")]
    public class vw_ListJobsController : ApiController
    {
        enum listServers
        {
            CNDDOSDOBI01 = 01,
            CNDDOSDOBI02 = 02,
            CNDDOSDOBIS = 03
        }
        private BUCDWEntities db = new BUCDWEntities();       
       
        // GET: api/vw_ListJobs
        [Route("getall")]
        public IQueryable<vw_ListJobs> Getvw_ListJobs()
        {
            return db.vw_ListJobs;
        }

        

        // POST: api/vw_ListJobs
        [ResponseType(typeof(vw_ListJobs)),Route("post")]
        public async  Task<IHttpActionResult> Postvw_ListJobs(vw_ListJobs vw_ListJobs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            try
            {
                    db.Database.CommandTimeout = 0;
                if (vw_ListJobs.serverName == listServers.CNDDOSDOBI01.ToString())
                {
                    await Task.Run(() => {
                        db.CorrerJob(vw_ListJobs.name, vw_ListJobs.serverName);
                    });  
                }
                else if (vw_ListJobs.serverName == listServers.CNDDOSDOBI02.ToString())
                {
                   await execute02(vw_ListJobs);
                }
                else if(vw_ListJobs.serverName == listServers.CNDDOSDOBIS.ToString())
                {
                    await executeBis(vw_ListJobs);
                }
                    
                    //while (TimerFinish)
                    //{
                        //SetTimer(vw_ListJobs);
                    //}
                  
                                          
            }
            catch (DbUpdateException)
            {
              return Conflict();
            }

            return  Ok(vw_ListJobs); // Retorna el index el cual se envio por descripcion
        }

        private async Task<int> execute02(vw_ListJobs vw_ListJobs)
        {            
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Con02"].ToString();
            string SQL = "proceso.CorrerJob";

            // Create ADO.NET objects.

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param;
            param = cmd.Parameters.Add("@job_name", SqlDbType.NVarChar, 150);
            param.Value = vw_ListJobs.name;
            con.Open();
            var compl = await cmd.ExecuteNonQueryAsync();
            con.Close();

            return compl;
        }

        private async Task<int> executeBis(vw_ListJobs vw_ListJobs)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConBis"].ToString();
            string SQL = "proceso.CorrerJob";

            // Create ADO.NET objects.

            SqlConnection con = new SqlConnection(connectionString);            
            SqlCommand cmd = new SqlCommand(SQL, con);            
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param;
            param = cmd.Parameters.Add("@job_name", SqlDbType.NVarChar, 150);
            param.Value = vw_ListJobs.name;
            con.Open();
            var compl = await cmd.ExecuteNonQueryAsync();
            con.Close();

            return compl;
        }

        //private void SetTimer(vw_ListJobs vw_ListJobs)
        //{
        //    Timer Timer1 = new Timer();
        //    Timer1.Interval = 1000;
        //    Vw_current = vw_ListJobs;
        //    Timer1.Elapsed += new ElapsedEventHandler(Timer1_Tick);

        //    // Enable timer.  
        //    Timer1.Enabled = true;

        //}

        //private void Timer1_Tick(object Sender, EventArgs e)
        //{
        //    Vw_current.
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vw_ListJobsExists(string id)
        {
            return db.vw_ListJobs.Count(e => e.name == id) > 0;
        }
    }
}