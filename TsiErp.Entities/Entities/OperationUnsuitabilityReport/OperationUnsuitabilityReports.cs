using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.OperationUnsuitabilityReport
{
    /// <summary>
    /// Operasyon Uygunsuzluk Raporları
    /// </summary>
    public class OperationUnsuitabilityReports : FullAuditedEntity
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
        /// Uygunsuzluk İş Emri Oluşacak
        /// </summary>
        public bool IsUnsuitabilityWorkOrder { get; set; }
        /// <summary>
        /// Ölçü Kontrol Form Beyan
        /// </summary>
        public decimal ControlFormDeclaration { get; set; }
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
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// İstasyon Grup ID
        /// </summary>
        public Guid StationGroupID { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid OperationID { get; set; }

    }
}
