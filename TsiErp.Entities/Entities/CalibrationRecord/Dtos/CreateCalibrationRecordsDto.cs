using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CalibrationRecord.Dtos
{
    public class CreateCalibrationRecordsDto : FullAuditedEntityDto
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
        /// CihazID
        /// </summary>
        public Guid? EquipmentID { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date { get; set; }
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
