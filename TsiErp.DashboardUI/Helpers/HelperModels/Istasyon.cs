namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class Istasyon
    {
        public int ID { get; set; }
        public string MAKINEKODU { get; set; }
        public string MAKINEADI { get; set; }
        public int BOLUMID { get; set; }
        public int VARDIYA { get; set; }
        public decimal VARDIYACALISMASURESI { get; set; }
        public decimal TEORIKSURE { get; set; }
        public decimal MESAICALISMASURESI { get; set; }
        public decimal PLANLIDURUSSURESI { get; set; }
        public bool FASON { get; set; }
        public bool EKIPMAN { get; set; }
    }
}
