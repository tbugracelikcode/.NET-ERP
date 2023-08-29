using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos
{
    public class ListOperationUnsuitabilityReportsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Rapor Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemsName { get; set; }
        /// <summary>
        /// Uygunsuzluk İş Emri Oluşacak
        /// </summary>
        public bool IsUnsuitabilityWorkOrder { get; set; }
        /// <summary>
        /// Uygun Olmayan Miktar
        /// </summary>
        public decimal UnsuitableAmount { get; set; }
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// İstasyon Açıklaması
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// İstasyon Grup Kodu
        /// </summary>
        public string StationGroupCode { get; set; }
        /// <summary>
        /// İstasyon Grup Açıklaması
        /// </summary>
        public string StationGroupName { get; set; }
        /// <summary>
        /// Çalışan Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Operasyon Açıklaması
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }
    }
}
