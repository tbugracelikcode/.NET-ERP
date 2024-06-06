using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Connector.Services
{
    public interface IProtocolServices
    {
        /// <summary>
        /// PLC Saatini Oku
        /// </summary>
        string M001R(string ipAddress);

        /// <summary>
        /// PLC Saatini Değiştir
        /// </summary>
        string M001W(string ipAddress,string data);

        /// <summary>
        /// PLC Durumunu Oku
        /// </summary>
        string M002R(string ipAddress);

        /// <summary>
        /// Üretim Adetini Oku
        /// </summary>
        string M003R(string ipAddress);

        /// <summary>
        /// Makine Çalışma Bilgisi
        /// </summary>
        string M004R(string ipAddress);

        /// <summary>
        /// İş Emri Değiştir
        /// </summary>
        string M008W(string ipAddress);

        /// <summary>
        /// Vardiya Toplam Üretim Adeti
        /// </summary>
        string M010R(string ipAddress);

        /// <summary>
        /// Vardiya Değiştir
        /// </summary>
        string M011W(string ipAddress);

        /// <summary>
        /// Vardiya Toplam Duruş Bilgisi
        /// </summary>
        string M012R(string ipAddress);

        /// <summary>
        /// Vardiya Toplam Çalışma Bilgisi
        /// </summary>
        string M013R(string ipAddress);

        /// <summary>
        /// Makinayı Çalıştır
        /// </summary>
        string M014W(string ipAddress);

        /// <summary>
        /// Vardiyada İçerisinde Manuel'de Kalma Süresi
        /// </summary>
        string M015R(string ipAddress);

        /// <summary>
        /// İş Emri Başlama Saatini Başlat
        /// </summary>
        string M016W(string ipAddress);

        /// <summary>
        /// İş Emri Başlama Saatini Bitir
        /// </summary>
        string M017W(string ipAddress);

        /// <summary>
        /// İş Emri Başlama Saatini Oku
        /// </summary>
        string M018R(string ipAddress);

        /// <summary>
        /// İş Emri Bitiş Saatini Oku
        /// </summary>
        string M019R(string ipAddress);

        /// <summary>
        /// Anlık Hız Oku
        /// </summary>
        string M020R(string ipAddress);

        /// <summary>
        /// Ortalama Hız Oku
        /// </summary>
        string M021R(string ipAddress);

        /// <summary>
        /// Üretilecek Adeti Yaz
        /// </summary>
        string M026W(string ipAddress,string data);

        /// <summary>
        /// Üretilecek Adeti Oku
        /// </summary>
        string M026R(string ipAddress);
    }
}
