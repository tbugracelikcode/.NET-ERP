namespace TsiErp.DashboardUI.Models.Vsm
{
    public class InformationBoxModel
    {
        /// <summary>
        /// İşleme Zamanı
        /// </summary>
        public int ProcessingTime { get; set; }

        /// <summary>
        /// Çevrim Zamanı
        /// </summary>
        public int CycleTime { get; set; }

        /// <summary>
        /// Vardiya
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        /// Fire
        /// </summary>
        public decimal Wastage { get; set; }

        /// <summary>
        /// İş İstasyonu Departmanları
        /// </summary>
        public List<StationDepartments> StationDepartments { get; set; }

        /// <summary>
        /// İş İstasyonları
        /// </summary>
        public List<Stations> Stations { get; set; }

        public InformationBoxModel()
        {
            StationDepartments = new List<StationDepartments>();
            Stations = new List<Stations>();
        }
    }
}
