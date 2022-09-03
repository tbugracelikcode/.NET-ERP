namespace TsiErp.DashboardUI.Helpers.HelperModels
{
    public class OperasyonUygunsuzluk
    {
        public int ID { get; set; }
        public DateTime TARIH { get; set; }
        public int ISTASYONID { get; set; }
        public int URETIMEMRIID { get; set; }
        public int ISEMRIID { get; set; }
        public int CALISANID { get; set; }
        public int STOKID { get; set; }
        public string ISEMRINO { get; set; }
        public string FISNO { get; set; }
        public string CALISANAD { get; set; }
        public string MAKINEADI { get; set; }
        public string MAKINEKODU { get; set; }
        public string ESKISTOKKODU { get; set; }
        public string STOKACIKLAMASI { get; set; }
        public string HATAACIKLAMA { get; set; }
        public int OLCUKONTROLFORMBEYAN { get; set; }
        public bool HURDA { get; set; }
        public bool DUZELTME { get; set; }
        public bool OLDUGUGIBIKULLANILACAK { get; set; }
        public string RAPORNO { get; set; }
        public string ACIKLAMADETAY { get; set; }
        public bool ISTVERIMLILIIKANALIZI { get; set; }
        public bool PERVERIMLILIKANALIZI { get; set; }
        public string KOD { get; set; }
        public int TUR { get; set; }
    }
}
