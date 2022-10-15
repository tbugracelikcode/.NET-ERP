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
                                  "FASON," +
                                  "ISNULL(VERITOPLAMA,0) as VERITOPLAMA, " +
                                  "EKIPMAN" +
                                  " FROM TUR_IST";
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
                    VERITOPLAMA = Convert.ToBoolean(reader["VERITOPLAMA"]),
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

            SqlCommand command = new SqlCommand("sp_OperasyonSatirlari");
            #region eski
            //command.CommandText = "SELECT " +
            //                      "ID," +
            //                      "ISNULL(CALISANID,0) as CALISANID, " +
            //                      "ISNULL(VARDIYAID,0) as VARDIYAID, " +
            //                      "ISNULL(ISTASYONID,0) as ISTASYONID, " +
            //                      "ISNULL(URETILENADET,0) as URETILENADET, " +
            //                       "ISNULL(HURDAADET,0) as HURDAADET, " +
            //                      "ISNULL(OPERASYONSURESI,0) as OPERASYONSURESI, " +
            //                      "ISNULL(AYARSURESI,0) as AYARSURESI, " +
            //                      "ISNULL(ATILSURE,0) as ATILSURE, " +
            //                      "OPRBASLANGICTRH, " +
            //                      "OPRBITISTRH, " +
            //                      "ISNULL(OEE,0) as OEE, " +
            //                      "ISNULL(KALITE,0) as KALITE, " +
            //                      "ISNULL(MESAI,0) as MESAI, " +
            //                      "ISNULL(ROTAID,0) as ROTAID, " +
            //                      "ISNULL(VARYANTID,0) as VARYANTID, " +
            //                      "ISNULL(STOKID,0) as STOKID, " +
            //                      "ISNULL(SIPARISID,0) as SIPARISID, " +
            //                      "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
            //                      "ISNULL(OPERASYONID,0) as OPERASYONID, " +
            //                      "MAKINEKODU, " +
            //                      "ISNULL(BIRIMSURE,0) as BIRIMSURE, " +
            //                      "ISNULL(ISEMRIID,0) as ISEMRIID, " +
            //                      "ACIKLAMA, " +
            //                      "TARIH," +
            //                      "ISNULL(VARDIYA,0) as VARDIYA, " +
            //                      "ISNULL(VARDIYACALISMASURESI,0) as VARDIYACALISMASURESI, " +
            //                      "ISEMRINO, " +
            //                      "URETIMEMRINUMARASI, " +
            //                      "ISNULL(PLNMIKTAR,0) as PLNMIKTAR, " +
            //                      "ISNULL(AGIRLIK,0) as AGIRLIK, " +
            //                      "CALISAN, " +
            //                      "STOKKODU, " +
            //                      "ISNULL(URUNGRPID,0) as URUNGRPID, " +
            //                      "URUNGRUBU, " +
            //                      "ISNULL(AYARVEKONTROLSURESI,0) as AYARVEKONTROLSURESI, " +
            //                      "ISNULL(PLANLANANOPRSURESI,0) as PLANLANANOPRSURESI, " +
            //                      "ISNULL(AGIRLIK,0) as OPRID, " +
            //                      "ISNULL(PERFORMANS,0) as PERFORMANS, " +
            //                      "ISNULL(KULLANILABILIRLIK,0) as KULLANILABILIRLIK, " +
            //                      "ISNULL(ISLEMESURESI,0) as ISLEMESURESI," +
            //                      "ISNULL(DEPARTMAN,0) as DEPARTMAN," +
            //                      "ISNULL(STOKTURU,0) as STOKTURU," +
            //                      "ISNULL(GRCMIKTAR,0) as GRCMIKTAR" +
            //                      " FROM TUR_VW_EKR_OPERASYON " +
            //                      "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'"; 
            #endregion
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@startDate",startDate);
            command.Parameters.AddWithValue("@endDate", endDate);

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
                    STOKTURU = Convert.ToInt32(reader["STOKTURU"]),
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
                                  "ISNULL(CALISANID,0) as CALISANID, " +
                                  "ISNULL(VARDIYAID,0) as VARDIYAID, " +
                                  "ISNULL(ISTASYONID,0) as ISTASYONID, " +
                                  "ISNULL(URETILENADET,0) as URETILENADET, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "ISNULL(OPERASYONSURESI,0) as OPERASYONSURESI, " +
                                  "ISNULL(AYARSURESI,0) as AYARSURESI, " +
                                  "ISNULL(ATILSURE,0) as ATILSURE, " +
                                  "ISNULL(OPRBASLANGICTRH,'1900-01-01') as OPRBASLANGICTRH, " +
                                  "ISNULL(OPRBITISTRH,'1900-01-01') as OPRBITISTRH,  " +
                                  "ISNULL(OEE,0) as OEE, " +
                                  "ISNULL(KALITE,0) as KALITE, " +
                                  "ISNULL(MESAI,0) as MESAI, " +
                                  "ISNULL(ROTAID,0) as ROTAID, " +
                                  "ISNULL(VARYANTID,0) as VARYANTID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(OPERASYONID,0) as OPERASYONID, " +
                                  "ISNULL(MAKINEKODU,0) as MAKINEKODU, " +
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
                                  "ISNULL(OPRID,0) as OPRID, " +
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

            SqlCommand command = new SqlCommand("sp_GetCalendarQuery");
            #region eski
            //command.CommandText = "SELECT " +
            //                      "ID, " +
            //                      "TAKVIMID, " +
            //                      "ISTASYONID, " +
            //                      "TARIH, " +
            //                      "CALISMADURUMU, " +
            //                      "GUNDUZYARIMGUN, " +
            //                      "GUNDUZVARDIYASI, " +
            //                      "GUNDUZFAZLAMESAI, " +
            //                      "ISNULL(GUNDUZMESAISURESI,0) as GUNDUZMESAISURESI, " +
            //                      "ISNULL(GUNDUZFAZLAMESAISURESI,0) as GUNDUZFAZLAMESAISURESI, " +
            //                      "ISNULL(GUNDUZPLNDURUSSURESI,0) as GUNDUZPLNDURUSSURESI, " +
            //                      "GECEYARIMGUN, " +
            //                      "GECEVARDIYASI, " +
            //                      "GECEFAZLAMESAI, " +
            //                      "ISNULL(GECEMESAISURESI,0) as GECEMESAISURESI, " +
            //                      "ISNULL(GECEFAZLAMESAISURESI,0) as GECEFAZLAMESAISURESI, " +
            //                      "ISNULL(GECEPLNDURUSSURESI,0) as GECEPLNDURUSSURESI, " +
            //                      "BAKIMDURUMU, " +
            //                      "PLANLIBAKIMVARDIYASI, " +
            //                      "ISNULL(BAKIMSURESI,0) as BAKIMSURESI, " +
            //                      "ISNULL(GUNDUZTOPLAMCALISMAZAMANI,0) as GUNDUZTOPLAMCALISMAZAMANI, " +
            //                      "ISNULL(GECETOPLAMCALISMAZAMANI,0) as GECETOPLAMCALISMAZAMANI, " +
            //                      "ISNULL(TOPLAMCALISABILIRSURE,0) as TOPLAMCALISABILIRSURE, " +
            //                      "PLANLANAN," +
            //                      "ISNULL(VERITOPLAMA,0) as VERITOPLAMA " +
            //                      "FROM TUR_VW_IST_CALIS_TAKVIMI_SATIRLAR_YENI " +
            //                      "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'"; 
            #endregion
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);

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
                    PLANLANAN = Convert.ToString(reader["PLANLANAN"]),
                    VERITOPLAMA = Convert.ToBoolean(reader["VERITOPLAMA"])
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
                                  "ISNULL(YKK,0) as  YKK," +
                                  "ISNULL(MKD,0) as  MKD," +
                                  "ISNULL(PKD,0) as PKD  " +
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
                    YKK = Convert.ToBoolean(reader["YKK"]),
                    MKD = Convert.ToBoolean(reader["MKD"]),
                    PKD = Convert.ToBoolean(reader["PKD"])
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
                                  "ISNULL(YKK,0) as YKK, " +
                                  "ISNULL(MKD,0) as  MKD," +
                                  "ISNULL(PKD,0) as PKD  " +
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
                    YKK = Convert.ToBoolean(reader["YKK"]),
                    MKD = Convert.ToBoolean(reader["MKD"]),
                    PKD = Convert.ToBoolean(reader["PKD"])
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
                                  "ISNULL(YKK,0) as YKK, " +
                                  "ISNULL(MKD,0) as  MKD," +
                                  "ISNULL(PKD,0) as PKD  " +
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
                    YKK = Convert.ToBoolean(reader["YKK"]),
                    MKD = Convert.ToBoolean(reader["MKD"]),
                    PKD = Convert.ToBoolean(reader["PKD"])
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

            SqlCommand command = new SqlCommand("sp_GetUnsuitabilityQuery");
            #region eski
            //command.CommandText = "SELECT " +
            //                      "ID, " +
            //                      "ISNULL(ISTASYONID,0) as ISTASYONID, " +
            //                      "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
            //                      "ISNULL(ISEMRIID,0) as ISEMRIID, " +
            //                      "ISNULL(CALISANID,0) as CALISANID, " +
            //                      "ISNULL(STOKID,0) as STOKID, " +
            //                      "TARIH, " +
            //                      "ISEMRINO, " +
            //                      "FISNO, " +
            //                      "CALISANAD, " +
            //                      "MAKINEADI, " +
            //                      "MAKINEKODU, " +
            //                      "ESKISTOKKODU, " +
            //                      "STOKACIKLAMASI, " +
            //                      "HATAACIKLAMA, " +
            //                      "ISNULL(OLCUKONTROLFORMBEYAN,0) as OLCUKONTROLFORMBEYAN, " +
            //                      "ISNULL(HURDA,0) as HURDA, " +
            //                      "ISNULL(DUZELTME,0) as DUZELTME, " +
            //                      "ISNULL(OLDUGUGIBIKULLANILACAK,0) as OLDUGUGIBIKULLANILACAK, " +
            //                      "RAPORNO, " +
            //                      "ACIKLAMADETAY, " +
            //                      "ISNULL(ISTVERIMLILIIKANALIZI,0) as ISTVERIMLILIIKANALIZI, " +
            //                      "ISNULL(PERVERIMLILIKANALIZI,0) as  PERVERIMLILIKANALIZI," +
            //                      "ISNULL(TUR,0) as  TUR," +
            //                      "KOD, " +
            //                      "ISNULL(URUNGRPID,0) as URUNGRPID " +
            //                      "FROM TUR_VW_OPERASYON_UYGUNSUZLUK " +
            //                      "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'"; 
            #endregion
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);

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
                    URUNGRUPID = Convert.ToInt32(reader["URUNGRPID"]),
                    KOD = Convert.ToString(reader["KOD"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<OperasyonUygunsuzluk> GetUnsuitabilityEmployeeQuery(int calisanID, DateTime startDate, DateTime endDate)
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

        public static List<FasonUygunsuzluk> GetContractUnsuitabilityQuery(DateTime startDate, DateTime endDate)
        {
            List<FasonUygunsuzluk> unsuitabilityLines = new List<FasonUygunsuzluk>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(URETIMEMRIID,0) as URETIMEMRIID, " +
                                  "ISNULL(ISEMRIID,0) as ISEMRIID, " +
                                  "ISNULL(CARIID,0) as CARIID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "TARIH, " +
                                  "ISEMRINO, " +
                                  "ACIKLAMA, " +
                                  "FISNO, " +
                                  "CARIKOD, " +
                                  "CARIUNVAN, " +
                                  "ESKISTOKKODU, " +
                                  "STOKACIKLAMASI, " +
                                  "HATAACIKLAMA, " +
                                  "ISNULL(UYGUNOLMAYANMIKTAR,0) as UYGUNOLMAYANMIKTAR, " +
                                  "ISNULL(RED,0) as RED, " +
                                  "ISNULL(DUZELTME,0) as DUZELTME, " +
                                  "ISNULL(HURDA,0) as HURDA, " +
                                  "ISNULL(OLDUGUGIBIKULLANILACAK,0) as OLDUGUGIBIKULLANILACAK, " +
                                  "RAPORNO, " +
                                  "RAPORACIKLAMA," +
                                  "ISNULL(HATAID,0) as HATAID," +
                                  "URETIMEMRINO " +
                                  "FROM TUR_VW_FASON_UYGUNSUZLUK " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new FasonUygunsuzluk()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    URETIMEMRIID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    ISEMRIID = Convert.ToInt32(reader["ISEMRIID"]),
                    CARIID = Convert.ToInt32(reader["CARIID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    ISEMRINO = Convert.ToString(reader["ISEMRINO"]),
                    ACIKLAMA = Convert.ToString(reader["ACIKLAMA"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    CARIKOD = Convert.ToString(reader["CARIKOD"]),
                    CARIUNVAN = Convert.ToString(reader["CARIUNVAN"]),
                    ESKISTOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    HATAACIKLAMA = Convert.ToString(reader["HATAACIKLAMA"]),
                    STOKACIKLAMASI = Convert.ToString(reader["STOKACIKLAMASI"]),
                    UYGUNOLMAYANMIKTAR = Convert.ToInt32(reader["UYGUNOLMAYANMIKTAR"]),
                    RED = Convert.ToBoolean(reader["RED"]),
                    HURDA = Convert.ToBoolean(reader["HURDA"]),
                    DUZELTME = Convert.ToBoolean(reader["DUZELTME"]),
                    OLDUGUGIBIKULLANILACAK = Convert.ToBoolean(reader["OLDUGUGIBIKULLANILACAK"]),
                    RAPORNO = Convert.ToString(reader["RAPORNO"]),
                    RAPORACIKLAMA = Convert.ToString(reader["RAPORACIKLAMA"]),
                    HATAID = Convert.ToInt32(reader["HATAID"]),
                    URETIMEMRINO = Convert.ToString(reader["URETIMEMRINO"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<FasonUygunsuzlukCari> GetContractUnsuitabilityQueryGeneral(DateTime startDate, DateTime endDate)
        {
            List<FasonUygunsuzlukCari> unsuitabilityLines = new List<FasonUygunsuzlukCari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "UYGUNSUZLUK.URETIMEMRIID, " +
                                  "UYGUNSUZLUK.TARIH ," +
                                  "UYGUNSUZLUK.CARIID, " +
                                  "SUM(UYGUNSUZLUK.UYGUNOLMAYANMIKTAR) as MIKTAR, " +
                                  "UYGUNSUZLUK.CARIUNVAN," +
                                  "UYGUNSUZLUK.FASONFISIADETI " +
                                  "FROM TUR_VW_FASON_UYGUNSUZLUK as UYGUNSUZLUK " +
                                  "WHERE UYGUNSUZLUK.UYGUNOLMAYANMIKTAR > 0 AND UYGUNSUZLUK.TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND UYGUNSUZLUK.TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'" +
                                  " GROUP BY UYGUNSUZLUK.URETIMEMRIID,UYGUNSUZLUK.CARIID,UYGUNSUZLUK.CARIUNVAN,UYGUNSUZLUK.FASONFISIADETI,UYGUNSUZLUK.TARIH";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new FasonUygunsuzlukCari()
                {
                    CariID = Convert.ToInt32(reader["CARIID"]),
                    UretimEmriID = Convert.ToInt32(reader["URETIMEMRIID"]),
                    CariUnvan = Convert.ToString(reader["CARIUNVAN"]),
                    FasonFisiAdeti = Convert.ToInt32(reader["FASONFISIADETI"]),
                    Miktar = Convert.ToInt32(reader["MIKTAR"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                });
            }

            return unsuitabilityLines;
        }

        public static List<TederikciUygunsuzluk> GetSuppliertUnsuitabilityQuery(DateTime startDate, DateTime endDate)
        {
            List<TederikciUygunsuzluk> unsuitabilityLines = new List<TederikciUygunsuzluk>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(CARIID,0) as CARIID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "TARIH, " +
                                  "ACIKLAMA, " +
                                  "FISNO, " +
                                  "CARIKOD, " +
                                  "CARIUNVAN, " +
                                  "ESKISTOKKODU, " +
                                  "STOKACIKLAMASI, " +
                                  "HATAACIKLAMA, " +
                                  "ISNULL(UYGUNOLMAYANMIKTAR,0) as UYGUNOLMAYANMIKTAR, " +
                                  "ISNULL(RED,0) as RED, " +
                                  "ISNULL(DUZELTME,0) as DUZELTME, " +
                                  "ISNULL(TEDARIKCIIRTIBAT,0) as TEDARIKCIIRTIBAT, " +
                                  "ISNULL(OLDUGUGIBIKULLANILACAK,0) as OLDUGUGIBIKULLANILACAK, " +
                                  "RAPORNO, " +
                                  "ISNULL(HATAID,0) as HATAID " +
                                  "FROM TUR_VW_HM_UYGUNSUZLUK " +
                                  "WHERE TARIH > '" + startDate.ToString("yyyy-MM-dd") + "' AND TARIH < '" + endDate.ToString("yyyy-MM-dd") + "'";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new TederikciUygunsuzluk()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    CARIID = Convert.ToInt32(reader["CARIID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    ACIKLAMA = Convert.ToString(reader["ACIKLAMA"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    CARIKOD = Convert.ToString(reader["CARIKOD"]),
                    CARIUNVAN = Convert.ToString(reader["CARIUNVAN"]),
                    ESKISTOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    HATAACIKLAMA = Convert.ToString(reader["HATAACIKLAMA"]),
                    STOKACIKLAMASI = Convert.ToString(reader["STOKACIKLAMASI"]),
                    UYGUNOLMAYANMIKTAR = Convert.ToInt32(reader["UYGUNOLMAYANMIKTAR"]),
                    RED = Convert.ToBoolean(reader["RED"]),
                    TEDARIKCIIRTIBAT = Convert.ToBoolean(reader["TEDARIKCIIRTIBAT"]),
                    DUZELTME = Convert.ToBoolean(reader["DUZELTME"]),
                    OLDUGUGIBIKULLANILACAK = Convert.ToBoolean(reader["OLDUGUGIBIKULLANILACAK"]),
                    RAPORNO = Convert.ToString(reader["RAPORNO"]),
                    HATAID = Convert.ToInt32(reader["HATAID"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<TedarikciUygunsuzlukSatir> GetSuppliertUnsuitabilityLinesQuery()
        {
            List<TedarikciUygunsuzlukSatir> unsuitabilityLines = new List<TedarikciUygunsuzlukSatir>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(CARIID,0) as CARIID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "ISNULL(SATIRTURU,0) as SATIRTURU, " +
                                  "ISNULL(SATIRNR,0) as SATIRNR, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(VARYANTID,0) as VARYANTID, " +
                                  "ISNULL(DEPOID,0) as DEPOID, " +
                                  "ISNULL(TUR,0) as TUR, " +
                                  "FISNO, " +
                                  "TARIH, " +
                                  "TEMINTARIHI, " +
                                  "ISNULL(BIRIMSETID,0) as BIRIMSETID, " +
                                  "ISNULL(PARABIRIMID,0) as PARABIRIMID, " +
                                  "ISNULL(ADET,0) as ADET, " +
                                  "ISNULL(DEPOADET,0) as DEPOADET, " +
                                  "ISNULL(DURUM,0) as DURUM, " +
                                  "ISNULL(SIPARISKABULSATIRID,0) as SIPARISKABULSATIRID " +
                                  "FROM TUR_SIPARIS_SATIR";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new TedarikciUygunsuzlukSatir()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    CARIID = Convert.ToInt32(reader["CARIID"]),
                    SATIRTURU = Convert.ToInt32(reader["SATIRTURU"]),
                    SATIRNR = Convert.ToInt32(reader["SATIRNR"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    VARYANTID = Convert.ToInt32(reader["VARYANTID"]),
                    DEPOID = Convert.ToInt32(reader["DEPOID"]),
                    TUR = Convert.ToInt32(reader["TUR"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    TEMINTARIHI = Convert.ToDateTime(reader["TEMINTARIHI"]),
                    BIRIMSETID = Convert.ToInt32(reader["BIRIMSETID"]),
                    PARABIRIMID = Convert.ToInt32(reader["PARABIRIMID"]),
                    ADET = Convert.ToDecimal(reader["ADET"]),
                    DEPOADET = Convert.ToDecimal(reader["DEPOADET"]),
                    DURUM = Convert.ToInt32(reader["DURUM"]),
                    SIPARISKABULSATIRID = Convert.ToInt32(reader["SIPARISKABULSATIRID"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<TedarikciUygunsuzlukSatir> GetSuppliertUnsuitabilityLinesQueryWithID(int SiparisID)
        {
            List<TedarikciUygunsuzlukSatir> unsuitabilityLines = new List<TedarikciUygunsuzlukSatir>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID, " +
                                  "ISNULL(CARIID,0) as CARIID, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "ISNULL(SATIRTURU,0) as SATIRTURU, " +
                                  "ISNULL(SATIRNR,0) as SATIRNR, " +
                                  "ISNULL(STOKID,0) as STOKID, " +
                                  "ISNULL(VARYANTID,0) as VARYANTID, " +
                                  "ISNULL(DEPOID,0) as DEPOID, " +
                                  "ISNULL(TUR,0) as TUR, " +
                                  "FISNO, " +
                                  "TARIH, " +
                                  "TEMINTARIHI, " +
                                  "ISNULL(BIRIMSETID,0) as BIRIMSETID, " +
                                  "ISNULL(PARABIRIMID,0) as PARABIRIMID, " +
                                  "ISNULL(ADET,0) as ADET, " +
                                  "ISNULL(DEPOADET,0) as DEPOADET, " +
                                  "ISNULL(DURUM,0) as DURUM, " +
                                  "ISNULL(SIPARISKABULSATIRID,0) as SIPARISKABULSATIRID " +
                                  "FROM TUR_SIPARIS_SATIR" +
                                  " WHERE SIPARISID = " + SiparisID.ToString();
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                unsuitabilityLines.Add(new TedarikciUygunsuzlukSatir()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    CARIID = Convert.ToInt32(reader["CARIID"]),
                    SATIRTURU = Convert.ToInt32(reader["SATIRTURU"]),
                    SATIRNR = Convert.ToInt32(reader["SATIRNR"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    VARYANTID = Convert.ToInt32(reader["VARYANTID"]),
                    DEPOID = Convert.ToInt32(reader["DEPOID"]),
                    TUR = Convert.ToInt32(reader["TUR"]),
                    FISNO = Convert.ToString(reader["FISNO"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    TEMINTARIHI = Convert.ToDateTime(reader["TEMINTARIHI"]),
                    BIRIMSETID = Convert.ToInt32(reader["BIRIMSETID"]),
                    PARABIRIMID = Convert.ToInt32(reader["PARABIRIMID"]),
                    ADET = Convert.ToDecimal(reader["ADET"]),
                    DEPOADET = Convert.ToDecimal(reader["DEPOADET"]),
                    DURUM = Convert.ToInt32(reader["DURUM"]),
                    SIPARISKABULSATIRID = Convert.ToInt32(reader["SIPARISKABULSATIRID"])
                });
            }

            return unsuitabilityLines;
        }

        public static List<UygunsuzlukBasliklari> GetContractCauses()
        {
            List<UygunsuzlukBasliklari> hurdaSebepleri = new List<UygunsuzlukBasliklari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ID," +
                                  "ACIKLAMA, " +
                                  "ISNULL(TUR,0) as TUR" +
                                  " FROM TUR_KK_UYGUNSUZLUK_BASLIKLARI" +
                                  " WHERE TUR = 2";
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

        public static List<BakimKayitlari> GetMaintenanceRecords()
        {
            List<BakimKayitlari> bakimKayitlari = new List<BakimKayitlari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ISNULL(ID,0) as ID," +
                                  "KAYITNO, " +
                                  "BAKIMTURU, " +
                                  "ISNULL(PLANLANANTARIH,'1999-01-01') as PLANLANANTARIH, " +
                                  "ISNULL(PLANLANANBAKIMSURESI,0) as PLANLANANBAKIMSURESI, " +
                                  "ISNULL(BASLANGICTARIHI,'1999-01-01') as BASLANGICTARIHI, " +
                                  "ISNULL(TAMAMLANMATARIHI,'1999-01-01') as TAMAMLANMATARIHI, " +
                                  "ISNULL(ISTID,0) as ISTID, " +
                                  "ISNULL(PERIYOT,0) as PERIYOT, " +
                                  "ISNULL(KALANSURE,0) as KALANSURE, " +
                                  "DURUM, " +
                                  "BAKIMIYAPAN, " +
                                  "ISNULL(BAKIMSURESI,0) as BAKIMSURESI, " +
                                  "NOT_, " +
                                  "ISNULL(GUNLUK,0) as GUNLUK" +
                                  " FROM TUR_ISTASYON_BAKIM_KAYITLARI";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bakimKayitlari.Add(new BakimKayitlari()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    KAYITNO = Convert.ToString(reader["KAYITNO"]),
                    BAKIMTURU = Convert.ToString(reader["BAKIMTURU"]),
                    PLANLANANTARIH = Convert.ToDateTime(reader["PLANLANANTARIH"]),
                    PLANLANANBAKIMSURESI = Convert.ToInt32(reader["PLANLANANBAKIMSURESI"]),
                    BASLANGICTARIHI = Convert.ToDateTime(reader["BASLANGICTARIHI"]),
                    TAMAMLANMATARIHI = Convert.ToDateTime(reader["TAMAMLANMATARIHI"]),
                    ISTID = Convert.ToInt32(reader["ISTID"]),
                    PERIYOT = Convert.ToInt32(reader["PERIYOT"]),
                    KALANSURE = Convert.ToInt32(reader["KALANSURE"]),
                    DURUM = Convert.ToString(reader["DURUM"]),
                    BAKIMIYAPAN = Convert.ToString(reader["BAKIMIYAPAN"]),
                    BAKIMSURESI = Convert.ToInt32(reader["BAKIMSURESI"]),
                    NOT_ = Convert.ToString(reader["NOT_"]),
                    GUNLUK = Convert.ToBoolean(reader["GUNLUK"])
                });
            }

            return bakimKayitlari;
        }

        public static List<BakimView> GetMaintenanceRecordsView()
        {
            List<BakimView> bakimView = new List<BakimView>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "ISNULL(ID,0) as ID," +
                                  "KAYITNO, " +
                                  "BAKIMTURU, " +
                                  "ISNULL(PLANLANANTARIH,'1999-01-01') as PLANLANANTARIH, " +
                                  "ISNULL(PLANLANANBAKIMSURESI,0) as PLANLANANBAKIMSURESI, " +
                                  "ISNULL(BASLANGICTARIHI,'1999-01-01') as BASLANGICTARIHI, " +
                                  "ISNULL(TAMAMLANMATARIHI,'1999-01-01') as TAMAMLANMATARIHI, " +
                                  "ISNULL(ISTID,0) as ISTID, " +
                                  "ISNULL(PERIYOT,0) as PERIYOT, " +
                                  "ISNULL(KALANSURE,0) as KALANSURE, " +
                                  "DURUM, " +
                                  "BAKIMIYAPAN, " +
                                  "ISNULL(BAKIMSURESI,0) as BAKIMSURESI, " +
                                  "NOT_, " +
                                  "ISNULL(SIPARISID,0) as SIPARISID, " +
                                  "MUSTERISIPARISNO, " +
                                  "ISNULL(URETILENADET,0) as URETILENADET, " +
                                  "ISNULL(HURDAADET,0) as HURDAADET, " +
                                  "ISNULL(KALITE,0) as KALITE, " +
                                  "ISNULL(CALISMABILGISI,0) as CALISMABILGISI, " +
                                  "CARIUNVAN, " +
                                  "ISNULL(TARIH,'1999-01-01') as TARIH, " +
                                  "ISNULL(SIPARISDURUM,0) as SIPARISDURUM, " +
                                  "ISNULL(KAT,0) as KAT," +
                                  "BOLUM," +
                                  "BOLUMID " +
                                  "FROM TUR_VW_DASHBOARD_BAKIM ORDER BY TARIH DESC";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bakimView.Add(new BakimView()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    KAYITNO = Convert.ToString(reader["KAYITNO"]),
                    BAKIMTURU = Convert.ToString(reader["BAKIMTURU"]),
                    PLANLANANTARIH = Convert.ToDateTime(reader["PLANLANANTARIH"]),
                    PLANLANANBAKIMSURESI = Convert.ToInt32(reader["PLANLANANBAKIMSURESI"]),
                    BASLANGICTARIHI = Convert.ToDateTime(reader["BASLANGICTARIHI"]),
                    TAMAMLANMATARIHI = Convert.ToDateTime(reader["TAMAMLANMATARIHI"]),
                    ISTID = Convert.ToInt32(reader["ISTID"]),
                    PERIYOT = Convert.ToInt32(reader["PERIYOT"]),
                    KALANSURE = Convert.ToInt32(reader["KALANSURE"]),
                    DURUM = Convert.ToString(reader["DURUM"]),
                    BAKIMIYAPAN = Convert.ToString(reader["BAKIMIYAPAN"]),
                    BAKIMSURESI = Convert.ToInt32(reader["BAKIMSURESI"]),
                    NOT_ = Convert.ToString(reader["NOT_"]),
                    SIPARISID = Convert.ToInt32(reader["SIPARISID"]),
                    MUSTERISIPARISNO = Convert.ToString(reader["MUSTERISIPARISNO"]),
                    URETILENADET = Convert.ToDecimal(reader["URETILENADET"]),
                    HURDAADET = Convert.ToDecimal(reader["HURDAADET"]),
                    KALITE = Convert.ToInt32(reader["KALITE"]),
                    CALISMABILGISI = Convert.ToInt32(reader["CALISMABILGISI"]),
                    CARIUNVAN = Convert.ToString(reader["CARIUNVAN"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    SIPARISDURUM = Convert.ToInt32(reader["SIPARISDURUM"]),
                    KAT = Convert.ToInt32(reader["KAT"]),
                    BOLUM = Convert.ToString(reader["BOLUM"]),
                    BOLUMID = Convert.ToInt32(reader["BOLUMID"])
                });
            }

            return bakimView;
        }

        public static List<BakimSatirlari> GetMaintenanceLineRecords()
        {
            List<BakimSatirlari> bakimKayitlari = new List<BakimSatirlari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT YP.ID," +
                                   "YP.STOKID," +
                                   "YP.KAYITID as BAKIMID," +
                                   "YP.BAKIMTALIMATI," +
                                   "STOK.ESKISTOKKODU," +
                                   "STOK.STOKACIKLAMASI," +
                                   "STOK.BIRIMSETKOD," +
                                   "YP.MIKTAR as IHTIYACMIKTARI," +
                                   "(ISNULL(STOK.ADET, 0) - ISNULL(STOK.REZERVE, 0)) as STOKMIKTARI " +
                                   "FROM dbo.TUR_ISTASYON_BAKIM_YEDEK_PARCALAR as YP LEFT JOIN " +
                                   "TUR_VW_STOK_BROWSER as STOK ON YP.STOKID = STOK.ID";
                                                      command.Connection = connection;
                                          
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bakimKayitlari.Add(new BakimSatirlari()
                {
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    BAKIMID = Convert.ToInt32(reader["BAKIMID"]),
                    BAKIMTALIMATI = Convert.ToString(reader["BAKIMTALIMATI"]),
                    ESKISTOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    STOKACIKLAMASI = Convert.ToString(reader["STOKACIKLAMASI"]),
                    BIRIMSETKOD = Convert.ToString(reader["BIRIMSETKOD"]),
                    IHTIYACMIKTARI = Convert.ToDecimal(reader["IHTIYACMIKTARI"]),
                    STOKMIKTARI = Convert.ToDecimal(reader["STOKMIKTARI"])
                });
            }

            return bakimKayitlari;
        }
         public static List<SatinAlmaDetaylari> GetPurchaseDetails()
        {
            List<SatinAlmaDetaylari> purchaseLines = new List<SatinAlmaDetaylari>();

            SqlConnection connection = GetSqlConnection();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT " +
                                  "SIPARISSATIR.ID, " +
                                  "SIPARISSATIR.STOKID, " +
                                  "STOK.ESKISTOKKODU, " +
                                  "STOK.BIRIMSETKOD, " +
                                  "SIPARISSATIR.PARABIRIMID," +
                                  "PARABIRIMI.CurrencyCode AS PARABIRIMI, " +
                                  "CARI.CARIUNVAN, " +
                                  "SIPARISSATIR.TARIH, " +
                                  "SIPARISSATIR.BIRIMFIYAT " +
                                  "FROM TUR_SIPARIS_SATIR AS SIPARISSATIR " +
                                  "LEFT JOIN TUR_VW_STOK_BROWSER AS STOK ON SIPARISSATIR.STOKID = STOK.ID " +
                                  "LEFT JOIN TUR_CARI AS CARI ON SIPARISSATIR.CARIID = CARI.ID " +
                                  "LEFT JOIN TUR_DOVIZ AS PARABIRIMI ON SIPARISSATIR.PARABIRIMID = PARABIRIMI.ID " +
                                  "WHERE STOK.URUNGRPID IN (64) "

                                  ;
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                purchaseLines.Add(new SatinAlmaDetaylari()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    STOKID = Convert.ToInt32(reader["STOKID"]),
                    STOKKODU = Convert.ToString(reader["ESKISTOKKODU"]),
                    BIRIMSETKOD = Convert.ToString(reader["BIRIMSETKOD"]),
                    PARABIRIMIID = Convert.ToInt32(reader["PARABIRIMID"]),
                    PARABIRIMI = Convert.ToString(reader["PARABIRIMI"]),
                    CARIUNVAN = Convert.ToString(reader["CARIUNVAN"]),
                    TARIH = Convert.ToDateTime(reader["TARIH"]),
                    BIRIMFIYAT = Convert.ToString(reader["BIRIMFIYAT"])
                });
            }

            return purchaseLines;
        }
    }
}
