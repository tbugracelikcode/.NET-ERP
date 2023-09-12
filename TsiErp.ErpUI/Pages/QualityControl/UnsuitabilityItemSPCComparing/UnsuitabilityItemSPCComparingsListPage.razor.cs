using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItemSPCComparing
{
    public partial class UnsuitabilityItemSPCComparingsListPage
    {
        public class UnsuitabilitySPCComparingModel
        {
            public Guid? UnsuitabilityItemID { get; set; }

            public string UnsuitabilityItemName { get; set; }

            public int FirstRPNValue { get; set; }

            public int SecondRPNValue { get; set; }
        }

        public List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public List<UnsuitabilitySPCComparingModel> GridList = new List<UnsuitabilitySPCComparingModel>();

        public List<ListUnsuitabilityItemSPCsDto> UnsuitabilityItemSPCList = new List<ListUnsuitabilityItemSPCsDto>();

        public DateTime? FirstDate = new DateTime();
        public DateTime? SecondDate = new DateTime();

        protected override async Task OnInitializedAsync()
        {
            UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.ToList();

            UnsuitabilityItemSPCList = (await UnsuitabilityItemSPCsAppService.GetListAsync(new ListUnsuitabilityItemSPCsParameterDto())).Data.OrderByDescending(t => t.Date_).ToList();

            FirstDate = UnsuitabilityItemSPCList[0].Date_;

            SecondDate = UnsuitabilityItemSPCList[1].Date_;

            var firstSPCLineList = (await UnsuitabilityItemSPCsAppService.GetAsync(UnsuitabilityItemSPCList[0].Id)).Data.SelectUnsuitabilityItemSPCLines;

            var secondSPCLineList = (await UnsuitabilityItemSPCsAppService.GetAsync(UnsuitabilityItemSPCList[1].Id)).Data.SelectUnsuitabilityItemSPCLines;

            foreach (var unsuitabilityItem in UnsuitabilityItemsList)
            {
                UnsuitabilitySPCComparingModel gridModel = new UnsuitabilitySPCComparingModel
                {

                    UnsuitabilityItemID = unsuitabilityItem.Id,
                    UnsuitabilityItemName = unsuitabilityItem.Name,
                    FirstRPNValue = firstSPCLineList.Where(t => t.UnsuitabilityItemID == unsuitabilityItem.Id).Select(t => t.RPN).FirstOrDefault(),
                    SecondRPNValue = secondSPCLineList.Where(t => t.UnsuitabilityItemID == unsuitabilityItem.Id).Select(t => t.RPN).FirstOrDefault()

                };
            }
        }
    }
}
