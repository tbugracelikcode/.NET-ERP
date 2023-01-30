using Castle.Core.Resource;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.Station
{
    public partial class StationsListPage
    {
        SfTextBox TextBoxSearchObj;
        bool SelectStationGroupPopupVisible = false;


        SfComboBox<string, ListStationGroupsDto> StationGroupComboBox;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();



        public async Task OnCreateSearch()
        {
            // Event creation with event handler
            var Click = EventCallback.Factory.Create<MouseEventArgs>(this, SearchClick);
            await TextBoxSearchObj.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", Click } });
        }



        public async void SearchClick()
        {
            SelectStationGroupPopupVisible = true;
            await InvokeAsync(StateHasChanged);
        }



        protected override async void OnInitialized()
        {
            BaseCrudService = StationsService;
            await GetStationGroupsList();
        }

        private async Task GetStationGroupsList()
        {
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectStationsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public async Task StationGroupFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await StationGroupComboBox.FilterAsync(StationGroupList, query);
        }

        public async Task StationGroupValueChangeHandler(ChangeEventArgs<string, ListStationGroupsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.GroupID = args.ItemData.Id;
                DataSource.StationGroup = args.ItemData.Name;
            }
            else
            {
                DataSource.GroupID = Guid.Empty;
                DataSource.StationGroup = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
