﻿using DevExpress.Blazor.Reporting;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.CodeAnalysis;
using Microsoft.SqlServer.Management.XEvent;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.PackageFiche.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.CommercialInvoice;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.CustomsInstruction;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.PackingList;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.ShippingInstruction;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.UploadConfirmation;
using TsiErp.ErpUI.Reports.ShippingManagement.PalletReports.PalletLabels;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ShippingManagement.PackingList
{
    public partial class PackingListsListPage : IDisposable
    {

        public class PalletSelectionModal
        {
            public bool SelectedPallet { get; set; }
            public Guid PalletID { get; set; }
            public Guid? CurrentAccountCardID { get; set; }
            public string PalletCode { get; set; }
            public string PalletName { get; set; }
            public int NumberofPackage { get; set; }
            public int Width_ { get; set; }
            public int Length_ { get; set; }
            public int Height_ { get; set; }
            public string PackageType { get; set; }
        }

        private SfGrid<SelectPackingListPalletLinesDto> _LinePalletGrid;
        private SfGrid<SelectPackingListPalletPackageLinesDto> _LinePalletPackageGrid;
        private SfGrid<SelectPackingListPalletCubageLinesDto> _LineCubageGrid;
        private SfGrid<PalletSelectionModal> _LinePalletSelectionGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletSelectionGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletPackageGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPackingListPalletLinesDto> GridLinePalletList = new List<SelectPackingListPalletLinesDto>();
        List<SelectPackingListPalletPackageLinesDto> GridLinePalletPackageList = new List<SelectPackingListPalletPackageLinesDto>();
        List<SelectPackingListPalletCubageLinesDto> GridLineCubageList = new List<SelectPackingListPalletCubageLinesDto>();
        List<ListPalletRecordsDto> PalletRecordsList = new List<ListPalletRecordsDto>();
        List<PalletSelectionModal> PalletSelectionList = new List<PalletSelectionModal>();

        #region Stock Parameters

        bool autoCostParameter;

        Guid? DefaultBranchID = Guid.Empty;
        Guid? DefaultWarehouseID = Guid.Empty;
        string DefaultBranchCode = string.Empty;
        string DefaultWarehouseCode = string.Empty;

        #endregion

        public bool ShowPalletsModal = false;
        public bool ShowApproveModal = false;
        public bool isSalesInvoce = false;
        public bool isUnitPricesFromOrder = false;
        public bool DisabledUnitPricesFromOrder = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = PackingListsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PackingListsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreatePalletContextMenuItems();
            CreatePalletPackageContextMenuItems();
            CreatePalletSelectionContextMenuItems();

            autoCostParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.AutoCostParameter;

        }

        #region Çeki Listeleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPackingListsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("PackingListsChildMenu"),
                PackingListState = PackingListStateEnum.Hazırlanıyor,
            };

            DataSource.SelectPackingListPalletCubageLines = new List<SelectPackingListPalletCubageLinesDto>();
            GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;

            DataSource.SelectPackingListPalletLines = new List<SelectPackingListPalletLinesDto>();
            GridLinePalletList = DataSource.SelectPackingListPalletLines;

            DataSource.SelectPackingListPalletPackageLines = new List<SelectPackingListPalletPackageLinesDto>();
            GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

            PalletSelectionList = new List<PalletSelectionModal>();

            PalletSelectionList.Clear();

            #region Enum Combobox Localization

            foreach (var item in salesTypes)
            {
                item.SalesTypeName = L[item.SalesTypeName];
            }

            foreach (var item in tIRTypes)
            {
                item.TIRTypeName = L[item.TIRTypeName];
            }

            foreach (var item in packingListStates)
            {
                item.PackingListStateName = L[item.PackingListStateName];
            }

            #endregion

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {
            #region Enum Combobox Localization

            foreach (var item in salesTypes)
            {
                item.SalesTypeName = L[item.SalesTypeName];
            }

            foreach (var item in tIRTypes)
            {
                item.TIRTypeName = L[item.TIRTypeName];
            }

            foreach (var item in packingListStates)
            {
                item.PackingListStateName = L[item.PackingListStateName];
            }

            #endregion

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
                    PalletSelectionList.Clear();
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
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
                            case "PackingListsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextAdd"], Id = "new" }); break;
                            case "PackingListsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextChange"], Id = "changed" }); break;
                            case "PackingListsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextDelete"], Id = "delete" }); break;
                            case "PackingListsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextRefresh"], Id = "refresh" }); break;
                            case "PackingListsContextPrint":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "PackingListsContextPackingList":
                                                subMenus.Add(new MenuItem { Text = L["PackingListsContextPackingList"], Id = "packinglist" }); break;
                                            case "PackingListsContextPackingListEng":
                                                subMenus.Add(new MenuItem { Text = L["PackingListsContextPackingListEng"], Id = "packinglisteng" }); break;
                                            case "PalletRecordsContextCommercialInvoice":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextCommercialInvoice"], Id = "commercialinvoice" }); break;
                                            case "PalletRecordsContextCustomClearanceInstruction":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextCustomClearanceInstruction"], Id = "custominstruction" }); break;
                                            case "PalletRecordsContextShippingInstruction":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextShippingInstruction"], Id = "shippinginstruction" }); break;
                                            case "PalletRecordsContextUploadConfirmation":
                                                subMenus.Add(new MenuItem { Text = L["PalletRecordsContextUploadConfirmation"], Id = "uploadconfirmation" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextPrint"], Id = "print", Items = subMenus }); break;

                            case "PackingListsContextState":

                                List<MenuItem> subMenusState = new List<MenuItem>();

                                var subListState = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenuState in subListState)
                                {
                                    var subPermissionState = UserPermissionsList.Where(t => t.MenuId == subMenuState.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermissionState)
                                    {
                                        switch (subMenuState.MenuName)
                                        {
                                            case "PackingListsContextPreparing":
                                                subMenusState.Add(new MenuItem { Text = L["PackingListsContextPreparing"], Id = "preparing" }); break;
                                            case "PackingListsContextCompleted":
                                                subMenusState.Add(new MenuItem { Text = L["PackingListsContextCompleted"], Id = "completed" }); break;
                                            case "PackingListsContextApprove":
                                                subMenusState.Add(new MenuItem { Text = L["PackingListsContextApprove"], Id = "approve" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextState"], Id = "state", Items = subMenusState }); break;

                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreatePalletContextMenuItems()
        {
            if (PalletGridContextMenu.Count() == 0)
            {



                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PackingListsPalletLineContextSelectPallet":
                                PalletGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletLineContextSelectPallet"], Id = "selectpallet" }); break;
                            case "PackingListsPalletLineContextRemovePallet":
                                PalletGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletLineContextRemovePallet"], Id = "removepallet" }); break;

                            default: break;
                        }
                    }
                }

            }
        }

        protected void CreatePalletPackageContextMenuItems()
        {
            if (PalletPackageGridContextMenu.Count() == 0)
            {

                var contextID = contextsList.Where(t => t.MenuName == "PackingListsPalletPackageLineContextEnumarate").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    PalletPackageGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletPackageLineContextEnumarate"], Id = "enumarate" });
                }


            }
        }

        protected void CreatePalletSelectionContextMenuItems()
        {
            if (PalletSelectionGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PackingListsPalletSelectionContextSelect":
                                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextSelect"], Id = "select" }); break;
                            case "PackingListsPalletSelectionContextSelectAll":
                                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextSelectAll"], Id = "selectall" }); break;
                            case "PackingListsPalletSelectionContextRemove":
                                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextRemove"], Id = "remove" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPackingListsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;
                        GridLinePalletList = DataSource.SelectPackingListPalletLines;
                        GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "approve":
                    if (args.RowInfo.RowData != null)
                    {


                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;
                        GridLinePalletList = DataSource.SelectPackingListPalletLines;
                        GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

                        ShowApproveModal = true;
                        isSalesInvoce = true;
                        isUnitPricesFromOrder = false;
                        DisabledUnitPricesFromOrder = false;

                        var salesManagementParameter = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data;

                        if (salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty)
                        {
                            DefaultBranchID = salesManagementParameter.DefaultBranchID;
                            DefaultBranchCode = salesManagementParameter.DefaultBranchCode;
                            DefaultWarehouseID = salesManagementParameter.DefaultWarehouseID;
                            DefaultWarehouseCode = salesManagementParameter.DefaultWarehouseCode;
                        }



                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "preparing":
                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                        await Task.Delay(100);
                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.PackingListState = PackingListStateEnum.Hazırlanıyor;

                        var updatePackingInput2 = ObjectMapper.Map<SelectPackingListsDto, UpdatePackingListsDto>(DataSource);

                        await PackingListsAppService.UpdateAsync(updatePackingInput2);

                        SpinnerService.Hide();
                        await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessagePreparingMessage"]);
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "completed":
                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                        await Task.Delay(100);
                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.PackingListState = PackingListStateEnum.Tamamlandı;

                        var updatePackingInput3 = ObjectMapper.Map<SelectPackingListsDto, UpdatePackingListsDto>(DataSource);

                        await PackingListsAppService.UpdateAsync(updatePackingInput3);

                        SpinnerService.Hide();
                        await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessageCompletedMessage"]);
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
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

                case "print":
                    if (args.RowInfo.RowData != null)
                    {

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "packinglist":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        #region Enum Combobox Localization

                        DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

                        DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

                        DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

                        #endregion


                        PackingListDynamicReport = new XtraReport();
                        PackingListReportVisible = true;
                        await CreateTRPackingListReport(DataSource);
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "packinglisteng":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        #region Enum Combobox Localization

                        DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

                        DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

                        DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

                        #endregion


                        PackingListEngDynamicReport = new XtraReport();
                        PackingListEngReportVisible = true;
                        await CreateEngPackingListReport(DataSource);
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "commercialinvoice":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        #region Enum Combobox Localization

                        DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

                        DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

                        DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

                        #endregion


                        CommercialInvoiceDynamicReport = new XtraReport();
                        CommercialInvoiceReportVisible = true;
                        await CreateCommercialInvoiceReport(DataSource);
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "custominstruction":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        CustomsInstructionReport = new XtraReport();
                        CustomsInstructionReportVisible = true;
                        await CreateCustomsInstructionReport(DataSource);

                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "shippinginstruction":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        ShippingInstructionReport = new XtraReport();
                        ShippingInstructionReportVisible = true;
                        await CreateShippingInstructionReport(DataSource);

                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "uploadconfirmation":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        UploadConfirmationDynamicReport = new XtraReport();
                        UploadConfirmationReportVisible = true;
                        await CreateUploadConfirmationReport(DataSource);

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void PalletLineContextMenuClick(ContextMenuClickEventArgs<SelectPackingListPalletLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "selectpallet":


                    if (DataSource.Id == Guid.Empty)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningPalletSelectionTitle"], L["UIWarningPalletSelectionMessage"]);
                    }
                    else
                    {
                        PalletSelectionList.Clear();
                        PalletLineIndex = GridLinePalletList.Count;
                        PalletRecordsList = (await PalletRecordsAppService.GetListAsync(new ListPalletRecordsParameterDto())).Data.Where(t => t.PackingListID == DataSource.Id).ToList();

                        PalletRecordsList = PalletRecordsList.OrderBy(t => t.Name).ToList();

                        foreach (var pallet in PalletRecordsList)
                        {
                            if (DataSource.SelectPackingListPalletLines.Where(t => t.PalletID == pallet.Id).Count() == 0)
                            {
                                PalletSelectionModal palletSelectionModel = new PalletSelectionModal
                                {
                                    PalletCode = pallet.Code,
                                    PalletID = pallet.Id,
                                    PalletName = pallet.Name,
                                    NumberofPackage = pallet.PalletPackageNumber,
                                    Height_ = pallet.Height_,
                                    Length_ = pallet.Lenght_,
                                    PackageType = pallet.PackageType,
                                    CurrentAccountCardID = pallet.CurrentAccountCardID,
                                    Width_ = pallet.Width_,
                                    SelectedPallet = false
                                };

                                PalletSelectionList.Add(palletSelectionModel);
                            }

                        }

                        ShowPalletsModal = true;
                    }



                    break;

                case "removepallet":

                    if (args.RowInfo.RowData != null)
                    {

                        var selectedPallet = args.RowInfo.RowData;

                        if (selectedPallet.Id != Guid.Empty)
                        {
                            var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["UIWarningPalletRemoveMessage"]);

                            if (res == true)
                            {
                                int selectedPalletIndex = DataSource.SelectPackingListPalletLines.IndexOf(selectedPallet);

                                if (selectedPalletIndex > -1)
                                {
                                    List<SelectPackingListPalletLinesDto> removedPallets = new List<SelectPackingListPalletLinesDto>();

                                    if (selectedPalletIndex == 0)
                                    {
                                        removedPallets = DataSource.SelectPackingListPalletLines.ToList();
                                    }
                                    else if (selectedPalletIndex == DataSource.SelectPackingListPalletLines.Count - 1)
                                    {
                                        removedPallets.Add(DataSource.SelectPackingListPalletLines[selectedPalletIndex]);
                                    }
                                    else
                                    {
                                        if (selectedPalletIndex == DataSource.SelectPackingListPalletLines.Count - 2)
                                        {
                                            removedPallets.Add(selectedPallet);
                                            removedPallets.Add(DataSource.SelectPackingListPalletLines[selectedPalletIndex + 1]);
                                        }
                                        else
                                        {
                                            removedPallets = DataSource.SelectPackingListPalletLines.ToList().GetRange(selectedPalletIndex, DataSource.SelectPackingListPalletLines.Count - 1);
                                        }
                                    }

                                    foreach (var pallet in removedPallets)
                                    {

                                        var removedPackageFiches = DataSource.SelectPackingListPalletPackageLines.Where(t => t.PalletID == pallet.Id).ToList();

                                        foreach (var packageFiche in removedPackageFiches)
                                        {
                                            await PackingListsAppService.DeleteLinePalletPackageAsync(packageFiche.Id);
                                        }

                                        await PackingListsAppService.DeleteLinePalletAsync(pallet.Id);

                                        DataSource.SelectPackingListPalletLines.Remove(pallet);
                                    }

                                    foreach (var cubage in DataSource.SelectPackingListPalletCubageLines)
                                    {
                                        await PackingListsAppService.DeleteLineCubageAsync(cubage.Id);
                                    }

                                    GridLineCubageList.Clear();

                                    foreach (var item in DataSource.SelectPackingListPalletLines)
                                    {
                                        var pallet = (await PalletRecordsAppService.GetAsync(item.PalletID.GetValueOrDefault())).Data;

                                        PalletSelectionModal palletSelectionModel = new PalletSelectionModal
                                        {
                                            PalletCode = pallet.Code,
                                            PalletID = pallet.Id,
                                            PalletName = pallet.Name,
                                            NumberofPackage = pallet.PalletPackageNumber,
                                            Height_ = pallet.Height_,
                                            Length_ = pallet.Lenght_,
                                            PackageType = pallet.PackageType,
                                            CurrentAccountCardID = pallet.CurrentAccountCardID,
                                            Width_ = pallet.Width_,
                                            SelectedPallet = false
                                        };

                                        PalletSelectionList.Add(palletSelectionModel);
                                    }

                                    #region Palet Kübaj İşlemleri

                                    var palletList = PalletSelectionList.GroupBy(t => t.PackageType).Select(t => new { PackageType = t.Key, Pallet = t.ToList() }).ToList();

                                    foreach (var pallet in palletList)
                                    {
                                        int height = pallet.Pallet.Select(t => t.Height_).FirstOrDefault();
                                        int width = pallet.Pallet.Select(t => t.Width_).FirstOrDefault();
                                        int lenght = pallet.Pallet.Select(t => t.Length_).FirstOrDefault();

                                        SelectPackingListPalletCubageLinesDto palletCubageLineModel = new SelectPackingListPalletCubageLinesDto
                                        {
                                            NumberofPallet = pallet.Pallet.Count,
                                            Height_ = height,
                                            Width_ = width,
                                            Load_ = lenght,
                                            Cubage = (height * width * lenght * pallet.Pallet.Count) / 1000000
                                        };

                                        GridLineCubageList.Add(palletCubageLineModel);
                                    }

                                    await _LineCubageGrid.Refresh();

                                    #endregion

                                }
                            }
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        #region Palet Seçim Modalı

        public async void PalletSelectionLineContextMenuClick(ContextMenuClickEventArgs<PalletSelectionModal> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    if (args.RowInfo.RowData != null)
                    {

                        var pallet = args.RowInfo.RowData;
                        int selectedPalletIndex = PalletSelectionList.IndexOf(pallet);

                        bool isAdded = false;

                        foreach (var pal in PalletSelectionList)
                        {
                            int palIndex = PalletSelectionList.IndexOf(pal);

                            if (palIndex < selectedPalletIndex)
                            {
                                if (!pal.SelectedPallet)
                                {
                                    await ModalManager.WarningPopupAsync(L["UIWarningSelectedIndexTitle"], L["UIWarningSelectedIndexMessage"]);
                                    break;
                                }
                            }
                            else
                            {
                                isAdded = true;
                                break;
                            }
                        }

                        if (isAdded)
                        {
                            PalletSelectionList[selectedPalletIndex].SelectedPallet = true;
                            await _LinePalletSelectionGrid.Refresh();
                        }


                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "selectall":

                    if (args.RowInfo.RowData != null)
                    {

                        foreach (var line in PalletSelectionList)
                        {
                            int lineIndex = PalletSelectionList.IndexOf(line);
                            PalletSelectionList[lineIndex].SelectedPallet = true;
                        }

                        await _LinePalletSelectionGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "remove":
                    if (args.RowInfo.RowData != null)
                    {

                        var palletRemove = args.RowInfo.RowData;
                        int selectedRemovePalletIndex = PalletSelectionList.IndexOf(palletRemove);

                        PalletSelectionList[selectedRemovePalletIndex].SelectedPallet = false;

                        await _LinePalletSelectionGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }
        }


        public int PalletLineIndex { get; set; }

        public async void OnTransferSelectedPalletButtonClicked()
        {
            PalletSelectionList = PalletSelectionList.Where(t => t.SelectedPallet == true).OrderBy(t => t.PalletName).ToList();

            int packageNo = 1;
            foreach (var pallet in PalletSelectionList)
            {
                #region Palet Line İşlemleri

                if (GridLinePalletList.Count == 0)
                {
                    SelectPackingListPalletLinesDto palletLineModel = new SelectPackingListPalletLinesDto
                    {
                        LineNr = GridLinePalletList.Count + 1,
                        PalletID = pallet.PalletID,
                        PalletName = pallet.PalletName.Split("-")[0],
                        NumberofPackage = pallet.NumberofPackage,
                        FirstPackageNo = packageNo.ToString(),
                        LastPackageNo = (packageNo + pallet.NumberofPackage - 1).ToString(),
                    };

                    packageNo += pallet.NumberofPackage;

                    GridLinePalletList.Add(palletLineModel);
                    PalletLineIndex = PalletLineIndex + 1;
                }
                else
                {
                    string firstPackageNo = Convert.ToString(Convert.ToInt32(GridLinePalletList[PalletLineIndex - 1].LastPackageNo) + 1);

                    SelectPackingListPalletLinesDto palletLineModel = new SelectPackingListPalletLinesDto
                    {
                        LineNr = GridLinePalletList.Count + 1,
                        PalletID = pallet.PalletID,
                        PalletName = pallet.PalletName.Split("-")[0],
                        NumberofPackage = pallet.NumberofPackage,
                        FirstPackageNo = firstPackageNo,
                        LastPackageNo = (Convert.ToInt32(firstPackageNo) + pallet.NumberofPackage - 1).ToString(),
                    };

                    packageNo += pallet.NumberofPackage;

                    GridLinePalletList.Add(palletLineModel);
                    PalletLineIndex = PalletLineIndex + 1;
                }


                #endregion

                #region Palet Paket Line İşlemleri

                var palletLineList = (await PalletRecordsAppService.GetAsync(pallet.PalletID)).Data.SelectPalletRecordLines;
                decimal packageKG = 0;
                var currentAccountDataSource = (await CurrentAccountCardsAppService.GetAsync(pallet.CurrentAccountCardID.GetValueOrDefault())).Data;

                if (pallet.PackageType == L["BigPackage"].Value)
                {
                    packageKG = currentAccountDataSource.BigPackageKG;
                }
                else if (pallet.PackageType == L["SmallPackage"].Value)
                {
                    packageKG = currentAccountDataSource.SmallPackageKG;
                }

                foreach (var palletLine in palletLineList)
                {
                    var unitKG = (await ProductsAppService.GetAsync(palletLine.ProductID.GetValueOrDefault())).Data.UnitWeight;

                    var packageFiche = (await PackageFichesAppService.GetAsync(palletLine.PackageFicheID.GetValueOrDefault())).Data;

                    var productionOrderListDto = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.FinishedProductID == packageFiche.ProductID && t.OrderID == packageFiche.SalesOrderID).FirstOrDefault();

                    var productionOrder = (await ProductionOrdersAppService.GetAsync(productionOrderListDto.Id)).Data;

                    decimal onePackageNetKG = palletLine.PackageContent * unitKG;
                    decimal onePackageGrossKG = onePackageNetKG + packageKG;

                    var product = (await ProductsAppService.GetAsync(palletLine.ProductID.GetValueOrDefault())).Data;

                    SelectPackingListPalletPackageLinesDto palletPackageLineModel = new SelectPackingListPalletPackageLinesDto
                    {
                        PackageType = palletLine.PackageType,
                        PackageFicheID = palletLine.PackageFicheID.GetValueOrDefault(),
                        ProductID = palletLine.ProductID,
                        ProductCode = palletLine.ProductCode,
                        ProductName = palletLine.ProductName,
                        CustomerCode = palletLine.CustomerCode,
                        CustomerID = palletLine.CurrentAccountCardID,
                        NumberofPackage = palletLine.NumberofPackage,
                        TotalAmount = palletLine.TotalAmount,
                        TotalGrossKG = palletLine.TotalGrossKG,
                        TotalNetKG = palletLine.TotalNetKG,
                        OnePackageGrossKG = onePackageGrossKG,
                        OnePackageNetKG = onePackageNetKG,
                        PackageContent = palletLine.PackageContent,
                        PackageNo = string.Empty,
                        ProductionOrderID = productionOrder.Id,
                        SalesOrderID = productionOrder.OrderID,
                        SalesOrderLineID = productionOrder.OrderLineID,
                        ProductGroupID = product.ProductGrpID,
                        PalletID = pallet.PalletID
                    };


                    GridLinePalletPackageList.Add(palletPackageLineModel);
                }

                #endregion
            }

            #region Palet Kübaj İşlemleri

            var palletList = PalletSelectionList.GroupBy(t => t.PackageType).Select(t => new { PackageType = t.Key, Pallet = t.ToList() }).ToList();

            foreach (var pallet in palletList)
            {
                int height = pallet.Pallet.Select(t => t.Height_).FirstOrDefault();
                int width = pallet.Pallet.Select(t => t.Width_).FirstOrDefault();
                int lenght = pallet.Pallet.Select(t => t.Length_).FirstOrDefault();

                if (!GridLineCubageList.Any(t => t.Height_ == height && t.Width_ == width && t.Load_ == lenght))
                {
                    SelectPackingListPalletCubageLinesDto palletCubageLineModel = new SelectPackingListPalletCubageLinesDto
                    {
                        NumberofPallet = pallet.Pallet.Count,
                        Height_ = height,
                        Width_ = width,
                        Load_ = lenght,
                        Cubage = (height * width * lenght * pallet.Pallet.Count) / 1000000
                    };

                    GridLineCubageList.Add(palletCubageLineModel);
                }
                else
                {
                    var updatedCubageLine = GridLineCubageList.FirstOrDefault(t => t.Height_ == height && t.Width_ == width && t.Load_ == lenght);
                    updatedCubageLine.NumberofPallet = updatedCubageLine.NumberofPallet + pallet.Pallet.Count;
                    updatedCubageLine.Height_ = height;
                    updatedCubageLine.Width_ = width;
                    updatedCubageLine.Load_ = lenght;
                    updatedCubageLine.Cubage = (height * width * lenght * pallet.Pallet.Count) / 1000000;

                }


            }

            await _LineCubageGrid.Refresh();

            #endregion

            GridLinePalletList = GridLinePalletList.OrderBy(t => t.PalletName).ToList();

            await _LinePalletGrid.Refresh();

            PalletSelectionList.Clear();

            ShowPalletsModal = false;

            await InvokeAsync(StateHasChanged);
        }

        #endregion

        public async void PalletPackageLineContextMenuClick(ContextMenuClickEventArgs<SelectPackingListPalletPackageLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "enumarate":
                    if (args.RowInfo.RowData != null)
                    {

                        if (GridLinePalletPackageList != null && GridLinePalletPackageList.Count > 0)
                        {
                            int packageNo = 1;

                            foreach (var line in GridLinePalletPackageList)
                            {
                                int lineIndex = GridLinePalletPackageList.IndexOf(line);
                                if (GridLinePalletPackageList[lineIndex].NumberofPackage != 1)
                                {
                                    GridLinePalletPackageList[lineIndex].PackageNo = packageNo.ToString() + "-" + (packageNo + line.NumberofPackage - 1).ToString();
                                }
                                else
                                {
                                    GridLinePalletPackageList[lineIndex].PackageNo = packageNo.ToString();
                                }
                                packageNo = packageNo + line.NumberofPackage;
                            }
                        }

                        await _LinePalletPackageGrid.Refresh();
                    }

                    break;

                default:
                    break;
            }


            await Task.CompletedTask;
        }

        public async void DateValueChangeHandler(ChangedEventArgs<DateTime?> args)
        {
            DataSource.BillDate = DataSource.BillDate.Value.AddDays(DataSource.TransmitterPaymentTermDay);
            await InvokeAsync(StateHasChanged);
        }

        public async void PackingListTransferredClickEvent()
        {
            SpinnerService.Show();
            await Task.Delay(100);

            var now = GetSQLDateAppService.GetDateFromSQL();

            if (isSalesInvoce)
            {
                if (isUnitPricesFromOrder)
                {

                    #region SATIŞ FATURA OLUŞACAK VE BİRİM FİYATLAR SİPARİŞTEN GELECEK
                    List<SelectSalesInvoiceLinesDto> SalesInvoiceLinesList = new List<SelectSalesInvoiceLinesDto>();

                    List<SelectStockFicheLinesDto> StockFicheLinesList = new List<SelectStockFicheLinesDto>();

                    foreach (var item in GridLinePalletPackageList)
                    {
                        var salesOrder = (await SalesOrdersAppService.GetAsync(item.SalesOrderID.GetValueOrDefault())).Data;

                        if (salesOrder != null && salesOrder.Id != Guid.Empty)
                        {
                            #region Sales Order Line Sevk Edildi Update

                            var salesOrderLine = salesOrder.SelectSalesOrderLines.Where(t => t.Id == item.SalesOrderLineID.GetValueOrDefault()).FirstOrDefault();
                            int lineIndex = salesOrder.SelectSalesOrderLines.IndexOf(salesOrderLine);
                            salesOrder.SelectSalesOrderLines[lineIndex].SalesOrderLineStateEnum = SalesOrderLineStateEnum.SevkEdildi;

                            var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(salesOrder);

                            await SalesOrdersAppService.UpdateAsync(updateInput);
                            #endregion

                            #region Sales Invoice Line Oluşturma
                            SelectSalesInvoiceLinesDto salesInvoiceLineModel = new SelectSalesInvoiceLinesDto
                            {
                                BranchCode = salesOrderLine.BranchCode,
                                BranchID = salesOrderLine.BranchID,
                                BranchName = salesOrderLine.BranchName,
                                CurrentAccountCardCode = salesOrderLine.CurrentAccountCardCode,
                                CurrentAccountCardID = salesOrderLine.CurrentAccountCardID,
                                CurrentAccountCardName = salesOrderLine.CurrentAccountCardName,
                                DiscountAmount = salesOrderLine.DiscountAmount,
                                DiscountRate = salesOrderLine.DiscountRate,
                                ExchangeRate = salesOrderLine.ExchangeRate,
                                LikedPropositionLineID = salesOrderLine.LikedPropositionLineID,
                                LineAmount = salesOrderLine.LineAmount,
                                LineDescription = salesOrderLine.LineDescription,
                                LineNr = SalesInvoiceLinesList.Count + 1,
                                Date_ = now.Date,
                                LineTotalAmount = salesOrderLine.LineTotalAmount,
                                LinkedSalesPropositionID = salesOrderLine.LinkedSalesPropositionID,
                                OrderAcceptanceRecordID = salesOrderLine.OrderAcceptanceRecordID,
                                OrderAcceptanceRecordLineID = salesOrderLine.OrderAcceptanceRecordLineID,
                                PaymentPlanID = salesOrderLine.PaymentPlanID,
                                PaymentPlanName = salesOrderLine.PaymentPlanName,
                                ProductCode = salesOrderLine.ProductCode,
                                ProductGroupID = salesOrderLine.ProductGroupID,
                                ProductGroupName = salesOrderLine.ProductGroupName,
                                ProductID = salesOrderLine.ProductID,
                                ProductName = salesOrderLine.ProductName,
                                Quantity = salesOrderLine.Quantity,
                                TransactionExchangeDiscountAmount = salesOrderLine.TransactionExchangeDiscountAmount,
                                TransactionExchangeLineAmount = salesOrderLine.TransactionExchangeLineAmount,
                                TransactionExchangeLineTotalAmount = salesOrderLine.TransactionExchangeLineTotalAmount,
                                TransactionExchangeUnitPrice = salesOrderLine.TransactionExchangeUnitPrice,
                                TransactionExchangeVATamount = salesOrderLine.TransactionExchangeVATamount,
                                UnitPrice = salesOrderLine.UnitPrice,
                                UnitSetCode = salesOrderLine.UnitSetCode,
                                UnitSetID = salesOrderLine.UnitSetID,
                                VATamount = salesOrderLine.VATamount,
                                VATrate = salesOrderLine.VATrate,
                                PurchaseSupplyDate = salesOrderLine.PurchaseSupplyDate,
                                WarehouseCode = salesOrderLine.WarehouseCode,
                                WarehouseID = salesOrderLine.WarehouseID,
                                WarehouseName = salesOrderLine.WarehouseName,
                            };

                            SalesInvoiceLinesList.Add(salesInvoiceLineModel);
                            #endregion
                        }

                        #region Üretim Emri ve Bağlı Üretim Emrinin Durumunu Sevk Edildi Yapma
                        var productionOrder = (await ProductionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;

                        if (productionOrder != null && productionOrder.Id != Guid.Empty)
                        {
                            productionOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                            var updateProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                            await ProductionOrdersAppService.UpdateAsync(updateProdOrderInput);

                            var linkedProdOrders = (await ProductionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data.ToList();

                            if (linkedProdOrders != null && linkedProdOrders.Count > 0)
                            {
                                foreach (var linkedProdOrder in linkedProdOrders)
                                {
                                    linkedProdOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                                    var updateLinkedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(linkedProdOrder);

                                    await ProductionOrdersAppService.UpdateAsync(updateLinkedProdOrderInput);
                                }
                            }
                        }
                        #endregion
                    }

                    #region Sales Invoice Create İşlemi
                    CreateSalesInvoiceDto createSalesInvoiceModel = new CreateSalesInvoiceDto
                    {
                        BranchID = DefaultBranchID,
                        ConfirmedLoadingDate = DataSource.LoadingDate,
                        CurrencyID = Guid.Empty,
                        CurrentAccountCardID = DataSource.RecieverID,
                        Description_ = string.Empty,
                        CustomerOrderNr = string.Empty,
                        CustomerRequestedDate = null,
                        Date_ = now.Date,
                        ExchangeRate = 1,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesInvoicesChildMenu"),
                        GrossAmount = SalesInvoiceLinesList.Sum(x => x.LineAmount) + SalesInvoiceLinesList.Sum(x => x.DiscountAmount),
                        isStandart = false,
                        LinkedSalesPropositionID = Guid.Empty,
                        NetAmount = SalesInvoiceLinesList.Sum(x => x.LineTotalAmount),
                        OrderAcceptanceRecordID = Guid.Empty,
                        PaymentPlanID = Guid.Empty,
                        PricingCurrency = SalesInvoiceLinesList.Sum(x => x.TransactionExchangeLineTotalAmount) > 0 ? 2 : 1,
                        ShippingAdressID = DataSource.ShippingAddressID,
                        SpecialCode = string.Empty,
                        Time_ = string.Empty,
                        TotalDiscountAmount = SalesInvoiceLinesList.Sum(x => x.DiscountAmount),
                        TotalVatAmount = SalesInvoiceLinesList.Sum(x => x.VATamount),
                        TotalVatExcludedAmount = (SalesInvoiceLinesList.Sum(x => x.LineAmount) + SalesInvoiceLinesList.Sum(x => x.DiscountAmount)) - SalesInvoiceLinesList.Sum(x => x.DiscountAmount),
                        TransactionExchangeCurrencyID = Guid.Empty,
                        TransactionExchangeGrossAmount = SalesInvoiceLinesList.Sum(x => x.TransactionExchangeLineAmount) + SalesInvoiceLinesList.Sum(x => x.TransactionExchangeDiscountAmount),
                        TransactionExchangeNetAmount = SalesInvoiceLinesList.Sum(x => x.TransactionExchangeLineTotalAmount),
                        TransactionExchangeTotalDiscountAmount = SalesInvoiceLinesList.Sum(x => x.TransactionExchangeDiscountAmount),
                        TransactionExchangeTotalVatAmount = SalesInvoiceLinesList.Sum(x => x.TransactionExchangeVATamount),
                        TransactionExchangeTotalVatExcludedAmount = (SalesInvoiceLinesList.Sum(x => x.TransactionExchangeLineAmount) + SalesInvoiceLinesList.Sum(x => x.TransactionExchangeDiscountAmount)) - SalesInvoiceLinesList.Sum(x => x.TransactionExchangeDiscountAmount),
                        WarehouseID = DefaultWarehouseID,
                        WorkOrderCreationDate = null,
                    };

                    createSalesInvoiceModel.SelectSalesInvoiceLines = SalesInvoiceLinesList;

                    var salesInvoiceResult = (await SalesInvoicesAppService.CreateAsync(createSalesInvoiceModel)).Data;
                    #endregion

                    #region Stok Çıkış Fişi Create İşlemi
                    SelectSalesInvoiceDto createdSalesInvoiceDataSource = (await SalesInvoicesAppService.GetAsync(salesInvoiceResult.Id)).Data;

                    if (createdSalesInvoiceDataSource != null && createdSalesInvoiceDataSource.Id != Guid.Empty && createdSalesInvoiceDataSource.SelectSalesInvoiceLines.Count > 0)
                    {
                        foreach (var invoiceLine in createdSalesInvoiceDataSource.SelectSalesInvoiceLines)
                        {
                            SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                            {
                                Date_ = now.Date,
                                FicheType = StockFicheTypeEnum.StokCikisFisi,
                                InputOutputCode = 1,
                                LineAmount = invoiceLine.LineAmount,
                                LineDescription = string.Empty,
                                LineNr = StockFicheLinesList.Count + 1,
                                MRPID = Guid.Empty,
                                MRPLineID = Guid.Empty,
                                PartyNo = string.Empty,
                                ProductID = invoiceLine.ProductID,
                                ProductCode = invoiceLine.ProductCode,
                                ProductionDateReferenceID = Guid.Empty,
                                ProductionOrderID = Guid.Empty,
                                ProductName = invoiceLine.ProductName,
                                PurchaseOrderID = Guid.Empty,
                                PurchaseOrderLineID = Guid.Empty,
                                Quantity = invoiceLine.Quantity,
                                TransactionExchangeLineAmount = invoiceLine.TransactionExchangeLineAmount,
                                TransactionExchangeUnitPrice = invoiceLine.TransactionExchangeUnitPrice,
                                UnitOutputCost = 0,
                                UnitPrice = invoiceLine.UnitPrice,
                                UnitSetCode = invoiceLine.UnitSetCode,
                                UnitSetID = invoiceLine.UnitSetID,
                                PurchaseInvoiceID = Guid.Empty,
                                PurchaseInvoiceLineID = Guid.Empty,
                                SalesInvoiceID = createdSalesInvoiceDataSource.Id,
                                SalesInvoiceLineID = invoiceLine.Id,
                            };

                            StockFicheLinesList.Add(stockFicheLineModel);
                        }
                    }

                    CreateStockFichesDto createStockFicheModel = new CreateStockFichesDto
                    {
                        BranchID = DefaultBranchID,
                        CurrencyID = Guid.Empty,
                        Date_ = now.Date,
                        Description_ = string.Empty,
                        ExchangeRate = 0,
                        FicheType = 51,
                        InputOutputCode = 1,
                        ProductionOrderID = Guid.Empty,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                        NetAmount = StockFicheLinesList.Sum(x => x.LineAmount),
                        ProductionDateReferenceID = Guid.Empty,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseRequestID = Guid.Empty,
                        SpecialCode = string.Empty,
                        Time_ = now.TimeOfDay,
                        TransactionExchangeCurrencyID = Guid.Empty,
                        WarehouseID = DefaultWarehouseID,
                        SalesInvoiceID = salesInvoiceResult.Id,
                        PurchaseInvoiceID = Guid.Empty
                    };

                    createStockFicheModel.SelectStockFicheLines = StockFicheLinesList;

                    await StockFichesAppService.CreateAsync(createStockFicheModel);
                    #endregion


                    #region Çeki Listesi Durumunu Sevk Edildi Yapma
                    DataSource.PackingListState = PackingListStateEnum.SevkEdildi;

                    var updatePackingInput = ObjectMapper.Map<SelectPackingListsDto, UpdatePackingListsDto>(DataSource);

                    await PackingListsAppService.UpdateAsync(updatePackingInput);
                    #endregion

                    SpinnerService.Hide();
                    await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessageApproveMessage"]);
                    await InvokeAsync(StateHasChanged);
                    #endregion
                }

                else
                {
                    #region SATIŞ FATURA OLUŞACAK AMA BİRİM FİYATLAR SİPARİŞTEN GELMEYECEK
                    List<SelectSalesInvoiceLinesDto> SalesInvoiceLinesList = new List<SelectSalesInvoiceLinesDto>();

                    List<SelectStockFicheLinesDto> StockFicheLinesList = new List<SelectStockFicheLinesDto>();

                    foreach (var item in GridLinePalletPackageList)
                    {
                        var salesOrder = (await SalesOrdersAppService.GetAsync(item.SalesOrderID.GetValueOrDefault())).Data;

                        if (salesOrder != null && salesOrder.Id != Guid.Empty)
                        {
                            #region Sales Order Line Sevk Edildi Update

                            var salesOrderLine = salesOrder.SelectSalesOrderLines.Where(t => t.Id == item.SalesOrderLineID.GetValueOrDefault()).FirstOrDefault();
                            int lineIndex = salesOrder.SelectSalesOrderLines.IndexOf(salesOrderLine);
                            salesOrder.SelectSalesOrderLines[lineIndex].SalesOrderLineStateEnum = SalesOrderLineStateEnum.SevkEdildi;

                            var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(salesOrder);

                            await SalesOrdersAppService.UpdateAsync(updateInput);
                            #endregion

                            #region Sales Invoice Line Oluşturma
                            SelectSalesInvoiceLinesDto salesInvoiceLineModel = new SelectSalesInvoiceLinesDto
                            {
                                BranchCode = salesOrderLine.BranchCode,
                                BranchID = salesOrderLine.BranchID,
                                BranchName = salesOrderLine.BranchName,
                                CurrentAccountCardCode = salesOrderLine.CurrentAccountCardCode,
                                CurrentAccountCardID = salesOrderLine.CurrentAccountCardID,
                                CurrentAccountCardName = salesOrderLine.CurrentAccountCardName,
                                DiscountAmount = 0,
                                DiscountRate = 0,
                                ExchangeRate = 0,
                                LikedPropositionLineID = salesOrderLine.LikedPropositionLineID,
                                LineAmount = 0,
                                LineDescription = salesOrderLine.LineDescription,
                                LineNr = SalesInvoiceLinesList.Count + 1,
                                Date_ = now.Date,
                                LineTotalAmount = 0,
                                LinkedSalesPropositionID = salesOrderLine.LinkedSalesPropositionID,
                                OrderAcceptanceRecordID = salesOrderLine.OrderAcceptanceRecordID,
                                OrderAcceptanceRecordLineID = salesOrderLine.OrderAcceptanceRecordLineID,
                                PaymentPlanID = salesOrderLine.PaymentPlanID,
                                PaymentPlanName = salesOrderLine.PaymentPlanName,
                                ProductCode = salesOrderLine.ProductCode,
                                ProductGroupID = salesOrderLine.ProductGroupID,
                                ProductGroupName = salesOrderLine.ProductGroupName,
                                ProductID = salesOrderLine.ProductID,
                                ProductName = salesOrderLine.ProductName,
                                Quantity = salesOrderLine.Quantity,
                                TransactionExchangeDiscountAmount = 0,
                                TransactionExchangeLineAmount = 0,
                                TransactionExchangeLineTotalAmount = 0,
                                TransactionExchangeUnitPrice = 0,
                                TransactionExchangeVATamount = 0,
                                UnitPrice = 0,
                                UnitSetCode = salesOrderLine.UnitSetCode,
                                UnitSetID = salesOrderLine.UnitSetID,
                                VATamount = 0,
                                VATrate = 0,
                                PurchaseSupplyDate = salesOrderLine.PurchaseSupplyDate,
                                WarehouseCode = salesOrderLine.WarehouseCode,
                                WarehouseID = salesOrderLine.WarehouseID,
                                WarehouseName = salesOrderLine.WarehouseName,
                            };

                            SalesInvoiceLinesList.Add(salesInvoiceLineModel);
                            #endregion
                        }

                        #region Üretim Emri ve Bağlı Üretim Emrinin Durumunu Sevk Edildi Yapma
                        var productionOrder = (await ProductionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;

                        if (productionOrder != null && productionOrder.Id != Guid.Empty)
                        {
                            productionOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                            var updateProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                            await ProductionOrdersAppService.UpdateAsync(updateProdOrderInput);

                            var linkedProdOrders = (await ProductionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data.ToList();

                            if (linkedProdOrders != null && linkedProdOrders.Count > 0)
                            {
                                foreach (var linkedProdOrder in linkedProdOrders)
                                {
                                    linkedProdOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                                    var updateLinkedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(linkedProdOrder);

                                    await ProductionOrdersAppService.UpdateAsync(updateLinkedProdOrderInput);
                                }
                            }
                        }
                        #endregion
                    }

                    #region Sales Invoice Create İşlemi
                    CreateSalesInvoiceDto createSalesInvoiceModel = new CreateSalesInvoiceDto
                    {
                        BranchID = DefaultBranchID,
                        ConfirmedLoadingDate = DataSource.LoadingDate,
                        CurrencyID = Guid.Empty,
                        CurrentAccountCardID = DataSource.RecieverID,
                        Description_ = string.Empty,
                        CustomerOrderNr = string.Empty,
                        CustomerRequestedDate = null,
                        Date_ = now.Date,
                        ExchangeRate = 1,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesInvoicesChildMenu"),
                        GrossAmount = 0,
                        isStandart = false,
                        LinkedSalesPropositionID = Guid.Empty,
                        NetAmount = 0,
                        OrderAcceptanceRecordID = Guid.Empty,
                        PaymentPlanID = Guid.Empty,
                        PricingCurrency = 1,
                        ShippingAdressID = DataSource.ShippingAddressID,
                        SpecialCode = string.Empty,
                        Time_ = string.Empty,
                        TotalDiscountAmount = 0,
                        TotalVatAmount = 0,
                        TotalVatExcludedAmount = 0,
                        TransactionExchangeCurrencyID = Guid.Empty,
                        TransactionExchangeGrossAmount = 0,
                        TransactionExchangeNetAmount = 0,
                        TransactionExchangeTotalDiscountAmount = 0,
                        TransactionExchangeTotalVatAmount = 0,
                        TransactionExchangeTotalVatExcludedAmount = 0,
                        WarehouseID = DefaultWarehouseID,
                        WorkOrderCreationDate = null,
                    };

                    createSalesInvoiceModel.SelectSalesInvoiceLines = SalesInvoiceLinesList;

                    var salesInvoiceResult = (await SalesInvoicesAppService.CreateAsync(createSalesInvoiceModel)).Data;
                    #endregion

                    #region Stok Çıkış Fişi Create İşlemi
                    SelectSalesInvoiceDto createdSalesInvoiceDataSource = (await SalesInvoicesAppService.GetAsync(salesInvoiceResult.Id)).Data;

                    if (createdSalesInvoiceDataSource != null && createdSalesInvoiceDataSource.Id != Guid.Empty && createdSalesInvoiceDataSource.SelectSalesInvoiceLines.Count > 0)
                    {
                        foreach (var invoiceLine in createdSalesInvoiceDataSource.SelectSalesInvoiceLines)
                        {
                            SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                            {
                                Date_ = now.Date,
                                FicheType = StockFicheTypeEnum.StokCikisFisi,
                                InputOutputCode = 1,
                                LineAmount = 0,
                                LineDescription = string.Empty,
                                LineNr = StockFicheLinesList.Count + 1,
                                MRPID = Guid.Empty,
                                MRPLineID = Guid.Empty,
                                PartyNo = string.Empty,
                                ProductID = invoiceLine.ProductID,
                                ProductCode = invoiceLine.ProductCode,
                                ProductionDateReferenceID = Guid.Empty,
                                ProductionOrderID = Guid.Empty,
                                ProductName = invoiceLine.ProductName,
                                PurchaseOrderID = Guid.Empty,
                                PurchaseOrderLineID = Guid.Empty,
                                Quantity = invoiceLine.Quantity,
                                TransactionExchangeLineAmount = 0,
                                TransactionExchangeUnitPrice = 0,
                                UnitOutputCost = 0,
                                UnitPrice = invoiceLine.UnitPrice,
                                UnitSetCode = invoiceLine.UnitSetCode,
                                UnitSetID = invoiceLine.UnitSetID,
                                PurchaseInvoiceID = Guid.Empty,
                                PurchaseInvoiceLineID = Guid.Empty,
                                SalesInvoiceID = createdSalesInvoiceDataSource.Id,
                                SalesInvoiceLineID = invoiceLine.Id,
                            };

                            StockFicheLinesList.Add(stockFicheLineModel);
                        }
                    }

                    CreateStockFichesDto createStockFicheModel = new CreateStockFichesDto
                    {
                        BranchID = DefaultBranchID,
                        CurrencyID = Guid.Empty,
                        Date_ = now.Date,
                        Description_ = string.Empty,
                        ExchangeRate = 0,
                        FicheType = 51,
                        InputOutputCode = 1,
                        ProductionOrderID = Guid.Empty,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                        NetAmount = StockFicheLinesList.Sum(x => x.LineAmount),
                        ProductionDateReferenceID = Guid.Empty,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseRequestID = Guid.Empty,
                        SpecialCode = string.Empty,
                        Time_ = now.TimeOfDay,
                        TransactionExchangeCurrencyID = Guid.Empty,
                        WarehouseID = DefaultWarehouseID,
                        SalesInvoiceID = salesInvoiceResult.Id,
                        PurchaseInvoiceID = Guid.Empty
                    };

                    createStockFicheModel.SelectStockFicheLines = StockFicheLinesList;

                    await StockFichesAppService.CreateAsync(createStockFicheModel);
                    #endregion


                    #region Çeki Listesi Durumunu Sevk Edildi Yapma
                    DataSource.PackingListState = PackingListStateEnum.SevkEdildi;

                    var updatePackingInput = ObjectMapper.Map<SelectPackingListsDto, UpdatePackingListsDto>(DataSource);

                    await PackingListsAppService.UpdateAsync(updatePackingInput);
                    #endregion

                    SpinnerService.Hide();
                    await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessageApproveMessage"]);
                    await InvokeAsync(StateHasChanged);
                    #endregion
                }

                HideApproveModal();
            }
            else
            {
                #region SATIŞ FATURA OLUŞMAYACAK

                List<SelectStockFicheLinesDto> StockFicheLinesList = new List<SelectStockFicheLinesDto>();

                foreach (var item in GridLinePalletPackageList)
                {
                    var salesOrder = (await SalesOrdersAppService.GetAsync(item.SalesOrderID.GetValueOrDefault())).Data;

                    if (salesOrder != null && salesOrder.Id != Guid.Empty)
                    {
                        #region Sales Order Line Sevk Edildi Update

                        var salesOrderLine = salesOrder.SelectSalesOrderLines.Where(t => t.Id == item.SalesOrderLineID.GetValueOrDefault()).FirstOrDefault();
                        int lineIndex = salesOrder.SelectSalesOrderLines.IndexOf(salesOrderLine);
                        salesOrder.SelectSalesOrderLines[lineIndex].SalesOrderLineStateEnum = SalesOrderLineStateEnum.SevkEdildi;

                        var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(salesOrder);

                        await SalesOrdersAppService.UpdateAsync(updateInput);
                        #endregion

                        #region Stok Çıkış Fişi Line Oluşturma

                        SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            Date_ = now.Date,
                            FicheType = StockFicheTypeEnum.StokCikisFisi,
                            InputOutputCode = 1,
                            LineAmount = salesOrderLine.LineAmount,
                            LineDescription = string.Empty,
                            LineNr = StockFicheLinesList.Count + 1,
                            MRPID = Guid.Empty,
                            MRPLineID = Guid.Empty,
                            PartyNo = string.Empty,
                            ProductID = item.ProductID,
                            ProductCode = item.ProductCode,
                            ProductionDateReferenceID = Guid.Empty,
                            ProductionOrderID = Guid.Empty,
                            ProductName = item.ProductName,
                            PurchaseOrderID = Guid.Empty,
                            PurchaseOrderLineID = Guid.Empty,
                            Quantity = salesOrderLine.Quantity,
                            TransactionExchangeLineAmount = salesOrderLine.TransactionExchangeLineAmount,
                            TransactionExchangeUnitPrice = salesOrderLine.TransactionExchangeUnitPrice,
                            UnitOutputCost = 0,
                            UnitPrice = salesOrderLine.UnitPrice,
                            UnitSetCode = salesOrderLine.UnitSetCode,
                            UnitSetID = salesOrderLine.UnitSetID,
                            PurchaseInvoiceID = Guid.Empty,
                            PurchaseInvoiceLineID = Guid.Empty,
                            SalesInvoiceID = Guid.Empty,
                            SalesInvoiceLineID = Guid.Empty,
                        };

                        StockFicheLinesList.Add(stockFicheLineModel);

                        #endregion
                    }

                    #region Üretim Emri ve Bağlı Üretim Emrinin Durumunu Sevk Edildi Yapma
                    var productionOrder = (await ProductionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;

                    if (productionOrder != null && productionOrder.Id != Guid.Empty)
                    {
                        productionOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                        var updateProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                        await ProductionOrdersAppService.UpdateAsync(updateProdOrderInput);

                        var linkedProdOrders = (await ProductionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data.ToList();

                        if (linkedProdOrders != null && linkedProdOrders.Count > 0)
                        {
                            foreach (var linkedProdOrder in linkedProdOrders)
                            {
                                linkedProdOrder.ProductionOrderState = ProductionOrderStateEnum.SevkEdildi;

                                var updateLinkedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(linkedProdOrder);

                                await ProductionOrdersAppService.UpdateAsync(updateLinkedProdOrderInput);
                            }
                        }
                    }
                    #endregion
                }

                #region Stok Çıkış Fişi Create İşlemi

                CreateStockFichesDto createStockFicheModel = new CreateStockFichesDto
                {
                    BranchID = DefaultBranchID,
                    CurrencyID = Guid.Empty,
                    Date_ = now.Date,
                    Description_ = string.Empty,
                    ExchangeRate = 1,
                    FicheType = 51,
                    InputOutputCode = 1,
                    ProductionOrderID = Guid.Empty,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                    NetAmount = StockFicheLinesList.Sum(x => x.LineAmount),
                    ProductionDateReferenceID = Guid.Empty,
                    PurchaseOrderID = Guid.Empty,
                    PurchaseRequestID = Guid.Empty,
                    SpecialCode = string.Empty,
                    Time_ = now.TimeOfDay,
                    TransactionExchangeCurrencyID = Guid.Empty,
                    WarehouseID = DefaultWarehouseID,
                    SalesInvoiceID = Guid.Empty,
                    PurchaseInvoiceID = Guid.Empty
                };

                createStockFicheModel.SelectStockFicheLines = StockFicheLinesList;

                await StockFichesAppService.CreateAsync(createStockFicheModel);

                #endregion

                #region Çeki Listesi Durumunu Sevk Edildi Yapma
                DataSource.PackingListState = PackingListStateEnum.SevkEdildi;

                var updatePackingInput = ObjectMapper.Map<SelectPackingListsDto, UpdatePackingListsDto>(DataSource);

                await PackingListsAppService.UpdateAsync(updatePackingInput);
                #endregion

                SpinnerService.Hide();
                await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessageApproveMessage"]);
                await InvokeAsync(StateHasChanged);
                #endregion

                HideApproveModal();
            }

        }

        public void HideApproveModal()
        {
            ShowApproveModal = false;
        }

        private async void SalesInvoiceChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DisabledUnitPricesFromOrder = false;
                isSalesInvoce = true;
            }
            else
            {
                DisabledUnitPricesFromOrder = true;
                isUnitPricesFromOrder = false;
                isSalesInvoce = false;
            }
            await (InvokeAsync(StateHasChanged));
        }

        #endregion

        #region Buttonedit Metotları

        #region Şube ButtonEdit

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchOnCreateIcon()
        {
            var BranchButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DefaultBranchID = Guid.Empty;
                DefaultBranchCode = string.Empty;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {

                DefaultBranchID = selectedBranch.Id;
                DefaultBranchCode = selectedBranch.Code;

                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Depo ButtonEdit

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehouseOnCreateIcon()
        {
            var WarehouseButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehouseButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DefaultWarehouseID = Guid.Empty;
                DefaultWarehouseCode = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {

                DefaultWarehouseID = selectedWarehouse.Id;
                DefaultWarehouseCode = selectedWarehouse.Code;

                SelectWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Banka ButtonEdit

        SfTextBox BankAccountsButtonEdit;
        bool SelectBankAccountsPopupVisible = false;
        List<ListBankAccountsDto> BankAccountsList = new List<ListBankAccountsDto>();

        public async Task BankAccountOnCreateIcon()
        {
            var BankAccountButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BankAccountsButtonClickEvent);
            await BankAccountsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BankAccountButtonClick } });
        }

        public async void BankAccountsButtonClickEvent()
        {
            SelectBankAccountsPopupVisible = true;
            BankAccountsList = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void BankAccountsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BankID = Guid.Empty;
                DataSource.BankName = string.Empty;
            }
        }

        public async void BankAccountsDoubleClickHandler(RecordDoubleClickEventArgs<ListBankAccountsDto> args)
        {
            var selectedBankAccount = args.RowData;

            if (selectedBankAccount != null)
            {

                DataSource.BankID = selectedBankAccount.Id;
                DataSource.BankName = selectedBankAccount.Name;

                SelectBankAccountsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Gönderici ButtonEdit

        SfTextBox TransmittersButtonEdit;
        bool SelectTransmittersPopupVisible = false;
        List<ListCurrentAccountCardsDto> TransmittersList = new List<ListCurrentAccountCardsDto>();

        public async Task TransmitterOnCreateIcon()
        {
            var TransmitterButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TransmittersButtonClickEvent);
            await TransmittersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TransmitterButtonClick } });
        }

        public async void TransmittersButtonClickEvent()
        {
            SelectTransmittersPopupVisible = true;
            TransmittersList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void TransmittersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {

                DataSource.TransmitterID = Guid.Empty;
                DataSource.TransmitterCode = string.Empty;
                DataSource.TransmitterName = string.Empty;
                DataSource.TransmitterSupplierNo = string.Empty;
                DataSource.TransmitterEORINo = string.Empty;
                DataSource.RecieverCustomerCode = string.Empty;
                DataSource.TransmitterPaymentTermDay = 0;
            }
        }

        public async void TransmittersDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedTransmitter = args.RowData;

            if (selectedTransmitter != null)
            {

                DataSource.TransmitterID = selectedTransmitter.Id;
                DataSource.TransmitterCode = selectedTransmitter.Code;
                DataSource.TransmitterName = selectedTransmitter.Name;
                DataSource.TransmitterSupplierNo = selectedTransmitter.SupplierNo;
                DataSource.TransmitterEORINo = selectedTransmitter.EORINr;
                DataSource.RecieverCustomerCode = selectedTransmitter.CustomerCode;
                DataSource.TransmitterPaymentTermDay = selectedTransmitter.PaymentTermDay;

                SelectTransmittersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Gönderilen ButtonEdit

        SfTextBox RecieversButtonEdit;
        bool SelectRecieversPopupVisible = false;
        List<ListCurrentAccountCardsDto> RecieversList = new List<ListCurrentAccountCardsDto>();

        public async Task RecieverOnCreateIcon()
        {
            var RecieverButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, RecieversButtonClickEvent);
            await RecieversButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", RecieverButtonClick } });
        }

        public async void RecieversButtonClickEvent()
        {
            SelectRecieversPopupVisible = true;
            RecieversList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void RecieversOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.RecieverID = Guid.Empty;
                DataSource.ShippingAddressID = Guid.Empty;
                DataSource.RecieverCode = string.Empty;
                DataSource.RecieverName = string.Empty;
                DataSource.ShippingAddressAddress = string.Empty;
            }
        }

        public async void RecieversDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedReciever = args.RowData;

            if (selectedReciever != null)
            {
                DataSource.RecieverID = selectedReciever.Id;
                DataSource.RecieverCode = selectedReciever.Code;
                DataSource.RecieverName = selectedReciever.Name;
                DataSource.ShippingAddressAddress = selectedReciever.ShippingAddress;
                SelectRecieversPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        #region Combobox Metotları

        #region Satış Şekli Enum Combobox

        public IEnumerable<SelectPackingListsDto> salesTypes = GetEnumDisplaySalesTypesNames<PackingListSalesTypeEnum>();

        public static List<SelectPackingListsDto> GetEnumDisplaySalesTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PackingListSalesTypeEnum>()
                       .Select(x => new SelectPackingListsDto
                       {
                           SalesType = x,
                           SalesTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }


        #endregion

        #region TIR Şekli Enum Combobox

        public IEnumerable<SelectPackingListsDto> tIRTypes = GetEnumDisplayTIRTypesNames<PackingListTIRTypeEnum>();

        public static List<SelectPackingListsDto> GetEnumDisplayTIRTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PackingListTIRTypeEnum>()
                       .Select(x => new SelectPackingListsDto
                       {
                           TIRType = x,
                           TIRTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }


        #endregion

        #region Çeki Listesi Durum Enum Combobox

        public IEnumerable<SelectPackingListsDto> packingListStates = GetEnumDisplayStateNames<PackingListStateEnum>();

        public static List<SelectPackingListsDto> GetEnumDisplayStateNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PackingListStateEnum>()
                       .Select(x => new SelectPackingListsDto
                       {
                           PackingListState = x,
                           PackingListStateName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }


        #endregion 

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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PackingListsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Yazdır

        #region Gümrükleme Talimatı

        bool CustomsInstructionReportVisible { get; set; }

        DxReportViewer CustomsInstructionReportViewer { get; set; }

        XtraReport CustomsInstructionReport { get; set; }

        async Task CreateCustomsInstructionReport(SelectPackingListsDto packingList)
        {
            CustomsInstructionReport.ShowPrintMarginsWarning = false;
            CustomsInstructionReport.CreateDocument();

            #region Enum Combobox Localization

            DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

            DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

            DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

            #endregion

            if (packingList.Id != Guid.Empty)
            {
                int toplamPaletAdedi = packingList.SelectPackingListPalletCubageLines.Sum(t => t.NumberofPallet);
                string yetkili = packingList.CustomsOfficial;
                string teslimSekli = packingList.SalesTypeName;
                teslimSekli = teslimSekli.Substring(0, teslimSekli.Length - 3);
                DateTime teslimTarihi = packingList.DeliveryDate.GetValueOrDefault();
                DateTime faturaTarihi = packingList.BillDate.GetValueOrDefault();

                string ay = faturaTarihi.Month.ToString();

                if (ay.Length == 1)
                {
                    ay = "0" + ay;
                }

                string gun = faturaTarihi.Day.ToString();

                if (gun.Length == 1)
                {
                    gun = "0" + gun;
                }

                string yil = faturaTarihi.Year.ToString().Substring(2, faturaTarihi.Year.ToString().Length - 2);

                string refNo = yil + ay + gun + "/" + yil;

                var bank = (await BankAccountsAppService.GetAsync(packingList.BankID.GetValueOrDefault())).Data;

                CustomsInstructionReport customsInstructionReport = new CustomsInstructionReport();
                customsInstructionReport.RefNo.Text = refNo;
                customsInstructionReport.ToplamPaletAdedi.Text = toplamPaletAdedi.ToString();
                customsInstructionReport.Yetkili.Text = yetkili;
                customsInstructionReport.TeslimSekli.Text = teslimSekli;
                customsInstructionReport.Tarih.Text = faturaTarihi.ToShortDateString();
                customsInstructionReport.GumruklemeTarihi.Text = teslimTarihi.ToShortDateString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)teslimTarihi.DayOfWeek];
                customsInstructionReport.AraciBanka.Text = bank.BankInstructionDescription;
                customsInstructionReport.ShowPreviewMarginLines = false;
                customsInstructionReport.CreateDocument();

                CustomsInstructionReport.Pages.AddRange(customsInstructionReport.Pages);

                string PackListNo = packingList.Code2;

                List<string> siraListe = new List<string>()
                        {
                            "VİRAJ ROTU",
                            "ROT MİLİ",
                            "BURÇ",
                            "OYNAR BURÇ",
                            "ROTİL",
                            "ROT BAŞI",
                            "YAN ROT",
                            "BURÇ TAKIMI",
                            "DENGE KOLU",
                            "MONTAJ TAKIMI",
                            "CİVATA TAKIMI",
                            "BASKILI TORBA, PE",
                            "DEBRİYAJ CIRCIRI",
                            "ŞAFT ASKISI",
                            "AMORTİSÖR YAYI",
                            "SOMUN",
                            "BOŞ KOLİ",
                            "TAMİR TAKIMI"
                        };

                var satirlar = packingList.SelectPackingListPalletPackageLines;

                var malCinsileri = satirlar.Select(t => t.ProductGroupName).Distinct().ToList();

                List<FaxMessage> faksMesaji = new List<FaxMessage>();
                faksMesaji.Add(new FaxMessage()
                {
                    PackingListNo = "",
                    FaxMessageLines = new List<FaxMessageLines>(),
                    FaxMessageGTIPTable = new List<FaxMessageGTIPTable>()
                });

                for (int i = 0; i < siraListe.Count; i++)
                {
                    string baslik = siraListe[i];

                    for (int j = 0; j < malCinsileri.Count; j++)
                    {
                        string malCinsi = malCinsileri[j];

                        if (baslik == malCinsi)
                        {
                            FaxMessageLines satir = new FaxMessageLines
                            {
                                MalCinsiTurkce = malCinsi,
                                MalCinsiIngilizce = MalCinsiIngilizce(malCinsi),
                                TutarDagilimiEuro = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount * t.TransactionExchangeUnitPrice),
                                AdetDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount),
                                NetKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalNetKG),
                                BrutKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalGrossKG),
                                KoliSayisi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.NumberofPackage),
                                CesitDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Select(t => t.ProductID).Distinct().Count()
                            };

                            faksMesaji[0].FaxMessageLines.Add(satir);

                            faksMesaji[0].FaxMessageGTIPTable.Add(new FaxMessageGTIPTable
                            {
                                MalCinsiTurkce = malCinsi,
                                MalCinsiIngilizce = MalCinsiIngilizce(malCinsi),
                                GtipKodu = (await ProductGroupsAppService.GetByNameAsync(malCinsi)).Data.Name
                            });
                        }
                    }
                }

                faksMesaji[0].PackingListNo = PackListNo;


                CustomsInstructionFaxMessage report = new CustomsInstructionFaxMessage();

                report.FillDataSource();
                report.DataSource = faksMesaji;
                report.ShowPreviewMarginLines = false;

                XRSubreport subReport = report.FindControl("xrSubreport1", true) as XRSubreport;
                XtraReport reportSource = subReport.ReportSource as XtraReport;
                (reportSource.DataSource as ObjectDataSource).DataSource = faksMesaji[0].FaxMessageGTIPTable;
                reportSource.CreateDocument();


                report.CreateDocument();
                CustomsInstructionReport.Pages.AddRange(report.Pages);

                CustomsInstructionReport.PrintingSystem.ContinuousPageNumbering = true;
            }

            await Task.CompletedTask;
        }

        #endregion

        #region Nakliye Talimatı
        bool ShippingInstructionReportVisible { get; set; }

        DxReportViewer ShippingInstructionReportViewer { get; set; }

        XtraReport ShippingInstructionReport { get; set; }

        async Task CreateShippingInstructionReport(SelectPackingListsDto packingList)
        {
            ShippingInstructionReport.ShowPrintMarginsWarning = false;
            ShippingInstructionReport.CreateDocument();

            #region Enum Combobox Localization

            DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

            DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

            DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

            #endregion

            if (packingList.Id != Guid.Empty)
            {
                int toplamPaletAdedi = packingList.SelectPackingListPalletCubageLines.Sum(t => t.NumberofPallet);
                string yetkili = packingList.CustomsOfficial;
                string teslimSekli = packingList.SalesTypeName;
                teslimSekli = teslimSekli.Substring(0, teslimSekli.Length - 3);
                DateTime teslimTarihi = packingList.DeliveryDate.GetValueOrDefault();
                DateTime faturaTarihi = packingList.BillDate.GetValueOrDefault();

                string ay = faturaTarihi.Month.ToString();

                if (ay.Length == 1)
                {
                    ay = "0" + ay;
                }

                string gun = faturaTarihi.Day.ToString();

                if (gun.Length == 1)
                {
                    gun = "0" + gun;
                }

                string yil = faturaTarihi.Year.ToString().Substring(2, faturaTarihi.Year.ToString().Length - 2);

                string refNo = yil + ay + gun + "/" + yil;

                var bank = (await BankAccountsAppService.GetAsync(packingList.BankID.GetValueOrDefault())).Data;

                ShippingInstructionReport shippingInstructionReport = new ShippingInstructionReport();
                shippingInstructionReport.RefNo.Text = refNo;
                shippingInstructionReport.ToplamPaletAdedi.Text = toplamPaletAdedi.ToString();
                shippingInstructionReport.Yetkili.Text = yetkili;
                shippingInstructionReport.TeslimSekli.Text = teslimSekli;
                shippingInstructionReport.Tarih.Text = faturaTarihi.ToShortDateString();
                shippingInstructionReport.GumruklemeTarihi.Text = teslimTarihi.ToShortDateString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)teslimTarihi.DayOfWeek];
                shippingInstructionReport.AraciBanka.Text = bank.BankInstructionDescription;
                shippingInstructionReport.ShowPreviewMarginLines = false;
                shippingInstructionReport.CreateDocument();

                ShippingInstructionReport.Pages.AddRange(shippingInstructionReport.Pages);




                string PackListNo = packingList.Code2;

                List<string> siraListe = new List<string>()
                        {
                            "VİRAJ ROTU",
                            "ROT MİLİ",
                            "BURÇ",
                            "OYNAR BURÇ",
                            "ROTİL",
                            "ROT BAŞI",
                            "YAN ROT",
                            "BURÇ TAKIMI",
                            "DENGE KOLU",
                            "MONTAJ TAKIMI",
                            "CİVATA TAKIMI",
                            "BASKILI TORBA, PE",
                            "DEBRİYAJ CIRCIRI",
                            "ŞAFT ASKISI",
                            "AMORTİSÖR YAYI",
                            "SOMUN",
                            "BOŞ KOLİ",
                            "TAMİR TAKIMI"
                        };

                var satirlar = packingList.SelectPackingListPalletPackageLines;

                var malCinsileri = satirlar.Select(t => t.ProductGroupName).Distinct().ToList();

                List<FaxMessageLines> faksMesajiSatirlar = new List<FaxMessageLines>();

                for (int i = 0; i < siraListe.Count; i++)
                {
                    string baslik = siraListe[i];

                    for (int j = 0; j < malCinsileri.Count; j++)
                    {
                        string malCinsi = malCinsileri[j];

                        if (baslik == malCinsi)
                        {
                            FaxMessageLines satir = new FaxMessageLines
                            {
                                MalCinsiTurkce = malCinsi,
                                MalCinsiIngilizce = MalCinsiIngilizce(malCinsi),
                                TutarDagilimiEuro = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount * t.TransactionExchangeUnitPrice),
                                AdetDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount),
                                NetKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalNetKG),
                                BrutKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalGrossKG),
                                KoliSayisi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.NumberofPackage),
                                CesitDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Select(t => t.ProductID).Distinct().Count()
                            };

                            faksMesajiSatirlar.Add(satir);


                        }
                    }
                }



                List<NakliyePaletTablosu> paletListesi = new List<NakliyePaletTablosu>();

                for (int i = 0; i < packingList.SelectPackingListPalletCubageLines.Count; i++)
                {
                    NakliyePaletTablosu palet = new NakliyePaletTablosu
                    {
                        PaletAdedi = packingList.SelectPackingListPalletCubageLines[i].NumberofPallet,
                        Boy = packingList.SelectPackingListPalletCubageLines[i].Height_,
                        En = packingList.SelectPackingListPalletCubageLines[i].Width_,
                        ToplamM3 = packingList.SelectPackingListPalletCubageLines[i].Cubage,
                        Yuk = packingList.SelectPackingListPalletCubageLines[i].Load_
                    };

                    paletListesi.Add(palet);
                }

                ShippingInstructionSecondPageReport report2 = new ShippingInstructionSecondPageReport();
                report2.ShowPreviewMarginLines = false;
                report2.CreateDocument();

                XRSubreport subReport1 = report2.FindControl("xrSubreport1", true) as XRSubreport;
                XtraReport reportSource1 = subReport1.ReportSource as XtraReport;
                (reportSource1.DataSource as ObjectDataSource).DataSource = paletListesi;
                reportSource1.CreateDocument();

                XRSubreport subReport2 = report2.FindControl("xrSubreport2", true) as XRSubreport;
                XtraReport reportSource2 = subReport2.ReportSource as XtraReport;
                (reportSource2.DataSource as ObjectDataSource).DataSource = faksMesajiSatirlar;
                reportSource2.CreateDocument();

                report2.CreateDocument();

                ShippingInstructionReport.Pages.AddRange(report2.Pages);

                ShippingInstructionReport.PrintingSystem.ContinuousPageNumbering = true;

            }

            await Task.CompletedTask;
        }


        #endregion

        #region Yükleme Teyidi
        bool UploadConfirmationReportVisible { get; set; }

        DxReportViewer UploadConfirmationReportViewer { get; set; }

        XtraReport UploadConfirmationDynamicReport { get; set; }

        async Task CreateUploadConfirmationReport(SelectPackingListsDto packingList)
        {
            UploadConfirmationDynamicReport.ShowPrintMarginsWarning = false;
            UploadConfirmationDynamicReport.ShowPreviewMarginLines = false;
            UploadConfirmationDynamicReport.CreateDocument();

            #region Enum Combobox Localization

            DataSource.SalesTypeName = L[salesTypes.Where(t => t.SalesType == DataSource.SalesType).Select(t => t.SalesTypeName).FirstOrDefault()];

            DataSource.TIRTypeName = L[tIRTypes.Where(t => t.TIRType == DataSource.TIRType).Select(t => t.TIRTypeName).FirstOrDefault()];

            DataSource.PackingListStateName = L[packingListStates.Where(t => t.PackingListState == DataSource.PackingListState).Select(t => t.PackingListStateName).FirstOrDefault()];

            #endregion

            if (packingList.Id != Guid.Empty)
            {
                int toplamPaletAdedi = packingList.SelectPackingListPalletCubageLines.Sum(t => t.NumberofPallet);

                string teslimSekli = packingList.SalesTypeName;
                teslimSekli = teslimSekli.Substring(0, teslimSekli.Length - 3);
                DateTime teslimTarihi = packingList.DeliveryDate.GetValueOrDefault();
                DateTime faturaTarihi = packingList.BillDate.GetValueOrDefault();

                string tirSekli = packingList.TIRTypeName;

                string ay = faturaTarihi.Month.ToString();

                if (ay.Length == 1)
                {
                    ay = "0" + ay;
                }

                string gun = faturaTarihi.Day.ToString();

                if (gun.Length == 1)
                {
                    gun = "0" + gun;
                }

                string yil = faturaTarihi.Year.ToString().Substring(2, faturaTarihi.Year.ToString().Length - 2);

                string refNo = "A" + ay + gun + "/" + yil;

                List<UploadConfirmationReportDto> reportSource = new List<UploadConfirmationReportDto>();
                reportSource.Add(new UploadConfirmationReportDto
                {
                    RefNo = refNo,
                    ToplamPaletAdedi = toplamPaletAdedi.ToString(),
                    Tarih = faturaTarihi.ToShortDateString(),
                    TeslimTarihi = teslimTarihi.ToShortDateString(),
                    TirSekli = tirSekli
                });


                string PackListNo = packingList.Code2;

                List<string> siraListe = new List<string>()
                        {
                            "VİRAJ ROTU",
                            "ROT MİLİ",
                            "BURÇ",
                            "OYNAR BURÇ",
                            "ROTİL",
                            "ROT BAŞI",
                            "YAN ROT",
                            "BURÇ TAKIMI",
                            "DENGE KOLU",
                            "MONTAJ TAKIMI",
                            "CİVATA TAKIMI",
                            "BASKILI TORBA, PE",
                            "DEBRİYAJ CIRCIRI",
                            "ŞAFT ASKISI",
                            "AMORTİSÖR YAYI",
                            "SOMUN",
                            "BOŞ KOLİ",
                            "TAMİR TAKIMI"
                        };

                var satirlar = packingList.SelectPackingListPalletPackageLines;

                var malCinsileri = satirlar.Select(t => t.ProductGroupName).Distinct().ToList();


                for (int i = 0; i < siraListe.Count; i++)
                {
                    string baslik = siraListe[i];

                    for (int j = 0; j < malCinsileri.Count; j++)
                    {
                        string malCinsi = malCinsileri[j];

                        if (baslik == malCinsi)
                        {
                            FaxMessageLines satir = new FaxMessageLines
                            {
                                MalCinsiTurkce = malCinsi,
                                MalCinsiIngilizce = MalCinsiIngilizce(malCinsi),
                                TutarDagilimiEuro = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount * t.TransactionExchangeUnitPrice),
                                AdetDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalAmount),
                                NetKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalNetKG),
                                BrutKgDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.TotalGrossKG),
                                KoliSayisi = satirlar.Where(t => t.ProductGroupName == malCinsi).Sum(t => t.NumberofPackage),
                                CesitDagilimi = satirlar.Where(t => t.ProductGroupName == malCinsi).Select(t => t.ProductID).Distinct().Count()
                            };

                            reportSource[0].FaxMessageLines.Add(satir);
                        }
                    }
                }
                UploadConfirmationReport report = new UploadConfirmationReport();
                report.DataSource = reportSource;
                report.CreateDocument();

                UploadConfirmationDynamicReport.Pages.AddRange(report.Pages);
                UploadConfirmationDynamicReport.PrintingSystem.ContinuousPageNumbering = true;

            }

            await Task.CompletedTask;
        }


        #endregion

        #region Commercial Invoice
        bool CommercialInvoiceReportVisible { get; set; }

        DxReportViewer CommercialInvoiceReportViewer { get; set; }

        XtraReport CommercialInvoiceDynamicReport { get; set; }


        async Task CreateCommercialInvoiceReport(SelectPackingListsDto packingList)
        {
            //var list = (await PackingListsAppService.GetCommercialInvoiceReportDataSource(packingList));

            //var bank = (await BankAccountsAppService.GetAsync(packingList.BankID.GetValueOrDefault())).Data;

            //if (list.Count > 0)
            //{
            //    CommercialInvoiceDynamicReport.ShowPrintMarginsWarning = false;
            //    CommercialInvoiceDynamicReport.ShowPreviewMarginLines = false;
            //    CommercialInvoiceDynamicReport.CreateDocument();

            //    bool exWorks = false;

            //    if (list[0].SatisSekli == "EX-WORKS ISTANBUL (€)")
            //    {
            //        exWorks = true;
            //    }

            //    list[0].BankName = bank.Name;
            //    list[0].BankBranch = bank.BankBranchName;
            //    list[0].EuroAccNr = bank.EuroAccountNo;
            //    list[0].EuroAccIbanNo = bank.EuroAccountIBAN;
            //    list[0].UsdAccNr = bank.USDAccountNo;
            //    list[0].UsdAccIbanNo = bank.USDAccountIBAN;
            //    list[0].TlAccNr = bank.TLAccountNo;
            //    list[0].TlAccIbanNo = bank.TLAccountIBAN;
            //    list[0].GbpAccNr = bank.GBPAccountNo;
            //    list[0].GbpAccIbanNo = bank.GBPAccountIBAN;
            //    list[0].SwiftKodu = bank.SWIFTCode;

            //    CommercialInvoiceReport report = new CommercialInvoiceReport();
            //    report.DataSource = list;
            //    report.ShowPrintMarginsWarning = false;
            //    decimal toplam = list.Sum(t => t.ToplamTutar);
            //    report.lblNetToplamYazi.Text = PackingListsAppService.NumberToWords((double)toplam);
            //    report.TLRow.Visible = !string.IsNullOrEmpty(bank.TLAccountNo) ? true : false;
            //    report.GBPRow.Visible = !string.IsNullOrEmpty(bank.GBPAccountNo) ? true : false;
            //    report.DeliveryAddressLabel.Visible = exWorks ? true : false;
            //    report.DeliveryAddress.Visible = exWorks ? true : false;
            //    report.CreateDocument();
            //    CommercialInvoiceDynamicReport.Pages.AddRange(report.Pages);
            //    CommercialInvoiceDynamicReport.PrintingSystem.ContinuousPageNumbering = true;
            //}

            await Task.CompletedTask;
        }


        #endregion

        #region Çeki Listesi
        bool PackingListReportVisible { get; set; }

        DxReportViewer PackingListReportViewer { get; set; }

        XtraReport PackingListDynamicReport { get; set; }

        async Task CreateTRPackingListReport(SelectPackingListsDto packingList)
        {
            PackingListDynamicReport.ShowPrintMarginsWarning = false;
            PackingListDynamicReport.ShowPreviewMarginLines = false;
            PackingListDynamicReport.CreateDocument();

            List<PackingListReportDto> reportSource = new List<PackingListReportDto>();

            bool exWorks = false;

            if (packingList.SalesTypeName == "EX-WORKS ISTANBUL (€)")
            {
                exWorks = true;
            }

            Guid recieverCompanyId = packingList.RecieverID.GetValueOrDefault();
            var recieverCurrentAccountCard = (await CurrentAccountCardsAppService.GetAsync(recieverCompanyId)).Data;
            string shippingAddress = recieverCurrentAccountCard.ShippingAddress;

            foreach (var item in packingList.SelectPackingListPalletPackageLines)
            {
                string malzemeTanimi = item.ProductName;
                string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(item.ProductID.GetValueOrDefault(), packingList.TransmitterID.GetValueOrDefault());

                malzemeTanimi = malzemeTanimi + supplierReferanceNumber + "/049/" + item.CustomerOrderNr;

                PackingListReportDto report = new PackingListReportDto();
                report.EoriNr = packingList.TransmitterEORINo;
                report.FaturaNo = packingList.BillNo;
                report.CekiListesiNo = packingList.Code;
                report.FaturaTarihi = packingList.BillDate.Value.ToShortDateString();
                report.SiparisNo = packingList.OrderNo;
                report.AliciUnvan = packingList.TransmitterName;
                report.AliciAdres1 = packingList.TransmitterAddress1;
                report.AliciAdres2 = packingList.TransmitterAddress2;
                report.AliciTel = packingList.TransmitterTel;
                report.AliciFax = packingList.TransmitterFax;
                report.PaketNo = item.PackageNo;
                report.MalzemeTanimi = malzemeTanimi;
                report.PaketCinsi = item.PackageType;
                report.PaketIcerigi = item.PackageContent;
                report.PaketSayisi = item.NumberofPackage;
                report.ToplamAdet = item.TotalAmount;
                report.KoliNetKg = item.OnePackageNetKG;
                report.KoliBrutKg = item.OnePackageGrossKG;
                report.KoliToplamNetKg = item.TotalNetKG;
                report.KoliToplamBrutKg = item.TotalGrossKG;
                report.ToplamPaletAdedi = packingList.SelectPackingListPalletCubageLines.Sum(t => t.NumberofPallet);
                report.ToplamHacim = packingList.SelectPackingListPalletCubageLines.Sum(t => t.Cubage);
                report.SevkiyatAdresi = shippingAddress;

                reportSource.Add(report);
            }


            List<PackingListPalletQuantityReportDto> kubajList = new List<PackingListPalletQuantityReportDto>();

            foreach (var item in packingList.SelectPackingListPalletCubageLines)
            {
                PackingListPalletQuantityReportDto kubaj = new PackingListPalletQuantityReportDto();
                kubaj.Kubaj = item.Cubage;
                kubaj.Yukseklik = item.Load_;
                kubaj.PaletAdet = item.NumberofPallet;
                kubaj.Boy = item.Height_;
                kubaj.En = item.Width_;
                kubajList.Add(kubaj);
            }

            List<PackingListPalletDetailReportDto> paletList = new List<PackingListPalletDetailReportDto>();

            int packageNo = 1;

            int palletIndex = 0;

            foreach (var item in packingList.SelectPackingListPalletLines)
            {
                if (packageNo == 1)
                {
                    PackingListPalletDetailReportDto palet = new PackingListPalletDetailReportDto();
                    palet.IlkKoliNo = packageNo.ToString();
                    palet.SonKoliNo = (packageNo + item.NumberofPackage - 1).ToString();
                    palet.KoliSayisi = item.NumberofPackage;
                    palet.PaletAdi = item.PalletName.Split("-")[0];

                    packageNo += item.NumberofPackage;

                    paletList.Add(palet);
                    palletIndex = palletIndex + 1;
                }
                else
                {
                    string firstPackageNo = Convert.ToString(Convert.ToInt32(paletList[palletIndex - 1].SonKoliNo) + 1);

                    PackingListPalletDetailReportDto palet = new PackingListPalletDetailReportDto();
                    palet.IlkKoliNo = firstPackageNo;
                    palet.SonKoliNo = (Convert.ToInt32(firstPackageNo) + item.NumberofPackage - 1).ToString();
                    palet.KoliSayisi = item.NumberofPackage;
                    palet.PaletAdi = item.PalletName.Split("-")[0];

                    packageNo += item.NumberofPackage;

                    paletList.Add(palet);
                    palletIndex = palletIndex + 1;
                }
            }



            PackingListTrReport packingListTrReport = new PackingListTrReport();
            packingListTrReport.DataSource = reportSource;
            packingListTrReport.ShowPrintMarginsWarning = false;
            packingListTrReport.PackingListPalletQuantityReportDto = kubajList;
            packingListTrReport.PackingListPalletDetailReportDto = paletList;
            packingListTrReport.DeliveryAddressLabel.Visible = exWorks ? true : false;
            packingListTrReport.DeliveryAddress.Visible = exWorks ? true : false;
            packingListTrReport.CreateDocument();
            PackingListDynamicReport.Pages.AddRange(packingListTrReport.Pages);
            PackingListDynamicReport.PrintingSystem.ContinuousPageNumbering = true;

            PackingListDynamicReport.PrintingSystem.ContinuousPageNumbering = true;

            await Task.CompletedTask;
        }


        #endregion

        #region Packing List
        bool PackingListEngReportVisible { get; set; }

        DxReportViewer PackingListEngReportViewer { get; set; }

        XtraReport PackingListEngDynamicReport { get; set; }

        async Task CreateEngPackingListReport(SelectPackingListsDto packingList)
        {
            PackingListEngDynamicReport.ShowPrintMarginsWarning = false;
            PackingListEngDynamicReport.ShowPreviewMarginLines = false;
            PackingListEngDynamicReport.CreateDocument();

            List<PackingListReportDto> reportSource = new List<PackingListReportDto>();

            Guid recieverCompanyId = packingList.RecieverID.GetValueOrDefault();
            var recieverCurrentAccountCard = (await CurrentAccountCardsAppService.GetAsync(recieverCompanyId)).Data;
            string shippingAddress = recieverCurrentAccountCard.ShippingAddress;

            foreach (var item in packingList.SelectPackingListPalletPackageLines)
            {
                string malzemeTanimi = item.ProductName;
                string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(item.ProductID.GetValueOrDefault(), packingList.TransmitterID.GetValueOrDefault());

                malzemeTanimi = malzemeTanimi + supplierReferanceNumber + "/049/" + item.CustomerOrderNr;

                PackingListReportDto report = new PackingListReportDto();
                report.EoriNr = packingList.TransmitterEORINo;
                report.FaturaNo = packingList.BillNo;
                report.CekiListesiNo = packingList.Code;
                report.FaturaTarihi = packingList.BillDate.Value.ToShortDateString();
                report.SiparisNo = packingList.OrderNo;
                report.AliciUnvan = packingList.TransmitterName;
                report.AliciAdres1 = packingList.TransmitterAddress1;
                report.AliciAdres2 = packingList.TransmitterAddress2;
                report.AliciTel = packingList.TransmitterTel;
                report.AliciFax = packingList.TransmitterFax;
                report.PaketNo = item.PackageNo;
                report.MalzemeTanimi = malzemeTanimi;
                report.PaketCinsi = item.PackageType;
                report.PaketIcerigi = item.PackageContent;
                report.PaketSayisi = item.NumberofPackage;
                report.ToplamAdet = item.TotalAmount;
                report.KoliNetKg = item.OnePackageNetKG;
                report.KoliBrutKg = item.OnePackageGrossKG;
                report.KoliToplamNetKg = item.TotalNetKG;
                report.KoliToplamBrutKg = item.TotalGrossKG;
                report.ToplamPaletAdedi = packingList.SelectPackingListPalletCubageLines.Sum(t => t.NumberofPallet);
                report.ToplamHacim = packingList.SelectPackingListPalletCubageLines.Sum(t => t.Cubage);
                report.SevkiyatAdresi = shippingAddress;

                reportSource.Add(report);
            }



            List<PackingListPalletQuantityReportDto> kubajList = new List<PackingListPalletQuantityReportDto>();

            foreach (var item in packingList.SelectPackingListPalletCubageLines)
            {
                PackingListPalletQuantityReportDto kubaj = new PackingListPalletQuantityReportDto();
                kubaj.Kubaj = item.Cubage;
                kubaj.Yukseklik = item.Load_;
                kubaj.PaletAdet = item.NumberofPallet;
                kubaj.Boy = item.Height_;
                kubaj.En = item.Width_;
                kubajList.Add(kubaj);
            }

            List<PackingListPalletDetailReportDto> paletList = new List<PackingListPalletDetailReportDto>();

            int packageNo = 1;

            int palletIndex = 0;

            foreach (var item in packingList.SelectPackingListPalletLines)
            {
                if (packageNo == 1)
                {
                    PackingListPalletDetailReportDto palet = new PackingListPalletDetailReportDto();
                    palet.IlkKoliNo = packageNo.ToString();
                    palet.SonKoliNo = (packageNo + item.NumberofPackage - 1).ToString();
                    palet.KoliSayisi = item.NumberofPackage;
                    palet.PaletAdi = item.PalletName.Split("-")[0];

                    packageNo += item.NumberofPackage;

                    paletList.Add(palet);
                    palletIndex = palletIndex + 1;
                }
                else
                {
                    string firstPackageNo = Convert.ToString(Convert.ToInt32(paletList[palletIndex - 1].SonKoliNo) + 1);

                    PackingListPalletDetailReportDto palet = new PackingListPalletDetailReportDto();
                    palet.IlkKoliNo = firstPackageNo;
                    palet.SonKoliNo = (Convert.ToInt32(firstPackageNo) + item.NumberofPackage - 1).ToString();
                    palet.KoliSayisi = item.NumberofPackage;
                    palet.PaletAdi = item.PalletName.Split("-")[0];

                    packageNo += item.NumberofPackage;

                    paletList.Add(palet);
                    palletIndex = palletIndex + 1;
                }
            }

            PackingListEngReport packingListEngReport = new PackingListEngReport();
            packingListEngReport.DataSource = reportSource;
            packingListEngReport.ShowPrintMarginsWarning = false;
            packingListEngReport.PackingListPalletQuantityReportDto = kubajList;
            packingListEngReport.PackingListPalletDetailReportDto = paletList;
            packingListEngReport.CreateDocument();
            PackingListEngDynamicReport.Pages.AddRange(packingListEngReport.Pages);
            PackingListEngDynamicReport.PrintingSystem.ContinuousPageNumbering = true;

            PackingListEngDynamicReport.PrintingSystem.ContinuousPageNumbering = true;

            await Task.CompletedTask;
        }


        #endregion

        private string MalCinsiIngilizce(string malCinsi)
        {
            string result = "";

            switch (malCinsi)
            {
                case "VİRAJ ROTU":
                    result = "STABILIZER LINK";
                    break;
                case "ROT MİLİ":
                    result = "AXIAL JOINT";
                    break;
                case "BURÇ":
                    result = "BUSH";
                    break;
                case "OYNAR BURÇ":
                    result = "BALL JOINT";
                    break;
                case "ROTİL":
                    result = "BALL JOINT";
                    break;
                case "ROT BAŞI":
                    result = "TIE ROD END";
                    break;
                case "YAN ROT":
                    result = "ROD ASSEMBLY";
                    break;
                case "BURÇ TAKIMI":
                    result = "REPAIR KIT";
                    break;
                case "DENGE KOLU":
                    result = "CONTROL ARM";
                    break;
                case "MONTAJ TAKIMI":
                    result = "REPAIR KIT";
                    break;
                case "CİVATA TAKIMI":
                    result = "REPAIR KIT";
                    break;
                case "BASKILI TORBA, PE":
                    result = "PRINTED PE BAG";
                    break;
                case "DEBRİYAJ CIRCIRI":
                    result = "QUADRANT REPAIR KIT";
                    break;
                case "ŞAFT ASKISI":
                    result = "SHAFT COUPLING";
                    break;
                case "AMORTİSÖR YAYI":
                    result = "SUSPENSION SPRING";
                    break;
                case "SOMUN":
                    result = "NUT";
                    break;
                case "BOŞ KOLİ":
                    result = "EMPTY CART0ON BOX";
                    break;
                case "TAMİR TAKIMI":
                    result = "REPAIR KIT";
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }


    public class UploadConfirmationReportDto
    {
        public string RefNo { get; set; }
        public string ToplamPaletAdedi { get; set; }
        public string Tarih { get; set; }
        public string TeslimTarihi { get; set; }
        public string TirSekli { get; set; }

        public List<FaxMessageLines> FaxMessageLines { get; set; }

        public UploadConfirmationReportDto()
        {
            FaxMessageLines = new List<FaxMessageLines>();
        }
    }

    public class FaxMessage
    {
        public string PackingListNo { get; set; }

        public List<FaxMessageLines> FaxMessageLines { get; set; }

        public List<FaxMessageGTIPTable> FaxMessageGTIPTable { get; set; }

        public FaxMessage()
        {
            FaxMessageLines = new List<FaxMessageLines>();
            FaxMessageGTIPTable = new List<FaxMessageGTIPTable>();
        }
    }

    public class FaxMessageLines
    {
        public string MalCinsiTurkce { get; set; }

        public string MalCinsiIngilizce { get; set; }

        public decimal TutarDagilimiEuro { get; set; }

        public decimal AdetDagilimi { get; set; }

        public decimal NetKgDagilimi { get; set; }

        public decimal BrutKgDagilimi { get; set; }

        public decimal KoliSayisi { get; set; }

        public int CesitDagilimi { get; set; }
    }

    public class FaxMessageGTIPTable
    {
        public string MalCinsiTurkce { get; set; }

        public string MalCinsiIngilizce { get; set; }

        public string GtipKodu { get; set; }
    }

    public class NakliyePaletTablosu
    {
        public int PaletAdedi { get; set; }

        public decimal En { get; set; }

        public decimal Boy { get; set; }

        public decimal Yuk { get; set; }

        public decimal ToplamM3 { get; set; }
    }
}