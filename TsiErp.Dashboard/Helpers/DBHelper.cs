using Microsoft.Data.SqlClient;
using TsiErp.Dashboard.Helpers.HelperModels;

namespace TsiErp.Dashboard.Helpers
{
    public static class DBHelper
    {

        public static SqlConnection GetSqlConnection()
        {
            SqlConnection _connection = new SqlConnection();

            if (_connection == null)
                _connection = new SqlConnection("Server=192.168.98.4;Database=TURERP;UID=sa;PWD=Logo1234567890;");

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

    }
}
