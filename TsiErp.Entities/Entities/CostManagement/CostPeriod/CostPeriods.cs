using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.CostManagement.CostPeriod
{
    public class CostPeriods : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Hesaplama Başlangıç Tarihi
        /// </summary>
        public DateTime? CalculationStartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Hesaplama Bitiş Tarihi
        /// </summary>
        public DateTime? CalculationEndDate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Üretim Adedi
        /// </summary>
        public decimal TotalProductionQuantity { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Çalışma Günü
        /// </summary>
        public decimal AnnualWorkingDay { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Vardiya
        /// </summary>
        public decimal Shift { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Günlük Brüt Çalışma Saati
        /// </summary>
        public decimal DailyGrossWorkingHours { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Çalışma Saati
        /// </summary>
        public decimal AnnualWorkingHours { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Aylık 1 Vardiya Çalışma Saati
        /// </summary>
        public decimal MontlyShiftWorkingHour { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Vardiya Çalışma Zamanı
        /// </summary>
        public decimal ShiftWorkingTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Aylık Kira ve Aidat Bedeli
        /// </summary>
        public decimal MonthlyRentAndDues { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Kiralanan Alan
        /// </summary>
        public decimal TotalRentedArea { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// EUR Kur Tutarı
        /// </summary>
        public decimal EURExchangeRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Genel Yönetim Giderleri
        /// </summary>
        public decimal GeneralManagementExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşçi Sosyal Yardım Giderleri
        /// </summary>
        public decimal EmployeeSocialAssistanceExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşçi İş Elbiseleri Giderleri
        /// </summary>
        public decimal WorkClothesExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Pazarlama Gideri
        /// </summary>
        public decimal MarketingExpense { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Servis Gideri
        /// </summary>
        public decimal ServiceExpense { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yemek Gideri
        /// </summary>
        public decimal FoodExpense { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşyeri Sigortası
        /// </summary>
        public decimal WorkplaceInsurance { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Finansman Gideri
        /// </summary>
        public decimal FinancialExpense { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Elektrik ve Su Gideri
        /// </summary>
        public decimal ElectricityAndWaterExpense { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// MUHTELİF Dolaylı Malzemeler
        /// </summary>
        public decimal VariousIndirectMaterials { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// MUHTELİF Direct Malzemeler
        /// </summary>
        public decimal VariousDirectMaterials { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Malzeme Satınalma Toplamı
        /// </summary>
        public decimal MaterialPurchaseTotal { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Genel Yönetim Giderleri
        /// </summary>
        public decimal AnnualGeneralManagementExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Bitmiş Ürün Depolama Giderleri
        /// </summary>
        public decimal FinishedProductStorageExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Dağıtım Giderleri Oranı
        /// </summary>
        public decimal AnnualDistributionExpenseRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Üretim Giderleri 
        /// </summary>
        public decimal AnnualProductionExpenses { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Yıllık Ciro
        /// </summary>
        public decimal AnnualTurnover { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// SGA
        /// </summary>
        public decimal SGA { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
