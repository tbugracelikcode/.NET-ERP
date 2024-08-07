using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.Product.Dtos
{
    public class CreateProductsDto : FullAuditedEntityDto
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
        /// Birim Ağırlığı
        /// </summary>
        public decimal UnitWeight { get; set; }
        /// <summary>
        /// Temin Şekli
        /// </summary>
        public int SupplyForm { get; set; }
        /// <summary>
        /// Stok Boyu
        /// </summary>
        public decimal ProductSize { get; set; }
        /// <summary>
        /// GTIP
        /// </summary>
        public string GTIP { get; set; }
        /// <summary>
        /// Testere Fire
        /// </summary>
        public decimal SawWastage { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Confirmation { get; set; }
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalConfirmation { get; set; }
        /// <summary>
        /// Stok Türü
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// Ürün Grup ID
        /// </summary>
        public Guid? ProductGrpID { get; set; }
        /// <summary>
        /// Üretici Firma Kodu
        /// </summary>
        public string ManufacturerCode { get; set; }
        /// <summary>
        /// Satış KDV
        /// </summary>
        public int SaleVAT { get; set; }
        /// <summary>
        /// Satın Alma KDV
        /// </summary>
        public int PurchaseVAT { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Özellik Set ID
        /// </summary>
        public Guid FeatureSetID { get; set; }
        /// <summary>
        /// İngilizce Tanım
        /// </summary>
        public string EnglishDefinition { get; set; }
        /// <summary>
        /// İhracat Kategori No
        /// </summary>
        public string ExportCatNo { get; set; }
        /// <summary>
        /// OemRefNo
        /// </summary>
        public string OemRefNo { get; set; }
        /// <summary>
        /// OemRefNo2
        /// </summary>
        public string OemRefNo2 { get; set; }
        /// <summary>
        /// OemRefNo3
        /// </summary>
        public string OemRefNo3 { get; set; }
        /// <summary>
        /// Planlanan Fire
        /// </summary>
        public int PlannedWastage { get; set; }
        /// <summary>
        /// Kaplama Ağırlığı
        /// </summary>
        public decimal CoatingWeight { get; set; }
        /// <summary>
        /// Hammadde Türü
        /// </summary>
        public int RawMaterialType { get; set; }

        /// <summary>
        /// Dış Çap
        /// </summary>
        public decimal ExternalRadius { get; set; }
        /// <summary>
        /// İç Çap
        /// </summary>
        public decimal InternalRadius{ get; set; }
        /// <summary>
        /// Genişlik
        /// </summary>
        public decimal Width_ { get; set; }
        /// <summary>
        /// Kalınlık
        /// </summary>
        public decimal Tickness_ { get; set; }
        /// <summary>
        /// Çap Değeri
        /// </summary>
        public decimal RadiusValue { get; set; }

        [NoDatabaseAction]
        public List<SelectProductRelatedProductPropertiesDto> SelectProductRelatedProductProperties { get; set; }
    }
}
