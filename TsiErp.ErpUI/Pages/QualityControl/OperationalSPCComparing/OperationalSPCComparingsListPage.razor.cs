using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.ErpUI.Components.Commons.Spinner;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationalSPCComparing
{
    public partial class OperationalSPCComparingsListPage : IDisposable
    {

        SfGrid<OperationalSPCComparingModel> _GridComp;
        public class OperationalSPCComparingModel
        {
            public Guid? OperationID { get; set; }

            public string OperationName { get; set; }

            public string WorkCenterName { get; set; }

            public int FirstRPNValue { get; set; }

            public int SecondRPNValue { get; set; }
        }

        public List<ListProductsOperationsDto> OperationsList = new List<ListProductsOperationsDto>();

        public List<OperationalSPCComparingModel> GridList = new List<OperationalSPCComparingModel>();

        public List<ListOperationalSPCsDto> OperationalSPCList = new List<ListOperationalSPCsDto>();

        public DateTime FirstDate = new DateTime();
        public DateTime SecondDate = new DateTime();

        [Inject]
        SpinnerService Spinner {  get; set; }   

        protected override async Task OnInitializedAsync()
        {
            Spinner.Show();
            await Task.Delay(100);

            OperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();

            OperationalSPCList = (await OperationalSPCsAppService.GetListAsync(new ListOperationalSPCsParameterDto())).Data.OrderByDescending(t => t.Date_).ToList();

            if( OperationalSPCList.Count > 0 && OperationalSPCList != null)
            {
                FirstDate = OperationalSPCList[0].Date_;

                SecondDate = OperationalSPCList[1].Date_;

                var firstSPCLineList = (await OperationalSPCsAppService.GetAsync(OperationalSPCList[0].Id)).Data.SelectOperationalSPCLines;

                var secondSPCLineList = (await OperationalSPCsAppService.GetAsync(OperationalSPCList[1].Id)).Data.SelectOperationalSPCLines;

                foreach (var operation in OperationsList)
                {
                    OperationalSPCComparingModel gridModel = new OperationalSPCComparingModel
                    {

                        OperationID = operation.Id,
                        OperationName = operation.Name,
                        WorkCenterName =operation.WorkCenterName,
                        FirstRPNValue = firstSPCLineList.Where(t => t.OperationID == operation.Id).Select(t => t.RPN).FirstOrDefault(),
                        SecondRPNValue = secondSPCLineList.Where(t => t.OperationID == operation.Id).Select(t => t.RPN).FirstOrDefault()

                    };

                    GridList.Add(gridModel);

                }
                await _GridComp.Refresh();


            }

            Spinner.Hide();

        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
