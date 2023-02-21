using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.ErpUI.Pages.Period
{

    public partial class PeriodsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = PeriodsService;
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
