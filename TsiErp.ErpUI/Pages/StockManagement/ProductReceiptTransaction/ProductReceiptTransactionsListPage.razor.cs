using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.ProductReceiptTransaction
{
    public partial class ProductReceiptTransactionsListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }
        [Inject]
        SpinnerService Spinner { get; set; }

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public bool GrantWarehouseApprovalVisible = false;
        public string TemplatePartyNo = string.Empty;

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductReceiptTransactionsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductReceiptTransactionsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion

            CreateMainContextMenuItems();
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

                            case "ProductReceiptTransactionsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReceiptTransactionsContextAdd"], Id = "add" }); break;
                            case "ProductReceiptTransactionsContextApproveIncomingQuantity":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReceiptTransactionsContextApproveIncomingQuantity"], Id = "approveincomingquantity" }); break;
                            case "ProductReceiptTransactionsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReceiptTransactionsContextChange"], Id = "changed" }); break;
                            case "ProductReceiptTransactionsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReceiptTransactionsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductReceiptTransactionsDto()
            {
                WaybillDate = GetSQLDateAppService.GetDateFromSQL().Date,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProductReceiptTransactionsChildMenu")
            };

            await Task.CompletedTask;
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductReceiptTransactionsDto> args)
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

                        DataSource = (await ProductReceiptTransactionsService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var purchaseOrderLine = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data.SelectPurchaseOrderLinesDto.Where(t => t.Id == DataSource.PurchaseOrderLineID.GetValueOrDefault()).FirstOrDefault();

                    if (purchaseOrderLine != null && purchaseOrderLine.Id != Guid.Empty)
                    {
                        var purchaseOrderLineState = purchaseOrderLine.PurchaseOrderLineStateEnum;

                        if (purchaseOrderLineState == Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi)
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningStockTitle"], L["UIWarningStockMessage"]);
                        }
                        else
                        {
                            IsChanged = true;
                            DataSource.PurchaseOrderQuantity = purchaseOrderLine.Quantity;
                            ShowEditPage();
                        }
                    }

                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "approveincomingquantity":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductReceiptTransactionsService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.ProductReceiptTransactionStateEnum == Entities.Enums.ProductReceiptTransactionStateEnum.KaliteKontrolOnayVerildi)
                    {
                        DateTime wayBillDate = GetSQLDateAppService.GetDateFromSQL();
                        string YearStr = (wayBillDate.Year - 2000).ToString();
                        string DayStr = wayBillDate.Day.ToString();
                        string MonthStr = wayBillDate.Month.ToString();
                        string OrderNo = DataSource.PurchaseOrderFicheNo.TrimStart('0');

                        TemplatePartyNo = "AR" + YearStr + DayStr + MonthStr + "-" + OrderNo;

                        GrantWarehouseApprovalVisible = true;
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningGrantApprovalTitle"], L["UIWarningGrantApprovalMessage"]);
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

        public void HideGrantApproval()
        {
            GrantWarehouseApprovalVisible = false;
        }

        public async void OnGrantWarehouseApprovalSubmit()
        {
            if (DataSource.WarehouseReceiptQuantity > 0)
            {
                Spinner.Show();
                await Task.Delay(100);

                DataSource.ProductReceiptTransactionStateEnum = Entities.Enums.ProductReceiptTransactionStateEnum.DepoOnayiVerildi;

                DataSource.PartyNo = TemplatePartyNo + DataSource.PartyNo;

                var updatedEntity = ObjectMapper.Map<SelectProductReceiptTransactionsDto, UpdateProductReceiptTransactionsDto>(DataSource);

                await ProductReceiptTransactionsService.UpdateApproveIncomingQuantityAsync(updatedEntity);

                await GetListDataSourceAsync();

                #region Sipariş ve Sipariş Satır İrsaliye Durumunu Güncelleme

                var purchaseOrder = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data;

                if (purchaseOrder != null && purchaseOrder.Id != Guid.Empty && purchaseOrder.SelectPurchaseOrderLinesDto != null && purchaseOrder.SelectPurchaseOrderLinesDto.Count > 0)
                {
                    var purchaseOrderLine = purchaseOrder.SelectPurchaseOrderLinesDto.Where(t => t.Id == DataSource.PurchaseOrderLineID.GetValueOrDefault()).FirstOrDefault();

                    if (purchaseOrderLine != null)
                    {
                        int lineIndex = purchaseOrder.SelectPurchaseOrderLinesDto.IndexOf(purchaseOrderLine);

                        #region Sipariş Satır Onaylanma Durumu

                        var grantedApprovalList = ListDataSource.Where(t => t.PurchaseOrderLineID == DataSource.PurchaseOrderLineID && t.ProductReceiptTransactionStateEnum == ProductReceiptTransactionStateEnum.DepoOnayiVerildi).ToList();

                        if (grantedApprovalList.Select(t=>t.WarehouseReceiptQuantity).Sum() >= DataSource.PurchaseOrderQuantity)
                        {
                            purchaseOrder.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineStateEnum = PurchaseOrderLineStateEnum.Onaylandı;
                        }
                        else if(grantedApprovalList.Select(t => t.WarehouseReceiptQuantity).Sum() < DataSource.PurchaseOrderQuantity)
                        {
                            purchaseOrder.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineStateEnum = PurchaseOrderLineStateEnum.KismiOnayVerildi;
                        }

                        #endregion

                        purchaseOrder.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineWayBillStatusEnum = PurchaseOrderLineWayBillStatusEnum.Onaylandi;

                        purchaseOrder.SelectPurchaseOrderLinesDto[lineIndex].PartyNo = DataSource.PartyNo;

                        var updatedPurchaseOrderEntity = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(purchaseOrder);

                        await PurchaseOrdersAppService.UpdateAsync(updatedPurchaseOrderEntity);
                    }
                }

                #endregion

                GrantWarehouseApprovalVisible = false;

                Spinner.Hide();

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPartyNoQuantityTitle"], L["UIWarningPartyNoQuantityMessage"]);
            }
        }

        #region Satın Alma Sipariş Button Edit

        SfTextBox PurchaseOrdersButtonEdit;
        bool SelectPurchaseOrdersPopupVisible = false;
        List<ListPurchaseOrdersDto> PurchaseOrdersList = new List<ListPurchaseOrdersDto>();
        public async Task PurchaseOrdersOnCreateIcon()
        {
            var PurchaseOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, PurchaseOrdersButtonClickEvent);
            await PurchaseOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", PurchaseOrdersButtonClick } });
        }

        public async void PurchaseOrdersButtonClickEvent()
        {
            SelectPurchaseOrdersPopupVisible = true;
            PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void PurchaseOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PurchaseOrderID = Guid.Empty;
                DataSource.PurchaseOrderFicheNo = string.Empty;
                DataSource.PurchaseOrderDate = DateTime.MinValue;
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void PurchaseOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListPurchaseOrdersDto> args)
        {
            var selectedPurchaseOrder = args.RowData;

            if (selectedPurchaseOrder != null)
            {
                DataSource.PurchaseOrderID = selectedPurchaseOrder.Id;
                DataSource.PurchaseOrderFicheNo = selectedPurchaseOrder.FicheNo;
                DataSource.PurchaseOrderDate = selectedPurchaseOrder.Date_;
                DataSource.CurrentAccountCardID = selectedPurchaseOrder.CurrentAccountCardID;
                DataSource.CurrentAccountCardCode = selectedPurchaseOrder.CurrentAccountCardCode;
                DataSource.CurrentAccountCardName = selectedPurchaseOrder.CurrentAccountCardName;
                SelectPurchaseOrdersPopupVisible = false;
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
            if (DataSource.PurchaseOrderID != null && DataSource.PurchaseOrderID != Guid.Empty)
            {
                ProductsList = new List<ListProductsDto>();

                var productsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                var purchaseOrderLines = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data.SelectPurchaseOrderLinesDto;

                foreach (var product in purchaseOrderLines)
                {
                    var addedProduct = productsList.Where(t => t.Id == product.ProductID.GetValueOrDefault()).FirstOrDefault();

                    if (addedProduct != null && addedProduct.Id != Guid.Empty)
                    {
                        ProductsList.Add(addedProduct);
                    }
                }


                SelectProductsPopupVisible = true;

            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPurchaseOrderTitle"], L["UIWarningPurchaseOrderMessage"]);
            }

            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            if (DataSource.PurchaseOrderID != null && DataSource.PurchaseOrderID != Guid.Empty)
            {
                var productsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                var purchaseOrderLines = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data.SelectPurchaseOrderLinesDto;

                foreach (var product in purchaseOrderLines)
                {
                    var addedProduct = productsList.Where(t => t.Id == product.ProductID.GetValueOrDefault()).FirstOrDefault();

                    if (addedProduct != null && addedProduct.Id != Guid.Empty)
                    {
                        ProductsList.Add(addedProduct);
                    }
                }


                SelectProductsPopupVisible = true;

            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPurchaseOrderTitle"], L["UIWarningPurchaseOrderMessage"]);
            }

            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.SupplierProductCode = string.Empty;
                DataSource.PurchaseOrderLineID = Guid.Empty;
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
                DataSource.SupplierProductCode = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(DataSource.ProductID.GetValueOrDefault(), DataSource.CurrentAccountCardID.GetValueOrDefault());

                var purchaseOrderLine = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data.SelectPurchaseOrderLinesDto.Where(t => t.ProductID == selectedProduct.Id).FirstOrDefault();

                if (purchaseOrderLine != null && purchaseOrderLine.Id != Guid.Empty)
                {
                    DataSource.PurchaseOrderLineID = purchaseOrderLine.Id;
                    DataSource.PurchaseOrderQuantity = purchaseOrderLine.Quantity;
                }

                SelectProductsPopupVisible = false;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeAnnualSeniorityDifferencesChildMenu");
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
