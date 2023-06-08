using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard
{
    /// <summary>
    /// Cari Hesap Kartları
    /// </summary>
    public class CurrentAccountCards : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cari Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tedarikçi No
        /// </summary>
        public string SupplierNo { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Sevkiyat Adresi
        /// </summary>
        public string ShippingAddress { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Tür
        /// </summary>
        public int Type_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Adres1
        /// </summary>
        public string Address1 { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Adres2
        /// </summary>
        public string Address2 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ülke
        /// </summary>
        public string Country { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Posta Kodu
        /// </summary>
        public string PostCode { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tel1
        /// </summary>
        public string Tel1 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tel2
        /// </summary>
        public string Tel2 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Faks
        /// </summary>
        public string Fax { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İlgili
        /// </summary>
        public string Responsible { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// EPosta
        /// </summary>
        public string Email { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Web
        /// </summary>
        public string Web { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod1
        /// </summary>
        public string PrivateCode1 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod2
        /// </summary>
        public string PrivateCode2 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod3
        /// </summary>
        public string PrivateCode3 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod4 
        /// </summary>
        public string PrivateCode4 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod5
        /// </summary>
        public string PrivateCode5 { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Şahıs Şirketi
        /// </summary>
        public bool SoleProprietorship { get; set; }
        [SqlColumnType(MaxLength = 11, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// TC Kimlik No
        /// </summary>
        public string IDnumber { get; set; }
        [SqlColumnType(MaxLength = 75, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Vergi Dairesi
        /// </summary>
        public string TaxAdministration { get; set; }
        [SqlColumnType(MaxLength = 10, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Vergi Numarası
        /// </summary>
        public string TaxNumber { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Kaplama Müşterisi
        /// </summary>
        public bool CoatingCustomer { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Satış Sözleşmesi
        /// </summary>
        public string SaleContract { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Artı Yüzde Oran
        /// </summary>
        public int PlusPercentage { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Tedarikçi
        /// </summary>
        public bool Supplier { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Fason Tedarikçi
        /// </summary>
        public bool ContractSupplier { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

    }
}
