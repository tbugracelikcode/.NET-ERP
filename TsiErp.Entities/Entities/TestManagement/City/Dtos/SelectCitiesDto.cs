using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.TestManagement.District.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.TestManagement.City.Dtos
{
    public class SelectCitiesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Şehir Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Şehir Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Büyük Şehir
        /// </summary>
        public string BigCityIs { get; set; }
        /// <summary>
        /// Şehir Türü
        /// </summary>
        public CityTypeFormEnum CityTypeForm { get; set; }
        /// <summary>
        /// Şehir Türü
        /// </summary>
        public string CityTypeName { get; set; }

        /// <summary>
        /// Şehir Nüfus
        /// </summary>
        public int Population_ { get; set; }
        /// <summary>
        /// Büyük Şehir
        /// </summary>
        public bool IsBigCity { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectCityLinesDto> SelectCityLines { get; set; }
    }
}
