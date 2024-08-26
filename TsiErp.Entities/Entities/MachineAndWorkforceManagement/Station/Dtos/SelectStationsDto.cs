using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos
{
    public class SelectStationsDto : FullAuditedEntityDto
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
        /// Grup Kodu
        /// </summary>
        public string StationGroupCode { get; set; }
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
        /// İstasyon Grup ID
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Grup Adı
        /// </summary>
        public string StationGroup { get; set; }
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
        /// Çalışma Durumu Enum
        /// </summary>
        public StationWorkStateEnum StationWorkStateEnum { get; set; }

        /// <summary>
        /// Çalışma Durumu
        /// </summary>
        public string StationWorkState { get; set; }

        /// <summary>
        /// Bulunduğu Kat
        /// </summary>
        public string StationFloor { get; set; }


        /// <summary>
        /// IoT İstasyonu
        /// </summary>
        public bool IsIotStation { get; set; }
        [NoDatabaseAction]
        public List<SelectStationInventoriesDto> SelectStationInventoriesDto { get; set; }
    }
}
