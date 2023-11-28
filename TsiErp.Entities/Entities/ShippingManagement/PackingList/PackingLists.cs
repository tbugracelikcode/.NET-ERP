using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList
{
    public class PackingLists : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Çeki Liste Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Paket Fişi Kodu 2
        /// </summary>
        public string Code2 { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Gönderici ID
        /// </summary>
        public Guid TransmitterID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Gönderilen ID
        /// </summary>
        public Guid RecieverID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sevkiyat Adres ID
        /// </summary>
        public Guid ShippingAddressID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Teslimat Tarihi
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Yükleme Tarihi
        /// </summary>
        public DateTime? LoadingDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Ödeme Tarihi
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Fatura Tarihi
        /// </summary>
        public DateTime? BillDate { get; set; }

        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Sipariş No
        /// </summary>
        public string OrderNo { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Fatura No
        /// </summary>
        public string BillNo { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Banka ID
        /// </summary>
        public Guid? BankID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public PackingListStateEnum PackingListState { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satış Şekli
        /// </summary>
        public PackingListSalesTypeEnum SalesType { get; set; }
        [SqlColumnType(MaxLength=250,Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Gümrük Yetkili
        /// </summary>
        public string CustomsOfficial { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Nakliye Yetkili
        /// </summary>
        public string ShippingOfficial { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Nakliye Firma
        /// </summary>
        public string ShippingCompany { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Araç Plaka 1
        /// </summary>
        public string VehiclePlateNumber1 { get; set; }

        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Araç Plaka 2
        /// </summary>
        public string VehiclePlateNumber2 { get; set; }

        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Sürücü Ad Soyad
        /// </summary>
        public string DriverNameSurname { get; set; }


        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Sürücü Telefon
        /// </summary>
        public string DriverPhone { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// TIR Şekli
        /// </summary>
        public PackingListTIRTypeEnum TIRType { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Time)]
        /// <summary>
        /// Yükleme Saati
        /// </summary>
        public TimeSpan? LoadingHour { get; set; }

    }
}
