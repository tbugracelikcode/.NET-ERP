using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Services
{
    public static class AppService
    {
        public static Guid CurrentWorkOrderId { get; set; }

        public static bool AdjusmentModalVisible { get; set; }
    }
}
