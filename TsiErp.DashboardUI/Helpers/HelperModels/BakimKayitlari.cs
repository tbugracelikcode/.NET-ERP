namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class BakimKayitlari
    {
        public int ID { get; set; }
        public string KAYITNO { get; set; }
        public string BAKIMTURU { get; set; }
        public DateTime PLANLANANTARIH { get; set; }
        public int PLANLANANBAKIMSURESI { get; set; }
        public DateTime BASLANGICTARIHI { get; set; }
        public DateTime TAMAMLANMATARIHI { get; set; }
        public int ISTID { get; set; }
        public int PERIYOT { get; set; }
        public int KALANSURE { get; set; }
        public string DURUM { get; set; }
        public string BAKIMIYAPAN { get; set; }
        public int BAKIMSURESI { get; set; }
        public string NOT_ { get; set; }
        public bool GUNLUK { get; set; }

    }
}
