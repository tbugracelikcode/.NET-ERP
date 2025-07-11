﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos
{
    public class ListCalendarsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çalışma Takvimi Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Çalışma Takvimi Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Planlanan mı?
        /// </summary>
        public bool IsPlanned { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Toplam Gün Sayısı
        /// </summary>
        public decimal TotalDays { get; set; }
        /// <summary>
        /// Resmi Tatil Sayısı
        /// </summary>
        public decimal OfficialHolidayDays { get; set; }
        /// <summary>
        /// Çalışılabilir Gün Sayısı
        /// </summary>
        public decimal AvailableDays { get; set; }
    }
}
