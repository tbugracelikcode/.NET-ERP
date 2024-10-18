using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos
{
    public class UpdateUnsuitabilityItemsDto : FullAuditedEntityDto
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

        public Guid StationGroupId { get; set; }

        /// <summary>
        /// Uygunsuzluk Türü Başlığı 
        /// </summary>
        public Guid UnsuitabilityTypesItemsId { get; set; }

        /// <summary>
        /// Hayati Tehlike
        /// </summary>
        public string LifeThreatening { get; set; }

        /// <summary>
        /// Prestij Kaybı
        /// </summary>
        public string LossOfPrestige { get; set; }

        /// <summary>
        /// Ekstra Maliyet
        /// </summary>
        public string ExtraCost { get; set; }

        /// <summary>
        /// Ürün Ömrü Kısalması
        /// </summary>
        public string ProductLifeShortening { get; set; }

        /// <summary>
        /// Farkedilebilirlik
        /// </summary>
        public string Detectability { get; set; }

        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public string ToBeUsedAs { get; set; }

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
