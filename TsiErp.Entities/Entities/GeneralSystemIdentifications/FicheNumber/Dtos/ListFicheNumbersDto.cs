using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber.Dtos
{
    public class ListFicheNumbersDto : IEntityDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Menü
        /// </summary>
        public string Menu_ { get; set; }
    }
}
