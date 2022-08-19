using Microsoft.Data.SqlClient;

namespace TsiErp.Dashboard.Services
{
    public class IstasyonService
    {
        SqlConnection _connection;

        public IstasyonService()
        {
            if(_connection == null)
                _connection = new SqlConnection("Server=192.168.98.4;Database=TURERP;UID=sa;PWD=Logo1234567890;");
                
            if(_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();
        }

        public List<string> GetCodes()
        {
            List<string> codes = new List<string>();

            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT MAKINEKODU FROM TUR_IST";
            command.Connection = _connection;

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                codes.Add(reader["MAKINEKODU"].ToString());
            }

            return codes;
        }

    }
}
