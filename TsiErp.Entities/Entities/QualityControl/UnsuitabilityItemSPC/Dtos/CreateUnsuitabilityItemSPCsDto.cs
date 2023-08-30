using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos
{
    public class CreateUnsuitabilityItemSPCsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// SPC Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Hesaplama Başlangıç Tarihi
        /// </summary>
        public DateTime? MeasurementStartDate { get; set; }
        /// <summary>
        /// Hesaplama Bitiş Tarihi
        /// </summary>
        public DateTime? MeasurementEndDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectUnsuitabilityItemSPCLinesDto> SelectUnsuitabilityItemSPCLines { get; set; }
    }
}
