using Microsoft.Data.SqlClient;
using System.Data;
using TsiErp.Dashboard.Helpers.HelperModels;

namespace TsiErp.Dashboard.Helpers
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
                                  "VARDIYA, " +
                                  "VARDIYACALISMASURESI, " +
                                  "TEORIKSURE, " +
                                  "MESAICALISMASURESI, " +
                                  "PLANLIDURUSSURESI, " +
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
                                  "MAKINEADI, " +
                                  "ISTASYONID, " +
                                  "URETILENADET, " +
                                  "HURDAADET, " +
                                  "OPERASYONSURESI, " +
                                  "AYARSURESI, " +
                                  "ATILSURE, " +
                                  "OPRBASLANGICTRH, " +
                                  "OPRBITISTRH, " +
                                  "OEE, " +
                                  "TARIH," +
                                  "KALITE, " +
                                  "MESAI, " +
                                  "ROTAID, " +
                                  "VARYANTID, " +
                                  "STOKID, " +
                                  "SIPARISID, " +
                                  "URETIMEMRIID, " +
                                  "OPERASYONID, " +
                                  "MAKINEKODU, " +
                                  "BIRIMSURE, " +
                                  "ISEMRIID, " +
                                  "ACIKLAMA, " +
                                  "VARDIYA, " +
                                  "VARDIYACALISMASURESI, " +
                                  "ISEMRINO, " +
                                  "URETIMEMRINUMARASI, " +
                                  "PLNMIKTAR, " +
                                  "AGIRLIK, " +
                                  "CALISAN, " +
                                  "STOKKODU, " +
                                  "URUNGRPID, " +
                                  "URUNGRUBU, " +
                                  "AYARVEKONTROLSURESI, " +
                                  "PLANLANANOPRSURESI, " +
                                  "OPRID, " +
                                  "PERFORMANS, " +
                                  "KULLANILABILIRLIK, " +
                                  "ISLEMESURESI" +
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
                                  "CALISANID, " +
                                  "VARDIYAID, " +
                                  "ISTASYONID, " +
                                  "URETILENADET, " +
                                  "HURDAADET, " +
                                  "OPERASYONSURESI, " +
                                  "AYARSURESI, " +
                                  "ATILSURE, " +
                                  "OPRBASLANGICTRH, " +
                                  "OPRBITISTRH, " +
                                  "OEE, " +
                                  "KALITE, " +
                                  "MESAI, " +
                                  "ROTAID, " +
                                  "VARYANTID, " +
                                  "STOKID, " +
                                  "SIPARISID, " +
                                  "URETIMEMRIID, " +
                                  "OPERASYONID, " +
                                  "MAKINEKODU, " +
                                  "BIRIMSURE, " +
                                  "ISEMRIID, " +
                                  "ACIKLAMA, " +
                                  "TARIH," +
                                  "VARDIYA, " +
                                  "VARDIYACALISMASURESI, " +
                                  "ISEMRINO, " +
                                  "URETIMEMRINUMARASI, " +
                                  "PLNMIKTAR, " +
                                  "AGIRLIK, " +
                                  "CALISAN, " +
                                  "STOKKODU, " +
                                  "URUNGRPID, " +
                                  "URUNGRUBU, " +
                                  "AYARVEKONTROLSURESI, " +
                                  "PLANLANANOPRSURESI, " +
                                  "OPRID, " +
                                  "PERFORMANS, " +
                                  "KULLANILABILIRLIK, " +
                                  "ISLEMESURESI" +
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
                    OPRID = Convert.ToInt32(reader["OPRID"]),
                    PERFORMANS = Convert.ToDecimal(reader["PERFORMANS"]),
                    KULLANILABILIRLIK = Convert.ToDecimal(reader["KULLANILABILIRLIK"]),
                    ISLEMESURESI = Convert.ToDecimal(reader["ISLEMESURESI"])
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
                                  "GUNDUZMESAISURESI, " +
                                  "GUNDUZFAZLAMESAISURESI, " +
                                  "GUNDUZPLNDURUSSURESI, " +
                                  "GECEYARIMGUN, " +
                                  "GECEVARDIYASI, " +
                                  "GECEFAZLAMESAI, " +
                                  "GECEMESAISURESI, " +
                                  "GECEFAZLAMESAISURESI, " +
                                  "GECEPLNDURUSSURESI, " +
                                  "BAKIMDURUMU, " +
                                  "PLANLIBAKIMVARDIYASI, " +
                                  "BAKIMSURESI, " +
                                  "GUNDUZTOPLAMCALISMAZAMANI, " +
                                  "GECETOPLAMCALISMAZAMANI, " +
                                  "TOPLAMCALISABILIRSURE, " +
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
                                  "GUNDUZMESAISURESI, " +
                                  "GUNDUZFAZLAMESAISURESI, " +
                                  "GUNDUZPLNDURUSSURESI, " +
                                  "GECEYARIMGUN, " +
                                  "GECEVARDIYASI, " +
                                  "GECEFAZLAMESAI, " +
                                  "GECEMESAISURESI, " +
                                  "GECEFAZLAMESAISURESI, " +
                                  "GECEPLNDURUSSURESI, " +
                                  "BAKIMDURUMU, " +
                                  "PLANLIBAKIMVARDIYASI, " +
                                  "BAKIMSURESI, " +
                                  "GUNDUZTOPLAMCALISMAZAMANI, " +
                                  "GECETOPLAMCALISMAZAMANI, " +
                                  "TOPLAMCALISABILIRSURE, " +
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
                                  "DURUSSURE, " +
                                  "BASLANGIC, " +
                                  "BITIS, " +
                                  "CALISANID, " +
                                  "OPERASYONID, " +
                                  "ISTASYONID, " +
                                  "UNUTULDU, " +
                                  "TARIH, " +
                                  "OPR_TOPLAM_DURUS, " +
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
                                  "DURUS_ORANI " +
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
