using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos
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
        /// Demir Başlar
        /// </summary>
        public bool IsFixtures { get; set; }
        /// <summary>
        /// Fason
        /// </summary>
        public bool IsContract { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// IoT İstasyonu
        /// </summary>
        public bool IsIotStation { get; set; }


        /// <summary>
        /// Çalışma Durumu
        /// </summary>
        public int StationWorkStateEnum { get; set; }

        /// <summary>
        /// Bulunduğu Kat
        /// </summary>
        public string StationFloor { get; set; }

        [NoDatabaseAction]
        public List<SelectStationInventoriesDto> SelectStationInventoriesDto { get; set; }
    }
}
