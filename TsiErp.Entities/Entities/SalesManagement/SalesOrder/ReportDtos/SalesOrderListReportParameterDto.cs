using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.SalesOrder.ReportDtos
{
    public class SalesOrderListReportParameterDto
    {
        public List<Guid> Products { get; set; }

        public List<Guid> CurrentAccounts { get; set; }

        public List<SalesOrderLineStateEnum> SalesOrderLineState { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public SalesOrderListReportParameterDto()
        {
            Products = new List<Guid>();
            CurrentAccounts = new List<Guid>();
            SalesOrderLineState = new List<SalesOrderLineStateEnum>();
        }
    }
}
