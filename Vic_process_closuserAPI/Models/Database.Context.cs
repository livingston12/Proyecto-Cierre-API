//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vic_process_closuserAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BUCDWEntities : DbContext
    {
        public BUCDWEntities()
            : base("name=BUCDWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PlanFechaEjecucion> PlanFechaEjecucions { get; set; }
        public virtual DbSet<CantidadFechaEjecucion> CantidadFechaEjecucions { get; set; }
        public virtual DbSet<FechaEjecucion> FechaEjecucions { get; set; }
        public virtual DbSet<LogErrorSap> LogErrorSaps { get; set; }
        public virtual DbSet<Proceso> Procesoes { get; set; }
        public virtual DbSet<labelsPie> labelsPies { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<procesoFechasCierre> procesoFechasCierres { get; set; }
        public virtual DbSet<ControlCierre> ControlCierres { get; set; }
        public virtual DbSet<vw_ListJobs> vw_ListJobs { get; set; }
    
        public virtual ObjectResult<sp_GetValuesPie_Result> sp_GetValuesPie(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetValuesPie_Result>("sp_GetValuesPie", fechaParameter);
        }
    
        public virtual int CorrerJob(string job_name, string serverName)
        {
            var job_nameParameter = job_name != null ?
                new ObjectParameter("job_name", job_name) :
                new ObjectParameter("job_name", typeof(string));
    
            var serverNameParameter = serverName != null ?
                new ObjectParameter("serverName", serverName) :
                new ObjectParameter("serverName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CorrerJob", job_nameParameter, serverNameParameter);
        }
    }
}
