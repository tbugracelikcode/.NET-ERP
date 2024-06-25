using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecord.ReportDtos.BigPalletLabelDtos
{
    public class BigPalletLabelReportParameterDto
    {
        public List<Guid> Pallets { get; set; }

        public BigPalletLabelReportParameterDto()
        {
            Pallets = new List<Guid>();
        }
    }
}
