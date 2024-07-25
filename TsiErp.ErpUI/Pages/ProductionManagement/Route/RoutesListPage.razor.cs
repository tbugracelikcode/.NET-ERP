using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.Route
{
    public partial class RoutesListPage : IDisposable
    {
        private SfGrid<SelectRouteLinesDto> _LineGrid;
        private SfGrid<ListProductsOperationsDto> _ProductsOperationGrid;

        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectRouteLinesDto> GridLineList = new List<SelectRouteLinesDto>();

        List<ListProductsOperationsDto> GridProductsOperationList = new List<ListProductsOperationsDto>();

        List<SelectProductsOperationLinesDto> ProductsOperationLinesList = new List<SelectProductsOperationLinesDto>();
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = RoutesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "RoutesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();

        }


        #region Rota Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectRoutesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("RoutesChildMenu")
            };

            DataSource.SelectRouteLines = new List<SelectRouteLinesDto>();
            GridLineList = DataSource.SelectRouteLines;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "RouteContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["RouteContextAdd"], Id = "new" }); break;
                            case "RouteContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["RouteContextChange"], Id = "changed" }); break;
                            case "RouteContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["RouteContextDelete"], Id = "delete" }); break;
                            case "RouteContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["RouteContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListRoutesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await RoutesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    DataSource.SelectRouteLines = DataSource.SelectRouteLines.OrderBy(t=>t.LineNr).ToList();
                    GridLineList = DataSource.SelectRouteLines;
                    GridProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID).ToList();
                   


                    foreach (var item in GridLineList)
                    {
                       

                        if (GridProductsOperationList.Any(t => t.Id == item.ProductsOperationID))
                        {
                            var deletedOperation = GridProductsOperationList.Find(t => t.Id == item.ProductsOperationID);

                            if (deletedOperation != null)
                            {
                                GridProductsOperationList.Remove(deletedOperation);
                            }

                        }
                    }

                    ShowEditPage();
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

        public async void ArrowLeftBtnClicked()
        {
            if (GridLineList.Count != 0)
            {
                List<SelectRouteLinesDto> selectedRow = new List<SelectRouteLinesDto>();
                if(_LineGrid.SelectedRecords.Count > 0)
                {
                    selectedRow = _LineGrid.SelectedRecords;

                    foreach (var item in selectedRow)
                    {
                        ListProductsOperationsDto listProductsOperations = new ListProductsOperationsDto
                        {
                            Code = item.OperationCode,
                            Name = item.OperationName,
                            Id = item.ProductsOperationID
                        };

                        GridProductsOperationList.Add(listProductsOperations);
                        SelectRouteLinesDto removedItem = GridLineList.Where(t => t.OperationName == item.OperationName && t.OperationCode == item.OperationCode).FirstOrDefault();
                        GridLineList.Remove(removedItem);
                    }

                    for (int i = 0; i < GridLineList.Count; i++)
                    {
                        GridLineList[i].LineNr = i + 1;
                        GridLineList[i].Priority = i + 1;
                    }

                    DataSource.SelectRouteLines = GridLineList;

                    await _ProductsOperationGrid.Refresh();
                    await _LineGrid.Refresh();
                }
               
            }
        }

        public async void ArrowRightBtnClicked()
        {

            if (GridProductsOperationList.Count != 0)
            {
                ListProductsOperationsDto selectedRow = new ListProductsOperationsDto();
                if(_ProductsOperationGrid.SelectedRecords.Count > 0)
                {
                    selectedRow = _ProductsOperationGrid.SelectedRecords[0];

                    if (selectedRow.Id != Guid.Empty)
                    {
                        ProductsOperationLinesList = (await ProductsOperationsAppService.GetAsync(selectedRow.Id)).Data.SelectProductsOperationLines;
                    }


                    var productsOperationLine = ProductsOperationLinesList.Where(t => t.Priority == 1).FirstOrDefault();

                    if (productsOperationLine != null)
                    {

                        if (!GridLineList.Any(t => t.Id == productsOperationLine.Id))
                        {

                            SelectRouteLinesDto selectRouteLine = new SelectRouteLinesDto
                            {
                                LineNr = 0,
                                Priority = 0,
                                AdjustmentAndControlTime = (int)productsOperationLine.AdjustmentAndControlTime,
                                OperationCode = selectedRow.Code,
                                OperationName = selectedRow.Name,
                                OperationTime = productsOperationLine.OperationTime,
                                ProductCode = DataSource.ProductCode,
                                ProductName = DataSource.ProductName,
                                ProductID = DataSource.ProductID,
                                ProductsOperationID = productsOperationLine.ProductsOperationID.GetValueOrDefault()


                            };

                            GridLineList.Add(selectRouteLine);

                            for (int i = 0; i < GridLineList.Count; i++)
                            {
                                GridLineList[i].LineNr = i + 1;
                                GridLineList[i].Priority = i + 1;
                            }

                            GridProductsOperationList.Remove(selectedRow);

                            DataSource.SelectRouteLines = GridLineList;

                            await _ProductsOperationGrid.Refresh();
                            await _LineGrid.Refresh();
                        }

                    }
                } 
            }

        }


        public void OnRowDataBound(RowDataBoundEventArgs<SelectRouteLinesDto> args)
        {
            this._LineGrid.SelectRowAsync(SelectedRowIndex, true);
        }

        public int SelectedRowIndex { get; set; }

        public async void ArrowUpBtnClicked()
        {
            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == 0))
            {
                GridLineList[index].Priority -= 1;
                GridLineList[index - 1].Priority += 1;
                GridLineList[index].LineNr -= 1;
                GridLineList[index - 1].LineNr += 1;

                GridLineList = GridLineList.OrderBy(t => t.LineNr).ToList();

                DataSource.SelectRouteLines = GridLineList;

                SelectedRowIndex = index - 1;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == GridLineList.Count()-1))
            {
                GridLineList[index].Priority += 1;
                GridLineList[index + 1].Priority -= 1;
                GridLineList[index].LineNr += 1;
                GridLineList[index + 1].LineNr -= 1;

                GridLineList = GridLineList.OrderBy(t => t.LineNr).ToList();

                DataSource.SelectRouteLines = GridLineList;

                SelectedRowIndex = index + 1;
                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        protected override async  Task OnSubmit()
        {
            
            #region OnSubmit İşlemleri

            SelectRoutesDto result;

            if (DataSource.Id == Guid.Empty)
            {
                if (ListDataSource.Any(t => t.ProductID == DataSource.ProductID))
                {
                    await ModalManager.WarningPopupAsync(L["UIWarningSameProductTitle"], L["UIWarningSameProductMessage"]);
                }
                else
                {

                    var createInput = ObjectMapper.Map<SelectRoutesDto, CreateRoutesDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;

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



            }
            else
            {
                
                    var updateInput = ObjectMapper.Map<SelectRoutesDto, UpdateRoutesDto>(DataSource);

                    result = (await UpdateAsync(updateInput)).Data;

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

          

            

            #endregion
        }

        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.Name = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                DataSource.Name = selectedProduct.Code + " / " +  selectedProduct.Name;

                GridProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();
                GridProductsOperationList = GridProductsOperationList.Where(t => t.ProductID == selectedProduct.Id).ToList();
                await _ProductsOperationGrid.Refresh();
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İş Merkezi Button Edit

        SfTextBox StationGroupsButtonEdit;
        bool SelectStationGroupsPopupVisible = false;
        List<ListStationGroupsDto> StationGroupsList = new List<ListStationGroupsDto>();
        public async Task StationGroupsOnCreateIcon()
        {
            var StationGroupsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupsButtonClickEvent);
            await StationGroupsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupsButtonClick } });
        }

        public async void StationGroupsButtonClickEvent()
        {
            SelectStationGroupsPopupVisible = true;
            StationGroupsList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();    
            await InvokeAsync(StateHasChanged);
        }
        public void StationGroupsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationGroupID = Guid.Empty;
                DataSource.ProductionStart = string.Empty;
            }
        }

        public async void StationGroupsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.StationGroupID  = selectedStationGroup.Id;
                DataSource.ProductionStart = selectedStationGroup.Name;

                SelectStationGroupsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region GetList Metotları 
        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("RoutesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
