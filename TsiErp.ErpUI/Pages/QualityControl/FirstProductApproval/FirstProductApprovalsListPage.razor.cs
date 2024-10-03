using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Timers;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.FirstProductApproval
{
    public partial class FirstProductApprovalsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        private SfGrid<SelectFirstProductApprovalLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectFirstProductApprovalLinesDto LineDataSource;

        SelectOperationUnsuitabilityReportsDto OperationUnsuitabilityDataSource;

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
        //bool pdf = new();
        public bool isUnsuitabilityReportVisible = false;
        public bool isUnsuitabilityReportTabVisible = false;
        public bool CreateUnsuitabilityReport = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = FirstProductApprovalsAppService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "FirstProductApprovalChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            StartTimer();
        }



        #region İlk Ürün Onay Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectFirstProductApprovalsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("FirstProductApprovalChildMenu"),
                ControlDate = DateTime.Today
            };

            isUnsuitabilityReportVisible = true;
            CreateUnsuitabilityReport = false;
            isUnsuitabilityReportTabVisible = false;

            DataSource.SelectFirstProductApprovalLines = new List<SelectFirstProductApprovalLinesDto>();
            GridLineList = DataSource.SelectFirstProductApprovalLines;

            OperationPictureDataSource = new SelectOperationPicturesDto();

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

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    isUnsuitabilityReportVisible = false;

                    DataSource.EmployeeName = DataSource.EmployeeName + " " + DataSource.EmployeeSurname;

                    CreateUnsuitabilityReport = false;
                    isUnsuitabilityReportTabVisible = false;
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
                            case "FirstProductApprovalLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextAdd"], Id = "new" }); break;
                            case "FirstProductApprovalLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextChange"], Id = "changed" }); break;
                            case "FirstProductApprovalLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextDelete"], Id = "delete" }); break;
                            case "FirstProductApprovalLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalLineContextRefresh"], Id = "refresh" }); break;
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
                            case "FirstProductApprovalContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextAdd"], Id = "new" }); break;
                            case "FirstProductApprovalContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextChange"], Id = "changed" }); break;
                            case "FirstProductApprovalContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextDelete"], Id = "delete" }); break;
                            case "FirstProductApprovalContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["FirstProductApprovalContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
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

                    if (args.RowInfo.RowData != null)
                    {

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

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectFirstProductApprovalLinesDto> args)
        {
            if (DataSource.OperationQualityPlanID != null && DataSource.OperationQualityPlanID != Guid.Empty)
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
                        if (args.RowInfo.RowData != null)
                        {

                            LineDataSource = args.RowInfo.RowData;
                            LineCrudPopup = true;
                            await InvokeAsync(StateHasChanged);
                        }
                        break;

                    case "delete":

                        if (args.RowInfo.RowData != null)
                        {

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

                if (args.RowInfo.RowData != null)
                {
                    args.RowInfo.RowData = null;
                }
            }

            else
            {
                await ModalManager.WarningPopupAsync(L["UIQualityPlanMessageTitle"], L["UIQualityPlanMessageMessage"]);
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
                    if (fileExtension == "pdf")
                    {
                        //fileURL = OperationPictureDataSource.DrawingDomain + OperationPictureDataSource.DrawingFilePath.Replace(@"\", "/") + OperationPictureDataSource.UploadedFileName;

                        string webEnvironment = FileUploadService.GetRootPath();

                        fileURL = webEnvironment + @"\UploadedFiles" + OperationPictureDataSource.DrawingFilePath.Split("UploadedFiles")[1] + OperationPictureDataSource.UploadedFileName;
                        //pdf = true;
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
            //pdf = false;
            image = false;
            previewImagePopupTitle = string.Empty;
            fileExtension = string.Empty;
            fileURL = string.Empty;
        }

        protected override async Task OnSubmit()
        {
            #region Operasyon Uygunsuzluk Kaydı

            if (CreateUnsuitabilityReport && DataSource.ScrapQuantity > 0)
            {

                #region İlk Ürün Onay Kaydı

                if (DataSource.OperationQualityPlanID != null && DataSource.OperationQualityPlanID != Guid.Empty)
                {
                    SelectFirstProductApprovalsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, CreateFirstProductApprovalsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, UpdateFirstProductApprovalsDto>(DataSource);

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

                }

                else
                {
                    await ModalManager.WarningPopupAsync(L["UIQualityPlanMessageTitle"], L["UIQualityPlanMessageMessage"]);
                }

                #endregion

                #region Uygunsuzluk Kayıt

                if (OperationUnsuitabilityDataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectOperationUnsuitabilityReportsDto, CreateOperationUnsuitabilityReportsDto>(OperationUnsuitabilityDataSource);

                    await OperationUnsuitabilityReportsAppService.CreateAsync(createInput);



                }

                else
                {
                    var updateInput = ObjectMapper.Map<SelectOperationUnsuitabilityReportsDto, UpdateOperationUnsuitabilityReportsDto>(OperationUnsuitabilityDataSource);

                    await OperationUnsuitabilityReportsAppService.UpdateAsync(updateInput);
                }

                #endregion

            }

            else if (CreateUnsuitabilityReport && DataSource.ScrapQuantity == 0)
            {
                var res = await ModalManager.ConfirmationAsync(L["UIScrapQuantityTitle"], L["UIScrapQuantityMessage"]);

                if (res == true)
                {
                    #region İlk Ürün Onay Kaydı

                    if (DataSource.OperationQualityPlanID != null && DataSource.OperationQualityPlanID != Guid.Empty)
                    {
                        SelectFirstProductApprovalsDto result;

                        if (DataSource.Id == Guid.Empty)
                        {
                            var createInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, CreateFirstProductApprovalsDto>(DataSource);

                            result = (await CreateAsync(createInput)).Data;

                            if (result != null)
                                DataSource.Id = result.Id;
                        }
                        else
                        {
                            var updateInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, UpdateFirstProductApprovalsDto>(DataSource);

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

                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIQualityPlanMessageTitle"], L["UIQualityPlanMessageMessage"]);
                    }

                    #endregion
                }
            }

            else if (!CreateUnsuitabilityReport)
            {
                #region İlk Ürün Onay Kaydı

                if (DataSource.OperationQualityPlanID != null && DataSource.OperationQualityPlanID != Guid.Empty)
                {
                    SelectFirstProductApprovalsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, CreateFirstProductApprovalsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectFirstProductApprovalsDto, UpdateFirstProductApprovalsDto>(DataSource);

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

                }

                else
                {
                    await ModalManager.WarningPopupAsync(L["UIQualityPlanMessageTitle"], L["UIQualityPlanMessageMessage"]);
                }

                #endregion
            }

            #endregion
        }

        private async void CreateUnsuitabilityReportChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (CreateUnsuitabilityReport)
            {
                SelectWorkOrdersDto workOrderDataSource = new SelectWorkOrdersDto();

                if (DataSource.WorkOrderID != null && DataSource.WorkOrderID != Guid.Empty)
                {
                    workOrderDataSource = (await WorkOrdersAppService.GetAsync(DataSource.WorkOrderID.Value)).Data;
                }

                OperationUnsuitabilityDataSource = new SelectOperationUnsuitabilityReportsDto
                {
                    Date_ = GetSQLDateAppService.GetDateFromSQL(),
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("OprUnsRecordsChildMenu"),
                    UnsuitableAmount = DataSource.ScrapQuantity,
                    WorkOrderID = workOrderDataSource.Id,
                    WorkOrderNo = workOrderDataSource.WorkOrderNo,
                    ProductID = workOrderDataSource.ProductID,
                    ProductCode = workOrderDataSource.ProductCode,
                    ProductName = workOrderDataSource.ProductName,
                    StationGroupID = workOrderDataSource.StationGroupID,
                    StationGroupCode = workOrderDataSource.StationGroupCode,
                    OperationID = workOrderDataSource.ProductsOperationID,
                    OperationCode = workOrderDataSource.ProductsOperationCode,
                    ProductionOrderID = workOrderDataSource.ProductionOrderID,
                    ProductionOrderFicheNo = workOrderDataSource.ProductionOrderFicheNo
                };

                foreach (var item in _unsComboBox)
                {
                    item.Text = L[item.Text];
                }

                isUnsuitabilityReportTabVisible = true;
            }
            else
            {
                isUnsuitabilityReportTabVisible = false;
            }
        }

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            switch (args.ItemData.ID)
            {
                case "Scrap":
                    OperationUnsuitabilityDataSource.Action_ = L["ComboboxScrap"].Value;
                    break;

                case "Correction":
                    OperationUnsuitabilityDataSource.Action_ = L["ComboboxCorrection"].Value;
                    break;

                case "ToBeUsedAs":
                    OperationUnsuitabilityDataSource.Action_ = L["ComboboxToBeUsedAs"].Value;
                    break;

                default: break;
            }
        }

        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Scrap", Text="ComboboxScrap"},
            new UnsComboBox(){ID = "Correction", Text="ComboboxCorrection"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="ComboboxToBeUsedAs"}
        };

        private void ScrapValueChangeHandler(Syncfusion.Blazor.Inputs.ChangeEventArgs<decimal> args)
        {
            if (OperationUnsuitabilityDataSource != null && OperationUnsuitabilityDataSource.Id != Guid.Empty)
            {
                OperationUnsuitabilityDataSource.UnsuitableAmount = DataSource.ScrapQuantity;
            }
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
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.OperationQualityPlanID = Guid.Empty;
                DataSource.OperationQualityPlanDocumentNumber = string.Empty;
                if (OperationUnsuitabilityDataSource != null)
                {
                    OperationUnsuitabilityDataSource.WorkOrderID = Guid.Empty;
                    OperationUnsuitabilityDataSource.WorkOrderNo = string.Empty;
                    OperationUnsuitabilityDataSource.ProductID = Guid.Empty;
                    OperationUnsuitabilityDataSource.ProductCode = string.Empty;
                    OperationUnsuitabilityDataSource.ProductName = string.Empty;
                    OperationUnsuitabilityDataSource.StationGroupID = Guid.Empty;
                    OperationUnsuitabilityDataSource.StationGroupCode = string.Empty;
                    OperationUnsuitabilityDataSource.OperationID = Guid.Empty;
                    OperationUnsuitabilityDataSource.OperationCode = string.Empty;
                    OperationUnsuitabilityDataSource.ProductionOrderID = Guid.Empty;
                    OperationUnsuitabilityDataSource.ProductionOrderFicheNo = string.Empty;
                }
                OperationalQualityPlanLineList.Clear();
                GridLineList.Clear();
                _LineGrid.Refresh();
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            GridLineList.Clear();

            var selectedWorkOrder = args.RowData;

            if (selectedWorkOrder != null)
            {
                DataSource.WorkOrderID = selectedWorkOrder.Id;
                DataSource.WorkOrderNo = selectedWorkOrder.WorkOrderNo;
                DataSource.ProductID = selectedWorkOrder.ProductID;
                DataSource.ProductName = selectedWorkOrder.ProductName;
                DataSource.ProductCode = selectedWorkOrder.ProductCode;
                DataSource.ProductionOrderID = selectedWorkOrder.ProductionOrderID;

                if (OperationUnsuitabilityDataSource != null)
                {
                    OperationUnsuitabilityDataSource.WorkOrderID = selectedWorkOrder.Id;
                    OperationUnsuitabilityDataSource.WorkOrderNo = selectedWorkOrder.WorkOrderNo;
                    OperationUnsuitabilityDataSource.ProductID = selectedWorkOrder.ProductID;
                    OperationUnsuitabilityDataSource.ProductCode = selectedWorkOrder.ProductCode;
                    OperationUnsuitabilityDataSource.ProductName = selectedWorkOrder.ProductName;
                    OperationUnsuitabilityDataSource.StationGroupID = selectedWorkOrder.StationGroupID;
                    OperationUnsuitabilityDataSource.StationGroupCode = selectedWorkOrder.StationGroupCode;
                    OperationUnsuitabilityDataSource.OperationID = selectedWorkOrder.ProductsOperationID;
                    OperationUnsuitabilityDataSource.OperationCode = selectedWorkOrder.ProductsOperationCode;
                    OperationUnsuitabilityDataSource.ProductionOrderID = selectedWorkOrder.ProductionOrderID;
                    OperationUnsuitabilityDataSource.ProductionOrderFicheNo = selectedWorkOrder.ProductionOrderFicheNo;
                }


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
                        MeasureNumberInPicture = qualityplanline.MeasureNumberInPicture
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
                DataSource.EmployeeName = selectedEmployee.Name + " " + selectedEmployee.Surname;
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

        #region Operasyon Uygunsuzluk Kaydı Kod ButtonEdit

        SfTextBox OperationUnsuitabilityCodeButtonEdit;

        public async Task OperationUnsuitabilityCodeOnCreateIcon()
        {
            var OperationUnsuitabilityCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, OperationUnsuitabilityCodeButtonClickEvent);
            await OperationUnsuitabilityCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", OperationUnsuitabilityCodesButtonClick } });
        }

        public async void OperationUnsuitabilityCodeButtonClickEvent()
        {
            OperationUnsuitabilityDataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("OprUnsRecordsChildMenu");
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

        #region Hata Başlığı ButtonEdit

        SfTextBox UnsuitabilityItemsButtonEdit;
        bool SelectUnsuitabilityItemsPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemsOnCreateIcon()
        {
            var UnsuitabilityItemsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemsButtonClickEvent);
            await UnsuitabilityItemsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemsButtonClick } });
        }

        public async void UnsuitabilityItemsButtonClickEvent()
        {

            SelectUnsuitabilityItemsPopupVisible = true;
            await GetUnsuitabilityItemsList();

            await InvokeAsync(StateHasChanged);
        }

        private async Task GetUnsuitabilityItemsList()
        {
            var IsMerkeziId = (await StationsAppService.GetAsync(OperationUnsuitabilityDataSource.StationID.GetValueOrDefault())).Data.GroupID;
            if (IsMerkeziId != Guid.Empty)
            {

                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.
                    Where(t => t.StationGroupId == IsMerkeziId)
                    .ToList();
            }
        }

        public void UnsuitabilityItemsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                OperationUnsuitabilityDataSource.UnsuitabilityItemsID = Guid.Empty;
                OperationUnsuitabilityDataSource.UnsuitabilityItemsName = string.Empty;
            }
        }

        public async void UnsuitabilityItemsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedUnsuitabilityItem = args.RowData;

            if (selectedUnsuitabilityItem != null)
            {
                OperationUnsuitabilityDataSource.UnsuitabilityItemsID = selectedUnsuitabilityItem.Id;
                OperationUnsuitabilityDataSource.UnsuitabilityItemsName = selectedUnsuitabilityItem.Name;
                SelectUnsuitabilityItemsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Personel ButtonEdit

        SfTextBox OprUnsEmployeesButtonEdit;
        bool SelectOprUnsEmployeesPopupVisible = false;
        List<ListEmployeesDto> OprUnsEmployeesList = new List<ListEmployeesDto>();

        public async Task OprUnsEmployeesOnCreateIcon()
        {
            var OprUnsEmployeesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, OprUnsEmployeesButtonClickEvent);
            await OprUnsEmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", OprUnsEmployeesButtonClick } });
        }

        public async void OprUnsEmployeesButtonClickEvent()
        {
            if (OperationUnsuitabilityDataSource.WorkOrderID == Guid.Empty || OperationUnsuitabilityDataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageEmployeebase"]);
            }
            else
            {
                SelectOprUnsEmployeesPopupVisible = true;
                await GetOprUnsEmployeesList();

            }
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetOprUnsEmployeesList()
        {
            OprUnsEmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        public void OprUnsEmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                OperationUnsuitabilityDataSource.EmployeeID = Guid.Empty;
                OperationUnsuitabilityDataSource.EmployeeName = string.Empty;
            }
        }

        public async void OprUnsEmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                OperationUnsuitabilityDataSource.EmployeeID = selectedOrder.Id;
                OperationUnsuitabilityDataSource.EmployeeName = selectedOrder.Name + " " + selectedOrder.Surname;
                SelectOprUnsEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<SelectStationsDto> StationsList = new List<SelectStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            if (OperationUnsuitabilityDataSource.WorkOrderID == Guid.Empty || OperationUnsuitabilityDataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageStationbase"]);
            }
            else
            {
                SelectStationsPopupVisible = true;
                await GetStationsList();

            }
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetStationsList()
        {
            StationsList.Clear();
            var productOperationId = (await WorkOrdersAppService.GetAsync(OperationUnsuitabilityDataSource.WorkOrderID.GetValueOrDefault())).Data.ProductsOperationID;

            if (productOperationId != Guid.Empty)
            {
                var productOperation = (await ProductsOperationsAppService.GetAsync(productOperationId.GetValueOrDefault())).Data;

                foreach (var item in productOperation.SelectProductsOperationLines)
                {
                    var selectStationDto = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data;
                    StationsList.Add(selectStationDto);
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["EmptyProductOperationError"]);
                await InvokeAsync(StateHasChanged);
            }

            //StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            if (DataSource.WorkOrderID == Guid.Empty || DataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageStationbase"]);
            }
            else
            {
                SelectStationsPopupVisible = true;
                await GetStationsList();

            }
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                OperationUnsuitabilityDataSource.StationID = Guid.Empty;
                OperationUnsuitabilityDataSource.StationCode = string.Empty;
                OperationUnsuitabilityDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<SelectStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                OperationUnsuitabilityDataSource.StationID = selectedUnitSet.Id;
                OperationUnsuitabilityDataSource.StationCode = selectedUnitSet.Code;
                OperationUnsuitabilityDataSource.StationName = selectedUnitSet.Name;
                OperationUnsuitabilityDataSource.StationGroupID = selectedUnitSet.GroupID;
                OperationUnsuitabilityDataSource.StationGroupName = selectedUnitSet.StationGroup;
                OperationUnsuitabilityDataSource.StationGroupCode = selectedUnitSet.StationGroupCode;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
