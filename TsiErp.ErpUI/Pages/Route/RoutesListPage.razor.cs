using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Route
{
    public partial class RoutesListPage
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


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = RoutesAppService;
            _L = L;
            CreateMainContextMenuItems();

        }


        #region Rota Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectRoutesDto()
            {
                IsActive = true
            };

            DataSource.SelectRouteLines = new List<SelectRouteLinesDto>();
            GridLineList = DataSource.SelectRouteLines;

            EditPageVisible = true;


            await Task.CompletedTask;
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

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync("Bilgi", "Seçtiğiniz kayıt ..... tarafından kullanılmaktadır.");
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
                    GridLineList = DataSource.SelectRouteLines;
                    GridProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductId == DataSource.ProductID).ToList();
                   


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
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz rota, kalıcı olarak silinecektir.");
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
                var selectedRow = _LineGrid.SelectedRecords;

                foreach (var item in selectedRow)
                {
                    ListProductsOperationsDto listProductsOperations = new ListProductsOperationsDto
                    {
                        Code = item.OperationCode,
                        Name = item.OperationName,
                        Id = item.ProductsOperationID
                    };

                    GridProductsOperationList.Add(listProductsOperations);
                    SelectRouteLinesDto removedItem = GridLineList.Where(t=>t.OperationName == item.OperationName && t.OperationCode == item.OperationCode).FirstOrDefault();
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

        public async void ArrowRightBtnClicked()
        {

            if (GridProductsOperationList.Count != 0)
            {
                var selectedRow = _ProductsOperationGrid.SelectedRecords[0];

                if (selectedRow.Id != Guid.Empty)
                {
                    ProductsOperationLinesList = (await ProductsOperationsAppService.GetAsync(selectedRow.Id)).Data.SelectProductsOperationLines;
                }


                var productsOperationLine = ProductsOperationLinesList.Where(t => t.Priority == 1).FirstOrDefault();

                if (productsOperationLine != null)
                {

                    if(!GridLineList.Any(t=>t.Id==productsOperationLine.Id))
                    {

                        SelectRouteLinesDto selectRouteLine = new SelectRouteLinesDto
                        {
                            LineNr = 0,
                            Priority =0,
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

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == GridLineList.Count()))
            {
                GridLineList[index].Priority += 1;
                GridLineList[index + 1].Priority -= 1;
                GridLineList[index].LineNr += 1;
                GridLineList[index + 1].LineNr -= 1;

                GridLineList = GridLineList.OrderBy(t => t.LineNr).ToList();

                DataSource.SelectRouteLines = GridLineList;

                await _LineGrid.Refresh();
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

        #region GetList Metotları 
        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

    }
}
