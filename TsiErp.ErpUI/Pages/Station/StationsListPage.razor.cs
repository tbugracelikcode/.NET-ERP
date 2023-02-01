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
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Station
{
    public partial class StationsListPage
    {
        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            await GetStationGroupsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.GroupID = Guid.Empty;
                DataSource.StationGroup = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.GroupID = selectedStationGroup.Id;
                DataSource.StationGroup = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectStationInventoriesDto InventoryDataSource = new();


        private SfGrid<SelectStationInventoriesDto> _InventoryGrid;
        private SfGrid<ListStationsDto> _grid;
        public List<ContextMenuItemModel> InventoryGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectStationInventoriesDto> InventoryList = new List<SelectStationInventoriesDto>();


        protected override async void OnInitialized()
        {
            CreateLineContextMenuItems();
            CreateMainContextMenuItems();

            BaseCrudService = StationsService;
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListStationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await StationsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    InventoryList = DataSource.SelectStationInventoriesDto;
                    InventoryDataSource = new SelectStationInventoriesDto();

                    if(InventoryList != null)
                    {
                        foreach (var item in InventoryList)
                        {
                            item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                            item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                            item.StationID = DataSource.Id;
                        }
                    }


                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz istasyon, kalıcı olarak silinecektir.");
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectStationsDto()
            {
                IsActive = true
            };

            InventoryList = DataSource.SelectStationInventoriesDto;

            ShowEditPage();

        }

        protected void CreateLineContextMenuItems()
        {
            if (InventoryGridContextMenu.Count() == 0)
            {
                InventoryGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                InventoryGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
            }
        }

        public async void OnInventoryContextMenuClick(ContextMenuClickEventArgs<SelectStationInventoriesDto> args)
        {
            switch (args.Item.Id)
            {

                case "changed":

                    InventoryDataSource = args.RowInfo.RowData;

                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır, kalıcı olarak silinecektir.");
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _InventoryGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;


                default:
                    break;
            }
        }

        public async Task OnInventorySubmit()
        {
            if(InventoryList != null)
            {
                var line = InventoryList.Find(t => t.ProductID == InventoryDataSource.ProductID);

                if (line == null)
                {
                    InventoryList.Add(InventoryDataSource);

                    await _InventoryGrid.Refresh();

                    InventoryDataSource = new SelectStationInventoriesDto();

                }
                else
                {
                    line = InventoryDataSource;
                    await ModalManager.WarningPopupAsync("Uyarı", "Envanterde aynı stok kodlu bir satır bulunmaktadır.");
                    InventoryDataSource = new SelectStationInventoriesDto();
                    await _InventoryGrid.Refresh();
                }
            }
            else
            {
                InventoryList.Add(InventoryDataSource);
                await _InventoryGrid.Refresh();

            }

            await InvokeAsync(() => StateHasChanged());
        }

        protected override async Task OnSubmit()
        {
            SelectStationsDto result;

            DataSource.SelectStationInventoriesDto = InventoryList;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectStationsDto, CreateStationsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectStationsDto, UpdateStationsDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
        }


        #region İstasyon Grupları

        private async Task GetStationGroupsList()
        {
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
        }
    }
}
