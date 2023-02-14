using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Syncfusion.Blazor.Inputs;
using Microsoft.AspNetCore.Components.Web;

namespace TsiErp.ErpUI.Pages.BillsofMaterial
{
    public partial class BillsofMaterialsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> LineFinishedProductsComboBox;
        List<ListProductsDto> LineFinishedProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<SelectBillsofMaterialLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectBillsofMaterialLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectBillsofMaterialLinesDto> GridLineList = new List<SelectBillsofMaterialLinesDto>();

        private bool LineCrudPopup = false;

        #region Birim Setleri ButtonEdit
        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                LineDataSource.UnitSetID = selectedUnitSet.Id;
                LineDataSource.UnitSetCode = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = BillsofMaterialsAppService;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Reçete Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectBillsofMaterialsDto()
            {
                IsActive = true
            };

            DataSource.SelectBillsofMaterialLines = new List<SelectBillsofMaterialLinesDto>();
            GridLineList = DataSource.SelectBillsofMaterialLines;

            ShowEditPage();


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListBillsofMaterialsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await BillsofMaterialsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectBillsofMaterialLines;

                    foreach (var item in GridLineList)
                    {
                        item.FinishedProductCode = DataSource.FinishedProductCode;
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz reçete, kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectBillsofMaterialLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    if(DataSource.FinishedProductCode != null )
                    {
                        LineDataSource = new SelectBillsofMaterialLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
                        LineDataSource.FinishedProductID = DataSource.FinishedProductID;
                        LineDataSource.LineNr = GridLineList.Count + 1;
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync("Uyarı", "Mamül seçmeden yeni satır eklenemez.");
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectBillsofMaterialLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectBillsofMaterialLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectBillsofMaterialLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
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
            if (LineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Birim seti seçilmeden satır kaydetme işlemi yapılamaz.");
            }
            else
            {
                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectBillsofMaterialLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectBillsofMaterialLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                    }
                }

                LineDataSource.FinishedProductID = DataSource.FinishedProductID;
                LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
                GridLineList = DataSource.SelectBillsofMaterialLines;
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }
           
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
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                LineDataSource.ProductName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Mamül Button Edit

        SfTextBox FinishedProductsCodeButtonEdit;
        SfTextBox FinishedProductsNameButtonEdit;
        bool SelectFinishedProductsPopupVisible = false;
        List<ListProductsDto> FinishedProductsList = new List<ListProductsDto>();
        public async Task FinishedProductsCodeOnCreateIcon()
        {
            var FinishedProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, FinishedProductsCodeButtonClickEvent);
            await FinishedProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", FinishedProductsButtonClick } });
        }

        public async void FinishedProductsCodeButtonClickEvent()
        {
            SelectFinishedProductsPopupVisible = true;
            await GetFinishedProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task FinishedProductsNameOnCreateIcon()
        {
            var FinishedProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, FinishedProductsNameButtonClickEvent);
            await FinishedProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", FinishedProductsButtonClick } });
        }

        public async void FinishedProductsNameButtonClickEvent()
        {
            SelectFinishedProductsPopupVisible = true;
            await GetFinishedProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void FinishedProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.FinishedProductID = Guid.Empty;
                DataSource.FinishedProductCode = string.Empty;
                DataSource.FinishedProducName = string.Empty;
            }
        }

        public async void FinishedProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedFinishedProduct = args.RowData;

            if (selectedFinishedProduct != null)
            {
                DataSource.FinishedProductID = selectedFinishedProduct.Id;
                DataSource.FinishedProductCode = selectedFinishedProduct.Code;
                DataSource.FinishedProducName = selectedFinishedProduct.Name;
                SelectFinishedProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetFinishedProductsList()
        {
            FinishedProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion


    }
}
