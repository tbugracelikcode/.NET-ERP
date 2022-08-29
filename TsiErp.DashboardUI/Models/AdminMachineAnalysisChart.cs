namespace TsiErp.DashboardUI.Models
{
    public class AdminMachineAnalysisChart
    {
        /// <summary>
        /// Makina Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// OEE
        /// </summary>
        public decimal OEE { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public int Date { get; set; }
    }
}
