using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.DocIO.DLS;
using System.Diagnostics;
using Tsi.Core.Utilities.Guids;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.PaymentPlan.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.TemplateOperation.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ContractTrackingFiche
{
    public partial class ContractTrackingFichesListPage : IDisposable
    {

        private SfGrid<SelectContractTrackingFicheLinesDto> _LineGrid;
        private SfGrid<SelectContractTrackingFicheAmountEntryLinesDto> _AmountEntryLineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }


        SelectContractTrackingFicheLinesDto LineDataSource;
        SelectContractTrackingFicheAmountEntryLinesDto AmountEntryLineDataSource;
        SelectWorkOrdersDto WorkOrderDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> AmountEntryLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectContractTrackingFicheLinesDto> GridLineList = new List<SelectContractTrackingFicheLinesDto>();
        List<SelectContractTrackingFicheAmountEntryLinesDto> GridAmountEntryLineList = new List<SelectContractTrackingFicheAmountEntryLinesDto>();
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        //private bool LineCrudPopup = new();
        private bool AmountEntryLineCrudPopup = false;
        private bool AmountEntryLinePopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = ContractTrackingFichesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ContractTrackingFichesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
            CreateMainContextMenuItems();
            CreateAmountEntryLineContextMenuItems();
            //CreateLineContextMenuItems();

        }

        #region Fason Takip Fişleri Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractTrackingFichesDto()
            {
                FicheDate_ = GetSQLDateAppService.GetDateFromSQL(),
                EstimatedDate_ = GetSQLDateAppService.GetDateFromSQL(),
                FicheNr = FicheNumbersAppService.GetFicheNumberAsync("ContractTrackingFichesChildMenu")
            };

            DataSource.SelectContractTrackingFicheLines = new List<SelectContractTrackingFicheLinesDto>();
            DataSource.SelectContractTrackingFicheAmountEntryLines = new List<SelectContractTrackingFicheAmountEntryLinesDto>();
            GridLineList = DataSource.SelectContractTrackingFicheLines;
            GridAmountEntryLineList = DataSource.SelectContractTrackingFicheAmountEntryLines;

            EditPageVisible = true;


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
                            case "ContractTrackingFicheLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextAdd"], Id = "new" }); break;
                            case "ContractTrackingFicheLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextChange"], Id = "changed" }); break;
                            case "ContractTrackingFicheLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextDelete"], Id = "delete" }); break;
                            case "ContractTrackingFicheLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateAmountEntryLineContextMenuItems()
        {
            if (AmountEntryLineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ContractTrackingFicheAmountEntryLineContextAdd":
                                AmountEntryLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheAmountEntryLineContextAdd"], Id = "new" }); break;
                            case "ContractTrackingFicheAmountEntryLineContextChange":
                                AmountEntryLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheAmountEntryLineContextChange"], Id = "changed" }); break;
                            case "ContractTrackingFicheAmountEntryLineContextDelete":
                                AmountEntryLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheAmountEntryLineContextDelete"], Id = "delete" }); break;
                            case "ContractTrackingFicheAmountEntryLineContextRefresh":
                                AmountEntryLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheAmountEntryLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
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
                            case "ContractTrackingFicheContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextAdd"], Id = "new" }); break;
                            case "ContractTrackingFicheContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextChange"], Id = "changed" }); break;
                            case "ContractTrackingFicheContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextDelete"], Id = "delete" }); break;
                            case "ContractTrackingFicheContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextRefresh"], Id = "refresh" }); break;
                            case "ContractTrackingFicheContextAmountEntry":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextAmountEntry"], Id = "amountentry" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListContractTrackingFichesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await ContractTrackingFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectContractTrackingFicheLines;

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

                case "amountentry":
                    DataSource = (await ContractTrackingFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridAmountEntryLineList = DataSource.SelectContractTrackingFicheAmountEntryLines;

                    AmountEntryLinePopup = true;
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectContractTrackingFicheLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectContractTrackingFicheLinesDto
                    {

                    };
                    //LineCrudPopup = true;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    //LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectContractTrackingFicheLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await ContractTrackingFichesAppService.DeleteLineAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectContractTrackingFicheLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectContractTrackingFicheLines.Remove(line);
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

        public async void OnAmountEntryListContextMenuClick(ContextMenuClickEventArgs<SelectContractTrackingFicheAmountEntryLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    AmountEntryLineDataSource = new SelectContractTrackingFicheAmountEntryLinesDto
                    {
                        Date_ = GetSQLDateAppService.GetDateFromSQL()
                    };
                    AmountEntryLineCrudPopup = true;
                    AmountEntryLineDataSource.LineNr = GridAmountEntryLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    AmountEntryLineDataSource = args.RowInfo.RowData;
                    AmountEntryLineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var AmountEntryline = args.RowInfo.RowData;

                        if (AmountEntryline.Id == Guid.Empty)
                        {
                            DataSource.SelectContractTrackingFicheAmountEntryLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (AmountEntryline != null)
                            {
                                await ContractTrackingFichesAppService.DeleteAmountEntryLine(args.RowInfo.RowData.Id);
                                DataSource.SelectContractTrackingFicheAmountEntryLines.Remove(AmountEntryline);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectContractTrackingFicheAmountEntryLines.Remove(AmountEntryline);
                            }
                        }

                        await _AmountEntryLineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _AmountEntryLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            //LineCrudPopup = false;
        }

        public void HideAmountEntryLinesPopup()
        {
            AmountEntryLineCrudPopup = false;
        }

        public void HideAmountEntryPopup()
        {
            AmountEntryLinePopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectContractTrackingFicheLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectContractTrackingFicheLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectContractTrackingFicheLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectContractTrackingFicheLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectContractTrackingFicheLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectContractTrackingFicheLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectContractTrackingFicheLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        protected async Task OnAmountEntryLineSubmit()
        {

            if (AmountEntryLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectContractTrackingFicheAmountEntryLines.Contains(AmountEntryLineDataSource))
                {
                    int selectedAmountEntryLineIndex = DataSource.SelectContractTrackingFicheAmountEntryLines.FindIndex(t => t.LineNr == AmountEntryLineDataSource.LineNr);

                    if (selectedAmountEntryLineIndex > -1)
                    {
                        DataSource.SelectContractTrackingFicheAmountEntryLines[selectedAmountEntryLineIndex] = AmountEntryLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectContractTrackingFicheAmountEntryLines.Add(AmountEntryLineDataSource);
                }
            }
            else
            {
                int selectedAmountEntryLineIndex = DataSource.SelectContractTrackingFicheAmountEntryLines.FindIndex(t => t.Id == AmountEntryLineDataSource.Id);

                if (selectedAmountEntryLineIndex > -1)
                {
                    DataSource.SelectContractTrackingFicheAmountEntryLines[selectedAmountEntryLineIndex] = AmountEntryLineDataSource;
                }
            }

            GridAmountEntryLineList = DataSource.SelectContractTrackingFicheAmountEntryLines;
            await _AmountEntryLineGrid.Refresh();

            HideAmountEntryLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        protected override async Task OnSubmit()
        {

            int listCount = 1;

            if (DataSource.SelectContractTrackingFicheLines.Count != 0)
            {
                foreach (var line in DataSource.SelectContractTrackingFicheLines)
                {

                    if (listCount < DataSource.SelectContractTrackingFicheLines.Count)
                    {
                        WorkOrderDataSource = (await WorkOrdersAppService.GetAsync(line.WorkOrderID.GetValueOrDefault())).Data;

                        WorkOrderDataSource.WorkOrderState = Entities.Enums.WorkOrderStateEnum.Tamamlandi;
                        WorkOrderDataSource.OccuredStartDate = DataSource.FicheDate_;
                        WorkOrderDataSource.OccuredFinishDate = DataSource.FicheDate_;
                        WorkOrderDataSource.ProducedQuantity = DataSource.Amount_;

                        UpdateWorkOrdersDto updateWorkOrderModel = new UpdateWorkOrdersDto
                        {
                            AdjustmentAndControlTime = WorkOrderDataSource.AdjustmentAndControlTime,
                            CurrentAccountCardID = WorkOrderDataSource.CurrentAccountCardID,
                            IsCancel = WorkOrderDataSource.IsCancel,
                            LineNr = WorkOrderDataSource.LineNr,
                            LinkedWorkOrderID = WorkOrderDataSource.LinkedWorkOrderID,
                            OccuredFinishDate = WorkOrderDataSource.OccuredFinishDate,
                            OccuredStartDate = WorkOrderDataSource.OccuredStartDate,
                            OperationTime = WorkOrderDataSource.OperationTime,
                            PlannedQuantity = WorkOrderDataSource.PlannedQuantity,
                            ProducedQuantity = WorkOrderDataSource.ProducedQuantity,
                            ProductID = WorkOrderDataSource.ProductID,
                            ProductionOrderID = WorkOrderDataSource.ProductionOrderID,
                            ProductsOperationID = WorkOrderDataSource.ProductsOperationID,
                            PropositionID = WorkOrderDataSource.PropositionID,
                            RouteID = WorkOrderDataSource.RouteID,
                            StationGroupID = WorkOrderDataSource.StationGroupID,
                            StationID = WorkOrderDataSource.StationID,
                            WorkOrderNo = WorkOrderDataSource.WorkOrderNo,
                            WorkOrderState = 5,
                            Id = WorkOrderDataSource.Id,
                            CreationTime = WorkOrderDataSource.CreationTime,
                            CreatorId = WorkOrderDataSource.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = WorkOrderDataSource.DeleterId.GetValueOrDefault(),
                            DeletionTime = WorkOrderDataSource.DeletionTime.GetValueOrDefault(),
                            IsDeleted = WorkOrderDataSource.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        };

                        await WorkOrdersAppService.UpdateAsync(updateWorkOrderModel);

                        if (listCount + 1 == DataSource.SelectContractTrackingFicheLines.Count)
                        {
                            //var stockMovementsList = (await OperationStockMovementsAppService.GetListAsync(new ListOperationStockMovementsParameterDto())).Data.ToList();

                            var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(DataSource.ProductionOrderID.GetValueOrDefault(), WorkOrderDataSource.ProductsOperationID.GetValueOrDefault())).Data;

                            //var filterList = stockMovementsList.Where(t => t.ProductionorderID == DataSource.ProductionOrderID && t.OperationID == line.OperationID).ToList();

                            if (operationStockMovement.Id != Guid.Empty)
                            {
                                UpdateOperationStockMovementsDto updateStockMovementModel = new UpdateOperationStockMovementsDto
                                {
                                    Id = operationStockMovement.Id,
                                    OperationID = operationStockMovement.OperationID,
                                    ProductionorderID = operationStockMovement.ProductionorderID,
                                    TotalAmount = operationStockMovement.TotalAmount + DataSource.Amount_
                                };

                                await OperationStockMovementsAppService.UpdateAsync(updateStockMovementModel);
                            }
                            else
                            {

                                CreateOperationStockMovementsDto createStockMovementModel = new CreateOperationStockMovementsDto
                                {
                                    Id = Guid.Empty,
                                    OperationID = line.OperationID.GetValueOrDefault(),
                                    ProductionorderID = DataSource.ProductionOrderID.GetValueOrDefault(),
                                    TotalAmount = DataSource.Amount_
                                };

                                await OperationStockMovementsAppService.CreateAsync(createStockMovementModel);
                            }
                        }
                    }

                    else if (listCount == DataSource.SelectContractTrackingFicheLines.Count)
                    {
                        WorkOrderDataSource = (await WorkOrdersAppService.GetAsync(line.WorkOrderID.GetValueOrDefault())).Data;
                        WorkOrderDataSource.WorkOrderState = Entities.Enums.WorkOrderStateEnum.DevamEdiyor;
                        WorkOrderDataSource.OccuredStartDate = DataSource.FicheDate_;

                        UpdateWorkOrdersDto updateWorkOrderModel = new UpdateWorkOrdersDto
                        {
                            AdjustmentAndControlTime = WorkOrderDataSource.AdjustmentAndControlTime,
                            CurrentAccountCardID = WorkOrderDataSource.CurrentAccountCardID,
                            IsCancel = WorkOrderDataSource.IsCancel,
                            LineNr = WorkOrderDataSource.LineNr,
                            LinkedWorkOrderID = WorkOrderDataSource.LinkedWorkOrderID,
                            OccuredFinishDate = WorkOrderDataSource.OccuredFinishDate,
                            OccuredStartDate = WorkOrderDataSource.OccuredStartDate,
                            OperationTime = WorkOrderDataSource.OperationTime,
                            PlannedQuantity = WorkOrderDataSource.PlannedQuantity,
                            ProducedQuantity = WorkOrderDataSource.ProducedQuantity,
                            ProductID = WorkOrderDataSource.ProductID,
                            ProductionOrderID = WorkOrderDataSource.ProductionOrderID,
                            ProductsOperationID = WorkOrderDataSource.ProductsOperationID,
                            PropositionID = WorkOrderDataSource.PropositionID,
                            RouteID = WorkOrderDataSource.RouteID,
                            StationGroupID = WorkOrderDataSource.StationGroupID,
                            StationID = WorkOrderDataSource.StationID,
                            WorkOrderNo = WorkOrderDataSource.WorkOrderNo,
                            WorkOrderState = 4,
                            Id = WorkOrderDataSource.Id,
                            CreationTime = WorkOrderDataSource.CreationTime,
                            CreatorId = WorkOrderDataSource.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = WorkOrderDataSource.DeleterId.GetValueOrDefault(),
                            DeletionTime = WorkOrderDataSource.DeletionTime.GetValueOrDefault(),
                            IsDeleted = WorkOrderDataSource.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        };


                        await WorkOrdersAppService.UpdateAsync(updateWorkOrderModel);
                    }

                    listCount++;

                }
            }



            #region Fason Takip Fişleri Submit İşlemleri

            SelectContractTrackingFichesDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectContractTrackingFichesDto, CreateContractTrackingFichesDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectContractTrackingFichesDto, UpdateContractTrackingFichesDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);

            #endregion

        }

        protected async Task OnAmountEntrySubmit()
        {
            DataSource.OccuredAmount_ = GridAmountEntryLineList.Select(t => t.Amount_).Sum();

            #region İş Emri ve Operasyonlar Ara Stok Etkileşimleri

            int addedAmount = GridAmountEntryLineList.Select(_ => _.Amount_).LastOrDefault();

            if (DataSource.SelectContractTrackingFicheLines.Count > 0)
            {
                int listCount = 1;

                foreach (var line in DataSource.SelectContractTrackingFicheLines)
                {
                    WorkOrderDataSource = (await WorkOrdersAppService.GetAsync(line.WorkOrderID.GetValueOrDefault())).Data;

                    if (listCount + 1 == DataSource.SelectContractTrackingFicheLines.Count)
                    {
                        var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(DataSource.ProductionOrderID.GetValueOrDefault(), WorkOrderDataSource.ProductsOperationID.GetValueOrDefault())).Data;

                        UpdateOperationStockMovementsDto updateStockMovementModel = new UpdateOperationStockMovementsDto
                        {
                            Id = operationStockMovement.Id,
                            OperationID = operationStockMovement.OperationID,
                            ProductionorderID = operationStockMovement.ProductionorderID,
                            TotalAmount = operationStockMovement.TotalAmount - addedAmount
                        };

                        await OperationStockMovementsAppService.UpdateAsync(updateStockMovementModel);
                    }

                    else if (listCount == DataSource.SelectContractTrackingFicheLines.Count)
                    {
                        var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(DataSource.ProductionOrderID.GetValueOrDefault(), WorkOrderDataSource.ProductsOperationID.GetValueOrDefault())).Data;

                        UpdateOperationStockMovementsDto updateStockMovementModel = new UpdateOperationStockMovementsDto
                        {
                            Id = operationStockMovement.Id,
                            OperationID = operationStockMovement.OperationID,
                            ProductionorderID = operationStockMovement.ProductionorderID,
                            TotalAmount = operationStockMovement.TotalAmount + addedAmount
                        };

                        await OperationStockMovementsAppService.UpdateAsync(updateStockMovementModel);

                        UpdateWorkOrdersDto updateWorkOrderModel = new UpdateWorkOrdersDto
                        {
                            AdjustmentAndControlTime = WorkOrderDataSource.AdjustmentAndControlTime,
                            CurrentAccountCardID = WorkOrderDataSource.CurrentAccountCardID,
                            IsCancel = WorkOrderDataSource.IsCancel,
                            LineNr = WorkOrderDataSource.LineNr,
                            LinkedWorkOrderID = WorkOrderDataSource.LinkedWorkOrderID,
                            OccuredFinishDate = WorkOrderDataSource.OccuredFinishDate,
                            OccuredStartDate = WorkOrderDataSource.OccuredStartDate,
                            OperationTime = WorkOrderDataSource.OperationTime,
                            PlannedQuantity = WorkOrderDataSource.PlannedQuantity,
                            ProducedQuantity = WorkOrderDataSource.ProducedQuantity + addedAmount,
                            ProductID = WorkOrderDataSource.ProductID,
                            ProductionOrderID = WorkOrderDataSource.ProductionOrderID,
                            ProductsOperationID = WorkOrderDataSource.ProductsOperationID,
                            PropositionID = WorkOrderDataSource.PropositionID,
                            RouteID = WorkOrderDataSource.RouteID,
                            StationGroupID = WorkOrderDataSource.StationGroupID,
                            StationID = WorkOrderDataSource.StationID,
                            WorkOrderNo = WorkOrderDataSource.WorkOrderNo,
                            WorkOrderState = 4,
                            Id = WorkOrderDataSource.Id,
                            CreationTime = WorkOrderDataSource.CreationTime,
                            CreatorId = WorkOrderDataSource.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = WorkOrderDataSource.DeleterId.GetValueOrDefault(),
                            DeletionTime = WorkOrderDataSource.DeletionTime.GetValueOrDefault(),
                            IsDeleted = WorkOrderDataSource.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId
                        };


                        await WorkOrdersAppService.UpdateAsync(updateWorkOrderModel);
                    }

                    listCount++;
                }
            }

            #endregion

            #region Fason Takip Fişleri Submit İşlemleri

            SelectContractTrackingFichesDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectContractTrackingFichesDto, CreateContractTrackingFichesDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectContractTrackingFichesDto, UpdateContractTrackingFichesDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideAmountEntryPopup();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            await InvokeAsync(StateHasChanged);

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);

            #endregion


        }

        #endregion

        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public Guid FinishedProductID { get; set; }

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            await GetProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderNr = string.Empty;
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CustomerCode = string.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                DataSource.ProductionOrderID = selectedProductionOrder.Id;
                DataSource.ProductionOrderNr = selectedProductionOrder.FicheNo;
                DataSource.CurrentAccountCardID = selectedProductionOrder.CurrentAccountID;
                DataSource.CustomerCode = selectedProductionOrder.CustomerCode;
                DataSource.CurrentAccountCardCode = selectedProductionOrder.CurrentAccountCode;
                DataSource.CurrentAccountCardName = selectedProductionOrder.CurrentAccountName;
                FinishedProductID = selectedProductionOrder.FinishedProductID.GetValueOrDefault();
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş Tanımı ButtonEdit

        SfTextBox ContractQualityPlansButtonEdit;
        bool SelectContractQualityPlansPopupVisible = false;
        List<ListContractQualityPlansDto> ContractQualityPlansList = new List<ListContractQualityPlansDto>();

        public async Task ContractQualityPlansOnCreateIcon()
        {
            var ContractQualityPlansButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ContractQualityPlansButtonClickEvent);
            await ContractQualityPlansButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ContractQualityPlansButtonClick } });
        }

        public async void ContractQualityPlansButtonClickEvent()
        {
            if (DataSource.ProductionOrderID == null || DataSource.ProductionOrderID == Guid.Empty || DataSource.CurrentAccountCardID == null || DataSource.CurrentAccountCardID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);
            }
            else
            {
                SelectContractQualityPlansPopupVisible = true;
                await GetContractQualityPlansList();

            }
            await InvokeAsync(StateHasChanged);
        }

        public void ContractQualityPlansOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ContractQualityPlanID = Guid.Empty;
                DataSource.ContractQualityPlanDescription = string.Empty;
                DataSource.ContractQualityPlanDocumentNumber = string.Empty;
                DataSource.QualityPlanCurrentAccountCardID = Guid.Empty;
                DataSource.QualityPlanCurrentAccountCardCode = string.Empty;
                DataSource.QualityPlanCurrentAccountCardName = string.Empty;
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
            }
        }

        public async void ContractQualityPlansDoubleClickHandler(RecordDoubleClickEventArgs<ListContractQualityPlansDto> args)
        {
            var selectedContractQualityPlan = args.RowData;

            if (selectedContractQualityPlan != null)
            {
                DataSource.ContractQualityPlanID = selectedContractQualityPlan.Id;
                DataSource.ContractQualityPlanDescription = selectedContractQualityPlan.Description_;
                DataSource.ContractQualityPlanDocumentNumber = selectedContractQualityPlan.DocumentNumber;
                DataSource.QualityPlanCurrentAccountCardID = selectedContractQualityPlan.CurrrentAccountCardID;
                DataSource.QualityPlanCurrentAccountCardCode = selectedContractQualityPlan.CurrrentAccountCardCode;
                DataSource.QualityPlanCurrentAccountCardName = selectedContractQualityPlan.CurrrentAccountCardName;
                DataSource.ProductID = selectedContractQualityPlan.ProductID;
                DataSource.ProductCode = selectedContractQualityPlan.ProductCode;
                DataSource.ProductName = selectedContractQualityPlan.ProductName;

                var oprlist = (await ContractQualityPlansAppService.GetAsync(selectedContractQualityPlan.Id)).Data.SelectContractQualityPlanOperations;

                var workOrderList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == DataSource.ProductionOrderID).ToList();

                foreach (var opr in oprlist)
                {
                    var operation = (await ProductsOperationsAppService.GetAsync(opr.OperationID.GetValueOrDefault())).Data;
                    var wordorder = workOrderList.Where(t => t.ProductsOperationID == opr.OperationID.GetValueOrDefault()).FirstOrDefault();

                    SelectContractTrackingFicheLinesDto operationModel = new SelectContractTrackingFicheLinesDto
                    {
                        OperationID = operation.Id,
                        OperationCode = operation.Code,
                        OperationName = operation.Name,
                        WorkOrderID = wordorder.Id,
                        WorkOrderNr = wordorder.WorkOrderNo,
                        StationID = wordorder.StationID.GetValueOrDefault(),
                        StationCode = wordorder.StationCode,
                        StationName = wordorder.StationName,
                        IsSent = false,
                        LineNr = GridLineList.Count + 1
                    };

                    GridLineList.Add(operationModel);

                }

                await _LineGrid.Refresh();
                SelectContractQualityPlansPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region GetList Metotları

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        private async Task GetContractQualityPlansList()
        {
            ContractQualityPlansList = (await ContractQualityPlansAppService.GetListAsync(new ListContractQualityPlansParameterDto())).Data.Where(t => t.ProductID == FinishedProductID).ToList();
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
            DataSource.FicheNr = FicheNumbersAppService.GetFicheNumberAsync("ContractTrackingFichesChildMenu");
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
