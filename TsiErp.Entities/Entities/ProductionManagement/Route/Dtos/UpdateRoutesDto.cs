﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.Route.Dtos
{
    public class UpdateRoutesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? StationGroupID { get; set; }
        /// <summary>
        /// Ana Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Üretim Başlangıç
        /// </summary>
        public string ProductionStart { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalApproval { get; set; }
        [NoDatabaseAction]
        public List<SelectRouteLinesDto> SelectRouteLines { get; set; }
    }
}
