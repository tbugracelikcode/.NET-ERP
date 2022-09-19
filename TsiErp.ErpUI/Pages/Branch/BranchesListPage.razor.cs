using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        List<ListBranchesDto> Gridlist = new List<ListBranchesDto>();
        SfGrid<ListBranchesDto> Grid;

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
            Gridlist = (await BranchesService.GetListAsync(new ListBranchesParameterDto() { IsActive = true })).Data.ToList();
        }

        void BranchesPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }

        //private async void Click()
        //{

        //}
    }
}
