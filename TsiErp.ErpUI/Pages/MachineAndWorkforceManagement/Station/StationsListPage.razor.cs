using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Station
{
    public partial class StationsListPage
    {

        #region İstasyon Grubu ButtonEdit

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
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
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

        #endregion

        #region Stok ButtonEdit

        SfTextBox ProductButtonEdit;
        bool SelectProductPopupVisible = false;
        List<ListProductsDto> ProductList = new List<ListProductsDto>();

        public async Task ProductOnCreateIcon()
        {
            var ProductButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductButtonClickEvent);
            await ProductButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductButtonClick } });
        }

        public async void ProductButtonClickEvent()
        {
            SelectProductPopupVisible = true;
            ProductList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                InventoryDataSource.ProductID = Guid.Empty;
                InventoryDataSource.ProductCode = string.Empty;
            }
        }

        public async void ProductDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                InventoryDataSource.ProductID = selectedProduct.Id;
                InventoryDataSource.ProductCode = selectedProduct.Code;
                SelectProductPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

    

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectStationInventoriesDto InventoryDataSource = new();


        private SfGrid<SelectStationInventoriesDto> _InventoryGrid;
        public List<ContextMenuItemModel> InventoryGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectStationInventoriesDto> InventoryList = new List<SelectStationInventoriesDto>();


        protected override async void OnInitialized()
        {
            BaseCrudService = StationsService;
            _L = L;

            CreateLineContextMenuItems();
            CreateMainContextMenuItems();


        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
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

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
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

            DataSource.SelectStationInventoriesDto = new List<SelectStationInventoriesDto>();

            InventoryList = DataSource.SelectStationInventoriesDto;

            EditPageVisible=true;

        }

        protected void CreateLineContextMenuItems()
        {
            if (InventoryGridContextMenu.Count() == 0)
            {
                InventoryGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                InventoryGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
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
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);
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

                    await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);

                    InventoryDataSource = new SelectStationInventoriesDto();

                    await _InventoryGrid.Refresh();
                }
            }
            else
            {
                InventoryList.Add(InventoryDataSource);

                InventoryDataSource = new SelectStationInventoriesDto();

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

        #region GetList Metotları 

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion


    }
}
