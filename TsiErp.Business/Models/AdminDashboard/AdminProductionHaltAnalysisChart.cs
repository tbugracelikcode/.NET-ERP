using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminProductionHaltAnalysisChart
    {
        
        public decimal HALTTIME { get; set; }
        public int DAY { get; set; }
        public decimal DIFFHALT { get; set; }

        public string TIME { get; set; }
    }
}
