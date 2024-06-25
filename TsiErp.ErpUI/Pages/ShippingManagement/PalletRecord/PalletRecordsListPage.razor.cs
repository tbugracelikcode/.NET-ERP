using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.PalletRecord.Services;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;
using Syncfusion.Blazor.Navigations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.ProductReferanceNumber.Services;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.ReportDtos.BigPalletLabelDtos;
using DevExpress.XtraReports.UI;
using TsiErp.ErpUI.Reports.ShippingManagement.PalletReports.PalletLabels;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using DevExpress.Blazor.Reporting;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;

namespace TsiErp.ErpUI.Pages.ShippingManagement.PalletRecord
{
    public partial class PalletRecordsListPage : IDisposable
    {
        private SfGrid<SelectPalletRecordLinesDto> _LineGrid;
        private SfGrid<PackageFicheSelectionGrid> _PackageFichesGrid;
        private SfGrid<PalletDetailGrid> _PalletDetailGrid;
        private SfGrid<LoadingDetailGrid> _LoadingDetailGrid;
        private SfGrid<TicketListGrid> _TicketListGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPalletRecordLinesDto LineDataSource = new();
        PalletDetailGrid PalletDetailDataSource = new();
        LoadingDetailGrid LoadingDetailDataSource = new();
        TicketListGrid TicketListlDataSource = new();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletDetailGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> TicketListGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPalletRecordLinesDto> GridLineList = new List<SelectPalletRecordLinesDto>();
        List<ListPackageFichesDto> PackageFichesList = new List<ListPackageFichesDto>();
        List<PackageFicheSelectionGrid> PackageFichesSelectionList = new List<PackageFicheSelectionGrid>();
        List<SelectPackageFicheLinesDto> PackageFicheLinesList = new List<SelectPackageFicheLinesDto>();
        List<PalletDetailGrid> PalletDetailGridList = new List<PalletDetailGrid>();
        List<LoadingDetailGrid> LoadingDetailGridList = new List<LoadingDetailGrid>();
        List<TicketListGrid> TicketListGridList = new List<TicketListGrid>();

        #region UI Classlar

        public class PackageFicheSelectionGrid
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string SalesOrderFicheNo { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public Guid? ProductID { get; set; }
            public Guid? SalesOrderID { get; set; }
            public string CustomerCode { get; set; }
            public int PackageContent { get; set; }
            public int NumberofPackage { get; set; }
            public bool SelectedLine { get; set; }
            public string PackageType { get; set; }
            public string PalletNo { get; set; }
            public DateTime LoadingDate { get; set; }
            public int TotalQuantity { get; set; }

        }

        public class PalletDetailGrid
        {
            public Guid Id { get; set; }
            public Guid ProductID { get; set; }
            public string ProductName { get; set; }
            public string EnglishDefinition { get; set; }
            public Guid ProductReferenceNumberID { get; set; }
            public string OrderReferenceNo { get; set; }
            public string CustomerReferenceNo { get; set; }
            public Guid PackingListID { get; set; }
            public string PackingListNo { get; set; }
            public string TicketNo { get; set; }
            public string OrderNo { get; set; }
            public Guid CurrentAccountID { get; set; }
            public string CustomerCode { get; set; }
            public Guid PackageFicheID { get; set; }
            public decimal PackageFicheProductWeight { get; set; }
            public decimal OrderQuantity { get; set; }
            public decimal MaxProductionQuantity { get; set; }
            public decimal PackageTotalQuantity { get; set; }
            public decimal OrderunitPrice { get; set; }
            public decimal ApprovedUnitPrice { get; set; }
            public Guid SalesPriceID { get; set; }
            public decimal SalesPriceListPrice { get; set; }
            public decimal LastOrderPrice { get; set; }
            public decimal TotalNetKG { get; set; }
            public decimal TotalGrossKG { get; set; }
            public decimal LineAmount { get; set; }
        }

        public class LoadingDetailGrid
        {
            public string SupplierRefNo { get; set; }
            public string CustomerRefNo { get; set; }
            public string ProductCode { get; set; }
            public string PalletName { get; set; }
            public string PackageOrderNo { get; set; }
            public int QuantityInPackage { get; set; }
            public int PackageQuantity { get; set; }
            public int TotalPackageQuantity { get; set; }
            public decimal OrderUnitPrice { get; set; }
            public decimal LineAmount { get; set; }
            public string ProductGroupName { get; set; }
            public string CustomerCode { get; set; }
            public string OrderNo { get; set; }
            public DateTime ConfirmedShippingDate { get; set; }
            public decimal ProductWeight { get; set; }
            public int OrderQuantity { get; set; }
            public int MaxProductionQuantity { get; set; }
            public decimal ApprovedUnitPrice { get; set; }
            public decimal SalesPriceListPrice { get; set; }
            public decimal LastOrderPrice { get; set; }
            public decimal TotalNetKG { get; set; }
            public decimal TotalGrossKG { get; set; }
            public string ProductName { get; set; }
            public string EnglishDefinition { get; set; }
            public string CustomerReferenceNo { get; set; }
            public string TicketNo { get; set; }
            public string CurrentAccountName { get; set; }
        }

        public class TicketListGrid
        {
            public bool IsApproved { get; set; }
            public string PalletNo { get; set; }
            public string ProductCode { get; set; }
            public string PackageOrderNo { get; set; }
            public int QuantityInPackage { get; set; }
            public int PackageQuantity { get; set; }
            public int TotalPackageQuantity { get; set; }
            public string PackageType { get; set; }
            public string OrderRefNo { get; set; }
            public string CustomerRefNo { get; set; }
            public string CustomerCode { get; set; }
            public string Status { get; set; }
        }

        #endregion

        private bool LineCrudPopup = false;
        public bool SelectPackageFichesModal = false;
        public int selectedNumberofPackages = 0;
        public bool PalletDetailPopupVisible = false;
        public bool PalletDetailCrudPopupVisible = false;
        public bool LoadingDetailPopupVisible = false;
        public bool TicketListPopupVisible = false;

        public string PalletNameFilter = string.Empty;
        public string PackageTypeFilter = string.Empty;
        public DateTime? LoadingDateFilter;
        public string CurrentAccountNameFilter = string.Empty;
        public Guid CurrentAccountIDFilter = Guid.Empty;

        public bool isAllColumns = false;


        public List<ItemModel> LoadingDetailGridToolbarItems { get; set; } = new List<ItemModel>();
        public List<ItemModel> TicketListGridToolbarItems { get; set; } = new List<ItemModel>();


        protected override async void OnInitialized()
        {
            BaseCrudService = PalletRecordsAppService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PalletRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreatePalletDetailContextMenuItems();
            CreateTicketListContextMenuItems();

        }

        #region Palet Kayıtları Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPalletRecordsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("PalletRecordsChildMenu"),
                MaxPackageNumber = 0,
                Lenght_ = 100,
                Height_ = 120,
                Width_ = 80
            };

