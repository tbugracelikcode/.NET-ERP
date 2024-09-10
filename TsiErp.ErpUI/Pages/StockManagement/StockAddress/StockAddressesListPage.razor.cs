using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockSection.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockShelf.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.StockAddress
{
    public partial class StockAddressesListPage : IDisposable
    {
        private SfGrid<SelectStockAddressLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectStockAddressLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectStockAddressLinesDto> GridLineList = new List<SelectStockAddressLinesDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private bool LineCrudPopup = false;


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("StockAddressesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = StockAddressesService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "StockAddressesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
        }

        #region Stok Adresleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectStockAddressesDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("StockAddressesChildMenu")
            };

            DataSource.SelectStockAddressLines = new List<SelectStockAddressLinesDto>();
            GridLineList = DataSource.SelectStockAddressLines;

            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "StockAddressesLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesLineContextAdd"], Id = "new" }); break;
                            case "StockAddressesLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesLineContextChange"], Id = "changed" }); break;
                            case "StockAddressesLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesLineContextDelete"], Id = "delete" }); break;
                            case "StockAddressesLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }



        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {

                            case "StockAddressesContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesContextAdd"], Id = "add" }); break;
                            case "StockAddressesContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesContextChange"], Id = "changed" }); break;
                            case "StockAddressesContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesContextDelete"], Id = "delete" }); break;
                            case "StockAddressesContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockAddressesContextRefresh"], Id = "refresh" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListStockAddressesDto> args)
        {
            switch (args.Item.Id)
            {
                case "add":

                    await BeforeInsertAsync();
                    EditPageVisible = true;
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
 IsChanged = true;
                    DataSource = (await StockAddressesService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectStockAddressLines;


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                       
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectStockAddressLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    LineDataSource = new SelectStockAddressLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectStockAddressLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectStockAddressLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectStockAddressLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {


            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectStockAddressLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectStockAddressLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectStockAddressLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectStockAddressLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectStockAddressLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectStockAddressLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectStockAddressLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


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
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
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
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
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
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Stok Sütun Button Edit

        SfTextBox StockColumnsButtonEdit;
        bool SelectStockColumnsPopupVisible = false;
        List<ListStockColumnsDto> StockColumnsList = new List<ListStockColumnsDto>();
        public async Task StockColumnsOnCreateIcon()
        {
            var StockColumnsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockColumnsButtonClickEvent);
            await StockColumnsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockColumnsButtonClick } });
        }

        public async void StockColumnsButtonClickEvent()
        {
            SelectStockColumnsPopupVisible = true;
            StockColumnsList = (await StockColumnsService.GetListAsync(new ListStockColumnsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }
        
        public void StockColumnsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.StockColumnID = Guid.Empty;
                LineDataSource.StockColumnName = string.Empty;
            }
        }

        public async void StockColumnsDoubleClickHandler(RecordDoubleClickEventArgs<ListStockColumnsDto> args)
        {
            var selectedStockColumn = args.RowData;

            if (selectedStockColumn != null)
            {
                LineDataSource.StockColumnID = selectedStockColumn.Id;
                LineDataSource.StockColumnName = selectedStockColumn.Name;
                SelectStockColumnsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Stok Numara Button Edit

        SfTextBox StockNumbersButtonEdit;
        bool SelectStockNumbersPopupVisible = false;
        List<ListStockNumbersDto> StockNumbersList = new List<ListStockNumbersDto>();
        public async Task StockNumbersOnCreateIcon()
        {
            var StockNumbersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockNumbersButtonClickEvent);
            await StockNumbersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockNumbersButtonClick } });
        }

        public async void StockNumbersButtonClickEvent()
        {
            SelectStockNumbersPopupVisible = true;
            StockNumbersList = (await StockNumbersService.GetListAsync(new ListStockNumbersParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StockNumbersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.StockNumberID = Guid.Empty;
                LineDataSource.StockNumberName = string.Empty;
            }
        }

        public async void StockNumbersDoubleClickHandler(RecordDoubleClickEventArgs<ListStockNumbersDto> args)
        {
            var selectedStockNumber = args.RowData;

            if (selectedStockNumber != null)
            {
                LineDataSource.StockNumberID = selectedStockNumber.Id;
                LineDataSource.StockNumberName = selectedStockNumber.Name;
                SelectStockNumbersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Stok Bölüm Button Edit

        SfTextBox StockSectionsButtonEdit;
        bool SelectStockSectionsPopupVisible = false;
        List<ListStockSectionsDto> StockSectionsList = new List<ListStockSectionsDto>();
        public async Task StockSectionsOnCreateIcon()
        {
            var StockSectionsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockSectionsButtonClickEvent);
            await StockSectionsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockSectionsButtonClick } });
        }

        public async void StockSectionsButtonClickEvent()
        {
            SelectStockSectionsPopupVisible = true;
            StockSectionsList = (await StockSectionsService.GetListAsync(new ListStockSectionsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StockSectionsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.StockSectionID = Guid.Empty;
                LineDataSource.StockSectionName = string.Empty;
            }
        }

        public async void StockSectionsDoubleClickHandler(RecordDoubleClickEventArgs<ListStockSectionsDto> args)
        {
            var selectedStockSection = args.RowData;

            if (selectedStockSection != null)
            {
                LineDataSource.StockSectionID = selectedStockSection.Id;
                LineDataSource.StockSectionName = selectedStockSection.Name;
                SelectStockSectionsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Stok Raf Button Edit

        SfTextBox StockShelfsButtonEdit;
        bool SelectStockShelfsPopupVisible = false;
        List<ListStockShelfsDto> StockShelfsList = new List<ListStockShelfsDto>();
        public async Task StockShelfsOnCreateIcon()
        {
            var StockShelfsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockShelfsButtonClickEvent);
            await StockShelfsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockShelfsButtonClick } });
        }

        public async void StockShelfsButtonClickEvent()
        {
            SelectStockShelfsPopupVisible = true;
            StockShelfsList = (await StockShelfsService.GetListAsync(new ListStockShelfsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StockShelfsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.StockShelfID = Guid.Empty;
                LineDataSource.StockShelfName = string.Empty;
            }
        }

        public async void StockShelfsDoubleClickHandler(RecordDoubleClickEventArgs<ListStockShelfsDto> args)
        {
            var selectedStockShelf = args.RowData;

            if (selectedStockShelf != null)
            {
                LineDataSource.StockShelfID = selectedStockShelf.Id;
                LineDataSource.StockShelfName = selectedStockShelf.Name;
                SelectStockShelfsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
