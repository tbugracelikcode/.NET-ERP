﻿namespace TsiErp.DashboardUI.Models
{
    public class AdminProductGroupPerformanceAnalysisChart
    {
        public string Month { get; set; }
        public int THmonth { get; set; }
        public int Year { get; set; }
        public decimal Performance { get; set; }
        public decimal DIFFPER { get; set; }
        public int PLANNEDUNITTIME { get; set; }
        public decimal OCCUREDUNITTIME { get; set; }


    }
}
