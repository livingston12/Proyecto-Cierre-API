using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vic_process_closuserAPI.Models
{
    public class view_PostLog
    {
        public int[] processId { get; set; }
        public string[] dates_to_From { get; set; }
        public string dateExecute { get; set; }        
    }
}