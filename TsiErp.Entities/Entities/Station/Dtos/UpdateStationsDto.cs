using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.StationInventory.Dtos;

namespace TsiErp.Entities.Entities.Station.Dtos
{
    public class UpdateStationsDto : FullAuditedEntityDto
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
        /// <summary>
        /// KWA
        /// </summary>
        public decimal KWA { get; set; }
        /// <summary>
        /// Grup ID
        /// </summary>
        public Guid? GroupID { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public decimal X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal Y { get; set; }
        /// <summary>
        /// Kapladığı Alan
        /// </summary>
        public decimal AreaCovered { get; set; }
        /// <summary>
        /// Kullanım Alanı
        /// </summary>
        public decimal UsageArea { get; set; }
        /// <summary>
        /// Amortisman
        /// </summary>
        public int Amortization { get; set; }
        /// <summary>
        /// Makine Maliyeti
        /// </summary>
        public decimal MachineCost { get; set; }
        /// <summary>
        /// Vardiya
        /// </summary>
        public int Shift { get; set; }
        /// <summary>
        /// Vardiya Çalışma Süresi
        /// </summary>
        public decimal ShiftWorkingTime { get; set; }
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

        public List<SelectStationInventoriesDto> SelectStationInventoriesDto { get; set; }
    }
}
