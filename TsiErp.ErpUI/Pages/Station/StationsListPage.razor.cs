using Castle.Core.Resource;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.Station
{
    public partial class StationsListPage
    {

        SfComboBox<string, ListStationGroupsDto> StationGroupComboBox;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        private SfGrid<ListStationsDto> _grid;


        protected override async void OnInitialized()
        {
            BaseCrudService = StationsService;
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

        public async Task StationGroupOpened(PopupEventArgs args)
        {
            if(StationGroupList.Count==0)
            {
                await GetStationGroupsList();
            }
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }
    }
}
