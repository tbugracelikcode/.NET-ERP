namespace TsiErp.DashboardUI.Models
{
    public class AdminCommonOEEMachineAnalysisChart
    {
        public int ID { get; set; }
        public int ISTASYONID { get; set; }
        public decimal URETILENADET { get; set; }
        public decimal HURDAADET { get; set; }
        public decimal OPERASYONSURESI { get; set; }
        public decimal AYARSURESI { get; set; }
        public decimal ATILSURE { get; set; }
        public decimal OEE { get; set; }
        public decimal KALITE { get; set; }
        public DateTime TARIH { get; set; }
        public int ROTAID { get; set; }
        public int OPERASYONID { get; set; }
        public string MAKINEKODU { get; set; }
        public decimal BIRIMSURE { get; set; }
        public decimal VARDIYACALISMASURESI { get; set; }
        public decimal PLNMIKTAR { get; set; }
        public int PLANLANANOPRSURESI { get; set; }
        public int OPRID { get; set; }
        public decimal PERFORMANS { get; set; }
        public decimal KULLANILABILIRLIK { get; set; }
        public decimal ISLEMESURESI { get; set; }
        public decimal GRCMIKTAR { get; set; }

        public int? TAKVIMID { get; set; }
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
