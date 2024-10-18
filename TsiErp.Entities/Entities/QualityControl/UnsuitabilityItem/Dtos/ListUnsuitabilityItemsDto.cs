using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos
{
    public class ListUnsuitabilityItemsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Başlık Kodu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Başlık 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Açıklama 
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Uygunsuzluk Türü Başlığı 
        /// </summary>
        public Guid UnsuitabilityTypesItemsId { get; set; }
        /// <summary>
        /// Uygunsuzluk Türü Başlığı 
        /// </summary>
        public string UnsuitabilityTypesItemsName { get; set; }
        /// <summary>
        /// İş Merkezi ID 
        /// </summary>
        public Guid StationGroupId { get; set; }

        public string StationGroupName { get; set; }

        /// <summary>
        /// Şiddet Aralığı
        /// </summary>
        public int IntensityRange { get; set; }

        /// <summary>
        /// Şiddet Kat Sayısı
        /// </summary>
        public int IntensityCoefficient { get; set; }
        /// <summary>
        /// Personel Verimlilik Analizi
        /// </summary>
        public bool isEmployeeProductivityAnalysis { get; set; }
        /// <summary>
        /// İstasyon Verimlilik Analizi
        /// </summary>
        public bool isStationProductivityAnalysis { get; set; }
    }
}
