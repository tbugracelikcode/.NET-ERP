using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.Station
{
    /// <summary>
    /// İş İstasyonları
    /// </summary>
    public class Stations : FullAuditedEntity
    {
        /// <summary>
        /// Makine Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Makine Açıklaması
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Marka
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// Model
        /// </summary>
        public int Model { get; set; }
        /// <summary>
        /// Kapasite
        /// </summary>
        public string Capacity { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// KWA
        /// </summary>
        public decimal KWA { get; set; }
        /// <summary>
        /// Grup ID
        /// </summary>
        public Guid GroupID { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// X
        /// </summary>
        public decimal X { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Y
        /// </summary>
        public decimal Y { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Kapladığı Alan
        /// </summary>
        public decimal AreaCovered { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Kullanım Alanı
        /// </summary>
        public decimal UsageArea { get; set; }
        /// <summary>
        /// Amortisman
        /// </summary>
        public int Amortization { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Makine Maliyeti
        /// </summary>
        public decimal MachineCost { get; set; }
        /// <summary>
        /// Vardiya
        /// </summary>
        public int Shift { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Vardiya Çalışma Süresi
        /// </summary>
        public decimal ShiftWorkingTime { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Güç Faktörü
        /// </summary>
        public decimal PowerFactor { get; set; }       
        /// <summary>
        /// İş Güvenliği Talimatı
        /// </summary>
        public byte[] WorkSafetyInstruction { get; set; }
        /// <summary>
        /// Kullanım Talimatı
        /// </summary>
        public byte[] UsageInstruction { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Demir Başlar
        /// </summary>
        public bool IsFixtures { get; set; }
        /// <summary>
        /// Fason
        /// </summary>
        public bool IsContract { get; set; }

        public StationGroups StationGroups { get; set; }

        public ICollection<TemplateOperationLines> TemplateOperationLines { get; set; }

        public ICollection<ProductsOperationLines> ProductsOperationLines { get; set; }

        public ICollection<CalendarLines> CalendarLines { get; set; }

        public ICollection<WorkOrders> WorkOrders { get; set; }
    }
}
