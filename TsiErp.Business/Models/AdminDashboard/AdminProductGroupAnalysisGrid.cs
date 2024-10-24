using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminProductGroupAnalysisGrid
    {

        public string PRODUCTGROUP { get; set; }

        public Guid PRODUCTGROUPID { get; set; }
        public decimal SCRAPPERCENT { get; set; }

        public int YEAR { get; set; }

        public string MONTH { get; set; }

        public decimal DIFFSCRAPPERCENT { get; set; }
    }
}
