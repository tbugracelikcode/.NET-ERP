using Microsoft.Data.SqlClient;
using System.Data;
using TsiErp.DashboardUI.Helpers.HelperModels;

namespace TsiErp.DashboardUI.Helpers
{
    public static class DBHelper
    {

        public static SqlConnection GetSqlConnection()
        {
            SqlConnection _connection = new SqlConnection("Server=192.168.98.4;Database=TURERP;UID=sa;PWD=Logo1234567890;");

            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();

            return _connection;
        }

        public static List<Istasyon> GetStations()
        {
            List<Istasyon> stations = new List<Istasyon>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "MAKINEKODU, " +
                                  "MAKINEADI, " +
                                  "ISNULL(VARDIYA,0) as VARDIYA, " +
                                  "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
                                  "ISNULL(TEORIKSURE,0) as TEORIKSURE, " +
                                  "ISNULL(MESAICALISMASURESI,0) as MESAICALISMASURESI, " +
                                  "ISNULL(PLANLIDURUSSURESI,0) as PLANLIDURUSSURESI, " +
                                  "FASON, " +
                                  "EKIPMAN FROM TUR_IST";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                stations.Add(new Istasyon()
                {
                    BOLUMID = 0,
                    EKIPMAN = Convert.ToBoolean(reader["EKIPMAN"]),
                    FASON = Convert.ToBoolean(reader["FASON"]),
                    ID = Convert.ToInt32(reader["ID"]),
                    MAKINEADI = Convert.ToString(reader["MAKINEADI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    MESAICALISMASURESI = Convert.ToDecimal(reader["MESAICALISMASURESI"]),
                    PLANLIDURUSSURESI = Convert.ToDecimal(reader["PLANLIDURUSSURESI"]),
                    TEORIKSURE = Convert.ToDecimal(reader["TEORIKSURE"]),
                    VARDIYA = Convert.ToInt32(reader["VARDIYA"]),
                    VARDIYACALISMASURESI = Convert.ToDecimal(reader["VARDIYACALISMASURESI"])
                });
            }

            return stations;
        }

        public static List<OperasyonSatir> GetOperationLines()
        {
            List<OperasyonSatir> operasyonlar = new List<OperasyonSatir>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID," +
                                  "CALISANID, " +
                                  "VARDIYAID, " +
                                  "ISTASYONID, " +
                                  "ISNULL(URETILENADET,0) as URETILENADET, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "ISNULL(OPERASYONSURESI,0) as OPERASYONSURESI, " +
                                  "ISNULL(AYARSURESI,0) as AYARSURESI, " +
                                  "ISNULL(ATILSURE,0) as ATILSURE, " +
                                  "OPRBASLANGICTRH, " +
                                  "OPRBITISTRH, " +
                                  "ISNULL(OEE,0) as OEE, " +
                                  "TARIH," +
                                  "ISNULL(KALITE,0) as KALITE, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ROTAID, " +
                                  "VARYANTID, " +
                                  "STOKID, " +
                                  "SIPARISID, " +
                                  "URETIMEMRIID, " +
                                  "OPERASYONID, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(BIRIMSURE,0) as BIRIMSURE, " +
                                  "ISEMRIID, " +
                                  "ACIKLAMA, " +
                                  "ISNULL(VARDIYA,0) as VARDIYA, " +
                                  "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
                                  "ISEMRINO, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "ISNULL(AGIRLIK,0) as AGIRLIK, " +
                                  "CALISAN, " +
                                  "STOKKODU, " +
                                  "URUNGRPID, " +
                                  "URUNGRUBU, " +
                                  "ISNULL(AYARVEKONTROLSURESI,0) as AYARVEKONTROLSURESI, " +
                                  "ISNULL(PLANLANANOPRSURESI,0) as PLANLANANOPRSURESI, " +
                                  "OPRID, " +
                                  "ISNULL(PERFORMANS,0) as PERFORMANS, " +
                                  "ISNULL(KULLANILABILIRLIK,0) as KULLANILABILIRLIK, " +
                                  "ISNULL(ISLEMESURESI,0) as ISLEMESURESI" +
                                  " FROM TUR_VW_EKR_OPERASYON";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                operasyonlar.Add(new OperasyonSatir()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    VARDIYAID = Convert.ToInt32(reader["VARDIYAID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    URETILENADET = Convert.ToInt32(reader["URETILENADET"]),
                    HURDAADET = Convert.ToDecimal(reader["HURDAADET"]),
                    OPERASYONSURESI = Convert.ToDecimal(reader["OPERASYONSURESI"]),
                    AYARSURESI = Convert.ToDecimal(reader["AYARSURESI"]),
                    ATILSURE = Convert.ToDecimal(reader["ATILSURE"]),
                    OPRBASLANGICTRH = Convert.ToDateTime(reader["OPRBASLANGICTRH"]),
                    OPRBITISTRH = Convert.ToDateTime(reader["OPRBITISTRH"]),
                    OEE = Convert.ToDecimal(reader["OEE"]),
                    KALITE = Convert.ToDecimal(reader["KALITE"]),
                    MESAI = Convert.ToInt32(reader["MESAI"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    ROTAID = Convert.ToInt32(reader["ROTAID"]),
                    VARYANTID = Convert.ToInt32(reader["VARYANTID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    BIRIMSURE = Convert.ToDecimal(reader["BIRIMSURE"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    ACIKLAMA = Convert.ToString(reader["ACIKLAMA"]),
                    VARDIYA = Convert.ToInt32(reader["VARDIYA"]),
                    VARDIYACALISMASURESI = Convert.ToDecimal(reader["VARDIYACALISMASURESI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    AGIRLIK = Convert.ToDecimal(reader["AGIRLIK"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    URUNGRPID = Convert.ToInt32(reader["URUNGRPID"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUBU"]),
                    AYARVEKONTROLSURESI = Convert.ToInt32(reader["AYARVEKONTROLSURESI"]),
                    PLANLANANOPRSURESI = Convert.ToInt32(reader["PLANLANANOPRSURESI"]),
                    OPRID = Convert.ToInt32(reader["OPRID"]),
                    PERFORMANS = Convert.ToDecimal(reader["PERFORMANS"]),
                    KULLANILABILIRLIK = Convert.ToDecimal(reader["KULLANILABILIRLIK"]),
                    ISLEMESURESI = Convert.ToDecimal(reader["ISLEMESURESI"])
                });
            }

            return operasyonlar;
        }

        public static List<OperasyonSatir> GetOperationLinesQuery(DateTime startDate, DateTime endDate)
        {
            List<OperasyonSatir> operasyonlar = new List<OperasyonSatir>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID," +
                                  "ISNULL(CALISANID,0) as CALISANID, " +
                                  "ISNULL(VARDIYAID,0) as VARDIYAID, " +
                                  "ISNULL(ISTASYONID,0) as ISTASYONID, " +
                                  "ISNULL(URETILENADET,0) as URETILENADET, " +
                                   "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "ISNULL(OPERASYONSURESI,0) as OPERASYONSURESI, " +
                                  "ISNULL(AYARSURESI,0) as AYARSURESI, " +
                                  "ISNULL(ATILSURE,0) as ATILSURE, " +
                                  "OPRBASLANGICTRH, " +
                                  "OPRBITISTRH, " +
                                  "ISNULL(OEE,0) as OEE, " +
                                  "ISNULL(KALITE,0) as KALITE, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ISNULL(ROTAID,0) as ROTAID, " +
                                  "ISNULL(VARYANTID,0) as VARYANTID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(OPERASYONID,0) as OPERASYONID, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(BIRIMSURE,0) as BIRIMSURE, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ACIKLAMA, " +
                                  "TARIH," +
                                  "ISNULL(VARDIYA,0) as VARDIYA, " +
                                  "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
                                  "ISEMRINO, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "ISNULL(AGIRLIK,0) as AGIRLIK, " +
                                  "CALISAN, " +
                                  "STOKKODU, " +
                                  "ISNULL(URUNGRPID,0) as URUNGRPID, " +
                                  "URUNGRUBU, " +
                                  "ISNULL(AYARVEKONTROLSURESI,0) as AYARVEKONTROLSURESI, " +
                                  "ISNULL(PLANLANANOPRSURESI,0) as PLANLANANOPRSURESI, " +
                                  "ISNULL(AGIRLIK,0) as OPRID, " +
                                  "ISNULL(PERFORMANS,0) as PERFORMANS, " +
                                  "ISNULL(KULLANILABILIRLIK,0) as KULLANILABILIRLIK, " +
                                  "ISNULL(ISLEMESURESI,0) as ISLEMESURESI," +
                                  "ISNULL(DEPARTMAN,0) as DEPARTMAN," +
                                  "ISNULL(GRCMIKTAR,0) as GRCMIKTAR" +
                                  " FROM TUR_VW_EKR_OPERASYON " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                operasyonlar.Add(new OperasyonSatir()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    VARDIYAID = Convert.ToInt32(reader["VARDIYAID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    URETILENADET = Convert.ToInt32(reader["URETILENADET"]),
                    HURDAADET = Convert.ToDecimal(reader["HURDAADET"]),
                    OPERASYONSURESI = Convert.ToDecimal(reader["OPERASYONSURESI"]),
                    AYARSURESI = Convert.ToDecimal(reader["AYARSURESI"]),
                    ATILSURE = Convert.ToDecimal(reader["ATILSURE"]),
                    OPRBASLANGICTRH = Convert.ToDateTime(reader["OPRBASLANGICTRH"]),
                    OPRBITISTRH = Convert.ToDateTime(reader["OPRBITISTRH"]),
                    OEE = Convert.ToDecimal(reader["OEE"]),
                    KALITE = Convert.ToDecimal(reader["KALITE"]),
                    MESAI = Convert.ToInt32(reader["MESAI"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    ROTAID = Convert.ToInt32(reader["ROTAID"]),
                    VARYANTID = Convert.ToInt32(reader["VARYANTID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    BIRIMSURE = Convert.ToDecimal(reader["BIRIMSURE"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    ACIKLAMA = Convert.ToString(reader["ACIKLAMA"]),
                    VARDIYA = Convert.ToInt32(reader["VARDIYA"]),
                    VARDIYACALISMASURESI = Convert.ToDecimal(reader["VARDIYACALISMASURESI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    AGIRLIK = Convert.ToDecimal(reader["AGIRLIK"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    URUNGRPID = Convert.ToInt32(reader["URUNGRPID"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUBU"]),
                    AYARVEKONTROLSURESI = Convert.ToInt32(reader["AYARVEKONTROLSURESI"]),
                    PLANLANANOPRSURESI = Convert.ToInt32(reader["PLANLANANOPRSURESI"]),
                    DEPARTMAN = Convert.ToString(reader["DEPARTMAN"]),
                    OPRID = Convert.ToInt32(reader["OPRID"]),
                    PERFORMANS = Convert.ToDecimal(reader["PERFORMANS"]),
                    KULLANILABILIRLIK = Convert.ToDecimal(reader["KULLANILABILIRLIK"]),
                    ISLEMESURESI = Convert.ToDecimal(reader["ISLEMESURESI"]),
                    GRCMIKTAR = Convert.ToDecimal(reader["GRCMIKTAR"])
                });
            }

            return operasyonlar;
        }

        public static List<OperasyonSatir> GetOperationLinesStationQuery(int stationID, DateTime startDate, DateTime endDate)
        {
            List<OperasyonSatir> operasyonlar = new List<OperasyonSatir>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID," +
                                  "CALISANID, " +
                                  "VARDIYAID, " +
                                  "ISTASYONID, " +
                                  "ISNULL(URETILENADET,0) as URETILENADET, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "ISNULL(OPERASYONSURESI,0) as OPERASYONSURESI, " +
                                  "ISNULL(AYARSURESI,0) as AYARSURESI, " +
                                  "ISNULL(ATILSURE,0) as ATILSURE, " +
                                  "OPRBASLANGICTRH, " +
                                  "OPRBITISTRH, " +
                                  "ISNULL(OEE,0) as OEE, " +
                                  "ISNULL(KALITE,0) as KALITE, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ROTAID, " +
                                  "VARYANTID, " +
                                  "STOKID, " +
                                  "SIPARISID, " +
                                  "URETIMEMRIID, " +
                                  "OPERASYONID, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(BIRIMSURE,0) as BIRIMSURE, " +
                                  "ISEMRIID, " +
                                  "ACIKLAMA, " +
                                  "TARIH," +
                                  "ISNULL(VARDIYA,0) as VARDIYA, " +
                                  "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
                                  "ISEMRINO, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "ISNULL(AGIRLIK,0) as AGIRLIK, " +
                                  "CALISAN, " +
                                  "STOKKODU, " +
                                  "URUNGRPID, " +
                                  "URUNGRUBU, " +
                                  "ISNULL(AYARVEKONTROLSURESI,0) as AYARVEKONTROLSURESI, " +
                                  "ISNULL(PLANLANANOPRSURESI,0) as PLANLANANOPRSURESI, " +
                                  "OPRID, " +
                                  "ISNULL(PERFORMANS,0) as PERFORMANS, " +
                                  "ISNULL(KULLANILABILIRLIK,0) as KULLANILABILIRLIK, " +
                                  "ISNULL(ISLEMESURESI,0) as ISLEMESURESI," +
                                  "ISNULL(STOKTURU,0) as STOKTURU" +
                                  " FROM TUR_VW_EKR_OPERASYON " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "' AND ISTASYONID = " + stationID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                operasyonlar.Add(new OperasyonSatir()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    VARDIYAID = Convert.ToInt32(reader["VARDIYAID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    URETILENADET = Convert.ToInt32(reader["URETILENADET"]),
                    HURDAADET = Convert.ToDecimal(reader["HURDAADET"]),
                    OPERASYONSURESI = Convert.ToDecimal(reader["OPERASYONSURESI"]),
                    AYARSURESI = Convert.ToDecimal(reader["AYARSURESI"]),
                    ATILSURE = Convert.ToDecimal(reader["ATILSURE"]),
                    OPRBASLANGICTRH = Convert.ToDateTime(reader["OPRBASLANGICTRH"]),
                    OPRBITISTRH = Convert.ToDateTime(reader["OPRBITISTRH"]),
                    OEE = Convert.ToDecimal(reader["OEE"]),
                    KALITE = Convert.ToDecimal(reader["KALITE"]),
                    MESAI = Convert.ToInt32(reader["MESAI"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    ROTAID = Convert.ToInt32(reader["ROTAID"]),
                    VARYANTID = Convert.ToInt32(reader["VARYANTID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    BIRIMSURE = Convert.ToDecimal(reader["BIRIMSURE"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    ACIKLAMA = Convert.ToString(reader["ACIKLAMA"]),
                    VARDIYA = Convert.ToInt32(reader["VARDIYA"]),
                    VARDIYACALISMASURESI = Convert.ToDecimal(reader["VARDIYACALISMASURESI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    AGIRLIK = Convert.ToDecimal(reader["AGIRLIK"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    URUNGRPID = Convert.ToInt32(reader["URUNGRPID"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUBU"]),
                    AYARVEKONTROLSURESI = Convert.ToInt32(reader["AYARVEKONTROLSURESI"]),
                    PLANLANANOPRSURESI = Convert.ToInt32(reader["PLANLANANOPRSURESI"]),
                    OPRID = Convert.ToInt32(reader["OPRID"]),
                    PERFORMANS = Convert.ToDecimal(reader["PERFORMANS"]),
                    KULLANILABILIRLIK = Convert.ToDecimal(reader["KULLANILABILIRLIK"]),
                    ISLEMESURESI = Convert.ToDecimal(reader["ISLEMESURESI"]),
                    STOKTURU = Convert.ToInt32(reader["STOKTURU"])
                });
            }

            return operasyonlar;
        }

        public static List<Takvim> GetCalendar()
        {
            List<Takvim> calendarLines = new List<Takvim>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "TAKVIMID, " +
                                  "ISTASYONID, " +
                                  "TARIH, " +
                                  "CALISMADURUMU, " +
                                  "GUNDUZYARIMGUN, " +
                                  "GUNDUZVARDIYASI, " +
                                  "GUNDUZFAZLAMESAI, " +
                                  "ISNULL(GUNDUZMESAISURESI,0) as GUNDUZMESAISURESI, " +
                                  "ISNULL(GUNDUZFAZLAMESAISURESI,0) as GUNDUZFAZLAMESAISURESI, " +
                                  "ISNULL(GUNDUZPLNDURUSSURESI,0) as GUNDUZPLNDURUSSURESI, " +
                                  "GECEYARIMGUN, " +
                                  "GECEVARDIYASI, " +
                                  "GECEFAZLAMESAI, " +
                                  "ISNULL(GECEMESAISURESI,0) as GECEMESAISURESI, " +
                                  "ISNULL(GECEFAZLAMESAISURESI,0) as GECEFAZLAMESAISURESI, " +
                                  "ISNULL(GECEPLNDURUSSURESI,0) as GECEPLNDURUSSURESI, " +
                                  "BAKIMDURUMU, " +
                                  "PLANLIBAKIMVARDIYASI, " +
                                  "ISNULL(BAKIMSURESI,0) as BAKIMSURESI, " +
                                  "ISNULL(GUNDUZTOPLAMCALISMAZAMANI,0) as GUNDUZTOPLAMCALISMAZAMANI, " +
                                  "ISNULL(GECETOPLAMCALISMAZAMANI,0) as GECETOPLAMCALISMAZAMANI, " +
                                  "ISNULL(TOPLAMCALISABILIRSURE,0) as TOPLAMCALISABILIRSURE, " +
                                  "PLANLANAN " +
                                  "FROM TUR_IST_CALISMA_TAKVIMI_SATIRLAR_YENI";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                calendarLines.Add(new Takvim()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    TAKVIMID = Convert.ToInt32(reader["TAKVIMID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    CALISMADURUMU = Convert.ToString(reader["CALISMADURUMU"]),
                    GUNDUZYARIMGUN = Convert.ToString(reader["GUNDUZYARIMGUN"]),
                    GUNDUZVARDIYASI = Convert.ToString(reader["GUNDUZVARDIYASI"]),
                    GUNDUZFAZLAMESAI = Convert.ToString(reader["GUNDUZFAZLAMESAI"]),
                    GUNDUZMESAISURESI = Convert.ToInt32(reader["GUNDUZMESAISURESI"]),
                    GUNDUZFAZLAMESAISURESI = Convert.ToInt32(reader["GUNDUZFAZLAMESAISURESI"]),
                    GUNDUZPLNDURUSSURESI = Convert.ToInt32(reader["GUNDUZPLNDURUSSURESI"]),
                    GECEYARIMGUN = Convert.ToString(reader["GECEYARIMGUN"]),
                    GECEVARDIYASI = Convert.ToString(reader["GECEVARDIYASI"]),
                    GECEFAZLAMESAI = Convert.ToString(reader["GECEFAZLAMESAI"]),
                    GECEMESAISURESI = Convert.ToInt32(reader["GECEMESAISURESI"]),
                    GECEFAZLAMESAISURESI = Convert.ToInt32(reader["GECEFAZLAMESAISURESI"]),
                    GECEPLNDURUSSURESI = Convert.ToInt32(reader["GECEPLNDURUSSURESI"]),
                    BAKIMDURUMU = Convert.ToString(reader["BAKIMDURUMU"]),
                    PLANLIBAKIMVARDIYASI = Convert.ToString(reader["PLANLIBAKIMVARDIYASI"]),
                    BAKIMSURESI = Convert.ToInt32(reader["BAKIMSURESI"]),
                    GUNDUZTOPLAMCALISMAZAMANI = Convert.ToInt32(reader["GUNDUZTOPLAMCALISMAZAMANI"]),
                    GECETOPLAMCALISMAZAMANI = Convert.ToInt32(reader["GECETOPLAMCALISMAZAMANI"]),
                    TOPLAMCALISABILIRSURE = Convert.ToInt32(reader["TOPLAMCALISABILIRSURE"]),
                    PLANLANAN = Convert.ToString(reader["PLANLANAN"])
                });
            }

            return calendarLines;
        }

        public static List<Takvim> GetCalendarQuery(DateTime startDate, DateTime endDate)
        {
            List<Takvim> calendarLines = new List<Takvim>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "TAKVIMID, " +
                                  "ISTASYONID, " +
                                  "TARIH, " +
                                  "CALISMADURUMU, " +
                                  "GUNDUZYARIMGUN, " +
                                  "GUNDUZVARDIYASI, " +
                                  "GUNDUZFAZLAMESAI, " +
                                  "ISNULL(GUNDUZMESAISURESI,0) as GUNDUZMESAISURESI, " +
                                  "ISNULL(GUNDUZFAZLAMESAISURESI,0) as GUNDUZFAZLAMESAISURESI, " +
                                  "ISNULL(GUNDUZPLNDURUSSURESI,0) as GUNDUZPLNDURUSSURESI, " +
                                  "GECEYARIMGUN, " +
                                  "GECEVARDIYASI, " +
                                  "GECEFAZLAMESAI, " +
                                  "ISNULL(GECEMESAISURESI,0) as GECEMESAISURESI, " +
                                  "ISNULL(GECEFAZLAMESAISURESI,0) as GECEFAZLAMESAISURESI, " +
                                  "ISNULL(GECEPLNDURUSSURESI,0) as GECEPLNDURUSSURESI, " +
                                  "BAKIMDURUMU, " +
                                  "PLANLIBAKIMVARDIYASI, " +
                                  "ISNULL(BAKIMSURESI,0) as BAKIMSURESI, " +
                                  "ISNULL(GUNDUZTOPLAMCALISMAZAMANI,0) as GUNDUZTOPLAMCALISMAZAMANI, " +
                                  "ISNULL(GECETOPLAMCALISMAZAMANI,0) as GECETOPLAMCALISMAZAMANI, " +
                                  "ISNULL(TOPLAMCALISABILIRSURE,0) as TOPLAMCALISABILIRSURE, " +
                                  "PLANLANAN " +
                                  "FROM TUR_IST_CALISMA_TAKVIMI_SATIRLAR_YENI " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                calendarLines.Add(new Takvim()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    TAKVIMID = Convert.ToInt32(reader["TAKVIMID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    CALISMADURUMU = Convert.ToString(reader["CALISMADURUMU"]),
                    GUNDUZYARIMGUN = Convert.ToString(reader["GUNDUZYARIMGUN"]),
                    GUNDUZVARDIYASI = Convert.ToString(reader["GUNDUZVARDIYASI"]),
                    GUNDUZFAZLAMESAI = Convert.ToString(reader["GUNDUZFAZLAMESAI"]),
                    GUNDUZMESAISURESI = Convert.ToInt32(reader["GUNDUZMESAISURESI"]),
                    GUNDUZFAZLAMESAISURESI = Convert.ToInt32(reader["GUNDUZFAZLAMESAISURESI"]),
                    GUNDUZPLNDURUSSURESI = Convert.ToInt32(reader["GUNDUZPLNDURUSSURESI"]),
                    GECEYARIMGUN = Convert.ToString(reader["GECEYARIMGUN"]),
                    GECEVARDIYASI = Convert.ToString(reader["GECEVARDIYASI"]),
                    GECEFAZLAMESAI = Convert.ToString(reader["GECEFAZLAMESAI"]),
                    GECEMESAISURESI = Convert.ToInt32(reader["GECEMESAISURESI"]),
                    GECEFAZLAMESAISURESI = Convert.ToInt32(reader["GECEFAZLAMESAISURESI"]),
                    GECEPLNDURUSSURESI = Convert.ToInt32(reader["GECEPLNDURUSSURESI"]),
                    BAKIMDURUMU = Convert.ToString(reader["BAKIMDURUMU"]),
                    PLANLIBAKIMVARDIYASI = Convert.ToString(reader["PLANLIBAKIMVARDIYASI"]),
                    BAKIMSURESI = Convert.ToInt32(reader["BAKIMSURESI"]),
                    GUNDUZTOPLAMCALISMAZAMANI = Convert.ToInt32(reader["GUNDUZTOPLAMCALISMAZAMANI"]),
                    GECETOPLAMCALISMAZAMANI = Convert.ToInt32(reader["GECETOPLAMCALISMAZAMANI"]),
                    TOPLAMCALISABILIRSURE = Convert.ToInt32(reader["TOPLAMCALISABILIRSURE"]),
                    PLANLANAN = Convert.ToString(reader["PLANLANAN"])
                });
            }

            return calendarLines;
        }

        public static List<Durus> GetHalt()
        {
            List<Durus> haltLines = new List<Durus>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "DURUSID, " +
                                  "SEBEP, " +
                                  "ISNULL(DURUSSURE,0) as DURUSSURE, " +
                                  "BASLANGIC, " +
                                  "BITIS, " +
                                  "CALISANID, " +
                                  "OPERASYONID, " +
                                  "ISTASYONID, " +
                                  "UNUTULDU, " +
                                  "TARIH, " +
                                  "ISNULL(OPR_TOPLAM_DURUS,0) as OPR_TOPLAM_DURUS, " +
                                  "MESAI, " +
                                  "ISEMRINO, " +
                                  "OPERASYONACIKLAMASI, " +
                                  "MAKINEKODU, " +
                                  "STOKKODU, " +
                                  "VARYANTKODU, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISEMRIID, " +
                                  "URETIMEMRIID, " +
                                  "PLANLI, " +
                                  "CALISAN, " +
                                  "ISNULL(DURUS_ORANI,0) as DURUS_ORANI " +
                                  "YKK " +
                                  "FROM TUR_VW_EKR_DURUS";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                haltLines.Add(new Durus()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    DURUSID = Convert.ToInt32(reader["DURUSID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    DURUSSURE = Convert.ToInt32(reader["DURUSSURE"]),
                    BASLANGIC = Convert.ToDateTime(reader["BASLANGIC"]),
                    BITIS = Convert.ToDateTime(reader["BITIS"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    UNUTULDU = Convert.ToString(reader["UNUTULDU"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    OPR_TOPLAM_DURUS = Convert.ToDecimal(reader["OPR_TOPLAM_DURUS"]),
                    MESAI = Convert.ToString(reader["MESAI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    PLANLI = Convert.ToString(reader["PLANLI"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    DURUS_ORANI = Convert.ToDecimal(reader["DURUS_ORANI"]),
                    YKK = Convert.ToBoolean(reader["YKK"])
                });
            }

            return haltLines;
        }

        public static List<Durus> GetHaltQuery(DateTime startDate, DateTime endDate)
        {
            List<Durus> haltLines = new List<Durus>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "DURUSID, " +
                                  "SEBEP, " +
                                  "DURUSSURE, " +
                                  "BASLANGIC, " +
                                  "BITIS, " +
                                  "CALISANID, " +
                                  "OPERASYONID, " +
                                  "ISTASYONID, " +
                                  "UNUTULDU, " +
                                  "TARIH, " +
                                  "ISNULL(OPR_TOPLAM_DURUS,0) as OPR_TOPLAM_DURUS, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ISNULL(ISEMRINO,0) as ISEMRINO, " +
                                  "ISNULL(OPERASYONACIKLAMASI,0) as OPERASYONACIKLAMASI, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(STOKKODU,0) as STOKKODU, " +
                                  "ISNULL(VARYANTKODU,0) as VARYANTKODU, " +
                                  "ISNULL(URETIMEMRINUMARASI,0) as URETIMEMRINUMARASI, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(PLANLI,0) as PLANLI, " +
                                  "ISNULL(CALISAN,0) as CALISAN, " +
                                  "ISNULL(DURUS_ORANI,0) as  DURUS_ORANI," +
                                  "ISNULL(YKK,0) as  YKK " +
                                  "FROM TUR_VW_EKR_DURUS " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                haltLines.Add(new Durus()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    DURUSID = Convert.ToInt32(reader["DURUSID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    DURUSSURE = Convert.ToInt32(reader["DURUSSURE"]),
                    BASLANGIC = Convert.ToDateTime(reader["BASLANGIC"]),
                    BITIS = Convert.ToDateTime(reader["BITIS"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    UNUTULDU = Convert.ToString(reader["UNUTULDU"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    OPR_TOPLAM_DURUS = Convert.ToDecimal(reader["OPR_TOPLAM_DURUS"]),
                    MESAI = Convert.ToString(reader["MESAI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    PLANLI = Convert.ToString(reader["PLANLI"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    DURUS_ORANI = Convert.ToDecimal(reader["DURUS_ORANI"]),
                    YKK = Convert.ToBoolean(reader["YKK"])
                });
            }

            return haltLines;
        }

        public static List<Durus> GetHaltQueryStation(int stationID, DateTime startDate, DateTime endDate)
        {
            List<Durus> haltLines = new List<Durus>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "DURUSID, " +
                                  "SEBEP, " +
                                  "DURUSSURE, " +
                                  "BASLANGIC, " +
                                  "BITIS, " +
                                  "CALISANID, " +
                                  "OPERASYONID, " +
                                  "ISTASYONID, " +
                                  "UNUTULDU, " +
                                  "TARIH, " +
                                  "ISNULL(OPR_TOPLAM_DURUS,0) as OPR_TOPLAM_DURUS, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ISNULL(ISEMRINO,0) as ISEMRINO, " +
                                  "ISNULL(OPERASYONACIKLAMASI,0) as OPERASYONACIKLAMASI, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(STOKKODU,0) as STOKKODU, " +
                                  "ISNULL(VARYANTKODU,0) as VARYANTKODU, " +
                                  "ISNULL(URETIMEMRINUMARASI,0) as URETIMEMRINUMARASI, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(PLANLI,0) as PLANLI, " +
                                  "ISNULL(CALISAN,0) as CALISAN, " +
                                  "ISNULL(DURUS_ORANI,0) as DURUS_ORANI," +
                                  "ISNULL(YKK,0) as YKK " +
                                  "FROM TUR_VW_EKR_DURUS " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "' AND ISTASYONID = " + stationID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                haltLines.Add(new Durus()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    DURUSID = Convert.ToInt32(reader["DURUSID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    DURUSSURE = Convert.ToInt32(reader["DURUSSURE"]),
                    BASLANGIC = Convert.ToDateTime(reader["BASLANGIC"]),
                    BITIS = Convert.ToDateTime(reader["BITIS"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    UNUTULDU = Convert.ToString(reader["UNUTULDU"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    OPR_TOPLAM_DURUS = Convert.ToDecimal(reader["OPR_TOPLAM_DURUS"]),
                    MESAI = Convert.ToString(reader["MESAI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    PLANLI = Convert.ToString(reader["PLANLI"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    DURUS_ORANI = Convert.ToDecimal(reader["DURUS_ORANI"]),
                    YKK = Convert.ToBoolean(reader["YKK"])
                });
            }

            return haltLines;
        }

        public static List<Durus> GetHaltQueryEmployee(int employeeID, DateTime startDate, DateTime endDate)
        {
            List<Durus> haltLines = new List<Durus>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "DURUSID, " +
                                  "SEBEP, " +
                                  "DURUSSURE, " +
                                  "BASLANGIC, " +
                                  "BITIS, " +
                                  "CALISANID, " +
                                  "OPERASYONID, " +
                                  "ISTASYONID, " +
                                  "UNUTULDU, " +
                                  "TARIH, " +
                                  "ISNULL(OPR_TOPLAM_DURUS,0) as OPR_TOPLAM_DURUS, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ISNULL(ISEMRINO,0) as ISEMRINO, " +
                                  "ISNULL(OPERASYONACIKLAMASI,0) as OPERASYONACIKLAMASI, " +
                                  "MAKINEKODU, " +
                                  "ISNULL(STOKKODU,0) as STOKKODU, " +
                                  "ISNULL(VARYANTKODU,0) as VARYANTKODU, " +
                                  "ISNULL(URETIMEMRINUMARASI,0) as URETIMEMRINUMARASI, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(PLANLI,0) as PLANLI, " +
                                  "ISNULL(CALISAN,0) as CALISAN, " +
                                  "ISNULL(DURUS_ORANI,0) as DURUS_ORANI," +
                                  "ISNULL(YKK,0) as YKK " +
                                  "FROM TUR_VW_EKR_DURUS " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "' AND CALISANID = " + employeeID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                haltLines.Add(new Durus()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    DURUSID = Convert.ToInt32(reader["DURUSID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    DURUSSURE = Convert.ToInt32(reader["DURUSSURE"]),
                    BASLANGIC = Convert.ToDateTime(reader["BASLANGIC"]),
                    BITIS = Convert.ToDateTime(reader["BITIS"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    UNUTULDU = Convert.ToString(reader["UNUTULDU"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    OPR_TOPLAM_DURUS = Convert.ToDecimal(reader["OPR_TOPLAM_DURUS"]),
                    MESAI = Convert.ToString(reader["MESAI"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    PLANLI = Convert.ToString(reader["PLANLI"]),
                    CALISAN = Convert.ToString(reader["CALISAN"]),
                    DURUS_ORANI = Convert.ToDecimal(reader["DURUS_ORANI"]),
                    YKK = Convert.ToBoolean(reader["YKK"])
                });
            }

            return haltLines;
        }

        public static List<KayipSureKodlari> GetHaltCodes()
        {
            List<KayipSureKodlari> haltCodes = new List<KayipSureKodlari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "KOD, " +
                                  "PLANLI, " +
                                  "YKK " +
                                  "FROM TUR_KAYIP_SURE_KODLARI";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                haltCodes.Add(new KayipSureKodlari()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    KOD = Convert.ToString(reader["KOD"]),
                    PLANLI = Convert.ToBoolean(reader["PLANLI"]),
                    YKK = Convert.ToBoolean(reader["YKK"])
                });
            }

            return haltCodes;
        }
        public static List<Hurda> GetScrapLines()
        {
            List<Hurda> hurdalar = new List<Hurda>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "HURDAID," +
                                  "SEBEP, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "OPERASYONID, " +
                                  "CALISANID, " +
                                  "ISEMRINO, " +
                                  "OPERASYONACIKLAMASI, " +
                                  "ISTASYONKODU, " +
                                  "STOKKODU, " +
                                  "VARYANTKODU, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "URUNGRUPADI, " +
                                  "ID," +
                                  "TARIH" +
                                  " FROM TUR_VW_EKR_HURDA";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                hurdalar.Add(new Hurda()
                {
                    HURDAID = Convert.ToInt32(reader["HURDAID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    HURDAADET = Convert.ToInt32(reader["HURDAADET"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    ISTASYONKODU = Convert.ToString(reader["ISTASYONKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUBU"]),
                    ID = Convert.ToInt32(reader["ID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"])
                });
            }

            return hurdalar;
        }

        public static List<Hurda> GetScrapLinesQuery(DateTime startDate, DateTime endDate)
        {
            List<Hurda> hurdalar = new List<Hurda>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "HURDAID," +
                                  "SEBEP, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "OPERASYONID, " +
                                  "CALISANID, " +
                                  "ISEMRINO, " +
                                  "OPERASYONACIKLAMASI, " +
                                  "ISTASYONKODU, " +
                                  "STOKKODU, " +
                                  "VARYANTKODU, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "URUNGRUPADI, " +
                                  "ID," +
                                  "ISNULL(URUNGRPID,0) as URUNGRPID," +
                                  "ISNULL(URUNGRUBU,0) as URUNGRUBU," +
                                  "TARIH" +
                                  " FROM TUR_VW_EKR_HURDA " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                hurdalar.Add(new Hurda()
                {
                    HURDAID = Convert.ToInt32(reader["HURDAID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    HURDAADET = Convert.ToInt32(reader["HURDAADET"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    ISTASYONKODU = Convert.ToString(reader["ISTASYONKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUPADI"]),
                    URUNGRPID = Convert.ToInt32(reader["URUNGRPID"]),
                    ID = Convert.ToInt32(reader["ID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"])
                });
            }

            return hurdalar;
        }

        public static List<Hurda> GetScrapLinesGroupedQuery(int groupID, DateTime startDate, DateTime endDate)
        {
            List<Hurda> hurdalar = new List<Hurda>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "HURDAID," +
                                  "SEBEP, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "OPERASYONID, " +
                                  "CALISANID, " +
                                  "ISEMRINO, " +
                                  "OPERASYONACIKLAMASI, " +
                                  "ISTASYONKODU, " +
                                  "STOKKODU, " +
                                  "VARYANTKODU, " +
                                  "URETIMEMRINUMARASI, " +
                                  "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
                                  "ID," +
                                  "ISNULL(URUNGRPID,0) as URUNGRPID," +
                                  "ISNULL(URUNGRUBU,0) as URUNGRUBU," +
                                  "TARIH" +
                                  " FROM TUR_VW_EKR_HURDA " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "' AND URUNGRPID = " + groupID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                hurdalar.Add(new Hurda()
                {
                    HURDAID = Convert.ToInt32(reader["HURDAID"]),
                    SEBEP = Convert.ToString(reader["SEBEP"]),
                    HURDAADET = Convert.ToInt32(reader["HURDAADET"]),
                    OPERASYONID = Convert.ToInt32(reader["OPERASYONID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    OPERASYONACIKLAMASI = Convert.ToString(reader["OPERASYONACIKLAMASI"]),
                    ISTASYONKODU = Convert.ToString(reader["ISTASYONKODU"]),
                    STOKKODU = Convert.ToString(reader["STOKKODU"]),
                    VARYANTKODU = Convert.ToString(reader["VARYANTKODU"]),
                    URETIMEMRINUMARASI = Convert.ToString(reader["URETIMEMRINUMARASI"]),
                    PLNMIKTAR = Convert.ToDecimal(reader["PLNMIKTAR"]),
                    URUNGRUBU = Convert.ToString(reader["URUNGRUBU"]),
                    URUNGRPID = Convert.ToInt32(reader["URUNGRPID"]),
                    ID = Convert.ToInt32(reader["ID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"])
                });
            }

            return hurdalar;
        }

        public static List<UygunsuzlukBasliklari> GetScrapCauses()
        {
            List<UygunsuzlukBasliklari> hurdaSebepleri = new List<UygunsuzlukBasliklari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID," +
                                  "ACIKLAMA, " +
                                  "ISNULL(TUR,0) as TUR" +
                                  " FROM TUR_KK_UYGUNSUZLUK_BASLIKLARI" +
                                  " WHERE TUR = 3";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                hurdaSebepleri.Add(new UygunsuzlukBasliklari()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    BASLIK = Convert.ToString(reader["ACIKLAMA"]),
                    TUR = Convert.ToInt32(reader["TUR"])
                });
            }

            return hurdaSebepleri;
        }

        public static List<OperasyonUygunsuzluk> GetUnsuitabilityQuery(DateTime startDate, DateTime endDate)
        {
            List<OperasyonUygunsuzluk> unsuitabilityLines = new List<OperasyonUygunsuzluk>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(ISTASYONID,0) as ISTASYONID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(CALISANID,0) as CALISANID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "TARIH, " +
                                  "ISEMRINO, " +
                                  "FISNO, " +
                                  "CALISANAD, " +
                                  "MAKINEADI, " +
                                  "MAKINEKODU, " +
                                  "ESKISTOKKODU, " +
                                  "STOKACIKLAMASI, " +
                                  "HATAACIKLAMA, " +
                                  "ISNULL(OLCUKONTROLFORMBEYAN,0) as OLCUKONTROLFORMBEYAN, " +
                                  "ISNULL(HURDA,0) as HURDA, " +
                                  "ISNULL(DUZELTME,0) as DUZELTME, " +
                                  "ISNULL(OLDUGUGIBIKULLANILACAK,0) as OLDUGUGIBIKULLANILACAK, " +
                                  "RAPORNO, " +
                                  "ACIKLAMADETAY, " +
                                  "ISNULL(ISTVERIMLILIIKANALIZI,0) as ISTVERIMLILIIKANALIZI, " +
                                  "ISNULL(PERVERIMLILIKANALIZI,0) as  PERVERIMLILIKANALIZI," +
                                  "ISNULL(TUR,0) as  TUR," +
                                  "KOD " +
                                  "FROM TUR_VW_OPERASYON_UYGUNSUZLUK " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new OperasyonUygunsuzluk()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    CALISANAD = Convert.ToString(reader["CALISANAD"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    MAKINEADI = Convert.ToString(reader["MAKINEADI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    ESKISTOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    STOKACIKLAMASI = Convert.ToString(reader["STOKACIKLAMASI"]),
                    HATAACIKLAMA = Convert.ToString(reader["HATAACIKLAMA"]),
                    OLCUKONTROLFORMBEYAN = Convert.ToInt32(reader["OLCUKONTROLFORMBEYAN"]),
                    HURDA = Convert.ToBoolean(reader["HURDA"]),
                    DUZELTME = Convert.ToBoolean(reader["DUZELTME"]),
                    OLDUGUGIBIKULLANILACAK = Convert.ToBoolean(reader["OLDUGUGIBIKULLANILACAK"]),
                    RAPORNO = Convert.ToString(reader["RAPORNO"]),
                    ACIKLAMADETAY = Convert.ToString(reader["ACIKLAMADETAY"]),
                    ISTVERIMLILIIKANALIZI = Convert.ToBoolean(reader["ISTVERIMLILIIKANALIZI"]),
                    PERVERIMLILIKANALIZI = Convert.ToBoolean(reader["PERVERIMLILIKANALIZI"]),
                    TUR = Convert.ToInt32(reader["TUR"]),
                    KOD = Convert.ToString(reader["KOD"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<OperasyonUygunsuzluk> GetUnsuitabilityEmployeeQuery(int calisanID,DateTime startDate, DateTime endDate)
        {
            List<OperasyonUygunsuzluk> unsuitabilityLines = new List<OperasyonUygunsuzluk>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(ISTASYONID,0) as ISTASYONID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(CALISANID,0) as CALISANID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "TARIH, " +
                                  "ISEMRINO, " +
                                  "FISNO, " +
                                  "CALISANAD, " +
                                  "MAKINEADI, " +
                                  "MAKINEKODU, " +
                                  "ESKISTOKKODU, " +
                                  "STOKACIKLAMASI, " +
                                  "HATAACIKLAMA, " +
                                  "ISNULL(OLCUKONTROLFORMBEYAN,0) as OLCUKONTROLFORMBEYAN, " +
                                  "ISNULL(HURDA,0) as HURDA, " +
                                  "ISNULL(DUZELTME,0) as DUZELTME, " +
                                  "ISNULL(OLDUGUGIBIKULLANILACAK,0) as OLDUGUGIBIKULLANILACAK, " +
                                  "RAPORNO, " +
                                  "ACIKLAMADETAY, " +
                                  "ISNULL(ISTVERIMLILIIKANALIZI,0) as ISTVERIMLILIIKANALIZI, " +
                                  "ISNULL(PERVERIMLILIKANALIZI,0) as  PERVERIMLILIKANALIZI," +
                                  "ISNULL(TUR,0) as  TUR," +
                                  "KOD " +
                                  "FROM TUR_VW_OPERASYON_UYGUNSUZLUK " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "' AND CALISANID = " + calisanID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new OperasyonUygunsuzluk()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    ISTASYONID = Convert.ToInt32(reader["ISTASYONID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    CALISANID = Convert.ToInt32(reader["CALISANID"]),
                    CALISANAD = Convert.ToString(reader["CALISANAD"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    MAKINEADI = Convert.ToString(reader["MAKINEADI"]),
                    MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
                    ESKISTOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    STOKACIKLAMASI = Convert.ToString(reader["STOKACIKLAMASI"]),
                    HATAACIKLAMA = Convert.ToString(reader["HATAACIKLAMA"]),
                    OLCUKONTROLFORMBEYAN = Convert.ToInt32(reader["OLCUKONTROLFORMBEYAN"]),
                    HURDA = Convert.ToBoolean(reader["HURDA"]),
                    DUZELTME = Convert.ToBoolean(reader["DUZELTME"]),
                    OLDUGUGIBIKULLANILACAK = Convert.ToBoolean(reader["OLDUGUGIBIKULLANILACAK"]),
                    RAPORNO = Convert.ToString(reader["RAPORNO"]),
                    ACIKLAMADETAY = Convert.ToString(reader["ACIKLAMADETAY"]),
                    ISTVERIMLILIIKANALIZI = Convert.ToBoolean(reader["ISTVERIMLILIIKANALIZI"]),
                    PERVERIMLILIKANALIZI = Convert.ToBoolean(reader["PERVERIMLILIKANALIZI"]),
                    TUR = Convert.ToInt32(reader["TUR"]),
                    KOD = Convert.ToString(reader["KOD"])
                });
            }

            return unsuitabilityLines;
        }

        //public static IEnumerable<Dictionary<string, object>> GetStations2()
        //{

        //    SqlConnection connection = GetSqlConnection();

        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = "SELECT " +
        //                          "ID, " +
        //                          "MAKINEKODU, " +
        //                          "MAKINEADI, " +
        //                          "VARDIYA, " +
        //                          "VARDIYACALISMASURESI, " +
        //                          "TEORIKSURE, " +
        //                          "MESAICALISMASURESI, " +
        //                          "PLANLIDURUSSURESI, " +
        //                          "FASON, " +
        //                          "EKIPMAN FROM TUR_IST";
        //    command.Connection = connection;

        //    IDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        Dictionary<string, object> result = new Dictionary<string, object>();
        //        for (int column = 0; column < reader.FieldCount; column++)
        //            result.Add(reader.GetName(column), reader.GetValue(column));
        //        yield return result;
        //    }
        //}

        //public static List<Istasyon> GetStations3()
        //{

        //    SqlConnection connection = GetSqlConnection();

        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = "SELECT " +
        //                          "ID, " +
        //                          "MAKINEKODU, " +
        //                          "MAKINEADI, " +
        //                          "VARDIYA, " +
        //                          "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
        //                          "ISNULL(TEORIKSURE,0) as TEORIKSURE, " +
        //                          "ISNULL(MESAICALISMASURESI,0) as MESAICALISMASURESI, " +
        //                          "ISNULL(PLANLIDURUSSURESI,0) as PLANLIDURUSSURESI, " +
        //                          "FASON, " +
        //                          "EKIPMAN FROM TUR_IST";
        //    command.Connection = connection;

        //    SqlDataReader reader = command.ExecuteReader();
        //    List<Istasyon> stations = new List<Istasyon>();
        //    while (reader.Read())
        //    {
        //        stations.Add(new Istasyon()
        //        {
        //            BOLUMID = 0,
        //            EKIPMAN = Convert.ToBoolean(reader["EKIPMAN"]),
        //            FASON = Convert.ToBoolean(reader["FASON"]),
        //            ID = Convert.ToInt32(reader["ID"]),
        //            MAKINEADI = Convert.ToString(reader["MAKINEADI"]),
        //            MAKINEKODU = Convert.ToString(reader["MAKINEKODU"]),
        //            MESAICALISMASURESI = Convert.ToDecimal(reader["MESAICALISMASURESI"]),
        //            PLANLIDURUSSURESI = Convert.ToDecimal(reader["PLANLIDURUSSURESI"]),
        //            TEORIKSURE = Convert.ToDecimal(reader["TEORIKSURE"]),
        //            VARDIYA = Convert.ToInt32(reader["VARDIYA"]),
        //            VARDIYACALISMASURESI = Convert.ToDecimal(reader["VARDIYACALISMASURESI"])
        //        });

        //    }

        //    return stations;
        //}
    }
}
