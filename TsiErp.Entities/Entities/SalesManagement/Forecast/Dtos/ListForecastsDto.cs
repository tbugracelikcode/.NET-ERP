using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos
{
    public class ListForecastsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Oluşturma Tarihi
        /// </summary>
        public DateTime CreationDate_ { get; set; }
        /// <summary>
        /// Geçerlilik Başlangıç Tarihi
        /// </summary>
        public DateTime? ValidityStartDate { get; set; }
        /// <summary>
        /// Geçerlilik Bitiş Tarihi
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Cari Kod
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }
        /// <summary>
        /// Toplam Adet
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Satır Sayısı
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Dönem Kodu
        /// </summary>
        public string PeriodCode { get; set; }
        /// <summary>
        /// Dönem Adı
        /// </summary>
        public string PeriodName { get; set; }
    }
}
