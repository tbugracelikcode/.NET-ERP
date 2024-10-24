using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminProductGroupAnalysisChart
    {
        

        public int YEAR { get; set; }

        public string MONTH { get; set; }
        public decimal OEE { get; set; }
        public decimal SCRAPPERCENT { get; set; }

        public decimal DIFFSCRAPPERCENT { get; set; }

    }
}
