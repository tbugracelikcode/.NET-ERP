namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class SatinAlmaDetaylari
    {
        public int ID { get; set; }

        public int STOKID { get; set; }

        public string STOKKODU { get; set; }

        public string BIRIMSETKOD { get; set; }

        public int PARABIRIMIID { get; set; }

        public string PARABIRIMI { get; set; }

        public string CARIUNVAN { get; set; }

        public DateTime TARIH { get; set; }

        public string BIRIMFIYAT { get; set; }
        public decimal BIRIMFIYATDECIMAL { get; set; }

        public decimal IHTIYACMIKTARI { get; set; }

        public decimal TOPLAMFIYAT { get; set; }
    }
}
