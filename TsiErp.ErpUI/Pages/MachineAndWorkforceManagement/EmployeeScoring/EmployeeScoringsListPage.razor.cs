using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.PackageFiche.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeScoring
{
    public partial class EmployeeScoringsListPage : IDisposable
    {
        private SfGrid<SelectEmployeeScoringLinesDto> _LineGrid;
        private SfGrid<SelectEmployeeOperationsDto> _LinesLineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }
        SelectEmployeeScoringLinesDto LineDataSource;
        SelectEmployeeOperationsDto LinesLineDataSource;

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LinesLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectEmployeeScoringLinesDto> GridLineList = new List<SelectEmployeeScoringLinesDto>();
        List<SelectEmployeeOperationsDto> EmployeeOperationsList = new List<SelectEmployeeOperationsDto>();
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private bool LineCrudPopup = false;
        private bool LinesLineCrudPopup = false;
        decimal taskCapabilityRatio = 0;
        decimal productionPerformanceRatio = 0;
        decimal attendanceRatio = 0;
        decimal educationLevelScore = 0;
        int oldScore = 0;

        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeScoringsService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateLinesLineContextMenuItems();
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EmployeeScoreChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion

        }

        #region Personel Puanlama Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeScoringsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeScoreChildMenu"),
                Year_ = DateTime.Today.Year,
                Month_ = DateTime.Today.Month,
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.Date,
                ScoringState = EmployeeScoringsStateEnum.Taslak
            };

            DataSource.SelectEmployeeScoringLines = new List<SelectEmployeeScoringLinesDto>();
            GridLineList = DataSource.SelectEmployeeScoringLines;

            EmployeeOperationsList = new List<SelectEmployeeOperationsDto>();

           

            #region Combobox Localization

            foreach (var item in _monthComboBox)
            {
                item.Text = L[item.Text];
            }
            foreach (var item in _yearComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in scoringStates)
            {
                item.ScoringStateName = L[item.ScoringStateName];
            }

            #endregion

            EditPageVisible = true;

            await Task.CompletedTask;
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
                    #region Combobox Localization

                    foreach (var item in _monthComboBox)
                    {
                        item.Text = L[item.Text];
                    }
                    foreach (var item in _yearComboBox)
                    {
                        item.Text = L[item.Text];
                    }

                    foreach (var item in scoringStates)
                    {
                        item.ScoringStateName = L[item.ScoringStateName];
                    }

                    #endregion

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
                            case "EmployeeScoringsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringsContextAdd"], Id = "new" }); break;
                            case "EmployeeScoringsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringsContextChange"], Id = "changed" }); break;
                            case "EmployeeScoringsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringsContextDelete"], Id = "delete" }); break;
                            case "EmployeeScoringsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {

                var contextId= contextsList.Where(t => t.MenuName == "EmployeeScoringLinesContextChange").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextId).Select(t => t.IsUserPermitted).FirstOrDefault();

                if (permission)
                {
                    LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringLinesContextChange"], Id = "changed" });
                }
            }
        }

        protected void CreateLinesLineContextMenuItems()
        {
            if (LinesLineGridContextMenu.Count() == 0)
            {

                var contextId = contextsList.Where(t => t.MenuName == "EmployeeScoringLineLinesContextChange").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextId).Select(t => t.IsUserPermitted).FirstOrDefault();

                if (permission)
                {
                    LinesLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeScoringLineLinesContextChange"], Id = "changed" });
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListEmployeeScoringsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await EmployeeScoringsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectEmployeeScoringLines;

                    ShowEditPage();
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectEmployeeScoringLinesDto> args)
        {
            switch (args.Item.Id)
            {
               

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListLineContextMenuClick(ContextMenuClickEventArgs<SelectEmployeeOperationsDto> args)
        {
            switch (args.Item.Id)
            {

                case "changed":
                    LinesLineDataSource = args.RowInfo.RowData;
                    oldScore = LinesLineDataSource.Score_;
                    LinesLineCrudPopup = true;
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

        public void HideLinesLinePopup()
        {
            LinesLineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectEmployeeScoringLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectEmployeeScoringLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectEmployeeScoringLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectEmployeeScoringLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectEmployeeScoringLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectEmployeeScoringLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectEmployeeScoringLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        protected async Task OnLinesLineSubmit()
        {
            LineDataSource = DataSource.SelectEmployeeScoringLines.Where(t => t.EmployeeID == LinesLineDataSource.EmployeeID).FirstOrDefault();

            if (LinesLineDataSource.Id == Guid.Empty)
            {
                if (LineDataSource.SelectEmployeeOperations.Contains(LinesLineDataSource))
                {
                    int selectedLineIndex = LineDataSource.SelectEmployeeOperations.FindIndex(t => t.LineNr == LinesLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        LineDataSource.SelectEmployeeOperations[selectedLineIndex] = LinesLineDataSource;
                    }
                }
                else
                {
                    LineDataSource.SelectEmployeeOperations.Add(LinesLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = LineDataSource.SelectEmployeeOperations.FindIndex(t => t.Id == LinesLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    LineDataSource.SelectEmployeeOperations[selectedLineIndex] = LinesLineDataSource;
                }
            }

            EmployeeOperationsList = LineDataSource.SelectEmployeeOperations;
            GridLineList = DataSource.SelectEmployeeScoringLines;
            GetLinesLineTotal();
            await _LineGrid.Refresh();
            await _LinesLineGrid.Refresh();

            HideLinesLinePopup();
            await InvokeAsync(StateHasChanged);
        }

        public override async void GetTotal()
        {
            #region Genel Beceri Puanı

            LineDataSource.GeneralSkillRatio = ((taskCapabilityRatio*LineDataSource.TaskCapabilityRatio) + (educationLevelScore *LineDataSource.EducationLevelScore) + (productionPerformanceRatio * LineDataSource.ProductionPerformanceRatio) + (attendanceRatio * LineDataSource.AttendanceRatio) + LineDataSource.SeniorityRatio);

            #endregion

            #region Değerlendirme Sonrası Maaş

            var startingSalaryID = (await StartingSalariesAppService.GetListAsync(new ListStartingSalariesParameterDto())).Data.Where(t => t.Year_.Value.Year == DataSource.Year_).Select(t => t.Id).FirstOrDefault();

            var startingSalary = (await StartingSalariesAppService.GetAsync(startingSalaryID)).Data.SelectStartingSalaryLines.Where(t => t.SeniorityID == LineDataSource.SeniorityID).FirstOrDefault();

            LineDataSource.AfterEvaluationSalary = LineDataSource.StartingSalaryofPosition + (startingSalary.Difference * LineDataSource.GeneralSkillRatio * LineDataSource.ManagementImprovementRatio);

            if(LineDataSource.AfterEvaluationSalary > startingSalary.CurrentSalaryUpperLimit)
            {
                LineDataSource.AfterEvaluationSalary = startingSalary.CurrentSalaryUpperLimit;
            }

            #endregion

            #region Yeniden Değerlendirme Oranı

            LineDataSource.ReevaluationRatio = (LineDataSource.AfterEvaluationSalary - LineDataSource.EmployeeCurrentSalary) * 100 / LineDataSource.EmployeeCurrentSalary;

            #endregion

            #region Pozisyonun Değerleme Zam Oranı

            LineDataSource.PositionValuationRaiseRatio = (LineDataSource.StartingSalaryofPosition - LineDataSource.EmployeeCurrentSalary) * 100 / LineDataSource.EmployeeCurrentSalary;

            #endregion

        }

        public async void GetLinesLineTotal()
        {
            LineDataSource = DataSource.SelectEmployeeScoringLines.Where(t => t.EmployeeID == LinesLineDataSource.EmployeeID).FirstOrDefault();

            LineDataSource.TaskCompetenceScore = LineDataSource.TaskCompetenceScore + ((LinesLineDataSource.Score_ - oldScore) * LinesLineDataSource.TemplateOperationWorkScore);

            var workScoreSum = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.Select(t => t.WorkScore).Sum();

            LineDataSource.TaskCapabilityRatio = (LineDataSource.TaskCompetenceScore) / (workScoreSum * 5) * 100;

        }

        public async void CreateLines()
        {
            var EmployeeList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();

            var ProductionTrackingList = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.OperationStartDate >= DataSource.StartDate && t.OperationEndDate <= DataSource.EndDate).ToList();

            foreach (var employee in EmployeeList)
            {

                #region Satırın Satırı İşlemleri

                var tempProductionTrackingList = ProductionTrackingList.Where(t => t.EmployeeID == employee.Id).DistinctBy(t => t.StationID).ToList();

                List<SelectEmployeeOperationsDto> tempEmployeeOperationsList = new List<SelectEmployeeOperationsDto>();

                foreach (var productionTracking in tempProductionTrackingList)
                {
                    var ProductsOperationId = (await WorkOrdersAppService.GetAsync(productionTracking.WorkOrderID)).Data.ProductsOperationID.GetValueOrDefault();

                    var TemplateOperationId = (await ProductsOperationsAppService.GetAsync(ProductsOperationId)).Data.TemplateOperationID.GetValueOrDefault();

                    var TemplateOperationDataSource = (await TemplateOperationsAppService.GetAsync(TemplateOperationId)).Data;

                    SelectEmployeeOperationsDto employeeOperationModel = new SelectEmployeeOperationsDto
                    {
                        EmployeeID = employee.Id,
                        LineNr = EmployeeOperationsList.Count + 1,
                        Score_ = 0,
                        TemplateOperationID = TemplateOperationId,
                        TemplateOperationName = TemplateOperationDataSource.Name,
                        TemplateOperationWorkScore = TemplateOperationDataSource.WorkScore,
                    };

                    EmployeeOperationsList.Add(employeeOperationModel);
                    tempEmployeeOperationsList.Add(employeeOperationModel);
                }

                #endregion

                #region Satır Verileri

                var DepartmentDataSource = (await DepartmentsAppService.GetAsync(employee.DepartmentID.GetValueOrDefault())).Data;

                var EducationLevelScore = (await EducationLevelScoresAppService.GetAsync(employee.EducationLevelID.GetValueOrDefault())).Data.Score;

                var startingSalaryID = (await StartingSalariesAppService.GetListAsync(new ListStartingSalariesParameterDto())).Data.Where(t => t.Year_.Value.Year == DataSource.Year_).Select(t => t.Id).FirstOrDefault();

                var startingSalary = (await StartingSalariesAppService.GetAsync(startingSalaryID)).Data.SelectStartingSalaryLines.Where(t=>t.SeniorityID == employee.SeniorityID.GetValueOrDefault()).FirstOrDefault();

                if(startingSalary == null)
                {
                    startingSalary = new SelectStartingSalaryLinesDto();
                    startingSalary.CurrentSalaryLowerLimit = 0;
                }

                #endregion

                SelectEmployeeScoringLinesDto scoringLineModel = new SelectEmployeeScoringLinesDto
                {
                    EmployeeID = employee.Id,
                    EmployeeName = employee.Name,
                    EmployeeSurname = employee.Surname,
                    EmployeeCurrentSalary = employee.CurrentSalary,
                    EmployeeHiringDate = employee.HiringDate,
                    EmployeeTaskDefinition = employee.TaskDefinition,
                    DepartmentID = DepartmentDataSource.Id,
                    DepartmentName = DepartmentDataSource.Name,
                    OfficialSeniorityID = DepartmentDataSource.SeniorityID.GetValueOrDefault(),
                    OfficialSeniorityName = DepartmentDataSource.SeniorityName,
                    SeniorityID = employee.SeniorityID.GetValueOrDefault(),
                    SeniorityName = DepartmentDataSource.SeniorityName,
                    TodaysDate = DateTime.Today,
                    SeniorityValue = Convert.ToDecimal(DateTime.Today.Subtract(employee.HiringDate).TotalDays) / 365,
                    EducationLevelID = employee.EducationLevelID.GetValueOrDefault(),
                    EducationLevelName = employee.EducationLevelName,
                    AbsencePeriod = 0,
                    ShiftValue = 1,
                    TaskCompetenceScore = 0,
                    TaskCapabilityRatio = 0,
                    EducationLevelScore = EducationLevelScore,
                    ProductionPerformanceRatio = 0,
                    AttendanceRatio = 0,
                    SeniorityRatio = 0,
                    GeneralSkillRatio = 0,
                    ManagementImprovementRatio=0,
                    StartingSalaryofPosition = startingSalary.CurrentSalaryLowerLimit,
                    AfterEvaluationSalary = 0,
                    ReevaluationRatio = 0,
                    PositionValuationRaiseRatio = 0,
                    RaiseMonth = employee.HiringDate.Month

                };

                scoringLineModel.SelectEmployeeOperations = tempEmployeeOperationsList;

                GridLineList.Add(scoringLineModel);

            }

            await _LineGrid.Refresh();
        }

        public void RowBound(RowDataBoundEventArgs<SelectEmployeeScoringLinesDto> args)
        {
            if (args.Data.TaskCompetenceScore > 0)
            {
                args.Row.AddClass(new string[] { "modified" });
            }
            else if (args.Data.TaskCompetenceScore <= 0)
            {

                args.Row.AddClass(new string[] { "notmodified" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeScoreChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Kriterlerin Öncelik Dağılımı ButtonEdit Methotları

        #region Görev Kabiliyet Oranı ButtonEdit

        SfTextBox GeneralSkillRecordPriorities1ButtonEdit;
        bool SelectGeneralSkillRecordPriorities1PopupVisible = false;
        List<ListGeneralSkillRecordPrioritiesDto> GeneralSkillRecordPriorities1List = new List<ListGeneralSkillRecordPrioritiesDto>();

        public async Task GeneralSkillRecordPriorities1OnCreateIcon()
        {
            var GeneralSkillRecordPriorities1ButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, GeneralSkillRecordPriorities1ButtonClickEvent);
            await GeneralSkillRecordPriorities1ButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", GeneralSkillRecordPriorities1ButtonClick } });
        }

        public async void GeneralSkillRecordPriorities1ButtonClickEvent()
        {
            SelectGeneralSkillRecordPriorities1PopupVisible = true;
            GeneralSkillRecordPriorities1List = (await GeneralSkillRecordPrioritiesAppService.GetListAsync(new ListGeneralSkillRecordPrioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void GeneralSkillRecordPriorities1OnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                taskCapabilityRatio = 0;
                LineDataSource.TaskCapabilityRatioName = string.Empty;
            }
        }

        public async void GeneralSkillRecordPriorities1DoubleClickHandler(RecordDoubleClickEventArgs<ListGeneralSkillRecordPrioritiesDto> args)
        {
            var selectedGeneralSkillRecordPrioritie = args.RowData;

            if (selectedGeneralSkillRecordPrioritie != null)
            {
                taskCapabilityRatio = selectedGeneralSkillRecordPrioritie.Score;
                LineDataSource.TaskCapabilityRatioName = selectedGeneralSkillRecordPrioritie.GeneralSkillName;
                SelectGeneralSkillRecordPriorities1PopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Eğitim Seviyesi ButtonEdit

        SfTextBox GeneralSkillRecordPriorities2ButtonEdit;
        bool SelectGeneralSkillRecordPriorities2PopupVisible = false;
        List<ListGeneralSkillRecordPrioritiesDto> GeneralSkillRecordPriorities2List = new List<ListGeneralSkillRecordPrioritiesDto>();

        public async Task GeneralSkillRecordPriorities2OnCreateIcon()
        {
            var GeneralSkillRecordPriorities2ButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, GeneralSkillRecordPriorities2ButtonClickEvent);
            await GeneralSkillRecordPriorities2ButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", GeneralSkillRecordPriorities2ButtonClick } });
        }

        public async void GeneralSkillRecordPriorities2ButtonClickEvent()
        {
            SelectGeneralSkillRecordPriorities2PopupVisible = true;
            GeneralSkillRecordPriorities2List = (await GeneralSkillRecordPrioritiesAppService.GetListAsync(new ListGeneralSkillRecordPrioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void GeneralSkillRecordPriorities2OnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                educationLevelScore = 0;
                LineDataSource.EducationLevelScoreName = string.Empty;
            }
        }

        public async void GeneralSkillRecordPriorities2DoubleClickHandler(RecordDoubleClickEventArgs<ListGeneralSkillRecordPrioritiesDto> args)
        {
            var selectedGeneralSkillRecordPrioritie = args.RowData;

            if (selectedGeneralSkillRecordPrioritie != null)
            {
                educationLevelScore = selectedGeneralSkillRecordPrioritie.Score;
                LineDataSource.EducationLevelScoreName = selectedGeneralSkillRecordPrioritie.GeneralSkillName;
                SelectGeneralSkillRecordPriorities2PopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Üretim Performans Oranı ButtonEdit

        SfTextBox GeneralSkillRecordPriorities3ButtonEdit;
        bool SelectGeneralSkillRecordPriorities3PopupVisible = false;
        List<ListGeneralSkillRecordPrioritiesDto> GeneralSkillRecordPriorities3List = new List<ListGeneralSkillRecordPrioritiesDto>();

        public async Task GeneralSkillRecordPriorities3OnCreateIcon()
        {
            var GeneralSkillRecordPriorities3ButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, GeneralSkillRecordPriorities3ButtonClickEvent);
            await GeneralSkillRecordPriorities3ButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", GeneralSkillRecordPriorities3ButtonClick } });
        }

        public async void GeneralSkillRecordPriorities3ButtonClickEvent()
        {
            SelectGeneralSkillRecordPriorities3PopupVisible = true;
            GeneralSkillRecordPriorities3List = (await GeneralSkillRecordPrioritiesAppService.GetListAsync(new ListGeneralSkillRecordPrioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void GeneralSkillRecordPriorities3OnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                productionPerformanceRatio = 0;
                LineDataSource.ProductionPerformanceRatioName = string.Empty;
            }
        }

        public async void GeneralSkillRecordPriorities3DoubleClickHandler(RecordDoubleClickEventArgs<ListGeneralSkillRecordPrioritiesDto> args)
        {
            var selectedGeneralSkillRecordPrioritie = args.RowData;

            if (selectedGeneralSkillRecordPrioritie != null)
            {
                productionPerformanceRatio = selectedGeneralSkillRecordPrioritie.Score;
                LineDataSource.ProductionPerformanceRatioName = selectedGeneralSkillRecordPrioritie.GeneralSkillName;
                SelectGeneralSkillRecordPriorities3PopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İşe Devam Oranı ButtonEdit

        SfTextBox GeneralSkillRecordPriorities4ButtonEdit;
        bool SelectGeneralSkillRecordPriorities4PopupVisible = false;
        List<ListGeneralSkillRecordPrioritiesDto> GeneralSkillRecordPriorities4List = new List<ListGeneralSkillRecordPrioritiesDto>();

        public async Task GeneralSkillRecordPriorities4OnCreateIcon()
        {
            var GeneralSkillRecordPriorities4ButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, GeneralSkillRecordPriorities4ButtonClickEvent);
            await GeneralSkillRecordPriorities4ButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", GeneralSkillRecordPriorities4ButtonClick } });
        }

        public async void GeneralSkillRecordPriorities4ButtonClickEvent()
        {
            SelectGeneralSkillRecordPriorities4PopupVisible = true;
            GeneralSkillRecordPriorities4List = (await GeneralSkillRecordPrioritiesAppService.GetListAsync(new ListGeneralSkillRecordPrioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void GeneralSkillRecordPriorities4OnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                attendanceRatio = 0;
                LineDataSource.AttendanceRatioName = string.Empty;
            }
        }

        public async void GeneralSkillRecordPriorities4DoubleClickHandler(RecordDoubleClickEventArgs<ListGeneralSkillRecordPrioritiesDto> args)
        {
            var selectedGeneralSkillRecordPrioritie = args.RowData;

            if (selectedGeneralSkillRecordPrioritie != null)
            {
                attendanceRatio = selectedGeneralSkillRecordPrioritie.Score;
                LineDataSource.AttendanceRatioName = selectedGeneralSkillRecordPrioritie.GeneralSkillName;
                SelectGeneralSkillRecordPriorities4PopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        #region Combobox İşlemleri

        #region Puantaj Durumu 

        public IEnumerable<SelectEmployeeScoringsDto> scoringStates = GetEnumDisplayScoringStatesNames<EmployeeScoringsStateEnum>();

        public static List<SelectEmployeeScoringsDto> GetEnumDisplayScoringStatesNames<T>()
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<EmployeeScoringsStateEnum>()
                       .Select(x => new SelectEmployeeScoringsDto
                       {
                           ScoringState = x,
                           ScoringStateName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();

        }

        #endregion

        #region Yıl

        public class YearComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<YearComboBox> _yearComboBox = new List<YearComboBox>
        {
            new YearComboBox(){ID = "23", Text="Combo23"},
            new YearComboBox(){ID = "24", Text="Combo24"},
            new YearComboBox(){ID = "25", Text="Combo25"},
            new YearComboBox(){ID = "26", Text="Combo26"},
            new YearComboBox(){ID = "27", Text="Combo27"},
            new YearComboBox(){ID = "28", Text="Combo28"},
            new YearComboBox(){ID = "29", Text="Combo29"},
            new YearComboBox(){ID = "30", Text="Combo30"},
            new YearComboBox(){ID = "31", Text="Combo31"},
        };

        private void YearComboBoxValueChangeHandler(ChangeEventArgs<string, YearComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "23":
                        DataSource.Year_ = 2023;
                        break;
                    case "24":
                        DataSource.Year_ = 2024;
                        break;
                    case "25":
                        DataSource.Year_ = 2025;
                        break;
                    case "26":
                        DataSource.Year_ = 2026;
                        break;
                    case "27":
                        DataSource.Year_ = 2027;
                        break;
                    case "28":
                        DataSource.Year_ = 2028;
                        break;
                    case "29":
                        DataSource.Year_ = 2029;
                        break;
                    case "30":
                        DataSource.Year_ = 2030;
                        break;
                    case "31":
                        DataSource.Year_ = 2031;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Ay

        public class MonthComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<MonthComboBox> _monthComboBox = new List<MonthComboBox>
        {
            new MonthComboBox(){ID = "1", Text="ComboJan"},
            new MonthComboBox(){ID = "2", Text="ComboFeb"},
            new MonthComboBox(){ID = "3", Text="ComboMar"},
            new MonthComboBox(){ID = "4", Text="ComboApr"},
            new MonthComboBox(){ID = "5", Text="ComboMay"},
            new MonthComboBox(){ID = "6", Text="ComboJun"},
            new MonthComboBox(){ID = "7", Text="ComboJul"},
            new MonthComboBox(){ID = "8", Text="ComboAug"},
            new MonthComboBox(){ID = "9", Text="ComboSep"},
            new MonthComboBox(){ID = "10", Text="ComboOct"},
            new MonthComboBox(){ID = "11", Text="ComboNov"},
            new MonthComboBox(){ID = "12", Text="ComboDec"},
        };

        private void MonhtComboBoxValueChangeHandler(ChangeEventArgs<string, MonthComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "1":
                        DataSource.Month_ = 1;
                        break;
                    case "2":
                        DataSource.Month_ = 2;
                        break;
                    case "3":
                        DataSource.Month_ = 3;
                        break;
                    case "4":
                        DataSource.Month_ = 4;
                        break;
                    case "5":
                        DataSource.Month_ = 5;
                        break;
                    case "6":
                        DataSource.Month_ = 6;
                        break;
                    case "7":
                        DataSource.Month_ = 7;
                        break;
                    case "8":
                        DataSource.Month_ = 8;
                        break;
                    case "9":
                        DataSource.Month_ = 9;
                        break;
                    case "10":
                        DataSource.Month_ = 10;
                        break;
                    case "11":
                        DataSource.Month_ = 11;
                        break;
                    case "12":
                        DataSource.Month_ = 12;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
