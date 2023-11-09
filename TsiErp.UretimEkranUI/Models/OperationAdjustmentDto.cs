using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Models
{
    public class OperationAdjustmentDto
    {
        public Guid SettingUserId { get; set; }

        public string SettingUserName { get; set; }

        public DateTime AdjustmentStartDate { get; set; }

        public TimeSpan AdjustmentStartTime { get; set; }

        public DateTime AdjustmentEndDate { get; set; }

        public TimeSpan AdjustmentEndTime { get; set; }

        public decimal SettingQuantity { get; set; }

        /// <summary>
        /// 1- Ayar Yapılıyor
        /// 2- Kalite Kontrol Onay Bekleniyor
        /// 3- Kalite Kontrol Onaylanmadı
        /// 4- Kalite Kontrol Onaylandı
        /// 5- Ayar Tamamlandı
        /// </summary>
        public int AdjustmentState { get; set; }
    }
}
