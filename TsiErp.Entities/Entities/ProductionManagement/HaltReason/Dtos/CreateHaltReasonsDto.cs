﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos
{
    public class CreateHaltReasonsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Duruş Sebebi Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Duruş Sebebi Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Planlı mı?
        /// </summary>
        public bool IsPlanned { get; set; }
        /// <summary>
        /// Makine Kaynaklı mı?
        /// </summary>
        public bool IsMachine { get; set; }
        /// <summary>
        /// Operatör Kaynaklı mı?
        /// </summary>
        public bool IsOperator { get; set; }

        /// <summary>
        /// Yönetim Kaynaklı mı?
        /// </summary>
        public bool IsManagement { get; set; }
        /// <summary>
        /// Arızi Duruş
        /// </summary>
        public bool IsIncidentalHalt { get; set; }
    }
}