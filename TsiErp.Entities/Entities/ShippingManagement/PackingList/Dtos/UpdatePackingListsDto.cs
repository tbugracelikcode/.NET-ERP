using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos
{
    public class UpdatePackingListsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Çeki Liste Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Paket Fişi Kodu 2
        /// </summary>
        public string Code2 { get; set; }
        /// <summary>
        /// Gönderici ID
        /// </summary>
        public Guid? TransmitterID { get; set; }
        /// <summary>
        /// Gönderilen ID
        /// </summary>
        public Guid? RecieverID { get; set; }
        /// <summary>
        /// Sevkiyat Adres ID
        /// </summary>
        public Guid? ShippingAddressID { get; set; }
        /// <summary>
        /// Teslimat Tarihi
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// Yükleme Tarihi
        /// </summary>
        public DateTime? LoadingDate { get; set; }
        /// <summary>
        /// Ödeme Tarihi
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Fatura Tarihi
        /// </summary>
        public DateTime? BillDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Sipariş No
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// Fatura No
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// Banka ID
        /// </summary>
        public Guid? BankID { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public int PackingListState { get; set; }
        /// <summary>
        /// Satış Şekli
        /// </summary>
        public int SalesType { get; set; }
        /// <summary>
        /// Gümrük Yetkili
        /// </summary>
        public string CustomsOfficial { get; set; }
        /// <summary>
        /// Nakliye Yetkili
        /// </summary>
        public string ShippingOfficial { get; set; }
        /// <summary>
        /// Nakliye Firma
        /// </summary>
        public string ShippingCompany { get; set; }
        /// <summary>
        /// Araç Plaka 1
        /// </summary>
        public string VehiclePlateNumber1 { get; set; }
        /// <summary>
        /// Araç Plaka 2
        /// </summary>
        public string VehiclePlateNumber2 { get; set; }
        /// <summary>
        /// Sürücü Ad Soyad
        /// </summary>
        public string DriverNameSurname { get; set; }
        /// <summary>
        /// Sürücü Telefon
        /// </summary>
        public string DriverPhone { get; set; }
        /// <summary>
        /// TIR Şekli
        /// </summary>
        public int TIRType { get; set; }
        /// <summary>
        /// Yükleme Saati
        /// </summary>
        public TimeSpan? LoadingHour { get; set; }

        [NoDatabaseAction]
        public List<SelectPackingListPalletCubageLinesDto> SelectPackingListPalletCubageLines { get; set; }
        [NoDatabaseAction]
        public List<SelectPackingListPalletLinesDto> SelectPackingListPalletLines { get; set; }
        [NoDatabaseAction]
        public List<SelectPackingListPalletPackageLinesDto> SelectPackingListPalletPackageLines { get; set; }
    }
}
