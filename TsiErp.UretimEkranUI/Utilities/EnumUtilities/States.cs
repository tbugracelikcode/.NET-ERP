using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Utilities.EnumUtilities
{
    public class States
    {
        public enum State
        {
            Reset,
            IstasyonKapali,
            IstasyonCalisiyor,
            Uretimde,
            Ayar,
            Bakim,
            Ariza,
            SerbestCalisma,
            Durus
        }

        public enum AdjustmentState
        {
            FromOperation,
            FromNonOperation
        }
    }
}
