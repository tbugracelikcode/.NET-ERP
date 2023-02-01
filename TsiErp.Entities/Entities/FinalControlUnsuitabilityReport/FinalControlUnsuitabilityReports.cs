using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Product;

namespace TsiErp.Entities.Entities.FinalControlUnsuitabilityReport
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
        [Precision(18, 6)]
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

        public Employees Employees { get; set; }
        public Products Products { get; set; }
    }
}
