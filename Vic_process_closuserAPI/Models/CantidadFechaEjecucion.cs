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
    using System.Collections.Generic;
    
    public partial class CantidadFechaEjecucion
    {
        public int PlanFechaEjecucionID { get; set; }
        public Nullable<int> cantidad { get; set; }
    
        public virtual PlanFechaEjecucion PlanFechaEjecucion { get; set; }
    }
}
