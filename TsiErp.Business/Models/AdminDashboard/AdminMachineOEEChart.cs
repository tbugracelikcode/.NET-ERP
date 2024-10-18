using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminMachineOEEChart
    {

        public decimal AVAILABILITY { get; set; }

        public decimal PERFORMANCE { get; set; }

        public decimal QUALITY { get; set; }

        public decimal OEE { get; set; }

        public int YEAR { get; set; }

        public string MONTH { get; set; }

        public decimal DIFFOEE { get; set; }

        public decimal DIFFAVA { get; set; }

        public decimal DIFFQUA { get; set; }

        public decimal DIFFPER { get; set; }
    }
}
