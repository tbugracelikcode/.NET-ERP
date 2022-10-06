using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.ErpUI.Pages.Period
{

    public partial class PeriodsListPage
    {
        SfComboBox<string, ListBranchesDto> BranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        private SfGrid<ListPeriodsDto> _grid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

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

            ShowEditPage();

            return Task.CompletedTask;
        }


        public async Task BranchFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await BranchesComboBox.FilterAsync(BranchesList, query);
        }

        private async Task GetBranchsList()
        {
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        public async Task BranchValueChangeHandler(ChangeEventArgs<string, ListBranchesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.BranchID = args.ItemData.Id;
                DataSource.BranchName = args.ItemData.Name;
            }
            else
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }



    }
}
