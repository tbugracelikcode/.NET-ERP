namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class TederikciUygunsuzluk
    {
        public int ID { get; set; }
        public DateTime TARIH { get; set; }
        public int SIPARISID { get; set; }
        public int CARIID { get; set; }
        public int STOKID { get; set; }
        public bool TEDARIKCIIRTIBAT { get; set; }
        public string ACIKLAMA { get; set; }
        public string FISNO { get; set; }
        public string CARIKOD { get; set; }
        public string CARIUNVAN { get; set; }
        public string ESKISTOKKODU { get; set; }
        public string STOKACIKLAMASI { get; set; }
        public string HATAACIKLAMA { get; set; }
        public int UYGUNOLMAYANMIKTAR { get; set; }
        public bool RED { get; set; }
        public bool DUZELTME { get; set; }
        public bool OLDUGUGIBIKULLANILACAK { get; set; }
        public string RAPORNO { get; set; }
        public int HATAID { get; set; }
    }
}
