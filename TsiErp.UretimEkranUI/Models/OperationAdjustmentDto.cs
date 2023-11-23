using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Models
{
    public class OperationAdjustmentDto
    {
        public Guid AdjustmentUserId { get; set; }

        public string AdjustmentUserName { get; set; }

        public string AdjustmentUserPassword { get; set; }

        public DateTime AdjustmentStartDate { get; set; }

        public DateTime AdjustmentEndDate { get; set; }

        public decimal AdjustmentQuantity { get; set; }


        public DateTime QualitControlApprovalStartDate { get; set; }

        public DateTime QualitControlApprovalEndDate { get; set; }
    }
}
