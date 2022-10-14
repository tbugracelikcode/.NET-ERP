using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using System.Globalization;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class UrunGrubuPerformansService : IUrunGrubuPerformansService
    {
        #region Chart

        public Task<List<AdminProductGroupPerformanceAnalysisChart>> GetProductGroupPerformanceAnalysisChart(DateTime startDate, DateTime endDate, int frequency)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Grid

        public Task<List<ProductGroupPerformanceAnalysis>> GetProductGroupPerformanceAnalysis(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        #endregion


        
    }
}
