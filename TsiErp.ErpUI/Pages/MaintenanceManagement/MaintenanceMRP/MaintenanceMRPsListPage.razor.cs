using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Guids;
using TsiErp.Business.Entities.GrandTotalStockMovement.Services;
using TsiErp.Business.Entities.MaintenanceInstruction.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenanceMRP
{
    public partial class MaintenanceMRPsListPage : IDisposable
    {
        private SfGrid<SelectMaintenanceMRPLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMaintenanceMRPLinesDto LineDataSource;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMaintenanceMRPLinesDto> GridLineList = new List<SelectMaintenanceMRPLinesDto>();

        List<ListPlannedMaintenancesDto> PlannedMaintenancesList = new List<ListPlannedMaintenancesDto>();

        List<SelectMaintenanceInstructionLinesDto> MaintenanceInstructionLinesList = new List<SelectMaintenanceInstructionLinesDto>();

        List<SelectMRPLinesDto> MRPLinesList = new List<SelectMRPLinesDto>();

        public bool enableEndDate = false;
        public bool disableMergeLines = true;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = MaintenanceMRPsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "MainMatReqPlanningChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }


        #region Bakım MRP Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenanceMRPsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MainMatReqPlanningChildMenu"),
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                TimeLeftforMaintenance = 0,
                FilterEndDate = GetSQLDateAppService.GetDateFromSQL(),
                FilterStartDate = GetSQLDateAppService.GetDateFromSQL(),
            };

            DataSource.SelectMaintenanceMRPLines = new List<SelectMaintenanceMRPLinesDto>();
            GridLineList = DataSource.SelectMaintenanceMRPLines;
            enableEndDate = true;

            foreach (var item in _timeLeftForMaintenaceComboBox)
            {
                item.Text = L[item.Text];
            }

            disableMergeLines = true;

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
                            case "MaintenanceMRPLinesContextStockUsage":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextStockUsage"], Id = "stockusage" }); break;
                            case "MaintenanceMRPLinesContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextDelete"], Id = "delete" }); break;
                            case "MaintenanceMRPLinesContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextRefresh"], Id = "refresh" }); break;
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
                            case "MaintenanceMRPsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextAdd"], Id = "new" }); break;
                            case "MaintenanceMRPsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextChange"], Id = "changed" }); break;
                            case "MaintenanceMRPsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextDelete"], Id = "delete" }); break;
                            case "MaintenanceMRPsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextRefresh"], Id = "refresh" }); break;
                            case "MaintenanceMRPsContextConvertMRP":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextConvertMRP"], Id = "convertmrp" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async override void ShowEditPage()
        {
            foreach (var item in _timeLeftForMaintenaceComboBox)
            {
                item.Text = L[item.Text];
            }


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
                    if (DataSource.SelectMaintenanceMRPLines.Count > 0)
                    {
                        disableMergeLines = false;
                    }
                    else if (DataSource.SelectMaintenanceMRPLines.Count == 0)
                    {
                        disableMergeLines = true;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMaintenanceMRPsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await MaintenanceMRPsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMaintenanceMRPLines;

                    foreach(var line in GridLineList)
                    {
                        int indexofLine = GridLineList.IndexOf(line);
                        GridLineList[indexofLine].ProductCode = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data.Code;
                    }


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalMessageBase"]);
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

                case "convertmrp":

                    DataSource = (await MaintenanceMRPsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMaintenanceMRPLines;

                    foreach (var line in GridLineList)
                    {
                        SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                        {
                            Amount = line.Amount,
                            AmountOfStock = line.AmountOfStock,
                            BranchCode = string.Empty,
                            BranchID = Guid.Empty,
                            LineNr = MRPLinesList.Count + 1,
                            WarehouseID = Guid.Empty,
                            WarehouseCode = string.Empty,
                            UnitSetID = line.UnitSetID,
                            UnitSetCode = line.UnitSetCode,
                            State_ = string.Empty,
                            SalesOrderLineID = Guid.Empty,
                            SalesOrderID = Guid.Empty,
                            SalesOrderFicheNo = string.Empty,
                            RequirementAmount = line.RequirementAmount,
                            ProductName = line.ProductName,
                            ProductID = line.ProductID,
                            ProductCode = line.ProductCode,
                            MRPID = Guid.Empty,
                            isCalculated = true,
                            isPurchase = false,
                            isStockUsage = false,
                            CreationTime = GetSQLDateAppService.GetDateFromSQL(),
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = Guid.Empty,
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                        };

                        MRPLinesList.Add(mrpLineModel);
                    }

                    CreateMRPsDto mrpModel = new CreateMRPsDto
                    {
                        Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu"),
                        Id = Guid.Empty,
                        State_ = string.Empty,
                        MaintenanceMRPID = DataSource.Id,
                        IsMaintenanceMRP = true,
                        Description_ = string.Empty,
                        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                    };

                    mrpModel.SelectMRPLines = MRPLinesList;

                    await MRPsAppService.CreateAsync(mrpModel);

                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMaintenanceMRPLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "stockusage":
                    LineDataSource = args.RowInfo.RowData;
                    int stockUsageIndex = GridLineList.IndexOf(LineDataSource);
                    GridLineList[stockUsageIndex].isStockUsage = !LineDataSource.isStockUsage;

                    if (GridLineList[stockUsageIndex].isStockUsage)
                    {
                        GridLineList[stockUsageIndex].RequirementAmount = GridLineList[stockUsageIndex].Amount - Convert.ToInt32(GridLineList[stockUsageIndex].AmountOfStock);
                        if (0 > GridLineList[stockUsageIndex].RequirementAmount)
                        {
                            GridLineList[stockUsageIndex].RequirementAmount = 0;
                        }
                    }

                    else
                    {
                        GridLineList[stockUsageIndex].RequirementAmount = GridLineList[stockUsageIndex].Amount;
                    }

                    await _LineGrid.Refresh();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalLineMessageBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectMaintenanceMRPLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectMaintenanceMRPLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectMaintenanceMRPLines.Remove(line);
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

        public async void CalculateButtonClicked()
        {
            PlannedMaintenancesList = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.PlannedDate >= DataSource.FilterStartDate && t.PlannedDate <= DataSource.FilterEndDate && t.Status == Entities.Enums.PlannedMaintenanceStateEnum.Yapılmadı).ToList();

            foreach (var plannedmaintenance in PlannedMaintenancesList)
            {
                MaintenanceInstructionLinesList = (await MaintenanceInstructionsAppService.GetbyPeriodStationAsync(plannedmaintenance.StationID, plannedmaintenance.PeriodID)).Data.SelectMaintenanceInstructionLines.ToList();

                foreach (var instructionline in MaintenanceInstructionLinesList)
                {
                    decimal amountofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == instructionline.ProductID).Select(t => t.Amount).Sum();

                    var product = (await ProductsAppService.GetAsync(instructionline.ProductID.GetValueOrDefault())).Data;

                    SelectMaintenanceMRPLinesDto maintenanceMRPModel = new SelectMaintenanceMRPLinesDto
                    {
                        Amount = Convert.ToInt32(instructionline.Amount),
                        AmountOfStock = amountofProduct,
                        RequirementAmount = Math.Abs(Convert.ToInt32(amountofProduct) - Convert.ToInt32(instructionline.Amount)),
                        isStockUsage = true,
                        LineNr = GridLineList.Count + 1,
                        UnitSetID = instructionline.UnitSetID,
                        UnitSetCode = instructionline.UnitSetCode,
                        ProductCode = product.Code,
                        ProductName = product.Name,
                        ProductID = instructionline.ProductID,

                    };

                    GridLineList.Add(maintenanceMRPModel);
                }

            }

            disableMergeLines = false;

            await _LineGrid.Refresh();
        }

        public void StartDateValueChangeHandler(ChangedEventArgs<DateTime?> args)
        {
            if(DataSource.TimeLeftforMaintenance >0)
            {
                switch(DataSource.TimeLeftforMaintenance)
                {
                    case 0: break;
                    case 1:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(7);   break;
                    case 2:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(14);  break;
                    case 3:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(30);  break;
                    case 4:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(60);  break;
                    case 5:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(90);  break;
                    case 6:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(120); break;
                    case 7:DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(180); break;
                    default:break;
                }
            }
        }

        private void MergeLinesSwitchChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsMergeLines)
            {
               
            }
            else
            {
            }
            _LineGrid.Refresh();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MainMatReqPlanningChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region ComboBox İşlemleri
        public class TimeLeftforMaintenanceComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<TimeLeftforMaintenanceComboBox> _timeLeftForMaintenaceComboBox = new List<TimeLeftforMaintenanceComboBox>
        {
            new TimeLeftforMaintenanceComboBox(){ID = "0", Text="SelectComboBoxItem"},
            new TimeLeftforMaintenanceComboBox(){ID = "1", Text="1Week"},
            new TimeLeftforMaintenanceComboBox(){ID = "2", Text="2Weeks"},
            new TimeLeftforMaintenanceComboBox(){ID = "3", Text="1Month"},
            new TimeLeftforMaintenanceComboBox(){ID = "4", Text="2Months"},
            new TimeLeftforMaintenanceComboBox(){ID = "5", Text="3Months"},
            new TimeLeftforMaintenanceComboBox(){ID = "6", Text="4Months"},
            new TimeLeftforMaintenanceComboBox(){ID = "7", Text="6Months"},
        };

        private void TimeLeftforMaintenanceComboBoxValueChangeHandler(ChangeEventArgs<string, TimeLeftforMaintenanceComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0": DataSource.TimeLeftforMaintenance = 0; enableEndDate = true; break;
                    case "1": DataSource.TimeLeftforMaintenance = 1; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(7);   break;
                    case "2": DataSource.TimeLeftforMaintenance = 2; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(14);  break;
                    case "3": DataSource.TimeLeftforMaintenance = 3; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(30);  break;
                    case "4": DataSource.TimeLeftforMaintenance = 4; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(60);  break;
                    case "5": DataSource.TimeLeftforMaintenance = 5; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(90);  break;
                    case "6": DataSource.TimeLeftforMaintenance = 6; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(120); break;
                    case "7": DataSource.TimeLeftforMaintenance = 7; enableEndDate = false; DataSource.FilterEndDate = DataSource.FilterStartDate.Value.AddDays(180); break;


                    default: break;
                }
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
