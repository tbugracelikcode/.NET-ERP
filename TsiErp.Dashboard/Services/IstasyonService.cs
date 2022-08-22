using Microsoft.Data.SqlClient;
using TsiErp.Dashboard.Helpers;
using TsiErp.Dashboard.Models;

namespace TsiErp.Dashboard.Services
{
    public class IstasyonService
    {
        SqlConnection _connection;

        public IstasyonService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<StationGeneralAnalysis> GetStationGeneralAnalyses(DateTime startDate, DateTime endDate)
        {
            List<StationGeneralAnalysis> stationGeneralAnalyses = new List<StationGeneralAnalysis>();

            var stationList = DBHelper.GetStations();

            //var operationLines = DBHelper.GetOperationLines();

            if(stationList != null)
            {
                foreach (var item in stationList)
                {
                    
                    StationGeneralAnalysis analysis = new StationGeneralAnalysis
                    {
                          
                    };
                }
            }

            return stationGeneralAnalyses;
        }

        //private decimal _MakineninAcikOlduguZaman(int? makineID)
        //{
           
               
        //            var toplamSure = operationLines.Where(t => t.ISTASYONID == makineID && t.TARIH > baslangic && t.TARIH < bitis).Sum(t => t.OPERASYONSURESI).GetValueOrDefault();

        //            return toplamSure;
                
           


        //}
    }
}
