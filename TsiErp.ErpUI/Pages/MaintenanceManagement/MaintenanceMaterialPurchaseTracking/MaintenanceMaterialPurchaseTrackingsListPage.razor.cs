using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using TsiErp.Business.Entities.MaintenanceMRP.Services;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenanceMRP.MaintenanceMRPsListPage;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenanceMaterialPurchaseTracking
{
    public partial class MaintenanceMaterialPurchaseTrackingsListPage : IDisposable
    {
        public class MaintenanceMaterialPurchaseTracking
        {
            public string PurchaseOrderNo { get; set; }
            public string PurchaseCurrentAccountCode { get; set; }
            public string PurchaseCurrentAccountName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal RequirementAmount { get; set; }
            public decimal OrderAmount { get; set; }
            public string SupplyStatus { get; set; }
            public DateTime? SupplyDate { get; set; }

        }

        private SfGrid<MaintenanceMaterialPurchaseTracking> _grid;

        public List<MaintenanceMaterialPurchaseTracking> ListDataSource = new List<MaintenanceMaterialPurchaseTracking>();
        List<ListPurchaseOrdersDto> PurchaseOrdersList = new List<ListPurchaseOrdersDto>();
        List<SelectPurchaseOrderLinesDto> PurchaseOrderLinesList = new List<SelectPurchaseOrderLinesDto>();


        [Inject]
        IJSRuntime JsRuntime { get; set; }
        [Inject]
        ModalManager ModalManager { get; set; }

        #region Değişkenler

        DateTime? FilterStartDate = DateTime.Today;
        DateTime? FilterEndDate = DateTime.Today;
        Guid? productID = Guid.Empty;
        string productCode = string.Empty;
        string productName = string.Empty;
        int supplyStatus = 0;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };
        public List<ItemModel> GridToolbarItems { get; set; } = new List<ItemModel>();
        public string GridSearchText { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            foreach (var item in _supplyStatusComboBox)
            {
                item.Text = L[item.Text];
            }

            CreateGridToolbar();
        }

        public async void FilterButtonClicked()
        {
            if (productID == null || productID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningProductTitle"], L["UIWarningProductMessage"]);
            }
            else
            {
                ListDataSource.Clear();

                if (supplyStatus == 0)
                {
                    PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList();

                    foreach (var item in PurchaseOrdersList)
                    {
                        PurchaseOrderLinesList = (await PurchaseOrdersAppService.GetAsync(item.Id)).Data.SelectPurchaseOrderLinesDto.Where(t => t.ProductID == productID.GetValueOrDefault()).ToList();


                        foreach (var line in PurchaseOrderLinesList)
                        {

                            decimal requirementAmount = (await MRPsAppService.GetAsync(item.MRPID.GetValueOrDefault())).Data.SelectMRPLines.Where(t => t.ProductID == line.ProductID).Select(t => t.RequirementAmount).Sum();

                            string stateLine = string.Empty;

                            switch(line.PurchaseOrderLineStateEnum)
                            {
                                case Entities.Enums.PurchaseOrderLineStateEnum.Beklemede: stateLine = L["EnumWaiting"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi: stateLine = L["EnumInPartialCompleted"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Onaylandı: stateLine = L["EnumApproved"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Iptal: stateLine = L["EnumCancel"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi: stateLine = L["EnumCompleted"]; break;
                                default:break;
                            }

                            MaintenanceMaterialPurchaseTracking gridModel = new MaintenanceMaterialPurchaseTracking
                            {
                                PurchaseCurrentAccountCode = item.CurrentAccountCardCode,
                                PurchaseCurrentAccountName = item.CurrentAccountCardName,
                                OrderAmount = Convert.ToInt32(line.Quantity),
                                ProductCode = line.ProductCode,
                                ProductName = line.ProductName,
                                SupplyDate = line.SupplyDate,
                                SupplyStatus = stateLine,
                                PurchaseOrderNo = item.FicheNo,
                                RequirementAmount = requirementAmount
                            };

                            ListDataSource.Add(gridModel);
                        }
                    }

                    await _grid.Refresh();
                }
                else
                {

                    switch (supplyStatus)
                    {
                        case 1: PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Beklemede && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList(); break;
                        case 2: PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Onaylandı && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList();  break;
                        case 3: PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Tamamlandi && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList(); break;
                        case 4: PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Iptal && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList(); break;
                        case 5: PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.Where(t => t.Date_ >= FilterStartDate && t.Date_ <= FilterEndDate && t.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.KismiTamamlandi && t.MaintenanceMRPID != null && t.MaintenanceMRPID != Guid.Empty).ToList(); break;
                        default: break;
                    }

                    foreach (var item in PurchaseOrdersList)
                    {
                        PurchaseOrderLinesList = (await PurchaseOrdersAppService.GetAsync(item.Id)).Data.SelectPurchaseOrderLinesDto.Where(t => t.ProductID == productID.GetValueOrDefault()).ToList();

                        foreach (var line in PurchaseOrderLinesList)
                        {

                            decimal requirementAmount = (await MRPsAppService.GetAsync(item.MRPID.GetValueOrDefault())).Data.SelectMRPLines.Where(t => t.ProductID == line.ProductID).Select(t => t.RequirementAmount).Sum();

                            string stateLine = string.Empty;

                            switch (line.PurchaseOrderLineStateEnum)
                            {
                                case Entities.Enums.PurchaseOrderLineStateEnum.Beklemede: stateLine = L["EnumWaiting"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi: stateLine = L["EnumInPartialCompleted"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Onaylandı: stateLine = L["EnumApproved"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Iptal: stateLine = L["EnumCancel"]; break;
                                case Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi: stateLine = L["EnumCompleted"]; break;
                                default: break;
                            }

                            MaintenanceMaterialPurchaseTracking gridModel = new MaintenanceMaterialPurchaseTracking
                            {
                                PurchaseCurrentAccountCode = item.CurrentAccountCardCode,
                                PurchaseCurrentAccountName = item.CurrentAccountCardName,
                                OrderAmount = Convert.ToInt32(line.Quantity),
                                ProductCode = line.ProductCode,
                                ProductName = line.ProductName,
                                SupplyDate = line.SupplyDate,
                                SupplyStatus = stateLine,
                                PurchaseOrderNo = item.FicheNo,
                                RequirementAmount = requirementAmount
                            };

                            ListDataSource.Add(gridModel);
                        }
                    }

                    await _grid.Refresh();

                }


            }
        }

        #region Grid İşlemleri

        public void CreateGridToolbar()
        {
            var loc = (IStringLocalizer)L;

            RenderFragment search = (builder) =>
            {
                builder.OpenComponent(0, typeof(SfTextBox));
                builder.AddAttribute(1, "ID", "srcText");
                builder.AddAttribute(2, "CssClass", "TSITxtBox");
                builder.AddAttribute(3, "Value", BindConverter.FormatValue(GridSearchText));
                builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => GridSearchText = __value, GridSearchText));
                builder.AddAttribute(5, "onkeydown", OnToolbarSearchChange);
                builder.AddAttribute(6, "ShowClearButton", true);
                builder.CloseComponent();
            };


            GridToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = @loc["UIExportFileName"] });


            GridToolbarItems.Add(new ItemModel() { Id = "PDFExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIPdfIcon", TooltipText = @loc["UIExportFileName"] });


            GridToolbarItems.Add(new ItemModel() { Id = "Search", CssClass = "TSIToolbarTxtBox", Type = ItemType.Input, Template = search, Text = GridSearchText });


        }

        public async void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "ExcelExport")
            {
                ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();
                ExcelExportProperties.FileName = args.Item.TooltipText + ".xlsx";
                await this._grid.ExportToExcelAsync(ExcelExportProperties);
            }

            if (args.Item.Id == "PDFExport")
            {
                PdfExportProperties PdfExportProperties = new PdfExportProperties();
                PdfExportProperties.PageOrientation = PageOrientation.Landscape;
                PdfExportProperties.FileName = args.Item.TooltipText + ".pdf";
                await this._grid.ExportToPdfAsync(PdfExportProperties);
            }

        }

        public async void OnToolbarSearchChange(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                if (!string.IsNullOrEmpty(GridSearchText))
                {
                    await _grid.SearchAsync(GridSearchText.ToLower());

                    await JsRuntime.InvokeVoidAsync("focusInput", "srcText");
                }
                else
                {
                    await _grid.SearchAsync("");

                    await JsRuntime.InvokeVoidAsync("focusInput", "srcText");
                }
            }
        }

        #endregion

        #region ComboBox İşlemleri
        public class SupplyStatusComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<SupplyStatusComboBox> _supplyStatusComboBox = new List<SupplyStatusComboBox>
        {
            new SupplyStatusComboBox(){ID = "0", Text="Select"},
            new SupplyStatusComboBox(){ID = "1", Text="EnumWaiting"},
            new SupplyStatusComboBox(){ID = "2", Text="EnumApproved"},
            new SupplyStatusComboBox(){ID = "3", Text="EnumCompleted"},
            new SupplyStatusComboBox(){ID = "4", Text="EnumCancel"},
            new SupplyStatusComboBox(){ID = "5", Text="EnumInPartialCompleted"},
        };

        private void SupplyStatusComboBoxValueChangeHandler(ChangeEventArgs<string, SupplyStatusComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0": supplyStatus = 0; break;
                    case "1": supplyStatus = 1; break;
                    case "2": supplyStatus = 2; break;
                    case "3": supplyStatus = 3; break;
                    case "4": supplyStatus = 4; break;
                    case "5": supplyStatus = 5; break;

                    default: break;
                }
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
                productID = Guid.Empty;
                productCode = string.Empty;
                productName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                productID = selectedProduct.Id;
                productCode = selectedProduct.Code;
                productName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
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
