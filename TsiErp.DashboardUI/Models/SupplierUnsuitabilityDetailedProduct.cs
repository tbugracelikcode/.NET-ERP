﻿namespace TsiErp.DashboardUI.Models
{
    public class SupplierUnsuitabilityDetailedProduct
    {
        public int Quantity { get; set; }

        public string ProductCode { get; set; }
        /// <summary>
        /// Fason Uygunsuzluk ID 
        /// </summary>
        public int ContractUnsuitabilityID { get; set; }
        /// <summary>
        /// Ay 
        /// </summary>
        public string Ay { get; set; }
        /// <summary>
        /// Üretim Emri ID 
        /// </summary>
        public int ProductionOrderID { get; set; }
        /// <summary>
        /// Fason Tedarikçi
        /// </summary>
        public string ContractSupplier { get; set; }
        /// <summary>
        /// Fason Tedarikçi ID
        /// </summary>
        public int ContractSupplierID { get; set; }
        /// <summary>
        /// Fason Fiş Adet
        /// </summary>
        public int ContractReceiptQuantity { get; set; }
        /// <summary>
        /// Red Miktar 
        /// </summary>
        public int RefuseQuantity { get; set; }
        /// <summary>
        /// Uygunsuzluk Sebebi 
        /// </summary>
        public string UnsuitabilityReason { get; set; }
        /// <summary>
        /// Düzeltme
        /// </summary>
        public int Correction { get; set; }
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public int ToBeUsedAs { get; set; }
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public int ScrapQuantity { get; set; }
        /// <summary>
        /// Toplam 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Hata ID 
        /// </summary>
        public int ErrorID { get; set; }
        /// <summary>
        /// Oran 
        /// </summary>
        public double Percent { get; set; }

    }
}
