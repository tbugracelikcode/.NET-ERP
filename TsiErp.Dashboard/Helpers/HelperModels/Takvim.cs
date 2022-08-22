namespace TsiErp.Dashboard.Helpers.HelperModels
{
    public class Takvim
    {
        public int? ID { get; set; }
        public int? TAKVIMID { get; set; }
        public int? ISTASYONID { get; set; }
        public DateTime? TARIH { get; set; }
        public string CALISMADURUMU { get; set; }
        public string GUNDUZYARIMGUN { get; set; }
        public string GUNDUZVARDIYASI { get; set; }
        public string GUNDUZFAZLAMESAI { get; set; }
        public int? GUNDUZMESAISURESI { get; set; }
        public int? GUNDUZFAZLAMESAISURESI { get; set; }
        public int? GUNDUZPLNDURUSSURESI { get; set; }
        public string GECEYARIMGUN { get; set; }
        public string GECEVARDIYASI { get; set; }
        public string GECEFAZLAMESAI { get; set; }
        public int? GECEMESAISURESI { get; set; }
        public int? GECEFAZLAMESAISURESI { get; set; }
        public int? GECEPLNDURUSSURESI { get; set; }
        public string BAKIMDURUMU { get; set; }
        public string PLANLIBAKIMVARDIYASI { get; set; }
        public int? BAKIMSURESI { get; set; }
        public int? GUNDUZTOPLAMCALISMAZAMANI { get; set; }
        public int? GECETOPLAMCALISMAZAMANI { get; set; }
        public int? TOPLAMCALISABILIRSURE { get; set; }
        public string PLANLANAN { get; set; }


    }
}
