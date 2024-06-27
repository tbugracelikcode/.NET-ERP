using DevExpress.Blazor.Reporting;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Lists;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using TsiErp.Business.Entities.PackageFiche.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.CustomsInstruction;
using TsiErp.ErpUI.Reports.ShippingManagement.PalletReports.PalletLabels;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.ShippingManagement.PalletRecord.PalletRecordsListPage;

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

        #endregion

        public bool ShowPalletsModal = false;

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
                            case "PackingListsContextApprove":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextApprove"], Id = "approve" }); break;
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
                var contextID = contextsList.Where(t => t.MenuName == "PackingListsPalletLineContextSelectPallet").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    PalletGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletLineContextSelectPallet"], Id = "selectpallet" });
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
                    IsChanged = true;
                    DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;
                    GridLinePalletList = DataSource.SelectPackingListPalletLines;
                    GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "approve":
                    IsChanged = true;
                    DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;
                    GridLinePalletList = DataSource.SelectPackingListPalletLines;
                    GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

                    foreach (var item in GridLinePalletPackageList)
                    {
                        var salesOrder = (await SalesOrdersAppService.GetAsync(item.SalesOrderID.GetValueOrDefault())).Data;
                        var salesOrderLine = salesOrder.SelectSalesOrderLines.Where(t => t.Id == item.SalesOrderLineID.GetValueOrDefault()).FirstOrDefault();
                        int lineIndex = salesOrder.SelectSalesOrderLines.IndexOf(salesOrderLine);
                        salesOrder.SelectSalesOrderLines[lineIndex].SalesOrderLineStateEnum = SalesOrderLineStateEnum.SevkEdildi;

                        var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(salesOrder);

                        await SalesOrdersAppService.UpdateAsync(updateInput);
                    }
                    await ModalManager.MessagePopupAsync(L["MessageApproveTitle"], L["MessageApproveMessage"]);
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

                case "print":
                    await InvokeAsync(StateHasChanged);
                    break;

                case "packinglist":
                    await InvokeAsync(StateHasChanged);
                    break;
                case "commercialinvoice":
                    await InvokeAsync(StateHasChanged);
                    break;
                case "custominstruction":
                    DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    CustomsInstructionReport = new XtraReport();
                    CustomsInstructionReportVisible = true;
                    await CreateCustomsInstructionReport(DataSource);

                    await InvokeAsync(StateHasChanged);

                    break;
                case "shippinginstruction":
                    await InvokeAsync(StateHasChanged);
                    break;
                case "uploadconfirmation":
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
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
                default:
                    break;
            }
        }

        public async void PalletSelectionLineContextMenuClick(ContextMenuClickEventArgs<PalletSelectionModal> args)
        {
            switch (args.Item.Id)
            {
                case "select":
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

                    break;

                case "selectall":

                    foreach (var line in PalletSelectionList)
                    {
                        int lineIndex = PalletSelectionList.IndexOf(line);
                        PalletSelectionList[lineIndex].SelectedPallet = true;
                    }

                    await _LinePalletSelectionGrid.Refresh();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "remove":
                    var palletRemove = args.RowInfo.RowData;
                    int selectedRemovePalletIndex = PalletSelectionList.IndexOf(palletRemove);

                    PalletSelectionList[selectedRemovePalletIndex].SelectedPallet = false;

                    await _LinePalletSelectionGrid.Refresh();

                    await InvokeAsync(StateHasChanged);
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
                        ProductGroupID = product.ProductGrpID
                    };


                    GridLinePalletPackageList.Add(palletPackageLineModel);
                }

                    #endregion
                }
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

        public async void PalletPackageLineContextMenuClick(ContextMenuClickEventArgs<SelectPackingListPalletPackageLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "enumarate":

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

            foreach (var item in salesTypes)
            {
                packingList.SalesTypeName = L[item.SalesTypeName];
            }

            foreach (var item in tIRTypes)
            {
                packingList.TIRTypeName = L[item.TIRTypeName];
            }

            foreach (var item in packingListStates)
            {
                packingList.PackingListStateName = L[item.PackingListStateName];
            }

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

                //XtraReport mainReport = new XtraReport();
                //mainReport.ShowPrintMarginsWarning = false;
                //mainReport.CreateDocument();

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
                                //GtipKodu = entities.TUR_STOK_GRUP.Where(t => t.ACIKLAMA == malCinsi).Select(t => t.GTIP).FirstOrDefault()
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
}
