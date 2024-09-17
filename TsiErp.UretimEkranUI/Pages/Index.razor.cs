using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Timers;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Shared;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class Index : IDisposable
    {

        public bool IsMultipleUserModalVisible = false;
        public bool IsSingleUser = false;
        public bool IsMultipleUser = false;
        public bool MultipleUserSelectionEnable = false;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };
        public string SelectedUsers = string.Empty;

        protected override async void OnInitialized()
        {
            IsMultipleUserModalVisible = true;
            SelectedUsers =( await LoggedUserLocalDbService.GetListAsync()).Where(T=>T.IsAuthorizedUser).Select(T=>T.UserName).FirstOrDefault();
            IsSingleUser = true;

            ParameterControl();

            #region Yarım kalan operasyon kontrol
            var workOrderControl = (await OperationDetailLocalDbService.GetListAsync()).ToList();

            if (workOrderControl.Count > 0)
            {
                NavMenu menu = (NavMenu)mss["NavMenu"];
                menu.ChangeWorkOrderMenuEnabled(false);
                menu.ChangeLogoutMenuEnabled(false);
                menu.ChangeMainPageMenuEnabled(false);
                //menu.ChangeSettingsMenuEnabled(false);
                AppService.CurrentOperation = await OperationDetailLocalDbService.GetAsync(workOrderControl[0].Id);
            }
            #endregion


            #region System General Status

            var generalstatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).ToList();

            if (generalstatus == null || generalstatus.Count == 0)
            {
                SystemGeneralStatusTable generalStatus = new SystemGeneralStatusTable
                {
                    GeneralStatus = 0,
                    isLoadCell = false,
                    StationCode = string.Empty,
                    StationID = Guid.Empty
                };

                await SystemGeneralStatusLocalDbService.InsertAsync(generalStatus);
            }

            #endregion

            StartSystemIdleTimer();

            //string data = "";

            //ConnectorService.SendAndRead("M014W", out data, "127.0.0.1", 1644, 4416);

            //string sonuc = ProtocolServices.M001R("127.0.0.1");

        }


        private async void ParameterControl()
        {
            int parameterId = await ParametersTableLocalDbService.ParameterControl();

            if (parameterId == 0)
            {
                var createdParameter = new ParametersTable
                {
                    HaltTriggerSecond = 120000,
                    IsLoadcell = false,
                    MidControlPeriod = 0,
                    StationID = Guid.Empty
                };

                await ParametersTableLocalDbService.InsertAsync(createdParameter);

                var insertedParameters = await ParametersTableLocalDbService.GetAsync(1);

                if (insertedParameters.Id > 0)
                {
                    AppService.ProgramParameters = insertedParameters;
                }

            }
            else
            {
                AppService.ProgramParameters = await ParametersTableLocalDbService.GetAsync(parameterId);
            }
        }


        private async void GoToCurrentWorkOrder()
        {
            Navigation.NavigateTo("/work-order-detail");
            await Task.CompletedTask;
        }

        private async void ShowModal()
        {
            await Task.CompletedTask;
        }

        #region Halt Reasons

        public bool HaltReasonModalVisible { get; set; }

        public bool EndHaltReasonButtonDisable { get; set; } = true;

        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        ListHaltReasonsDto SelectedHaltReason = new ListHaltReasonsDto();

        private async Task GetHaltReasonsIsOperator()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsOperator == true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsMachine == true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsManagement == true).ToList();

            await Task.CompletedTask;
        }

        private async void OnSelectHaltReason(ListHaltReasonsDto haltReason)
        {
            if (!string.IsNullOrEmpty(haltReason.Name))
            {
                SelectedHaltReason = haltReason;
                EndHaltReasonButtonDisable = false;
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                EndHaltReasonButtonDisable = true;
                await InvokeAsync(() => StateHasChanged());
            }
        }

        #region Halt Reason Timer

        public int TotalHaltReasonTime { get; set; }

        public string HaltReasonTime { get; set; } = "0:0:0";

        System.Timers.Timer _haltReasonTimer = new System.Timers.Timer(1000);

        DateTime HaltReasonStartTime = DateTime.Now;

        void StartHaltReasonTimer()
        {
            HaltReasonStartTime = GetSQLDateAppService.GetDateFromSQL();
            _haltReasonTimer = new System.Timers.Timer(1000);
            _haltReasonTimer.Elapsed += HaltReasonOnTimedEvent;
            _haltReasonTimer.AutoReset = true;
            _haltReasonTimer.Enabled = true;
        }

        private void HaltReasonOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalHaltReasonTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalHaltReasonTime);

            HaltReasonTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 time.Hours,
                 time.Minutes,
                 time.Seconds);

            InvokeAsync(StateHasChanged);
        }

        #endregion

        private async void EndHaltReasonButtonClick()
        {
            _haltReasonTimer.Enabled = false;
            _haltReasonTimer.Stop();
            StartSystemIdleTimer();

            var haltReason = new OperationHaltReasonsTable
            {
                EmployeeID = AppService.EmployeeID,
                HaltReasonID = SelectedHaltReason.Id,
                StationID = AppService.ProgramParameters.StationID,
                TotalHaltReasonTime = TotalHaltReasonTime,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID
            };

            await OperationHaltReasonsTableLocalDbService.InsertAsync(haltReason);

            #region ERP Production Tracking Insert

            var workOrder = (await WorkOrdersAppService.GetAsync(AppService.CurrentOperation.WorkOrderID)).Data;

            Guid CurrentAccountID = Guid.Empty;

            if (workOrder != null && workOrder.Id != Guid.Empty)
            {
                CurrentAccountID = workOrder.CurrentAccountCardID.GetValueOrDefault();
            }

            var today = GetSQLDateAppService.GetDateFromSQL();

            CreateProductionTrackingsDto trackingModel = new CreateProductionTrackingsDto
            {
                AdjustmentTime = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                CurrentAccountCardID = CurrentAccountID,
                HaltReasonID = SelectedHaltReason.Id,
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                Description_ = string.Empty,
                HaltTime = Convert.ToDecimal(today.TimeOfDay.Subtract(HaltReasonStartTime.TimeOfDay).TotalSeconds),
                FaultyQuantity = AppService.CurrentOperation.ScrapQuantity,
                IsFinished = true,
                OperationEndDate = today.Date,
                OperationEndTime = today.TimeOfDay,
                OperationStartDate = HaltReasonStartTime.Date,
                OperationStartTime = HaltReasonStartTime.TimeOfDay,
                OperationTime = 0,
                PlannedQuantity = AppService.CurrentOperation.PlannedQuantity,
                ProducedQuantity = AppService.CurrentOperation.ProducedQuantity,
                ProductID = AppService.CurrentOperation.ProductID,
                ProductionTrackingTypes = 0,
                ProductionOrderID = AppService.CurrentOperation.ProductionOrderID,
                ProductsOperationID = AppService.CurrentOperation.ProductsOperationID,
                ShiftID = Guid.Empty,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                StationID = AppService.CurrentOperation.StationID,

            };

            await ProductionTrackingsAppService.CreateAsync(trackingModel);

            #endregion

            TotalHaltReasonTime = 0;

            HaltReasonTime = "0:0:0";
            SelectedHaltReason = new ListHaltReasonsDto();
            TotalSystemIdleTime = 0;
            SystemIdleTime = "0:0:0";

            HaltReasonModalVisible = false;

        }

        #endregion

        #region System Idle Time

        public int TotalSystemIdleTime { get; set; }

        public string SystemIdleTime { get; set; } = "00:00:00";

        System.Timers.Timer _systemIdleTimer = new System.Timers.Timer(1000);

        DateTime SystemIdleStartTime = DateTime.Now;

        void StartSystemIdleTimer()
        {
            SystemIdleStartTime = DateTime.Now;
            _systemIdleTimer = new System.Timers.Timer(1000);
            _systemIdleTimer.Elapsed += SystemIdleOnTimedEvent;
            _systemIdleTimer.AutoReset = true;
            _systemIdleTimer.Enabled = true;
        }

        private void SystemIdleOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalSystemIdleTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalSystemIdleTime);

            SystemIdleTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 time.Hours,
                 time.Minutes,
                 time.Seconds);

            if (time.Minutes == (((AppService.ProgramParameters.HaltTriggerSecond) / 1000) / 60))
            {
                HaltReasonModalVisible = true;
                StartHaltReasonTimer();
                _systemIdleTimer.Stop();
                _systemIdleTimer.Enabled = false;
                InvokeAsync(StateHasChanged);

            }

            InvokeAsync(StateHasChanged);
        }


        #endregion

        private async Task ShowHaltReasonModal()
        {
            HaltReasonModalVisible = true;
            StartHaltReasonTimer();
            _systemIdleTimer.Stop();
            _systemIdleTimer.Enabled = false;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {

        }

        private async void IsSingleChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (args.Checked)
            {
                IsMultipleUser = false;
                MultipleUserSelectionEnable = false;
            }
            else
            {
                IsMultipleUser = true;
                MultipleUserSelectionEnable = true;
            }

            await (InvokeAsync(StateHasChanged));
        }

        private async void IsMultipleChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (args.Checked)
            {
                IsSingleUser = false;
                MultipleUserSelectionEnable = true;
            }
            else
            {
                IsSingleUser = true;
                MultipleUserSelectionEnable = false;
            }

            await (InvokeAsync(StateHasChanged));
        }

        #region Operatör ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();
        List<LoggedUserTable> LoggedUsersList = new List<LoggedUserTable>();

        public async Task EmployeesCodeOnCreateIcon()
        {
            var EmployeesCodeButtonClick = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesCodeButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            SelectEmployeesPopupVisible = true;

            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.Where(t => t.IsProductionScreenUser == true).ToList();

            LoggedUsersList = await LoggedUserLocalDbService.GetListAsync();

            SelectedUsers = (await LoggedUserLocalDbService.GetListAsync()).Where(T => T.IsAuthorizedUser).Select(T => T.UserName).FirstOrDefault();
            
            await InvokeAsync(StateHasChanged);
        }


        public void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LoggedUsersList.Clear();
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                if (!LoggedUsersList.Any(t => t.UserID == selectedEmployee.Id)) // Seçilmemişse
                {
                    LoggedUserTable loggedUserModel = new LoggedUserTable
                    {
                        IsAuthorizedUser = false,
                        UserID = selectedEmployee.Id,
                        UserName = selectedEmployee.Name + ' ' + selectedEmployee.Surname,
                    };

                    LoggedUsersList.Add(loggedUserModel);
                }
                else
                {
                    var selectedUser = LoggedUsersList.Where(t => t.UserID == selectedEmployee.Id);

                    if (!selectedUser.Select(t => t.IsAuthorizedUser).FirstOrDefault())
                    {
                        var deletingUser = selectedUser.FirstOrDefault();

                        if (deletingUser != null)
                        {
                            LoggedUsersList.Remove(deletingUser);
                        }
                    }

                }
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void LoggedUsersOnSubmit()
        {
            if (LoggedUsersList != null && LoggedUsersList.Count > 0)
            {
                var loggedUserPreviously = await LoggedUserLocalDbService.GetListAsync();

                foreach (var user in LoggedUsersList)
                {
                    if (!loggedUserPreviously.Any(t=>t.UserID == user.UserID)) // mükerrer kayıt olmaması için
                    {
                        await LoggedUserLocalDbService.InsertAsync(user);
                    }
                }
            }

            IsMultipleUserModalVisible = false;
        }

        public void HideMultipleUserModal()
        {
            IsMultipleUserModalVisible = false;
        }

        public void HideUserSelectionModal()
        {
            SelectEmployeesPopupVisible = false;

            SelectedUsers = string.Empty;

            foreach (var item in LoggedUsersList)
            {
                if (string.IsNullOrEmpty(SelectedUsers))
                {
                    SelectedUsers = item.UserName;
                }
                else
                {
                    SelectedUsers = SelectedUsers + ", " + item.UserName;
                }
            }
        }
        #endregion
    }
}
