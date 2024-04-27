using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Business.Entities.UnsuitabilityItemSPC.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using static TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItemSPCComparing.UnsuitabilityItemSPCComparingsListPage;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationalSPCComparing
{
    public partial class OperationalSPCComparingsListPage : IDisposable
    {
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

        protected override async Task OnInitializedAsync()
        {
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
                        WorkCenterName = firstSPCLineList.Where(t => t.OperationID == operation.Id).Select(t => t.WorkCenterName).FirstOrDefault(),
                        FirstRPNValue = firstSPCLineList.Where(t => t.OperationID == operation.Id).Select(t => t.RPN).FirstOrDefault(),
                        SecondRPNValue = secondSPCLineList.Where(t => t.OperationID == operation.Id).Select(t => t.RPN).FirstOrDefault()

                    };
                }
            }

        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
