using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos
{
    public class ProductWarehouseStatusReportDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitSetCode { get; set; }

        public string ProductTypeName { get; set; }

        public List<ProductWarehouseStatusLinesReportDto> WarehousesLines { get; set; }

        public ProductWarehouseStatusReportDto()
        {
            WarehousesLines = new List<ProductWarehouseStatusLinesReportDto>();
        }
    }

    public class ProductWarehouseStatusLinesReportDto
    {

        public int LineNr { get; set; }

        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }

        /// <summary>
        /// Toplam Sarf
        /// </summary>
        public decimal TotalConsumption { get; set; }

        /// <summary>
        /// Toplam Fire
        /// </summary>
        public decimal TotalWastage { get; set; }

        /// <summary>
        /// Toplam Üretim
        /// </summary>
        public decimal TotalProduction { get; set; }

        /// <summary>
        /// Toplam Stok Girişi
        /// </summary>
        public decimal TotalGoodsReceipt { get; set; }

        /// <summary>
        /// Toplam Stok Çıkışı
        /// </summary>
        public decimal TotalGoodsIssue { get; set; }

        public decimal Amount { get; set; }

        public DateTime? LastTransactionDate { get; set; }
    }
}
