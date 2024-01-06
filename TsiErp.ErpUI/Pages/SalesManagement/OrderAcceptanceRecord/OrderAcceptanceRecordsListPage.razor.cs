using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.XlsIO;
using System.Data;
using System.Dynamic;
using System.Linq;
using TsiErp.Business.Entities.Forecast.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.ErpUI.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.OrderAcceptanceRecord
{
    public partial class OrderAcceptanceRecordsListPage : IDisposable
    {
        private SfGrid<VirtualLineModel> _VirtualLineGrid;

        VirtualLineModel VirtualLineDataSource;

        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<VirtualLineModel> GridVirtualLineList = new List<VirtualLineModel>();

        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        private bool VirtualLineCrudPopup = false;

        public class VirtualLineModel
        {
            public Guid Id { get; set; }
            public bool IsProductExists { get; set; }
            public Guid OrderAcceptanceRecordID { get; set; }
            public int LineNr { get; set; }
            public Guid? ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public Guid? ProductReferanceNumberID { get; set; }
            public string OrderReferanceNo { get; set; }
            public string CustomerReferanceNo { get; set; }
            public string CustomerBarcodeNo { get; set; }
            public decimal MinOrderAmount { get; set; }
            public decimal OrderAmount { get; set; }
            public Guid? UnitSetID { get; set; }
            public string UnitSetCode { get; set; }
            public decimal DefinedUnitPrice { get; set; }
            public decimal OrderUnitPrice { get; set; }
            public decimal LineAmount { get; set; }
            public string Description_ { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = OrderAcceptanceRecordsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Sipariş Kabul Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOrderAcceptanceRecordsDto()
            {
                Date_ = DateTime.Today,
                ConfirmedLoadingDate = DateTime.Today,
                CustomerRequestedDate = DateTime.Today,
                ProductionOrderLoadingDate = DateTime.Today,
                OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.Beklemede,
                Code = FicheNumbersAppService.GetFicheNumberAsync("OrderAcceptanceRecordsChildMenu")

            };

            DataSource.SelectOrderAcceptanceRecordLines = new List<SelectOrderAcceptanceRecordLinesDto>();
            GridVirtualLineList = new List<VirtualLineModel>();

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextRefresh"], Id = "refresh" });
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListOrderAcceptanceRecordsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var productList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                    foreach (var line in DataSource.SelectOrderAcceptanceRecordLines)
                    {
                        if (productList.Any(t => t.Id == line.ProductID))
                        {
                            VirtualLineModel virtualModel = new VirtualLineModel
                            {
                                Id = line.Id,
                                CustomerBarcodeNo = line.CustomerBarcodeNo,
                                CustomerReferanceNo = line.CustomerReferanceNo,
                                DefinedUnitPrice = line.DefinedUnitPrice,
                                Description_ = line.Description_,
                                IsProductExists = true,
                                LineAmount = line.LineAmount,
                                LineNr = line.LineNr,
                                MinOrderAmount = line.MinOrderAmount,
                                OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                OrderAmount = line.OrderAmount,
                                OrderReferanceNo = line.OrderReferanceNo,
                                OrderUnitPrice = line.OrderUnitPrice,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                ProductReferanceNumberID = line.ProductReferanceNumberID,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                            };

                            GridVirtualLineList.Add(virtualModel);
                        }

                        else
                        {
                            VirtualLineModel virtualModel = new VirtualLineModel
                            {
                                Id = line.Id,
                                CustomerBarcodeNo = line.CustomerBarcodeNo,
                                CustomerReferanceNo = line.CustomerReferanceNo,
                                DefinedUnitPrice = line.DefinedUnitPrice,
                                Description_ = line.Description_,
                                IsProductExists = false,
                                LineAmount = line.LineAmount,
                                LineNr = line.LineNr,
                                MinOrderAmount = line.MinOrderAmount,
                                OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                OrderAmount = line.OrderAmount,
                                OrderReferanceNo = line.OrderReferanceNo,
                                OrderUnitPrice = line.OrderUnitPrice,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                ProductReferanceNumberID = line.ProductReferanceNumberID,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                            };

                            GridVirtualLineList.Add(virtualModel);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<VirtualLineModel> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.CurrentAccountCardID == Guid.Empty || DataSource.CurrentAccountCardID == null)
                    {
                        await ModalManager.WarningPopupAsync(L["UICurrentAccountTitle"], L["UICurrentAccountMessage"]);
                    }
                    else
                    {
                        VirtualLineDataSource = new VirtualLineModel();
                        ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
                        VirtualLineCrudPopup = true;
                        VirtualLineDataSource.LineNr = GridVirtualLineList.Count + 1;
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    VirtualLineDataSource = args.RowInfo.RowData;
                    ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
                    VirtualLineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupLineMessage"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            GridVirtualLineList.Remove(line);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                GridVirtualLineList.Remove(line);
                                var actualLine = DataSource.SelectOrderAcceptanceRecordLines.Where(t => t.Id == line.Id).FirstOrDefault();
                                DataSource.SelectOrderAcceptanceRecordLines.Remove(actualLine);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                GridVirtualLineList.Remove(line);
                            }
                        }

                        await _VirtualLineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _VirtualLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            VirtualLineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (VirtualLineDataSource.Id == Guid.Empty)
            {
                if (GridVirtualLineList.Contains(VirtualLineDataSource))
                {
                    int selectedLineIndex = GridVirtualLineList.FindIndex(t => t.LineNr == VirtualLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        GridVirtualLineList[selectedLineIndex] = VirtualLineDataSource;
                    }
                }
                else
                {
                    GridVirtualLineList.Add(VirtualLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = GridVirtualLineList.FindIndex(t => t.Id == VirtualLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    GridVirtualLineList[selectedLineIndex] = VirtualLineDataSource;
                }
            }
            await _VirtualLineGrid.Refresh();
            HideLinesPopup();
            await InvokeAsync(StateHasChanged);

        }

        protected override Task OnSubmit()
        {
            foreach (var line in GridVirtualLineList)
            {
                if (!DataSource.SelectOrderAcceptanceRecordLines.Any(t => t.Id == line.Id))
                {
                    SelectOrderAcceptanceRecordLinesDto lineModel = new SelectOrderAcceptanceRecordLinesDto
                    {
                        Id = line.Id,
                        CustomerBarcodeNo = line.CustomerBarcodeNo,
                        UnitSetID = line.UnitSetID,
                        DefinedUnitPrice = line.DefinedUnitPrice,
                        Description_ = line.Description_,
                        LineAmount = line.LineAmount,
                        LineNr = line.LineNr,
                        MinOrderAmount = line.MinOrderAmount,
                        UnitSetCode = line.UnitSetCode,
                        ProductReferanceNumberID = line.ProductReferanceNumberID,
                        ProductName = line.ProductName,
                        ProductID = line.ProductID,
                        ProductCode = line.ProductCode,
                        OrderUnitPrice = line.OrderUnitPrice,
                        OrderReferanceNo = line.OrderReferanceNo,
                        OrderAmount = line.OrderAmount,
                        OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                        CustomerReferanceNo = line.CustomerReferanceNo,
                    };

                    DataSource.SelectOrderAcceptanceRecordLines.Add(lineModel);
                }

            }

            return base.OnSubmit();
        }

        private async void ProductCodeValueChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (ProductsList.Any(t => t.Code == VirtualLineDataSource.ProductCode))
            {
                var productRefNo = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.ProductCode == VirtualLineDataSource.ProductCode && t.CurrentAccountCardID == DataSource.CurrentAccountCardID).FirstOrDefault();

                var tempProductList = ProductsList.Where(t => t.Code == VirtualLineDataSource.ProductCode).ToList();

                VirtualLineDataSource.OrderReferanceNo = productRefNo.OrderReferanceNo;
                VirtualLineDataSource.CustomerReferanceNo = productRefNo.CustomerReferanceNo;
                VirtualLineDataSource.CustomerBarcodeNo = productRefNo.CustomerBarcodeNo;
                VirtualLineDataSource.MinOrderAmount = productRefNo.MinOrderAmount;
                VirtualLineDataSource.UnitSetCode = tempProductList.Select(t => t.UnitSetCode).FirstOrDefault();
                VirtualLineDataSource.UnitSetID = tempProductList.Select(t => t.UnitSetID).FirstOrDefault();
                VirtualLineDataSource.ProductID = tempProductList.Select(t => t.Id).FirstOrDefault();
                VirtualLineDataSource.ProductReferanceNumberID = productRefNo.Id;

                var salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.StartDate <= DataSource.Date_ && t.EndDate >= DataSource.Date_ && t.CurrentAccountCardID == DataSource.CurrentAccountCardID && t.IsActive == true && t.IsApproved == true).Select(t => t.Id).FirstOrDefault();
                var salesPriceLine = (await SalesPricesAppService.GetAsync(salesPriceID)).Data.SelectSalesPriceLines.Where(t => t.ProductCode == VirtualLineDataSource.ProductCode).FirstOrDefault();

                VirtualLineDataSource.DefinedUnitPrice = salesPriceLine.Price;
            }

            else
            {
                VirtualLineDataSource.OrderReferanceNo = string.Empty;
                VirtualLineDataSource.CustomerReferanceNo = string.Empty;
                VirtualLineDataSource.CustomerBarcodeNo = string.Empty;
                VirtualLineDataSource.MinOrderAmount = 0;
                VirtualLineDataSource.UnitSetCode = string.Empty;
                VirtualLineDataSource.UnitSetID = Guid.Empty;
                VirtualLineDataSource.DefinedUnitPrice = 0;
            }
        }

        private void OrderValueChangeHandler(ChangeEventArgs<decimal> args)
        {
            VirtualLineDataSource.LineAmount = VirtualLineDataSource.OrderAmount * VirtualLineDataSource.OrderUnitPrice;
        }

        #region Excel'den Aktarım
        SfGrid<ExpandoObject> Grid;
        public DataTable table = new DataTable();
        private void OnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                #region Expando Object Örneği

                var path = file.FileInfo.Name;
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                var check = ExcelService.ImportGetPath(path);
                FileStream openFileStream = new FileStream(check, FileMode.OpenOrCreate);
                file.Stream.WriteTo(openFileStream);
                FileStream fileStream = new FileStream(check, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets[1];
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                GenerateListFromTable(table);

                #endregion

            }
        }
        string[] Columns;
        public List<ExpandoObject> OrderList;
        public async void GenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            Columns = input.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            foreach (DataRow row in input.Rows)
            {
                System.Dynamic.ExpandoObject e = new System.Dynamic.ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            OrderList = list;


            foreach (var item in OrderList)
            {
                dynamic row = item;

                #region Row DBNull Kontrolleri

                string productCode =!string.IsNullOrEmpty(Convert.ToString(row.ProductCode)) ?  (string)row.ProductCode : string.Empty;
                string customerBarcodeNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerBarcodeNo)) ?  (string)row.CustomerBarcodeNo : string.Empty;
                string customerReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerReferanceNo)) ?  (string)row.CustomerReferanceNo : string.Empty;
                string orderReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.OrderReferanceNo)) ?  (string)row.OrderReferanceNo : string.Empty;
                decimal lineAmount =!string.IsNullOrEmpty(Convert.ToString(row.LineAmount)) ? Convert.ToDecimal(row.LineAmount) : 0;
                decimal orderAmount = !string.IsNullOrEmpty(Convert.ToString(row.OrderAmount)) ? Convert.ToDecimal(row.OrderAmount) : 0;
                decimal orderUnitPrice = !string.IsNullOrEmpty(Convert.ToString(row.OrderUnitPrice)) ? Convert.ToDecimal(row.OrderUnitPrice) : 0;

                #endregion

                var product = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.Code == productCode).FirstOrDefault();

                if (product != null)
                {
                    var salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.StartDate <= DataSource.Date_ && t.EndDate >= DataSource.Date_ && t.CurrentAccountCardID == DataSource.CurrentAccountCardID && t.IsActive == true && t.IsApproved == true).Select(t => t.Id).FirstOrDefault();
                    var salesPriceLine = (await SalesPricesAppService.GetAsync(salesPriceID)).Data.SelectSalesPriceLines.Where(t => t.ProductCode == product.Code).FirstOrDefault();
                    var productRefNo = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.ProductCode == product.Code && t.CurrentAccountCardID == DataSource.CurrentAccountCardID).FirstOrDefault();

                    VirtualLineModel virtualModel = new VirtualLineModel
                    {
                        CustomerBarcodeNo = customerBarcodeNo,
                        ProductCode = product.Code,
                        ProductName = product.Name,
                        ProductID = product.Id,
                        CustomerReferanceNo = customerReferanceNo,
                        DefinedUnitPrice = salesPriceLine.Price,
                        Description_ = string.Empty,
                        IsProductExists = true,
                        LineAmount = lineAmount,
                        LineNr = GridVirtualLineList.Count + 1,
                        MinOrderAmount = productRefNo.MinOrderAmount,
                        OrderAmount = orderAmount,
                        OrderReferanceNo = orderReferanceNo,
                        OrderUnitPrice = orderUnitPrice,
                        ProductReferanceNumberID = productRefNo.Id,
                        UnitSetID = product.UnitSetID,
                        UnitSetCode = product.UnitSetCode,
                    };

                    GridVirtualLineList.Add(virtualModel);
                }
                else if(product == null && productCode!= string.Empty)
                {
                    VirtualLineModel virtualModel = new VirtualLineModel
                    {
                        CustomerBarcodeNo = customerBarcodeNo,
                        ProductCode = productCode,
                        ProductName = string.Empty,
                        ProductID = Guid.Empty,
                        CustomerReferanceNo = customerReferanceNo,
                        DefinedUnitPrice = 0,
                        Description_ = string.Empty,
                        IsProductExists = false,
                        LineAmount = lineAmount,
                        LineNr = GridVirtualLineList.Count + 1,
                        MinOrderAmount = 0,
                        OrderAmount = orderAmount,
                        OrderReferanceNo = orderReferanceNo,
                        OrderUnitPrice = orderUnitPrice,
                        ProductReferanceNumberID = Guid.Empty,
                        UnitSetID = Guid.Empty,
                        UnitSetCode = string.Empty,
                    };

                    GridVirtualLineList.Add(virtualModel);
                }

            }

            await _VirtualLineGrid.Refresh();
        }

        #endregion

        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsNameButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCodeButtonClickEvent);
            await CurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task CurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsNameButtonClickEvent);
            await CurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsNameButtonClick } });
        }

        public async void CurrentAccountCardsNameButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CurrentAccountCardCustomerCode = string.Empty;
                DataSource.CurrenyID = Guid.Empty;
                DataSource.CurrenyCode = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccount = args.RowData;

            if (selectedCurrentAccount != null)
            {
                DataSource.CurrentAccountCardID = selectedCurrentAccount.Id;
                DataSource.CurrentAccountCardCode = selectedCurrentAccount.Code;
                DataSource.CurrentAccountCardName = selectedCurrentAccount.Name;
                DataSource.CurrentAccountCardCustomerCode = selectedCurrentAccount.CustomerCode;
                DataSource.CurrenyID = selectedCurrentAccount.CurrencyID;
                DataSource.CurrenyCode = selectedCurrentAccount.Currency;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("OrderAcceptanceRecordsChildMenu");
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