            DataSource.SelectPalletRecordLines = new List<SelectPalletRecordLinesDto>();
            GridLineList = DataSource.SelectPalletRecordLines;


            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {
            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

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
                            case "PalletRecordLinesContextAddPackageFiche":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextAddPackageFiche"], Id = "addpackagefiche" }); break;
                            case "PalletRecordLinesContextRemovePackageFiche":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextRemovePackageFiche"], Id = "removepackagefiche" }); break;
                            case "PalletRecordLinesContextLineApproval":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextLineApproval"], Id = "approval" }); break;
                            case "PalletRecordLinesContextLineApprovalRemove":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextLineApprovalRemove"], Id = "approvalremove" }); break;
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
                            case "PalletRecordsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextAdd"], Id = "new" }); break;
                            case "PalletRecordsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextChange"], Id = "changed" }); break;
                            case "PalletRecordsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextDelete"], Id = "delete" }); break;
                            case "PalletRecordsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextRefresh"], Id = "refresh" }); break;

                            case "PalletRecordsContextState":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "PalletRecordsContextStatePreparing":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextStatePreparing"], Id = "preparing" }); break;
                                            case "PalletRecordsContextStateCompleted":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextStateCompleted"], Id = "completed" }); break;
                                            case "PalletRecordsContextStateApproved":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextStateApproved"], Id = "approved" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextState"], Id = "state", Items = subMenus }); break;

                            case "PalletRecordsContextTicketState":

                                List<MenuItem> subTicketMenus = new List<MenuItem>();

                                var subTicketList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subTicketMenu in subTicketList)
                                {
                                    var subTicketPermission = UserPermissionsList.Where(t => t.MenuId == subTicketMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subTicketPermission)
                                    {
                                        switch (subTicketMenu.MenuName)
                                        {
                                            case "PalletRecordsContextTicketStatePending":
                                                subTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextTicketStatePending"], Id = "ticketpending" }); break;
                                            case "PalletRecordsContextTicketStateCompleted":
                                                subTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextTicketStateCompleted"], Id = "ticketcompleted" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextTicketState"], Id = "ticketstate", Items = subTicketMenus }); break;

                            case "PalletRecordsContextPrintTicket":

                                List<MenuItem> subPrintTicketMenus = new List<MenuItem>();

                                var subPrintTicketList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subPrintTicketMenu in subPrintTicketList)
                                {
                                    var subPrintTicketPermission = UserPermissionsList.Where(t => t.MenuId == subPrintTicketMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPrintTicketPermission)
                                    {
                                        switch (subPrintTicketMenu.MenuName)
                                        {
                                            case "PalletRecordsContextPrintSmall":
                                                subPrintTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextPrintSmall"], Id = "printsmall" }); break;
                                            case "PalletRecordsContextPrintBig":
                                                subPrintTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextPrintBig"], Id = "printbig" }); break;
                                            case "PalletRecordsContextPrintPallet":
                                                subPrintTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextPrintPallet"], Id = "printpallet" }); break;
                                            case "PalletRecordsContextTicketList":
                                                subPrintTicketMenus.Add(new MenuItem { Text = L["PalletRecordsContextTicketList"], Id = "ticketlist" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextPrintTicket"], Id = "printticket", Items = subPrintTicketMenus }); break;

                            case "PalletRecordsContextPalletDetail":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextPalletDetail"], Id = "palletdetail" }); break;

                            case "PalletRecordsContextLoadingDetail":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextLoadingDetail"], Id = "loadingdetail" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreatePalletDetailContextMenuItems()
        {
            PalletDetailGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletDetailApproveUnitPrice"], Id = "approve" });
        }

        protected void CreateTicketListContextMenuItems()
        {
            TicketListGridContextMenu.Add(new ContextMenuItemModel { Text = L["TicketListPrint"], Id = "print" });
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPalletRecordsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPalletRecordLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "preparing":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PalletRecordsStateEnum != Entities.Enums.PalletRecordsStateEnum.Hazirlaniyor)
                    {
                        var resState = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIStatePreparingMessage"]);

                        if (resState == true)
                        {
                            DataSource.PalletRecordsStateEnum = Entities.Enums.PalletRecordsStateEnum.Hazirlaniyor;

                            var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                            await PalletRecordsAppService.UpdateAsync(updatedEntity);
                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningPreparingStateMessage"]);
                    }


                    await InvokeAsync(StateHasChanged);
                    break;

                case "completed":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PalletRecordsStateEnum != Entities.Enums.PalletRecordsStateEnum.Tamamlandi)
                    {
                        var resState2 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIStateCompletedMessage"]);

                        if (resState2 == true)
                        {
                            DataSource.PalletRecordsStateEnum = Entities.Enums.PalletRecordsStateEnum.Tamamlandi;

                            var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                            await PalletRecordsAppService.UpdateAsync(updatedEntity);
                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningCompletedStateMessage"]);
                    }



                    await InvokeAsync(StateHasChanged);
                    break;

                case "approved":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PalletRecordsStateEnum != Entities.Enums.PalletRecordsStateEnum.Onaylandi)
                    {
                        var resState3 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIStateApprovedMessage"]);

                        if (resState3 == true)
                        {
                            DataSource.PalletRecordsStateEnum = Entities.Enums.PalletRecordsStateEnum.Onaylandi;

                            var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                            await PalletRecordsAppService.UpdateAsync(updatedEntity);
                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningApprovedStateMessage"]);
                    }



                    await InvokeAsync(StateHasChanged);
                    break;

                case "ticketpending":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PalletRecordsTicketStateEnum != Entities.Enums.PalletRecordsTicketStateEnum.Bekliyor)
                    {
                        var resState4 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UITicketStatePendingMessage"]);

                        if (resState4 == true)
                        {
                            DataSource.PalletRecordsTicketStateEnum = Entities.Enums.PalletRecordsTicketStateEnum.Bekliyor;

                            var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                            await PalletRecordsAppService.UpdateAsync(updatedEntity);
                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningPendingTicketStateMessage"]);
                    }



                    await InvokeAsync(StateHasChanged);
                    break;

                case "ticketcompleted":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PalletRecordsTicketStateEnum != Entities.Enums.PalletRecordsTicketStateEnum.Tamamlandi)
                    {
                        var resState5 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UITicketStateCompletedMessage"]);

                        if (resState5 == true)
                        {
                            DataSource.PalletRecordsTicketStateEnum = Entities.Enums.PalletRecordsTicketStateEnum.Tamamlandi;

                            var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                            await PalletRecordsAppService.UpdateAsync(updatedEntity);
                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningCompletedTicketStateMessage"]);
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "printsmall":
                    //DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    //if (DataSource.PalletRecordsPrintTicketEnum != Entities.Enums.PalletRecordsPrintTicketEnum.KucukEtiket)
                    //{
                    //    var resState6 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIPrintSmallCompletedMessage"]);

                    //    if (resState6 == true)
                    //    {
                    //        DataSource.PalletRecordsPrintTicketEnum = Entities.Enums.PalletRecordsPrintTicketEnum.KucukEtiket;

                    //        var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                    //        await PalletRecordsAppService.UpdateAsync(updatedEntity);
                    //        await GetListDataSourceAsync();

                    //        await _grid.Refresh();
                    //        await InvokeAsync(StateHasChanged);
                    //    }
                    //}

                    //else
                    //{
                    //    await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningPrintSmallStateMessage"]);
                    //}

                    await InvokeAsync(StateHasChanged);
                    break;

                case "printbig":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    CreateLargePalletLabelReport(DataSource);

                    //if (DataSource.PalletRecordsPrintTicketEnum != Entities.Enums.PalletRecordsPrintTicketEnum.BuyukEtiket)
                    //{
                    //    var resState7 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIPrintBigCompletedMessage"]);

                    //    if (resState7 == true)
                    //    {
                    //        DataSource.PalletRecordsPrintTicketEnum = Entities.Enums.PalletRecordsPrintTicketEnum.BuyukEtiket;

                    //        var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                    //        await PalletRecordsAppService.UpdateAsync(updatedEntity);
                    //        await GetListDataSourceAsync();

                    //        await _grid.Refresh();
                    //        await InvokeAsync(StateHasChanged);
                    //    }
                    //}

                    //else
                    //{
                    //    await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningPrintBigStateMessage"]);
                    //}

                    await InvokeAsync(StateHasChanged);
                    break;

                case "ticketlist":

                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var palletList = (await PalletRecordsAppService.GetListAsync(new ListPalletRecordsParameterDto())).Data.Where(t=>t.PackingListID == DataSource.PackingListID).ToList();

                    if(palletList.Count >0 && palletList != null)
                    {
                        foreach (var pallet in palletList)
                        {
                            var palletRecord = (await PalletRecordsAppService.GetAsync(pallet.Id)).Data;

                            var packingList = (await PackingListsAppService.GetAsync(pallet.PackingListID.GetValueOrDefault())).Data;

                            string palletNo = palletRecord.Name;

                            foreach (var line in palletRecord.SelectPalletRecordLines)
                            {

                                #region Değişkenler

                                var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                                string productCode = product.Code;

                                string packageOrderNo = string.Empty;

                                int quantityInPackage = 0;

                                int packageQuantity = 0;

                                if (packingList != null && packingList.Id != Guid.Empty)
                                {
                                    var packingListPalletPackageLine = packingList.SelectPackingListPalletPackageLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault() && t.CustomerID == line.CurrentAccountCardID.GetValueOrDefault() && t.PackageFicheID == line.PackageFicheID.GetValueOrDefault()).FirstOrDefault();

                                    if (packingListPalletPackageLine != null && packingListPalletPackageLine.Id != Guid.Empty)
                                    {
                                        packageOrderNo = packingListPalletPackageLine.PackageNo;
                                        quantityInPackage = packingListPalletPackageLine.PackageContent;
                                        packageQuantity = packingListPalletPackageLine.NumberofPackage;
                                    }
                                }

                                int totalPackageQuantity = quantityInPackage * packageQuantity;

                                string packageType = line.PackageType;

                                string orderRefNo = string.Empty;

                                string supplierRefNo = string.Empty;

                                string customerRefNo = string.Empty;

                                var productRefNo = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.ProductID == line.ProductID.GetValueOrDefault() && t.CurrentAccountCardID == line.CurrentAccountCardID.GetValueOrDefault()).FirstOrDefault();

                                if (productRefNo != null && productRefNo.Id != Guid.Empty)
                                {
                                    supplierRefNo = productRefNo.ReferanceNo;
                                    customerRefNo = productRefNo.CustomerReferanceNo;
                                    orderRefNo = productRefNo.OrderReferanceNo;
                                }

                                var currentAccount = (await CurrentAccountCardsAppService.GetAsync(line.CurrentAccountCardID.GetValueOrDefault())).Data;

                                string customerCode = currentAccount.CustomerCode;

                                string ticketNo = customerRefNo;

                                if (currentAccount != null && currentAccount.Id != Guid.Empty)
                                {
                                    if (currentAccount.CustomerCode == "049")
                                    {
                                        if (customerRefNo != "-")
                                        {
                                            int lenght = customerRefNo.Length;

                                            ticketNo = customerRefNo.Remove(lenght - 2, 2) + "000";
                                            ticketNo = ticketNo.Replace(" ", string.Empty);
                                        }
                                    }
                                }

                                string status = string.Empty;

                                int statusInt = (int)pallet.PalletRecordsStateEnum;

                                switch (statusInt)
                                {
                                    case 1: status = L["EnumPreparing"]; break;
                                    case 2: status = L["EnumCompleted"]; break;
                                    case 3: status = L["EnumApproved"]; break;
                                    default: break;
                                }

                                bool isApproved = false;

                                if(statusInt == 3)
                                {
                                    isApproved = true;
                                }

                                #endregion

                                TicketListGrid ticketListModel = new TicketListGrid
                                {
                                    PalletNo = palletNo,
                                    ProductCode = productCode,
                                    PackageOrderNo = packageOrderNo,
                                    QuantityInPackage = quantityInPackage,
                                    PackageQuantity = packageQuantity,
                                    TotalPackageQuantity = totalPackageQuantity,
                                    PackageType = packageType,
                                    OrderRefNo = orderRefNo,
                                    CustomerCode = customerCode,
                                    CustomerRefNo = ticketNo,
                                    Status = status,
                                    IsApproved = isApproved,
                                };

                                TicketListGridList.Add(ticketListModel);

                                await InvokeAsync(StateHasChanged);

                            }

                        }
                    }

                    TicketListGridToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = GetSQLDateAppService.GetDateFromSQL().ToShortDateString() + "-TicketList" });

                    TicketListPopupVisible = true;

                    await InvokeAsync(StateHasChanged);
                    break;

                case "printpallet":
                    //DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    //if (DataSource.PalletRecordsPrintTicketEnum != Entities.Enums.PalletRecordsPrintTicketEnum.PaletEtiketi)
                    //{
                    //    var resState7 = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIPrintPalletCompletedMessage"]);

                    //    if (resState7 == true)
                    //    {
                    //        DataSource.PalletRecordsPrintTicketEnum = Entities.Enums.PalletRecordsPrintTicketEnum.PaletEtiketi;

                    //        var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);
                    //        await PalletRecordsAppService.UpdateAsync(updatedEntity);
                    //        await GetListDataSourceAsync();

                    //        await _grid.Refresh();
                    //        await InvokeAsync(StateHasChanged);
                    //    }
                    //}

                    //else
                    //{
                    //    await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningPrintPalletStateMessage"]);
                    //}

                    await InvokeAsync(StateHasChanged);
                    break;

                case "palletdetail":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    foreach (var line in DataSource.SelectPalletRecordLines)
                    {
                        #region Değişkenler

                        var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                        var productReferenceNoList = (await ProductReferanceNumbersAppService.GetSelectListAsync(line.ProductID.GetValueOrDefault())).Data;

                        string customerReferenceNo = string.Empty;

                        string orderReferenceNo = string.Empty;

                        Guid productRefNoID = Guid.Empty;

                        var productRefNo = productReferenceNoList.Where(t => t.CurrentAccountCardID == line.CurrentAccountCardID.GetValueOrDefault()).FirstOrDefault();

                        if (productRefNo != null && productRefNo.Id != Guid.Empty)
                        {
                            orderReferenceNo = productRefNo.OrderReferanceNo;
                            customerReferenceNo = productRefNo.CustomerReferanceNo;
                            productRefNoID = productRefNo.Id;
                        }

                        var currentAccount = (await CurrentAccountCardsAppService.GetAsync(line.CurrentAccountCardID.GetValueOrDefault())).Data;

                        string ticketNo = customerReferenceNo;

                        if (currentAccount != null && currentAccount.Id != Guid.Empty)
                        {
                            if (currentAccount.CustomerCode == "049")
                            {
                                if (customerReferenceNo != "-")
                                {
                                    int lenght = customerReferenceNo.Length;

                                    ticketNo = customerReferenceNo.Remove(lenght - 2, 2) + "000";
                                    ticketNo = ticketNo.Replace(" ", string.Empty);
                                }
                            }
                        }

                        string orderNo = string.Empty;

                        var packingList = (await PackingListsAppService.GetAsync(DataSource.PackingListID.GetValueOrDefault())).Data;

                        if (packingList != null && packingList.Id != Guid.Empty)
                        {
                            orderNo = packingList.OrderNo;
                        }

                        decimal productWeight = 0;

                        var packageFiche = (await PackageFichesAppService.GetAsync(line.PackageFicheID.GetValueOrDefault())).Data;

                        if (packageFiche != null && packageFiche.Id != Guid.Empty)
                        {
                            productWeight = packageFiche.UnitWeight;
                        }

                        var salesOrder = new SelectSalesOrderDto();

                        decimal orderQuantity = 0;

                        decimal maxProductionQuantity = 0;

                        decimal orderUnitPrice = 0;

                        var salesPrice = new SelectSalesPricesDto();

                        decimal listPrice = 0;

                        var lastOrderID = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.CurrentAccountCardID == line.CurrentAccountCardID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                        decimal lastOrderPrice = (await SalesOrdersAppService.GetAsync(lastOrderID)).Data.SelectSalesOrderLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault()).Select(t => t.UnitPrice).FirstOrDefault();

                        salesOrder = (await SalesOrdersAppService.GetAsync(line.SalesOrderID.GetValueOrDefault())).Data;

                        if (salesOrder != null && salesOrder.Id != Guid.Empty)
                        {
                            var salesOrderLinesList = salesOrder.SelectSalesOrderLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault()).ToList();

                            orderQuantity = salesOrderLinesList.Select(t => t.Quantity).Sum();

                            maxProductionQuantity = Math.Ceiling(orderQuantity + (orderQuantity / 10));

                            orderUnitPrice = salesOrderLinesList.Select(t => t.UnitPrice).FirstOrDefault();

                            Guid salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.CurrentAccountCardID == line.CurrentAccountCardID && t.CurrencyCode == salesOrder.CurrencyCode && t.StartDate <= DataSource.PlannedLoadingTime && t.EndDate >= DataSource.PlannedLoadingTime && t.CustomerCode == currentAccount.CustomerCode && t.IsActive == true).Select(t => t.Id).FirstOrDefault();

                            salesPrice = (await SalesPricesAppService.GetAsync(salesPriceID)).Data;

                            listPrice = salesPrice.SelectSalesPriceLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault()).Select(t => t.Price).FirstOrDefault();
                        }

                        decimal lineAmount = line.TotalAmount * line.ApprovedUnitPrice;

                        #endregion

                        PalletDetailGrid palletDetailModel = new PalletDetailGrid
                        {
                            Id = line.Id,
                            ProductID = line.ProductID.GetValueOrDefault(),
                            ProductName = line.ProductName,
                            EnglishDefinition = product.EnglishDefinition,
                            CustomerReferenceNo = customerReferenceNo,
                            OrderReferenceNo = orderReferenceNo,
                            PackingListID = DataSource.PackingListID.GetValueOrDefault(),
                            PackingListNo = DataSource.PackingListCode,
                            TicketNo = ticketNo,
                            OrderNo = orderNo,
                            CurrentAccountID = currentAccount.Id,
                            CustomerCode = currentAccount.CustomerCode,
                            PackageFicheID = line.PackageFicheID.GetValueOrDefault(),
                            PackageFicheProductWeight = productWeight,
                            OrderQuantity = orderQuantity,
                            MaxProductionQuantity = maxProductionQuantity,
                            PackageTotalQuantity = line.TotalAmount,
                            OrderunitPrice = orderUnitPrice,
                            ApprovedUnitPrice = line.ApprovedUnitPrice,
                            SalesPriceListPrice = listPrice,
                            LastOrderPrice = lastOrderPrice,
                            TotalNetKG = line.TotalNetKG,
                            TotalGrossKG = line.TotalGrossKG,
                            SalesPriceID = salesPrice.Id,
                            ProductReferenceNumberID = productRefNoID,
                            LineAmount = lineAmount
                        };

                        PalletDetailGridList.Add(palletDetailModel);
                    }

                    PalletDetailPopupVisible = true;
                    await InvokeAsync(StateHasChanged);

                    break;

                case "loadingdetail":
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    LoadingDetailDataSource = new LoadingDetailGrid { };
                    isAllColumns = false;

                    foreach (var item in _packageTypeFilterComboBox)
                    {
                        item.Text = L[item.Text];
                    }


                    LoadingDetailGridToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = LoadingDateFilter.ToString() + "-LoadingDetail" });

                    //LoadingDateFilter = GetSQLDateAppService.GetDateFromSQL();
                    LoadingDetailPopupVisible = true;
                    await InvokeAsync(StateHasChanged);

                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPalletRecordLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "addpackagefiche":
                    if (DataSource.CurrentAccountCardID == null || DataSource.CurrentAccountCardID == Guid.Empty)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarninCurrentAccountTitle"], L["UIWarninCurrentAccountMessage"]);
                    }
                    else if (string.IsNullOrEmpty(DataSource.PackageType))
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarninCurrentAccountTitle"], L["UIWarninPackageTypeMessage"]);
                    }
                    else
                    {
                        selectedNumberofPackages = 0;
                        PackageFichesList = (await PackageFichesAppService.GetListAsync(new ListPackageFichesParameterDto())).Data.Where(t => t.PackageType == DataSource.PackageType && t.CurrentAccountID == DataSource.CurrentAccountCardID).ToList();

                        PackageFichesList = PackageFichesList.Where(t=>t.PackingListID == Guid.Empty || t.PackingListID == null).ToList();

                        if(PackageFichesList.Count >0 && PackageFichesList != null)
                        {
                            foreach (var packageFiche in PackageFichesList)
                            {
                                int totalQuantity = (await PackageFichesAppService.GetAsync(packageFiche.Id)).Data.SelectPackageFicheLines.Select(t => t.Quantity).Sum();

                                PackageFicheSelectionGrid packageFicheSelectionModel = new PackageFicheSelectionGrid
                                {
                                    Code = packageFiche.Code,
                                    SalesOrderID = packageFiche.SalesOrderID.GetValueOrDefault(),
                                    CustomerCode = packageFiche.CustomerCode,
                                    ProductID = packageFiche.ProductID,
                                    Id = packageFiche.Id,
                                    NumberofPackage = packageFiche.NumberofPackage,
                                    PackageContent = packageFiche.PackageContent,
                                    ProductCode = packageFiche.ProductCode,
                                    SalesOrderFicheNo = packageFiche.SalesOrderFicheNo,
                                    LoadingDate = DataSource.PlannedLoadingTime.GetValueOrDefault(),
                                    PackageType = packageFiche.PackageType,
                                    PalletNo = packageFiche.PalletNumber,
                                    ProductName = packageFiche.ProductName,
                                    TotalQuantity = totalQuantity,
                                    SelectedLine = false
                                };
                                PackageFichesSelectionList.Add(packageFicheSelectionModel);
                            }
                        }

                        SelectPackageFichesModal = true;
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "removepackagefiche":

                    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectPalletRecordLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPalletRecordLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPalletRecordLines.Remove(line);
                            }
                        }

                        DataSource.PalletPackageNumber = GridLineList.Select(t => t.NumberofPackage).Sum();

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "approval":

                    var res1 = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineApprovalConfirmation"]);

                    if (res1 == true)
                    {
                        var line = args.RowInfo.RowData;

                        int indexLine = GridLineList.IndexOf(line);

                        GridLineList[indexLine].LineApproval = true;

                        DataSource.SelectPalletRecordLines = GridLineList;

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "approvalremove":

                    var res2 = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineApprovalRemoveConfirmation"]);

                    if (res2 == true)
                    {
                        var line = args.RowInfo.RowData;

                        int indexLine = GridLineList.IndexOf(line);

                        GridLineList[indexLine].LineApproval = false;

                        DataSource.SelectPalletRecordLines = GridLineList;

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;


                default:
                    break;
            }
        }

        public async void OnPalletDetailContextMenuClick(ContextMenuClickEventArgs<PalletDetailGrid> args)
        {
            switch (args.Item.Id)
            {

                case "approve":

                    PalletDetailDataSource = args.RowInfo.RowData;
                    PalletDetailCrudPopupVisible = true;

                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnTicketListContextMenuClick(ContextMenuClickEventArgs<TicketListGrid> args)
        {
            switch (args.Item.Id)
            {

                case "print":

                    TicketListlDataSource = args.RowInfo.RowData;

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
        public void HidePalletDetailPopup()
        {
            PalletDetailPopupVisible = false;
            PalletDetailGridList.Clear();
        }
        public void HideLoadingDetailPopup()
        {
            LoadingDetailPopupVisible = false;
            PalletNameFilter = string.Empty;
            PackageTypeFilter = string.Empty;
            LoadingDateFilter = null;
            CurrentAccountNameFilter = string.Empty;
            CurrentAccountIDFilter = Guid.Empty;
            LoadingDetailGridList.Clear();
        }
        public void HidePalletDetailCrudPopup()
        {
            PalletDetailCrudPopupVisible = false;
        }
        public void HideTicketList()
        {
            TicketListGridList.Clear();

            TicketListPopupVisible = false;
        }
        protected async Task OnPalletDetailSubmit()
        {
            int index = PalletDetailGridList.IndexOf(PalletDetailDataSource);

            PalletDetailGridList[index].LineAmount = PalletDetailDataSource.ApprovedUnitPrice * PalletDetailDataSource.PackageTotalQuantity;

            PalletDetailGridList[index].ApprovedUnitPrice = PalletDetailDataSource.ApprovedUnitPrice;

            var updatedLine = DataSource.SelectPalletRecordLines.Where(t => t.Id == PalletDetailDataSource.Id).FirstOrDefault();

            if (updatedLine != null)
            {
                int lineIndex = DataSource.SelectPalletRecordLines.IndexOf(updatedLine);

                DataSource.SelectPalletRecordLines[lineIndex].ApprovedUnitPrice = PalletDetailDataSource.ApprovedUnitPrice;

                var updatedEntity = ObjectMapper.Map<SelectPalletRecordsDto, UpdatePalletRecordsDto>(DataSource);

                await PalletRecordsAppService.UpdateAsync(updatedEntity);
            }

            await _PalletDetailGrid.Refresh();
            HidePalletDetailCrudPopup();
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectPalletRecordLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPalletRecordLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPalletRecordLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPalletRecordLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPalletRecordLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPalletRecordLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPalletRecordLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }
        public async void LoadingDetailFilterClicked()
        {
            #region Filtreleme 

            var palletList = (await PalletRecordsAppService.GetListAsync(new ListPalletRecordsParameterDto())).Data.AsQueryable();


            if (LoadingDateFilter.HasValue)
            {
                palletList = palletList.Where(t => t.PlannedLoadingTime == LoadingDateFilter);
                //palletList = palletList.Where(t => t.PlannedLoadingTime == LoadingDateFilter.Value).ToList();
            }
            if (!string.IsNullOrEmpty(PalletNameFilter))
            {
                palletList = palletList.Where(t => t.Name == PalletNameFilter);
                //palletList = palletList.Where(t => t.Name == PalletNameFilter).ToList();
            }
            if (!string.IsNullOrEmpty(PackageTypeFilter))
            {
                string packageTypeFilter = L[PackageTypeFilter].Value;
                palletList = palletList.Where(t => t.PackageType == packageTypeFilter);
                //palletList = palletList.Where(t => t.PackageType == packageTypeFilter).ToList();
            }
            if (CurrentAccountIDFilter != Guid.Empty)
            {
                palletList = palletList.Where(t => t.CurrentAccountCardID == CurrentAccountIDFilter);
                //palletList = palletList.Where(t => t.CurrentAccountCardID == CurrentAccountIDFilter).ToList();
            }

            #endregion

            #region Grid Verilerini Çekme


            foreach (var pallet in palletList.ToList())
            {
                var palletRecord = (await PalletRecordsAppService.GetAsync(pallet.Id)).Data;

                var packingList = (await PackingListsAppService.GetAsync(pallet.PackingListID.GetValueOrDefault())).Data;

                foreach (var line in palletRecord.SelectPalletRecordLines)
                {
                    string supplierRefNo = string.Empty;
                    string customerRefNo = string.Empty;

                    var productRefNo = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.ProductID == line.ProductID.GetValueOrDefault() && t.CurrentAccountCardID == line.CurrentAccountCardID.GetValueOrDefault()).FirstOrDefault();

                    if (productRefNo != null && productRefNo.Id != Guid.Empty)
                    {
                        supplierRefNo = productRefNo.ReferanceNo;
                        customerRefNo = productRefNo.CustomerReferanceNo;
                    }

                    string packageOrderNo = string.Empty;
                    int quantityInPackage = 0;
                    int packageQuantity = 0;

                    if (packingList != null && packingList.Id != Guid.Empty)
                    {
                        var packingListPalletPackageLine = packingList.SelectPackingListPalletPackageLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault() && t.CustomerID == line.CurrentAccountCardID.GetValueOrDefault() && t.PackageFicheID == line.PackageFicheID.GetValueOrDefault()).FirstOrDefault();

                        if (packingListPalletPackageLine != null && packingListPalletPackageLine.Id != Guid.Empty)
                        {
                            packageOrderNo = packingListPalletPackageLine.PackageNo;
                            quantityInPackage = packingListPalletPackageLine.PackageContent;
                            packageQuantity = packingListPalletPackageLine.NumberofPackage;
                        }

                    }

                    var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                    var productGroup = (await ProductGroupsAppService.GetAsync(product.ProductGrpID)).Data;

                    var currentAccount = (await CurrentAccountCardsAppService.GetAsync(line.CurrentAccountCardID.GetValueOrDefault())).Data;

                    string ticketNo = customerRefNo;

                    if (currentAccount != null && currentAccount.Id != Guid.Empty)
                    {
                        if (currentAccount.CustomerCode == "049")
                        {
                            if (customerRefNo != "-")
                            {
                                int lenght = customerRefNo.Length;

                                ticketNo = customerRefNo.Remove(lenght - 2, 2) + "000";
                                ticketNo = ticketNo.Replace(" ", string.Empty);
                            }
                        }
                    }

                    decimal productWeight = 0;

                    var packageFiche = (await PackageFichesAppService.GetAsync(line.PackageFicheID.GetValueOrDefault())).Data;

                    if (packageFiche != null && packageFiche.Id != Guid.Empty)
                    {
                        productWeight = packageFiche.UnitWeight;
                    }

                    var salesOrder = new SelectSalesOrderDto();

                    decimal orderQuantity = 0;

                    decimal maxProductionQuantity = 0;

                    decimal orderUnitPrice = 0;

                    string orderNo = string.Empty;

                    var salesPrice = new SelectSalesPricesDto();

                    decimal listPrice = 0;

                    DateTime? confirmedShippingDate = null;

                    decimal lastOrderPrice = (SalesOrdersAppService.GetLastOrderPriceByCurrentAccountProduct(line.CurrentAccountCardID.Value, line.ProductID.Value));

                    salesOrder = (await SalesOrdersAppService.GetAsync(line.SalesOrderID.GetValueOrDefault())).Data;

                    if (salesOrder != null && salesOrder.Id != Guid.Empty)
                    {
                        var salesOrderLinesList = salesOrder.SelectSalesOrderLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault()).ToList();

                        orderQuantity = salesOrderLinesList.Select(t => t.Quantity).Sum();

                        maxProductionQuantity = Math.Ceiling(orderQuantity + (orderQuantity / 10));

                        orderUnitPrice = salesOrderLinesList.Select(t => t.UnitPrice).FirstOrDefault();

                        orderNo = salesOrder.FicheNo;

                        confirmedShippingDate = salesOrder.ConfirmedLoadingDate;

                        salesPrice = (await SalesPricesAppService.GetbyCurrentAccountCurrencyDateAsync(line.CurrentAccountCardID.Value, salesOrder.CurrencyID, DataSource.PlannedLoadingTime.Value)).Data;

                        listPrice = salesPrice.SelectSalesPriceLines.Where(t => t.ProductID == line.ProductID.GetValueOrDefault()).Select(t => t.Price).FirstOrDefault();
                    }

                    LoadingDetailGrid loadingDetailGridModel = new LoadingDetailGrid
                    {
                        SupplierRefNo = supplierRefNo,
                        CustomerRefNo = ticketNo,
                        ProductCode = line.ProductCode,
                        PalletName = pallet.Name,
                        PackageOrderNo = packageOrderNo,
                        QuantityInPackage = quantityInPackage,
                        PackageQuantity = packageQuantity,
                        TotalPackageQuantity = quantityInPackage * packageQuantity,
                        OrderUnitPrice = orderUnitPrice,
                        LineAmount = orderUnitPrice * quantityInPackage * packageQuantity,
                        ProductGroupName = productGroup.Name,
                        CustomerCode = currentAccount.CustomerCode,
                        OrderNo = orderNo,
                        ConfirmedShippingDate = confirmedShippingDate.GetValueOrDefault(),
                        ProductWeight = productWeight,
                        OrderQuantity = (int)orderQuantity,
                        MaxProductionQuantity = (int)maxProductionQuantity,
                        ApprovedUnitPrice = line.ApprovedUnitPrice,
                        SalesPriceListPrice = listPrice,
                        LastOrderPrice = lastOrderPrice,
                        TotalNetKG = line.TotalNetKG,
                        TotalGrossKG = line.TotalGrossKG,
                        ProductName = product.Name,
                        EnglishDefinition = product.EnglishDefinition,
                        CustomerReferenceNo = customerRefNo,
                        TicketNo = ticketNo,
                        CurrentAccountName = currentAccount.Name,
                    };

                    LoadingDetailGridList.Add(loadingDetailGridModel);

                }
            }

            await _LoadingDetailGrid.Refresh();
            await InvokeAsync(StateHasChanged);

            #endregion
        }
        public void ClearButtonClicked()
        {
            PalletNameFilter = string.Empty;
            PackageTypeFilter = string.Empty;
            LoadingDateFilter = null;
            CurrentAccountNameFilter = string.Empty;
            CurrentAccountIDFilter = Guid.Empty;
        }
        public async void LoadingDetailToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "ExcelExport")
            {
                ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();

                if (!isAllColumns)
                {
                    ExcelExportProperties.Columns = _LoadingDetailGrid.Columns.Where(t => t.Index <= 13).ToList();
                }


                //ExcelExportProperties.Footer = ExcelFooter{ };


                ExcelExportProperties.FileName = LoadingDateFilter.GetValueOrDefault().ToShortDateString() + "-LoadingDetails" + ".xlsx";
                await this._LoadingDetailGrid.ExportToExcelAsync(ExcelExportProperties);

            }
        }
        public async void TicketListToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "ExcelExport")
            {
                ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();

                ExcelExportProperties.FileName = DataSource.PlannedLoadingTime.GetValueOrDefault().ToShortDateString() + "-TicketList" + ".xlsx";
                await this._LoadingDetailGrid.ExportToExcelAsync(ExcelExportProperties);

            }
        }


        #endregion

        #region Paket Fişleri Modal İşlemleri

        public async void PackageFichesDoubleClickHandler(RecordDoubleClickEventArgs<PackageFicheSelectionGrid> args)
        {
            var selectedPackageFiche = args.RowData;

            if (selectedPackageFiche != null)
            {
                var packageFiche = (await PackageFichesAppService.GetAsync(selectedPackageFiche.Id)).Data;
                int selectedIndex = PackageFichesSelectionList.IndexOf(selectedPackageFiche);

                PackageFichesSelectionList[selectedIndex].SelectedLine = true;

                selectedNumberofPackages = selectedNumberofPackages + (packageFiche.SelectPackageFicheLines.Count * selectedPackageFiche.NumberofPackage);

                if (selectedNumberofPackages > DataSource.MaxPackageNumber)
                {
                    var res = await ModalManager.WarningPopupAsync(L["UIWarningNumberofPackagesTitle"], L["UIWarningNumberofPackagesMessage"]);

                    if (res == true)
                    {
                        selectedNumberofPackages = selectedNumberofPackages - (packageFiche.SelectPackageFicheLines.Count * selectedPackageFiche.NumberofPackage);
                        PackageFichesSelectionList[selectedIndex].SelectedLine = false;
                        await InvokeAsync(StateHasChanged);
                    }
                }

                await _PackageFichesGrid.Refresh();

                await InvokeAsync(StateHasChanged);
            }
        }

        public async void TransferSelectedPackageFichesClicked()
        {
            if (PackageFichesSelectionList.Where(t => t.SelectedLine == true).Count() > 0)
            {

                foreach (var selecteditem in PackageFichesSelectionList)
                {
                    if (selecteditem.SelectedLine)
                    {
                        PackageFicheLinesList = (await PackageFichesAppService.GetAsync(selecteditem.Id)).Data.SelectPackageFicheLines.ToList();
                        var currentAccountDataSource = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data;
                        string customerCode = currentAccountDataSource.CustomerCode;

                        decimal packageKG = 0;
                        decimal unitKG = (await ProductsAppService.GetAsync(selecteditem.ProductID.GetValueOrDefault())).Data.UnitWeight;

                        if (DataSource.PackageType == L["BigPackage"].Value)
                        {
                            packageKG = currentAccountDataSource.BigPackageKG;
                        }
                        else if (DataSource.PackageType == L["SmallPackage"].Value)
                        {
                            packageKG = currentAccountDataSource.SmallPackageKG;
                        }

                        decimal onePackageNetKG = selecteditem.PackageContent * unitKG;
                        decimal onePackageGrossKG = onePackageNetKG + packageKG;
                        decimal totalNetKG = onePackageNetKG + selecteditem.NumberofPackage;
                        decimal totalGrossKG = onePackageGrossKG + selecteditem.NumberofPackage;


                        foreach (var line in PackageFicheLinesList)
                        {
                            SelectPalletRecordLinesDto palletLineModel = new SelectPalletRecordLinesDto
                            {
                                ProductID = line.ProductID,
                                ProductCode = line.ProductCode,
                                ProductName = line.ProductName,
                                SalesOrderID = selecteditem.SalesOrderID,
                                CustomerCode = customerCode,
                                PackageType = L["LinePackageType"],
                                PackageContent = selecteditem.PackageContent,
                                NumberofPackage = selecteditem.NumberofPackage,
                                TotalAmount = selecteditem.PackageContent * selecteditem.NumberofPackage,
                                TotalGrossKG = totalGrossKG,
                                TotalNetKG = totalNetKG,
                                LineNr = GridLineList.Count + 1,
                                PackageFicheID = line.PackageFicheID,
                                CurrentAccountCardID = DataSource.CurrentAccountCardID.GetValueOrDefault()
                            };

                            GridLineList.Add(palletLineModel);
                        }
                    }
                }

                DataSource.PalletPackageNumber = GridLineList.Select(t => t.NumberofPackage).Sum();

                await _LineGrid.Refresh();

                HidePackageFichesPopup();
                await InvokeAsync(StateHasChanged);
            }
            else if (PackageFichesSelectionList.Where(t => t.SelectedLine == true).Count() == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSelectedPackageFicheTitle"], L["UIWarningSelectedPackageFicheMessage"]);
            }
        }

        public void HidePackageFichesPopup()
        {
            PackageFichesSelectionList.Clear();
            PackageFichesList.Clear();
            SelectPackageFichesModal = false;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PalletRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit = new();
        SfTextBox ProductsNameButtonEdit = new();
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

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsButtonClickEvent);
            await CurrentAccountCardsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsButtonClickEvent()
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
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DataSource.CurrentAccountCardID = selectedCurrentAccountCard.Id;
                DataSource.CurrentAccountCardCode = selectedCurrentAccountCard.Code;
                DataSource.CurrentAccountCardName = selectedCurrentAccountCard.Name;

                foreach (var item in GridLineList)
                {
                    item.CurrentAccountCardID = DataSource.CurrentAccountCardID.GetValueOrDefault();
                }

                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Çeki Listesi Button Edit

        SfTextBox PackingListsButtonEdit;
        bool SelectPackingListsPopupVisible = false;
        List<ListPackingListsDto> PackingListsList = new List<ListPackingListsDto>();
        public async Task PackingListsOnCreateIcon()
        {
            var PackingListsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, PackingListsButtonClickEvent);
            await PackingListsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", PackingListsButtonClick } });
        }

        public async void PackingListsButtonClickEvent()
        {
            SelectPackingListsPopupVisible = true;
            PackingListsList = (await PackingListsAppService.GetListAsync(new ListPackingListsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void PackingListsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PackingListID = Guid.Empty;
                DataSource.PackingListCode = string.Empty;
            }
        }

        public async void PackingListsDoubleClickHandler(RecordDoubleClickEventArgs<ListPackingListsDto> args)
        {
            var selectedPackingList = args.RowData;

            if (selectedPackingList != null)
            {
                DataSource.PackingListID = selectedPackingList.Id;
                DataSource.PackingListCode = selectedPackingList.Code;
                SelectPackingListsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Koli Türü ComboBox
        public class PackageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<PackageTypeComboBox> _packageTypeComboBox = new List<PackageTypeComboBox>
        {
            new PackageTypeComboBox(){ID = "Big", Text="BigPackage"},
            new PackageTypeComboBox(){ID = "Small", Text="SmallPackage"}
        };

        private void PackageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, PackageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "Big":
                        DataSource.PackageType = L["BigPackage"].Value;
                        DataSource.MaxPackageNumber = 18;
                        break;

                    case "Small":
                        DataSource.PackageType = L["SmallPackage"].Value;
                        DataSource.MaxPackageNumber = 30;
                        break;


                    default: break;
                }
            }
            else
            {
                DataSource.PackageType = string.Empty;
                DataSource.MaxPackageNumber = 0;
            }
        }

        #endregion

        #region Palet Adı ComboBox
        public class NameComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<NameComboBox> _nameComboBox = new List<NameComboBox>
        {
            new NameComboBox(){ID = "A-1"  , Text="A-1"  },
            new NameComboBox(){ID = "B-2"  , Text="B-2"  },
            new NameComboBox(){ID = "C-3"  , Text="C-3"  },
            new NameComboBox(){ID = "D-4"  , Text="D-4"  },
            new NameComboBox(){ID = "E-5"  , Text="E-5"  },
            new NameComboBox(){ID = "F-6"  , Text="F-6"  },
            new NameComboBox(){ID = "G-7"  , Text="G-7"  },
            new NameComboBox(){ID = "H-8"  , Text="H-8"  },
            new NameComboBox(){ID = "I-9"  , Text="I-9"  },
            new NameComboBox(){ID = "J-10" , Text="J-10" },
            new NameComboBox(){ID = "K-11" , Text="K-11" },
            new NameComboBox(){ID = "L-12" , Text="L-12" },
            new NameComboBox(){ID = "M-13" , Text="M-13" },
            new NameComboBox(){ID = "N-14" , Text="N-14" },
            new NameComboBox(){ID = "O-15" , Text="O-15" },
            new NameComboBox(){ID = "P-16" , Text="P-16" },
            new NameComboBox(){ID = "Q-17" , Text="Q-17" },
            new NameComboBox(){ID = "R-18" , Text="R-18" },
            new NameComboBox(){ID = "S-19" , Text="S-19" },
            new NameComboBox(){ID = "T-20" , Text="T-20" },
            new NameComboBox(){ID = "U-21" , Text="U-21" },
            new NameComboBox(){ID = "V-22" , Text="V-22" },
            new NameComboBox(){ID = "W-23" , Text="W-23" },
            new NameComboBox(){ID = "X-24" , Text="X-24" },
            new NameComboBox(){ID = "Y-25" , Text="Y-25" },
            new NameComboBox(){ID = "Z-26" , Text="Z-26" },
            new NameComboBox(){ID = "AA-27", Text="AA-27"},
            new NameComboBox(){ID = "BB-28", Text="BB-28"},
            new NameComboBox(){ID = "CC-29", Text="CC-29"},
            new NameComboBox(){ID = "DD-30", Text="DD-30"},
            new NameComboBox(){ID = "EE-31", Text="EE-31"},
            new NameComboBox(){ID = "FF-32", Text="FF-32"},
            new NameComboBox(){ID = "GG-33", Text="GG-33"},
            new NameComboBox(){ID = "HH-34", Text="HH-34"},
            new NameComboBox(){ID = "II-35", Text="II-35"},
            new NameComboBox(){ID = "JJ-36", Text="JJ-36"},
            new NameComboBox(){ID = "KK-37", Text="KK-37"},
            new NameComboBox(){ID = "LL-38", Text="LL-38"},
            new NameComboBox(){ID = "MM-39", Text="MM-39"},
            new NameComboBox(){ID = "NN-40", Text="NN-40"},
            new NameComboBox(){ID = "OO-41", Text="OO-41"},
            new NameComboBox(){ID = "PP-42", Text="PP-42"},
            new NameComboBox(){ID = "QQ-43", Text="QQ-43"},
            new NameComboBox(){ID = "RR-44", Text="RR-44"},
            new NameComboBox(){ID = "SS-45", Text="SS-45"},
            new NameComboBox(){ID = "TT-46", Text="TT-46"},
            new NameComboBox(){ID = "UU-47", Text="UU-47"},
            new NameComboBox(){ID = "VV-48", Text="VV-48"},
            new NameComboBox(){ID = "WW-49", Text="WW-49"},
            new NameComboBox(){ID = "XX-50", Text="XX-50"},
            new NameComboBox(){ID = "YY-51", Text="YY-51"},
            new NameComboBox(){ID = "ZZ-52", Text="ZZ-52"},
        };

        private void NameComboBoxValueChangeHandler(ChangeEventArgs<string, NameComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "A-1": DataSource.Name = "A-1"; break;
                    case "B-2": DataSource.Name = "B-2"; break;
                    case "C-3": DataSource.Name = "C-3"; break;
                    case "D-4": DataSource.Name = "D-4"; break;
                    case "E-5": DataSource.Name = "E-5"; break;
                    case "F-6": DataSource.Name = "F-6"; break;
                    case "G-7": DataSource.Name = "G-7"; break;
                    case "H-8": DataSource.Name = "H-8"; break;
                    case "I-9": DataSource.Name = "I-9"; break;
                    case "J-10": DataSource.Name = "J-10"; break;
                    case "K-11": DataSource.Name = "K-11"; break;
                    case "L-12": DataSource.Name = "L-12"; break;
                    case "M-13": DataSource.Name = "M-13"; break;
                    case "N-14": DataSource.Name = "N-14"; break;
                    case "O-15": DataSource.Name = "O-15"; break;
                    case "P-16": DataSource.Name = "P-16"; break;
                    case "Q-17": DataSource.Name = "Q-17"; break;
                    case "R-18": DataSource.Name = "R-18"; break;
                    case "S-19": DataSource.Name = "S-19"; break;
                    case "T-20": DataSource.Name = "T-20"; break;
                    case "U-21": DataSource.Name = "U-21"; break;
                    case "V-22": DataSource.Name = "V-22"; break;
                    case "W-23": DataSource.Name = "W-23"; break;
                    case "X-24": DataSource.Name = "X-24"; break;
                    case "Y-25": DataSource.Name = "Y-25"; break;
                    case "Z-26": DataSource.Name = "Z-26"; break;
                    case "AA-27": DataSource.Name = "AA-27"; break;
                    case "BB-28": DataSource.Name = "BB-28"; break;
                    case "CC-29": DataSource.Name = "CC-29"; break;
                    case "DD-30": DataSource.Name = "DD-30"; break;
                    case "EE-31": DataSource.Name = "EE-31"; break;
                    case "FF-32": DataSource.Name = "FF-32"; break;
                    case "GG-33": DataSource.Name = "GG-33"; break;
                    case "HH-34": DataSource.Name = "HH-34"; break;
                    case "II-35": DataSource.Name = "II-35"; break;
                    case "JJ-36": DataSource.Name = "JJ-36"; break;
                    case "KK-37": DataSource.Name = "KK-37"; break;
                    case "LL-38": DataSource.Name = "LL-38"; break;
                    case "MM-39": DataSource.Name = "MM-39"; break;
                    case "NN-40": DataSource.Name = "NN-40"; break;
                    case "OO-41": DataSource.Name = "OO-41"; break;
                    case "PP-42": DataSource.Name = "PP-42"; break;
                    case "QQ-43": DataSource.Name = "QQ-43"; break;
                    case "RR-44": DataSource.Name = "RR-44"; break;
                    case "SS-45": DataSource.Name = "SS-45"; break;
                    case "TT-46": DataSource.Name = "TT-46"; break;
                    case "UU-47": DataSource.Name = "UU-47"; break;
                    case "VV-48": DataSource.Name = "VV-48"; break;
                    case "WW-49": DataSource.Name = "WW-49"; break;
                    case "XX-50": DataSource.Name = "XX-50"; break;
                    case "YY-51": DataSource.Name = "YY-51"; break;
                    case "ZZ-52": DataSource.Name = "ZZ-52"; break;


                    default: break;
                }
            }
        }

        #endregion

        #region Palet Adı Filtre ComboBox
        public class NameFilterComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<NameFilterComboBox> _nameFilterComboBox = new List<NameFilterComboBox>
        {
            new NameFilterComboBox(){ID = "A-1"  , Text="A-1"  },
            new NameFilterComboBox(){ID = "B-2"  , Text="B-2"  },
            new NameFilterComboBox(){ID = "C-3"  , Text="C-3"  },
            new NameFilterComboBox(){ID = "D-4"  , Text="D-4"  },
            new NameFilterComboBox(){ID = "E-5"  , Text="E-5"  },
            new NameFilterComboBox(){ID = "F-6"  , Text="F-6"  },
            new NameFilterComboBox(){ID = "G-7"  , Text="G-7"  },
            new NameFilterComboBox(){ID = "H-8"  , Text="H-8"  },
            new NameFilterComboBox(){ID = "I-9"  , Text="I-9"  },
            new NameFilterComboBox(){ID = "J-10" , Text="J-10" },
            new NameFilterComboBox(){ID = "K-11" , Text="K-11" },
            new NameFilterComboBox(){ID = "L-12" , Text="L-12" },
            new NameFilterComboBox(){ID = "M-13" , Text="M-13" },
            new NameFilterComboBox(){ID = "N-14" , Text="N-14" },
            new NameFilterComboBox(){ID = "O-15" , Text="O-15" },
            new NameFilterComboBox(){ID = "P-16" , Text="P-16" },
            new NameFilterComboBox(){ID = "Q-17" , Text="Q-17" },
            new NameFilterComboBox(){ID = "R-18" , Text="R-18" },
            new NameFilterComboBox(){ID = "S-19" , Text="S-19" },
            new NameFilterComboBox(){ID = "T-20" , Text="T-20" },
            new NameFilterComboBox(){ID = "U-21" , Text="U-21" },
            new NameFilterComboBox(){ID = "V-22" , Text="V-22" },
            new NameFilterComboBox(){ID = "W-23" , Text="W-23" },
            new NameFilterComboBox(){ID = "X-24" , Text="X-24" },
            new NameFilterComboBox(){ID = "Y-25" , Text="Y-25" },
            new NameFilterComboBox(){ID = "Z-26" , Text="Z-26" },
            new NameFilterComboBox(){ID = "AA-27", Text="AA-27"},
            new NameFilterComboBox(){ID = "BB-28", Text="BB-28"},
            new NameFilterComboBox(){ID = "CC-29", Text="CC-29"},
            new NameFilterComboBox(){ID = "DD-30", Text="DD-30"},
            new NameFilterComboBox(){ID = "EE-31", Text="EE-31"},
            new NameFilterComboBox(){ID = "FF-32", Text="FF-32"},
            new NameFilterComboBox(){ID = "GG-33", Text="GG-33"},
            new NameFilterComboBox(){ID = "HH-34", Text="HH-34"},
            new NameFilterComboBox(){ID = "II-35", Text="II-35"},
            new NameFilterComboBox(){ID = "JJ-36", Text="JJ-36"},
            new NameFilterComboBox(){ID = "KK-37", Text="KK-37"},
            new NameFilterComboBox(){ID = "LL-38", Text="LL-38"},
            new NameFilterComboBox(){ID = "MM-39", Text="MM-39"},
            new NameFilterComboBox(){ID = "NN-40", Text="NN-40"},
            new NameFilterComboBox(){ID = "OO-41", Text="OO-41"},
            new NameFilterComboBox(){ID = "PP-42", Text="PP-42"},
            new NameFilterComboBox(){ID = "QQ-43", Text="QQ-43"},
            new NameFilterComboBox(){ID = "RR-44", Text="RR-44"},
            new NameFilterComboBox(){ID = "SS-45", Text="SS-45"},
            new NameFilterComboBox(){ID = "TT-46", Text="TT-46"},
            new NameFilterComboBox(){ID = "UU-47", Text="UU-47"},
            new NameFilterComboBox(){ID = "VV-48", Text="VV-48"},
            new NameFilterComboBox(){ID = "WW-49", Text="WW-49"},
            new NameFilterComboBox(){ID = "XX-50", Text="XX-50"},
            new NameFilterComboBox(){ID = "YY-51", Text="YY-51"},
            new NameFilterComboBox(){ID = "ZZ-52", Text="ZZ-52"},
        };

        private void NameFilterComboBoxValueChangeHandler(ChangeEventArgs<string, NameFilterComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "A-1": PalletNameFilter = "A-1"; break;
                    case "B-2": PalletNameFilter = "B-2"; break;
                    case "C-3": PalletNameFilter = "C-3"; break;
                    case "D-4": PalletNameFilter = "D-4"; break;
                    case "E-5": PalletNameFilter = "E-5"; break;
                    case "F-6": PalletNameFilter = "F-6"; break;
                    case "G-7": PalletNameFilter = "G-7"; break;
                    case "H-8": PalletNameFilter = "H-8"; break;
                    case "I-9": PalletNameFilter = "I-9"; break;
                    case "J-10": PalletNameFilter = "J-10"; break;
                    case "K-11": PalletNameFilter = "K-11"; break;
                    case "L-12": PalletNameFilter = "L-12"; break;
                    case "M-13": PalletNameFilter = "M-13"; break;
                    case "N-14": PalletNameFilter = "N-14"; break;
                    case "O-15": PalletNameFilter = "O-15"; break;
                    case "P-16": PalletNameFilter = "P-16"; break;
                    case "Q-17": PalletNameFilter = "Q-17"; break;
                    case "R-18": PalletNameFilter = "R-18"; break;
                    case "S-19": PalletNameFilter = "S-19"; break;
                    case "T-20": PalletNameFilter = "T-20"; break;
                    case "U-21": PalletNameFilter = "U-21"; break;
                    case "V-22": PalletNameFilter = "V-22"; break;
                    case "W-23": PalletNameFilter = "W-23"; break;
                    case "X-24": PalletNameFilter = "X-24"; break;
                    case "Y-25": PalletNameFilter = "Y-25"; break;
                    case "Z-26": PalletNameFilter = "Z-26"; break;
                    case "AA-27": PalletNameFilter = "AA-27"; break;
                    case "BB-28": PalletNameFilter = "BB-28"; break;
                    case "CC-29": PalletNameFilter = "CC-29"; break;
                    case "DD-30": PalletNameFilter = "DD-30"; break;
                    case "EE-31": PalletNameFilter = "EE-31"; break;
                    case "FF-32": PalletNameFilter = "FF-32"; break;
                    case "GG-33": PalletNameFilter = "GG-33"; break;
                    case "HH-34": PalletNameFilter = "HH-34"; break;
                    case "II-35": PalletNameFilter = "II-35"; break;
                    case "JJ-36": PalletNameFilter = "JJ-36"; break;
                    case "KK-37": PalletNameFilter = "KK-37"; break;
                    case "LL-38": PalletNameFilter = "LL-38"; break;
                    case "MM-39": PalletNameFilter = "MM-39"; break;
                    case "NN-40": PalletNameFilter = "NN-40"; break;
                    case "OO-41": PalletNameFilter = "OO-41"; break;
                    case "PP-42": PalletNameFilter = "PP-42"; break;
                    case "QQ-43": PalletNameFilter = "QQ-43"; break;
                    case "RR-44": PalletNameFilter = "RR-44"; break;
                    case "SS-45": PalletNameFilter = "SS-45"; break;
                    case "TT-46": PalletNameFilter = "TT-46"; break;
                    case "UU-47": PalletNameFilter = "UU-47"; break;
                    case "VV-48": PalletNameFilter = "VV-48"; break;
                    case "WW-49": PalletNameFilter = "WW-49"; break;
                    case "XX-50": PalletNameFilter = "XX-50"; break;
                    case "YY-51": PalletNameFilter = "YY-51"; break;
                    case "ZZ-52": PalletNameFilter = "ZZ-52"; break;


                    default: break;
                }
            }
        }

        #endregion

        #region Koli Türü Filtre ComboBox
        public class PackageTypeFilterComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<PackageTypeFilterComboBox> _packageTypeFilterComboBox = new List<PackageTypeFilterComboBox>
        {
            new PackageTypeFilterComboBox(){ID = "Big", Text="BigPackage"},
            new PackageTypeFilterComboBox(){ID = "Small", Text="SmallPackage"}
        };

        private void PackageTypeFilterComboBoxValueChangeHandler(ChangeEventArgs<string, PackageTypeFilterComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Big":
                        PackageTypeFilter = L["BigPackage"].Value;
                        break;

                    case "Small":
                        PackageTypeFilter = L["SmallPackage"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Cari Hesap Filtre ButtonEdit

        SfTextBox CurrentAccountCardsFilterButtonEdit;
        bool SelectCurrentAccountCardsFilterPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsFilterList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsFilterCodeOnCreateIcon()
        {
            var CurrentAccountCardsFilterCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsFilterButtonClickEvent);
            await CurrentAccountCardsFilterButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsFilterCodeButtonClick } });
        }

        public async void CurrentAccountCardsFilterButtonClickEvent()
        {
            SelectCurrentAccountCardsFilterPopupVisible = true;
            CurrentAccountCardsFilterList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void CurrentAccountCardsFilterOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                CurrentAccountNameFilter = string.Empty;
                CurrentAccountIDFilter = Guid.Empty;
            }
        }

        public async void CurrentAccountCardsFilterDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                CurrentAccountIDFilter = selectedCurrentAccountCard.Id;
                CurrentAccountNameFilter = selectedCurrentAccountCard.Name;
                SelectCurrentAccountCardsFilterPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Etiket Rapor Metotları

        DxReportViewer BigLabelReportViewer { get; set; }
        XtraReport BigLabelReport { get; set; }
        bool BigLabelReportVisible { get; set; }

        async void CreateLargePalletLabelReport(SelectPalletRecordsDto pallet)
        {
            #region Çeki Listesi Bilgisi

            var packingList = (await PackingListsAppService.GetAsync(pallet.PackingListID.GetValueOrDefault())).Data;

            if (packingList.Id != Guid.Empty)
            {
                Guid sentCompanyId = packingList.TransmitterID.GetValueOrDefault();
                var sentCurrentAccountCard = (await CurrentAccountCardsAppService.GetAsync(sentCompanyId)).Data;
                string supplierNo = sentCurrentAccountCard.SupplierNo;
                string sentCompany = sentCurrentAccountCard.Name;
                string sentCompanyAddress1 = sentCurrentAccountCard.Address1;
                string sentCompanyAddress2 = sentCurrentAccountCard.Address2;
                string sender = sentCompany + Environment.NewLine + sentCompanyAddress1 + Environment.NewLine + sentCompanyAddress2;


                Guid recieverCompanyId = packingList.RecieverID.GetValueOrDefault();
                var recieverCurrentAccountCard = (await CurrentAccountCardsAppService.GetAsync(recieverCompanyId)).Data;
                string recieverCompany = recieverCurrentAccountCard.Name;
                string recieverCompanyAddress1 = recieverCurrentAccountCard.Address1;
                string reciever = recieverCompany + ", " + recieverCompanyAddress1;
                string shippingAddress = recieverCurrentAccountCard.ShippingAddress;


                string areksPackingListNo = packingList.Code;
                string billNo = packingList.BillNo;

                string customerOrderNo = "";
                string mainLoad = sentCurrentAccountCard.CustomerCode;

                BigLabelReport = new XtraReport();
                BigLabelReport.ShowPrintMarginsWarning = false;
                BigLabelReport.CreateDocument();

                for (int i = 0; i < pallet.SelectPalletRecordLines.Count; i++)
                {
                    var line = pallet.SelectPalletRecordLines[i];

                    if (line != null)
                    {
                        var packageFiche = (await PackageFichesAppService.GetAsync(line.PackageFicheID.GetValueOrDefault())).Data;

                        if (packageFiche.Id != Guid.Empty)
                        {
                            var packageInfo = (await PackingListsAppService.GetPackingListLineByPackageId(packageFiche.Id)).Data.FirstOrDefault();
                            int startPackageNo = 0;
                            int endPackageNo = 0;

                            if (packageInfo.PackageNo.Contains("-"))
                            {
                                string[] packageArray = packageInfo.PackageNo.Split('-');
                                startPackageNo = Convert.ToInt32(packageArray[0]);
                                endPackageNo = Convert.ToInt32(packageArray[1]);
                            }
                            else
                            {
                                startPackageNo = Convert.ToInt32(packageInfo.PackageNo);
                                endPackageNo = Convert.ToInt32(packageInfo.PackageNo);
                            }

                            decimal numberofPackage = line.NumberofPackage;
                            Guid productId = line.ProductID.GetValueOrDefault();

                            if (mainLoad == "049")
                            {
                                var productReferenceNoList = (await ProductReferanceNumbersAppService.GetSelectListAsync(productId)).Data;

                                string customerReferenceNo = string.Empty;

                                string orderReferenceNo = string.Empty;

                                Guid productRefNoID = Guid.Empty;

                                var productRefNo = productReferenceNoList.Where(t => t.CurrentAccountCardID == line.CurrentAccountCardID.GetValueOrDefault()).FirstOrDefault();


                                string markingRefNo = productRefNo.CustomerReferanceNo;
                                string barcodeNo = productRefNo.CustomerBarcodeNo;
                                markingRefNo = markingRefNo.Replace(" ", "");
                                markingRefNo = markingRefNo.Substring(0, markingRefNo.Length - 2) + "000";
                                decimal netKg = line.TotalNetKG;
                                decimal grossKg = line.TotalGrossKG;
                                decimal quantity = line.PackageContent;
                                customerOrderNo = (await SalesOrdersAppService.GetAsync(packageFiche.SalesOrderID.Value)).Data.CustomerOrderNr;


                                for (int start = startPackageNo; start <= endPackageNo; start++)
                                {

                                    List<BigPalletLabelReportDto> reportSource = new List<BigPalletLabelReportDto>();

                                    BigPalletLabelReportDto p = new BigPalletLabelReportDto
                                    {
                                        Quantity = quantity,
                                        GrossKg = grossKg / numberofPackage,
                                        BillNo = billNo,
                                        AreksPackingNo = areksPackingListNo,
                                        MarkingRefNo = markingRefNo,
                                        NetKg = netKg / numberofPackage,
                                        PackageNo = start.ToString(),
                                        PackingListNo = packingList.Code2,
                                        SupplierNo = supplierNo,
                                        ShippingAddress = shippingAddress,
                                        SenderCompany = sentCompany,
                                        SentCompany = recieverCompany,
                                        CustomerOrderNo = customerOrderNo
                                    };

                                    reportSource.Add(p);

                                    BigPalletLabelReport rpr = new BigPalletLabelReport();
                                    rpr.BarcodeNo = barcodeNo;
                                    rpr.ShowPrintMarginsWarning = false;
                                    rpr.DataSource = reportSource;
                                    rpr.CreateDocument();
                                    BigLabelReport.Pages.AddRange(rpr.Pages);
                                }
                            }
                        }
                    }
                }

                BigLabelReportVisible = true;
                BigLabelReport.ShowPreviewMarginLines = false;
                await (InvokeAsync(StateHasChanged));
                
            }

            #endregion
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
