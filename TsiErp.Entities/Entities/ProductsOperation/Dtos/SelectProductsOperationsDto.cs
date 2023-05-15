using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;

namespace TsiErp.Entities.Entities.ProductsOperation.Dtos
{
    public class SelectProductsOperationsDto : FullAuditedEntityDto
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
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// İş Merkezi Kodu
        /// </summary>
        public string WorkCenterCode { get; set; }
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid? TemplateOperationID { get; set; }
        /// <summary>
        /// Şablon Operasyon Kodu
        /// </summary>
        public string TemplateOperationCode { get; set; }
        /// <summary>
        /// Şablon Operasyon Açıklaması
        /// </summary>
        public string TemplateOperationName { get; set; }
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ürün Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [NoDatabaseAction]
        public List<SelectProductsOperationLinesDto> SelectProductsOperationLines { get; set; }

    }
}
