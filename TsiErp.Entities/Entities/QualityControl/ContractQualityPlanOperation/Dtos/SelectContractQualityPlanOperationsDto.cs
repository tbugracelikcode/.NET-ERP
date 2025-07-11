﻿using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos
{
    public class SelectContractQualityPlanOperationsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fason Kalite Planı ID
        /// </summary>
        public Guid ContractQualityPlanID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
    }
}
