using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        List<ListBranchesDto> Gridlist = new List<ListBranchesDto>();

        [Parameter]
        public ButtonRenderStyle SubmitButtonRenderStyle { get; set; }
                       = ButtonRenderStyle.Secondary;

        [Parameter]
        public ButtonRenderStyleMode SubmitButtonRenderStyleMode { get; set; }
                           = ButtonRenderStyleMode.Outline;

        [Parameter] public string SubmitButtonText { get; set; }

        [Parameter]
        public ButtonRenderStyle CancelButtonRenderStyle { get; set; }
                           = ButtonRenderStyle.Secondary;

        [Parameter]
        public ButtonRenderStyleMode CancelButtonRenderStyleMode { get; set; }
                           = ButtonRenderStyleMode.Outline;

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await BranchesService.GetListAsync(new ListBranchesParameterDto() { IsActive = true })).Data.ToList();
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
