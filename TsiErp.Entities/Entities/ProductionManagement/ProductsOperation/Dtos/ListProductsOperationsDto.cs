﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos
{
    public class ListProductsOperationsDto : FullAuditedEntityDto
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
        /// İş Merkezi Kodu
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid WorkCenterID { get; set; }
        /// <summary>
        /// İş Merkezi Adı
        /// </summary>
        public string WorkCenterName { get; set; }
        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ürün Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Şablon Operasyon Kodu
        /// </summary>
        public string TemplateOperationCode { get; set; }
        /// <summary>
        /// Şablon Operasyon Açıklaması
        /// </summary>
        public string TemplateOperationName { get; set; }
        /// <summary>
        /// Ürün Id
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Toplam Miktar
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Üretim Adımları
        /// </summary>
        public CPRsManufacturingStepsEnum ManufacturingSteps { get; set; }
    }
}
