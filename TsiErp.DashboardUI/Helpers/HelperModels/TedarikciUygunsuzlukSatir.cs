namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class TedarikciUygunsuzlukSatir
    {
        public int ID { get; set; }
        public int SIPARISID { get; set; }
        public int CARIID { get; set; }
        public int SATIRTURU { get; set; }
        public int SATIRNR { get; set; }
        public int STOKID { get; set; }
        public int VARYANTID { get; set; }
        public int DEPOID { get; set; }
        public int TUR { get; set; }
        public string FISNO { get; set; }
        public DateTime TARIH { get; set; }
        public DateTime TEMINTARIHI { get; set; }
        public int BIRIMSETID { get; set; }
        public int PARABIRIMID { get; set; }
        public decimal ADET { get; set; }
        public decimal DEPOADET { get; set; }
        public int DURUM { get; set; }
        public int SIPARISKABULSATIRID { get; set; }
    }
}
