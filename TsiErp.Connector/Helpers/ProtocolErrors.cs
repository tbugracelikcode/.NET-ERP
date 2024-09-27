using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Connector.Helpers
{
    public class ProtocolErrors
    {
        /// <summary>
        /// Başarılı
        /// </summary>
        public static string Error0 = "Başarılı";

        /// <summary>
        /// Geçersiz Değer
        /// </summary>
        public static string Error2 = "Geçersiz Değer";

        /// <summary>
        /// Geçersiz Format veya Bağlantı Komutu Hatası
        /// </summary>
        public static string Error4 = "Geçersiz Format veya Bağlantı Komutu Hatası";

        /// <summary>
        /// Yürütülemez (PLC yürütülürken Ladder Checksum hatası)
        /// </summary>
        public static string Error5 = "Yürütülemez (PLC Yürütülürken Ladder Checksum Hatası)";

        /// <summary>
        /// Yürütülemez (PLC Yürütülürken PLC ID ve Ladder ID Eşit Değil)
        /// </summary>
        public static string Error6 = "Yürütülemez (PLC Yürütülürken PLC ID ve Ladder ID Eşit Değil)";

        /// <summary>
        /// Yürütülemez (PLC Yürütülürken Yazım Yanlışı Tespit Edildi)
        /// </summary>
        public static string Error7 = "Yürütülemez (PLC Yürütülürken Yazım Yanlışı Tespit Edildi)";

        /// <summary>
        /// Yürütülemez (Özellik Desteklenmiyor)
        /// </summary>
        public static string Error9 = "Yürütülemez (Özellik Desteklenmiyor)";

        /// <summary>
        /// Geçersiz Konum
        /// </summary>
        public static string ErrorA = "Geçersiz Konum";

        /// <summary>
        /// Result is null.
        /// </summary>
        public static string ErrorResultNull = "Result is null";

        /// <summary>
        /// TcpClient is null.
        /// </summary>
        public static string ErrorTcpClientNull = "TcpClient is null";

        /// <summary>
        /// An error was encountered while returning the value
        /// </summary>
        public static string ErrorGeneral = "An error was encountered while returning the value";
        /// <summary>
        /// PLC otomatik modda değil
        /// </summary>
        public static string ErrorAutomatic = "PLC otomatik modda değil";
    }
}
