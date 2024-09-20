using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos
{
    public class ListCostPeriodsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Hesaplama Başlangıç Tarihi
        /// </summary>
        public DateTime? CalculationStartDate { get; set; }
        /// <summary>
        /// Hesaplama Bitiş Tarihi
        /// </summary>
        public DateTime? CalculationEndDate { get; set; }
        /// <summary>
        /// Toplam Üretim Adedi
        /// </summary>
        public decimal TotalProductionQuantity { get; set; }
        /// <summary>
        /// Yıllık Çalışma Günü
        /// </summary>
        public decimal AnnualWorkingDay { get; set; }
        /// <summary>
        /// Vardiya
        /// </summary>
        public decimal Shift { get; set; }
        /// <summary>
        /// Günlük Brüt Çalışma Saati
        /// </summary>
        public decimal DailyGrossWorkingHours { get; set; }
        /// <summary>
        /// Yıllık Çalışma Saati
        /// </summary>
        public decimal AnnualWorkingHours { get; set; }
        /// <summary>
        /// Aylık 1 Vardiya Çalışma Saati
        /// </summary>
        public decimal MontlyShiftWorkingHour { get; set; }
        /// <summary>
        /// Vardiya Çalışma Zamanı
        /// </summary>
        public decimal ShiftWorkingTime { get; set; }
        /// <summary>
        /// Aylık Kira ve Aidat Bedeli
        /// </summary>
        public decimal MonthlyRentAndDues { get; set; }
        /// <summary>
        /// Toplam Kiralanan Alan
        /// </summary>
        public decimal TotalRentedArea { get; set; }
        /// <summary>
        /// EUR Kur Tutarı
        /// </summary>
        public decimal EURExchangeRate { get; set; }
        /// <summary>
        /// Genel Yönetim Giderleri
        /// </summary>
        public decimal GeneralManagementExpenses { get; set; }
        /// <summary>
        /// İşçi Sosyal Yardım Giderleri
        /// </summary>
        public decimal EmployeeSocialAssistanceExpenses { get; set; }
        /// <summary>
        /// İşçi İş Elbiseleri Giderleri
        /// </summary>
        public decimal WorkClothesExpenses { get; set; }
        /// <summary>
        /// Pazarlama Gideri
        /// </summary>
        public decimal MarketingExpense { get; set; }
        /// <summary>
        /// Servis Gideri
        /// </summary>
        public decimal ServiceExpense { get; set; }
        /// <summary>
        /// Yemek Gideri
        /// </summary>
        public decimal FoodExpense { get; set; }
        /// <summary>
        /// İşyeri Sigortası
        /// </summary>
        public decimal WorkplaceInsurance { get; set; }
        /// <summary>
        /// Finansman Gideri
        /// </summary>
        public decimal FinancialExpense { get; set; }
        /// <summary>
        /// Elektrik ve Su Gideri
        /// </summary>
        public decimal ElectricityAndWaterExpense { get; set; }
        /// <summary>
        /// MUHTELİF Dolaylı Malzemeler
        /// </summary>
        public decimal VariousIndirectMaterials { get; set; }
        /// <summary>
        /// MUHTELİF Direct Malzemeler
        /// </summary>
        public decimal VariousDirectMaterials { get; set; }
        /// <summary>
        /// Malzeme Satınalma Toplamı
        /// </summary>
        public decimal MaterialPurchaseTotal { get; set; }
        /// <summary>
        /// Yıllık Genel Yönetim Giderleri
        /// </summary>
        public decimal AnnualGeneralManagementExpenses { get; set; }
        /// <summary>
        /// Bitmiş Ürün Depolama Giderleri
        /// </summary>
        public decimal FinishedProductStorageExpenses { get; set; }
        /// <summary>
        /// Yıllık Dağıtım Giderleri Oranı
        /// </summary>
        public decimal AnnualDistributionExpenseRate { get; set; }
        /// <summary>
        /// Yıllık Üretim Giderleri 
        /// </summary>
        public decimal AnnualProductionExpenses { get; set; }
        /// <summary>
        /// Yıllık Ciro
        /// </summary>
        public decimal AnnualTurnover { get; set; }
        /// <summary>
        /// SGA
        /// </summary>
        public decimal SGA { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
