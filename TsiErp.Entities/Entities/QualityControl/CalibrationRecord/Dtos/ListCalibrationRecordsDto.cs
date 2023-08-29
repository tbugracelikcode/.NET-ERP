using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos
{
    public class ListCalibrationRecordsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kalibrasyon Takip Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kalibrasyon Takip Açıklaması
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Fiş No
        /// </summary>
        public string ReceiptNo { get; set; }
        /// <summary>
        /// Cihaz
        /// </summary>
        public string Equipment { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Bir Sonraki Kontrol
        /// </summary>
        public DateTime NextControl { get; set; }
        /// <summary>
        /// Mastar Sertifika No
        /// </summary>
        public string InfinitiveCertificateNo { get; set; }
        /// <summary>
        /// Sonuç
        /// </summary>
        public string Result { get; set; }
    }
}
