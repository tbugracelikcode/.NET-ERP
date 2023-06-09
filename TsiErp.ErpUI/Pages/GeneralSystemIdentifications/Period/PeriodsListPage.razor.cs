using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Period
{

    public partial class PeriodsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = PeriodsService;
            _L = L;
            await GetBranchsList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPeriodsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #region Şube ButtonEdit

        SfTextBox BranchButtonEdit;
        bool SelectBranchPopupVisible = false;
        List<ListBranchesDto> BranchList = new List<ListBranchesDto>();

        public async Task BranchOnCreateIcon()
        {
            var BranchButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchButtonClickEvent);
            await BranchButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchButtonClick } });
        }

        public async void BranchButtonClickEvent()
        {
            SelectBranchPopupVisible = true;
            BranchList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void BranchOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchName = string.Empty;
            }
        }

        public async void BranchDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                DataSource.BranchID = selectedBranch.Id;
                DataSource.BranchName = selectedBranch.Name;
                SelectBranchPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        private async Task GetBranchsList()
        {
            BranchList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }


    }
}
