using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos
{
    public class ListPackingListPalletLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Palet ID
        /// </summary>
        public Guid? PalletID { get; set; }
        /// <summary>
        /// Palet Adı
        /// </summary>
        public string PalletName { get; set; }
        /// <summary>
        /// İlk Koli No
        /// </summary>
        public string FirstPackageNo { get; set; }
        /// <summary>
        /// Son Koli No
        /// </summary>
        public string LastPackageNo { get; set; }
        /// <summary>
        /// Koli Sayı
        /// </summary>
        public int NumberofPackage { get; set; }
    }
}
