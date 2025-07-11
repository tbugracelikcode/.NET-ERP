﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class EmployeeGeneralAnalysis
    {
        /// <summary>
        /// Personel ID 
        /// </summary>
        public int EmployeeID { get; set; }
        /// <summary>
        /// Personel Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Çalışma Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Üretim Adet 
        /// </summary>
        public decimal TotalProduction { get; set; }
        /// <summary>
        /// Departman 
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public decimal TotalScrap { get; set; }
        /// <summary>
        /// Gerçekleşen Birim Süre
        /// </summary>
        public decimal OccuredUnitTime { get; set; }
        /// <summary>
        /// Planlanan Adet 
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Planlanan Birim Süre
        /// </summary>
        public decimal PlannedOperationTime { get; set; }
        /// <summary>
        /// Performans 
        /// </summary>
        public decimal Performance { get; set; }
        /// <summary>
        /// Kullanılabilirlik 
        /// </summary>
        public decimal Availability { get; set; }
        /// <summary>
        /// Kalite 
        /// </summary>
        public decimal Quality { get; set; }
        /// <summary>
        /// OEE
        /// </summary>
        public decimal OEE { get; set; }
    }
}
