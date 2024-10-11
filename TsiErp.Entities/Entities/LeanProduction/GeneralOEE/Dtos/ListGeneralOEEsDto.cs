using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos
{
    public class ListGeneralOEEsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Ay
        /// </summary>
        public int Month_ { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        /// <summary>
        /// Hurda Oranı
        /// </summary>
        public decimal ScrapRate { get; set; }
        /// <summary>
        /// Kullanılabilirlik
        /// </summary>
        public decimal Availability { get; set; }
        /// <summary>
        /// Verimlilik
        /// </summary>
        public decimal Productivity { get; set; }
    }
}
