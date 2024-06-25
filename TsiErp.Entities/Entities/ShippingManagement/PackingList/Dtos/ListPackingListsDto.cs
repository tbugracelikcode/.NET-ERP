using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos
{
    public class ListPackingListsDto : FullAuditedEntityDto
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
        /// Gönderici Ödeme Vade Günü
        /// </summary>
        public int TransmitterPaymentTermDay { get; set; }
        /// <summary>
        /// Gönderici ID
        /// </summary>
        public Guid? TransmitterID { get; set; }
        /// <summary>
        /// Gönderici Kod
        /// </summary>
        public string TransmitterCode { get; set; }
        /// <summary>
        /// Gönderici Ünvan
        /// </summary>
        public string TransmitterName { get; set; }
        /// <summary>
        /// Tedarikçi No
        /// </summary>
        public string TransmitterSupplierNo { get; set; }
        /// <summary>
        /// Gönderici EORI No
        /// </summary>
        public string TransmitterEORINo { get; set; }
        /// <summary>
        /// Gönderilen ID
        /// </summary>
        public Guid? RecieverID { get; set; }
        /// <summary>
        /// Gönderilen Kod
        /// </summary>
        public string RecieverCode { get; set; }
        /// <summary>
        /// Gönderilen Ünvan
        /// </summary>
        public string RecieverName { get; set; }
        /// <summary>
        /// Gönderilen Ana Yükleme
        /// </summary>
        public string RecieverCustomerCode { get; set; }
        /// <summary>
        /// Sevkiyat Adres ID
        /// </summary>
        public Guid? ShippingAddressID { get; set; }
        /// <summary>
        /// Sevkiyat Adresi
        /// </summary>
        public string ShippingAddressAddress { get; set; }
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
        /// Banka Adı
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public PackingListStateEnum PackingListState { get; set; }
        /// <summary>
        /// Satış Şekli
        /// </summary>
        public PackingListSalesTypeEnum SalesType { get; set; }
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
        public PackingListTIRTypeEnum TIRType { get; set; }
        /// <summary>
        /// Yükleme Saati
        /// </summary>
        public TimeSpan? LoadingHour { get; set; }
    }
}
