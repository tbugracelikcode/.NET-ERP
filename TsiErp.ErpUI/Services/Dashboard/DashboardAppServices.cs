using System.Globalization;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.ErpUI.Models.Dashboard;

namespace TsiErp.ErpUI.Services.Dashboard
{
    public class DashboardAppServices : IDashboardAppServices
    {

        public IProductionTrackingsAppService ProductionTrackingsAppService { get; set; }
        public IOperationUnsuitabilityReportsAppService OperationUnsuitabilityReportsAppService { get; set; }
        public IProductGroupsAppService ProductGroupsAppService { get; set; }

        private List<ListProductGroupsDto> ProductGroups = new List<ListProductGroupsDto>();

        public DashboardAppServices(IProductionTrackingsAppService productionTrackingsAppService, IOperationUnsuitabilityReportsAppService operationUnsuitabilityReportsAppService, IProductGroupsAppService productGroupsAppService)
        {
            ProductionTrackingsAppService = productionTrackingsAppService;
            OperationUnsuitabilityReportsAppService = operationUnsuitabilityReportsAppService;
            ProductGroupsAppService = productGroupsAppService;

            ProductGroups = (ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Result.Data.ToList();
        }

        public async Task<List<ProductGroupsAnalysis>> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ProductGroupsAnalysis> result = new List<ProductGroupsAnalysis>();

            var operationLines = (await ProductionTrackingsAppService.GetListbyOprStartDateRangeIsEqualAsync(startDate, endDate)).Data.ToList();

            var unsuitabilityLines = (await OperationUnsuitabilityReportsAppService.GetListbyOprStartDateRangeIsEqualAsync(startDate, endDate)).Data.ToList();

            var groupList = operationLines.Select(t => t.ProductGroupID).Distinct().ToList();

            if (groupList != null && groupList.Count > 0)
            {
                foreach (var groupID in groupList)
                {

                    var tempUnsuitability = unsuitabilityLines.Where(t => t.ProductGroupID == groupID).ToList();

                    Tuple<int, int> tuple = _PlanlananAdetHesapla(groupID, operationLines);

                    string productGroupName = ProductGroups.Where(t => t.Id == groupID).Select(t => t.Name).FirstOrDefault();

                    int totalScrap = Convert.ToInt32(tempUnsuitability.Where(t => t.Action_ == "Hurda").Sum(t => t.UnsuitableAmount));

                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = productGroupName,
                        PlannedQuantity = tuple.Item1,
                        TotalProduction = tuple.Item2,
                        TotalScrap = totalScrap,
                        Quality = tuple.Item1 > 0 && tuple.Item2 > 0 ? ((double)tuple.Item2 / (double)tuple.Item1) : 0
                    };
                    result.Add(analysis);
                }
            }

            await Task.CompletedTask;
            return result;
        }

        private Tuple<int, int> _PlanlananAdetHesapla(Guid? urunGrupID, List<SelectProductionTrackingsDto> opr)
        {

            int planlananAdet = 0;
            int gerceklesenAdet = 0;

            var uretimEmriList = opr.Where(t => t.ProductGroupID == urunGrupID).Select(t => t.ProductionOrderID).Distinct().ToList();

            for (int i = 0; i < uretimEmriList.Count; i++)
            {
                Guid ueEmriID = uretimEmriList[i];
                planlananAdet += Convert.ToInt32(opr.Where(t => t.ProductionOrderID == ueEmriID).Select(t => t.PlannedQuantity).FirstOrDefault());
                gerceklesenAdet += Convert.ToInt32(opr.Where(t => t.ProductionOrderID == ueEmriID).Select(t => t.ProducedQuantity).FirstOrDefault());
            }

            return Tuple.Create(planlananAdet, gerceklesenAdet);

        }

        public async Task<List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ProductGroupsAnalysis> result = new List<ProductGroupsAnalysis>();

            var operationLines = (await ProductionTrackingsAppService.GetListbyOprStartDateRangeIsEqualAsync(startDate, endDate)).Data.Where(t => t.ProductType == Entities.Enums.ProductTypeEnum.MM).ToList();
            var groupList = operationLines.Select(t => t.ProductGroupID).Distinct().ToList();

