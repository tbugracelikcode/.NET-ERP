using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.EquipmentRecord;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.CalibrationVerification
{
    /// <summary>
    /// Kalibrasyon Doğrulama
    /// </summary>
    public class CalibrationVerifications : FullAuditedEntity
    {
        /// <summary>
        /// Kalibrasyon Doğrulama Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kalibrasyon Doğrulama Açıklaması
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Fiş No
        /// </summary>
        public string ReceiptNo { get; set; }
        /// <summary>
        /// CihazID
        /// </summary>
        public Guid EquipmentID { get; set; }
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
