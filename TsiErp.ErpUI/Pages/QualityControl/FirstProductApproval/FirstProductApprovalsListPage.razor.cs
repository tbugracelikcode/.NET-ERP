using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Microsoft.AspNetCore.Components;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using Microsoft.SqlServer.Server;
using System.Timers;

namespace TsiErp.ErpUI.Pages.QualityControl.FirstProductApproval
{
    public partial class FirstProductApprovalsListPage : IDisposable
    {



        private SfGrid<SelectFirstProductApprovalLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectFirstProductApprovalLinesDto LineDataSource;

        SelectOperationalQualityPlansDto OperationalQualityPlanDataSource;

        SelectOperationPicturesDto OperationPictureDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectFirstProductApprovalLinesDto> GridLineList = new List<SelectFirstProductApprovalLinesDto>();

        List<SelectOperationalQualityPlanLinesDto> OperationalQualityPlanLineList = new List<SelectOperationalQualityPlanLinesDto>();

        private bool LineCrudPopup = false;
        private bool ImagePreviewPopup = false;
        public string previewImagePopupTitle = string.Empty;
        public string fileExtension = string.Empty;
        public string fileURL = string.Empty;
        bool image = false;
        bool pdf = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = FirstProductApprovalsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            StartTimer();
        }



        #region İlk Ürün Onay Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectFirstProductApprovalsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("FirstProductApprovalChildMenu")
            };

            DataSource.SelectFirstProductApprovalLines = new List<SelectFirstProductApprovalLinesDto>();
            GridLineList = DataSource.SelectFirstProductApprovalLines;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
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

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextRefresh"], Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListFirstProductApprovalsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":

                    if (_Timer.Enabled == true)
                    {
                        _Timer.Stop();
                        _Timer.Enabled = false;
                    }

                    IsChanged = true;
                    DataSource = (await FirstProductApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectFirstProductApprovalLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectFirstProductApprovalLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    LineDataSource = new SelectFirstProductApprovalLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.LineNr = GridLineList.Count + 1;


                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectFirstProductApprovalLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectFirstProductApprovalLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectFirstProductApprovalLines.Remove(line);
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

        public override void HideEditPage()
        {
            if (_Timer.Enabled == false)
            {
                _Timer.Start();
                _Timer.Enabled = true;
            }

            base.HideEditPage();
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectFirstProductApprovalLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectFirstProductApprovalLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectFirstProductApprovalLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectFirstProductApprovalLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectFirstProductApprovalLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectFirstProductApprovalLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectFirstProductApprovalLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public async void ShowOperationPictureButtonClicked()
        {
            if (OperationPictureDataSource != null)
            {
                if (OperationPictureDataSource.Id == Guid.Empty)
                {
                    await ModalManager.MessagePopupAsync(L["UIMessageOprPictureTitle"], L["UIMessageOprPictureMessage"]);
                }
                else
                {
                    string[] _SplitFileName = OperationPictureDataSource.UploadedFileName.Split('.');
                    fileExtension = _SplitFileName[1];

                    previewImagePopupTitle = OperationPictureDataSource.UploadedFileName;

                    if (fileExtension == "jpg" || fileExtension == "jpeg" || fileExtension == "png")
                    {
                        fileURL = OperationPictureDataSource.DrawingFilePath + OperationPictureDataSource.UploadedFileName;
                        image = true;
                        pdf = false;
                    }
                    else if (fileExtension == "pdf")
                    {
                        //fileURL = OperationPictureDataSource.DrawingDomain + OperationPictureDataSource.DrawingFilePath.Replace(@"\", "/") + OperationPictureDataSource.UploadedFileName;
                        fileURL = "wwwroot/" + OperationPictureDataSource.DrawingFilePath.Replace(@"\", "/") + OperationPictureDataSource.UploadedFileName;
                        pdf = true;
                        image = false;
                    }

                    ImagePreviewPopup = true;
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageOprPictureTitle"], L["UIMessageOprPictureNullMessage"]);
            }

        }

        public void HidePreviewPopup()
        {
            ImagePreviewPopup = false;
            pdf = false;
            image = false;
            previewImagePopupTitle = string.Empty;
            fileExtension = string.Empty;
            fileURL = string.Empty;
        }

        #endregion

        #region İş Emri ButtonEdit

        SfTextBox WorkOrdersButtonEdit;
        bool SelectWorkOrdersPopupVisible = false;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        public async Task WorkOrdersCodeOnCreateIcon()
        {
            var WorkOrdersCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WorkOrdersButtonClickEvent);
            await WorkOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WorkOrdersCodeButtonClick } });
        }

        public async void WorkOrdersButtonClickEvent()
        {
            SelectWorkOrdersPopupVisible = true;
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void WorkOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.ProductID = Guid.Empty;
                DataSource.WorkOrderNo = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.OperationQualityPlanID = Guid.Empty;
                DataSource.OperationQualityPlanDocumentNumber = string.Empty;
                OperationalQualityPlanLineList.Clear();
                GridLineList.Clear();
                _LineGrid.Refresh();
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedWorkOrder = args.RowData;

            if (selectedWorkOrder != null)
            {
                DataSource.WorkOrderID = selectedWorkOrder.Id;
                DataSource.WorkOrderNo = selectedWorkOrder.WorkOrderNo;
                DataSource.ProductID = selectedWorkOrder.ProductID;
                DataSource.ProductName = selectedWorkOrder.ProductName;
                DataSource.ProductCode = selectedWorkOrder.ProductCode;

                DataSource.OperationQualityPlanID = (await OperationalQualityPlansAppService.GetListAsync(new ListOperationalQualityPlansParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID && t.ProductsOperationID == selectedWorkOrder.ProductsOperationID).Select(t => t.Id).FirstOrDefault();

                OperationalQualityPlanDataSource = (await OperationalQualityPlansAppService.GetAsync(DataSource.OperationQualityPlanID.GetValueOrDefault())).Data;
                OperationalQualityPlanLineList = OperationalQualityPlanDataSource.SelectOperationalQualityPlanLines.Where(t => t.PeriodicControlMeasure == true).ToList();
                OperationPictureDataSource = OperationalQualityPlanDataSource.SelectOperationPictures.LastOrDefault();

                foreach (var qualityplanline in OperationalQualityPlanLineList)
                {
                    SelectFirstProductApprovalLinesDto firstProductApprovalLineModel = new SelectFirstProductApprovalLinesDto
                    {
                        BottomTolerance = qualityplanline.BottomTolerance,
                        Description_ = string.Empty,
                        IdealMeasure = qualityplanline.IdealMeasure,
                        IsCriticalMeasurement = qualityplanline.PeriodicControlMeasure,
                        LineNr = GridLineList.Count + 1,
                        UpperTolerance = qualityplanline.UpperTolerance,
                        MeasurementValue = string.Empty,
                    };

                    GridLineList.Add(firstProductApprovalLineModel);

                }
                await _LineGrid.Refresh();

                SelectWorkOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Çalışan ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        public async Task EmployeesCodeOnCreateIcon()
        {
            var EmployeesCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesCodeButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            SelectEmployeesPopupVisible = true;
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EmployeeID = Guid.Empty;
                DataSource.EmployeeName = string.Empty;
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                DataSource.EmployeeID = selectedEmployee.Id;
                DataSource.EmployeeName = selectedEmployee.Name;
                SelectEmployeesPopupVisible = false;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("FirstProductApprovalChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion



        #region Timer

        System.Timers.Timer _Timer;

        private void StartTimer()
        {
            _Timer = new System.Timers.Timer(10000);
            _Timer.Elapsed += _TimerTimedEvent;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }

        private async void _TimerTimedEvent(object source, ElapsedEventArgs e)
        {
            await base.GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }
        #endregion



        public void Dispose()
        {
            if (_Timer != null)
            {
                if (_Timer.Enabled)
                {
                    _Timer.Stop();
                    _Timer.Enabled = false;
                    _Timer.Dispose();
                }
            }

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