            if (groupList != null && groupList.Count > 0)
            {
                foreach (var groupID in groupList)
                {
                    Tuple<int, int> tuple = _PlanlananAdetHesapla(groupID, operationLines);


                    string productGroupName = ProductGroups.Where(t => t.Id == groupID).Select(t => t.Name).FirstOrDefault();
                    int totalScrap = Convert.ToInt32(operationLines.Where(t => t.ProductGroupID == groupID).Sum(t => t.FaultyQuantity));



                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = productGroupName,
                        PlannedQuantity = tuple.Item1,
                        TotalProduction = tuple.Item2,
                        TotalScrap = totalScrap,
                        Quality = tuple.Item1 > 0 && tuple.Item2 > 0 ? ((double)tuple.Item2 / (double)tuple.Item1) : 0
                    };
                    result.Add(analysis);
                }
            }

            await Task.CompletedTask;
            return result;
        }

        public async Task<List<AdminProductChart>> GetProductChart(DateTime startDate, DateTime endDate, int frequency, Guid? productionSelection)
        {
            List<AdminProductChart> result = new List<AdminProductChart>();

            var operationLines = (await ProductionTrackingsAppService.GetListbyOprStartDateRangeIsEqualAsync(startDate, endDate)).Data.ToList();

            var unsuitabilityLines = (await OperationUnsuitabilityReportsAppService.GetListbyOprStartDateRangeIsEqualAsync(startDate, endDate)).Data.ToList();

            Tuple<int, int> tuple = _PlanlananAdetHesapla(productionSelection, operationLines);

            double previousMonthScrapPercent = 0;
            decimal hurda = 0;
            decimal uretilenadet = 0;
            double scrappercent = 0;

            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.Where(t => t.ProductGroupID == productionSelection).OrderBy(t => t.OperationStartDate).GroupBy(t => new { Ay = t.OperationStartDate.Value.Month, YIL = t.OperationStartDate.Value.Year }).Select(t =>
                {

                    #region Değişkenler

                    previousMonthScrapPercent = scrappercent;

                    hurda = unsuitabilityLines.Where(x => x.ProductGroupID == productionSelection && x.Action_=="Hurda" && x.Date_.Value.Month == t.Key.Ay).Sum(x => x.UnsuitableAmount);

                    uretilenadet = t.Sum(t => t.ProducedQuantity);

                    scrappercent = (double)hurda / (double)uretilenadet;

                    decimal differenceScrapPercent = (decimal)(scrappercent - previousMonthScrapPercent);

                    #endregion

                    return new AdminProductChart
                    {
                        Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                        ScrapPercent = scrappercent,
                        DIFFSCRAPPERCENT = differenceScrapPercent,
                        PRODUCTION = uretilenadet,
                        SCRAP = hurda
                    };
                }).ToList();
                result = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.OperationStartDate.Value.Date, YIL = t.OperationStartDate.Value.Year }).OrderBy(t => t.Key.HAFTA).Select(t =>
                {

                    #region Değişkenler

                    previousMonthScrapPercent = scrappercent;

                    hurda = unsuitabilityLines.Where(x => x.ProductGroupID == productionSelection && x.Action_ == "Hurda" && x.Date_.Value.Date == t.Key.HAFTA).Sum(x => x.UnsuitableAmount);

                    uretilenadet = t.Sum(t => t.ProducedQuantity);

                    scrappercent = (double)hurda / (double)uretilenadet;

                    decimal differenceScrapPercent = (decimal)(scrappercent - previousMonthScrapPercent);

                    #endregion

                    return new AdminProductChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                        ScrapPercent = scrappercent,
                        DIFFSCRAPPERCENT = differenceScrapPercent,
                        PRODUCTION = uretilenadet,
                        SCRAP = hurda
                    };


                }).ToList();
                result = gList;
            }


            await Task.CompletedTask;
            return result;
        }

        private string GetMonth(int ay)
        {
            string aystr = string.Empty;
            switch (ay)
            {
                case 1: aystr = "Ocak"; break;
                case 2: aystr = "Şubat"; break;
                case 3: aystr = "Mart"; break;
                case 4: aystr = "Nisan"; break;
                case 5: aystr = "Mayıs"; break;
                case 6: aystr = "Haziran"; break;
                case 7: aystr = "Temmuz"; break;
                case 8: aystr = "Ağustos"; break;
                case 9: aystr = "Eylül"; break;
                case 10: aystr = "Ekim"; break;
                case 11: aystr = "Kasım"; break;
                case 12: aystr = "Aralık"; break;
                default: break;

            }
            return aystr;
        }
    }
}
