using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Route
{
    public partial class RoutesListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<ListRoutesDto> _grid;
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
            CreateMainContextMenuItems();


            await GetProductsList();
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

            ShowEditPage();


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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListRoutesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await RoutesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectRouteLines;
                    GridProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductId == DataSource.ProductID).ToList();
                   


                    foreach (var item in GridLineList)
                    {
                        item.OperationCode = GridProductsOperationList.Where(t => t.Id == item.ProductsOperationID).Select(t => t.Code).FirstOrDefault();
                        item.OperationName = GridProductsOperationList.Where(t => t.Id == item.ProductsOperationID).Select(t => t.Name).FirstOrDefault();

                        if (GridProductsOperationList.Any(t => t.Id == item.ProductsOperationID))
                        {
                            var deletedOperation = GridProductsOperationList.Find(t => t.Id == item.ProductsOperationID);

                            if (deletedOperation != null)
                            {
                                GridProductsOperationList.Remove(deletedOperation);
                            }

                        }
                    }




                    EditPageVisible = true;
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

        #region Ürünler 
        public async Task ProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductsComboBox.FilterAsync(ProductsList, query);
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task ProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductID = args.ItemData.Id;
                DataSource.ProductCode = args.ItemData.Code;
                DataSource.ProductName = args.ItemData.Name;

                GridProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductId == DataSource.ProductID).ToList();

            }
            else
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;

            }

            await _ProductsOperationGrid.Refresh();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
