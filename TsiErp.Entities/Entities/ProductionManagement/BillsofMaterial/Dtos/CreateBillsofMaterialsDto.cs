﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos
{
    public class CreateBillsofMaterialsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Reçete Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Reçete Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Stok Türü
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid? FinishedProductID { get; set; }

        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Genel Açıklama
        /// </summary>
        public string _Description { get; set; }

        [NoDatabaseAction]
        public List<SelectBillsofMaterialLinesDto> SelectBillsofMaterialLines { get; set; }
    }
}
