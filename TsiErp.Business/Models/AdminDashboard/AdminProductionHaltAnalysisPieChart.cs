using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminProductionHaltAnalysisPieChart
    {
        public string HALTTYPE { get; set; }

        public List<AdminProductionHaltAnalysisPieChartDetail> AdminProductionHaltAnalysisPieChartDetail { get; set; }

        public AdminProductionHaltAnalysisPieChart()
        {
            AdminProductionHaltAnalysisPieChartDetail = new List<AdminProductionHaltAnalysisPieChartDetail>();
        }
    }

    public class AdminProductionHaltAnalysisPieChartDetail
    {
        public decimal HALTTIME { get; set; }
        public string HALTTYPE { get; set; }
        public int QUANTITY { get; set; }
        public string TITLENAME { get; set; }
        public Guid? HALTID { get; set; }
        public string TIME { get; set; }
        public int YEAR { get; set; }
    }


}
