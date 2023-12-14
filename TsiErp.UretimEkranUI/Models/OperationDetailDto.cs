using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.UretimEkranUI.Models
{
    public class OperationDetailDto
    {

        public ListWorkOrdersDto WorkOrder { get; set; }

        public OperationAdjustmentDto OperationAdjustment { get; set; }

        public Guid EmployeeID { get; set; }

        public string EmployeeName { get; set; }


        public DateTime QualitControlApprovalDate { get; set; }

        public int TotalQualityControlApprovalTime { get; set; }


        public decimal ApprovedQuantity { get; set; } = 0;

        public decimal ScrapQuantity { get; set; } = 0;


        public OperationDetailDto()
        {
            WorkOrder = new ListWorkOrdersDto();
            OperationAdjustment = new OperationAdjustmentDto();
        }
    }
}
