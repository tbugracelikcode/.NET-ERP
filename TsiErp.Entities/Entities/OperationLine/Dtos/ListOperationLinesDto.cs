using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.OperationLine.Dtos
{
    public class ListOperationLinesDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Operasyon 
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// İstasyon 
        /// </summary>
        public string Station { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// İşlem Adet
        /// </summary>
        public int ProcessQuantity { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Alternatif
        /// </summary>
        public bool Alternative { get; set; }
    }
}
