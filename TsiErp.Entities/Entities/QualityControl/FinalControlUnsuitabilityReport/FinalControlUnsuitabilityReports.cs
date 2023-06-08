using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport
{
    /// <summary>
    /// Final Kontrol Uygunsuzluk Raporları
    /// </summary>
    public class FinalControlUnsuitabilityReports : FullAuditedEntity
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
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }
        /// <summary>
        /// Hurda
        /// </summary>
        public bool IsScrap { get; set; }
        /// <summary>
        /// Düzeltme
        /// </summary>
        public bool IsCorrection { get; set; }
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public bool IsToBeUsedAs { get; set; }
        /// <summary>
        /// Ölçü Kontrol Form Beyan
        /// </summary>
        public decimal ControlFormDeclaration { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
    }
}
