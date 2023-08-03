using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Linq;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductOperationQualtityPlan.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductsOperation
{
    public partial class ProductsOperationsListPage
    {
        private SfGrid<SelectProductsOperationLinesDto> _LineGrid;
        private SfGrid<SelectProductOperationQualityPlansDto> _QualityPlanGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectProductsOperationLinesDto StationLineDataSource;

        SelectProductOperationQualityPlansDto QualityPlansDataSource;

        public List<ContextMenuItemModel> StationLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> QualityPlanGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductsOperationLinesDto> StationGridLineList = new List<SelectProductsOperationLinesDto>();

        List<SelectProductOperationQualityPlansDto> QulityPlanGridLineList = new List<SelectProductOperationQualityPlansDto>();


        private bool StationLineCrudPopup = false;

        private bool QualityPlanLineCrudPopup = false;

        protected override async void OnInitialized()
        {
            CreateMainContextMenuItems();
            CreateStationLineContextMenuItems();
            CreateQualityPlanContextMenuItems();

            BaseCrudService = ProductsOperationsAppService;
            _L = L;

        }

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
            SelectProductsPopupVisible = true;
            await GetProductsList();
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
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
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
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StationLineDataSource.StationID = Guid.Empty;
                StationLineDataSource.StationCode = string.Empty;
                StationLineDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StationLineDataSource.StationID = selectedUnitSet.Id;
                StationLineDataSource.StationCode = selectedUnitSet.Code;
                StationLineDataSource.StationName = selectedUnitSet.Code;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şablon Operasyon ButtonEdit

        SfTextBox TemplateOperationButtonEdit;
        bool SelectTemplateOperationsPopupVisible = false;
        List<ListTemplateOperationsDto> TemplateOperationsList = new List<ListTemplateOperationsDto>();

        public async Task TemplateOperationOnCreateIcon()
        {
            var TemplateOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TemplateOperationButtonClickEvent);
            await TemplateOperationButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TemplateOperationsButtonClick } });
        }

        public async void TemplateOperationButtonClickEvent()
        {
            SelectTemplateOperationsPopupVisible = true;
            await GetTemplateOperationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void TemplateOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TemplateOperationID = Guid.Empty;
                DataSource.TemplateOperationCode = string.Empty;
                DataSource.TemplateOperationName = string.Empty;
            }
        }

        public async void TemplateOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListTemplateOperationsDto> args)
        {
            var selectedTemplateOperation = args.RowData;

            if (selectedTemplateOperation != null)
            {
                DataSource.TemplateOperationID = selectedTemplateOperation.Id;
                DataSource.TemplateOperationCode = selectedTemplateOperation.Code;
                DataSource.TemplateOperationName = selectedTemplateOperation.Code;
                foreach (var line in selectedTemplateOperation.SelectTemplateOperationLines)
                {
                    SelectProductsOperationLinesDto lineModel = new SelectProductsOperationLinesDto
                    {
                        AdjustmentAndControlTime = line.AdjustmentAndControlTime,
                        Alternative = line.Alternative,
                        LineNr = line.LineNr,
                        StationID = line.StationID,
                        StationCode = line.StationCode,
                        StationName = line.StationName,
                        ProductsOperationName = DataSource.Name,
                        ProductsOperationID = DataSource.Id,
                        ProductsOperationCode = DataSource.Code,
                        ProcessQuantity = line.ProcessQuantity,
                        Priority = line.Priority,
                        OperationTime = line.OperationTime,
                    };

                    StationGridLineList.Add(lineModel);
                    await _LineGrid.Refresh();
                }
                SelectTemplateOperationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Kontrol Türü Button Edit

        SfTextBox ControlTypesCodeButtonEdit;
        SfTextBox ControlTypesNameButtonEdit;
        bool SelectControlTypesPopupVisible = false;
        List<ListControlTypesDto> ControlTypesList = new List<ListControlTypesDto>();
        public async Task ControlTypesCodeOnCreateIcon()
        {
            var ControlTypesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlTypesCodeButtonClickEvent);
            await ControlTypesCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlTypesButtonClick } });
        }

        public async void ControlTypesCodeButtonClickEvent()
        {
            SelectControlTypesPopupVisible = true;
            await GetControlTypesList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ControlTypesNameOnCreateIcon()
        {
            var ControlTypesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlTypesNameButtonClickEvent);
            await ControlTypesNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlTypesButtonClick } });
        }

        public async void ControlTypesNameButtonClickEvent()
        {
            SelectControlTypesPopupVisible = true;
            await GetControlTypesList();
            await InvokeAsync(StateHasChanged);
        }

        public void ControlTypesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                QualityPlansDataSource.ControlTypesID = Guid.Empty;
                QualityPlansDataSource.ControlTypesName = string.Empty;
            }
        }

        public async void ControlTypesDoubleClickHandler(RecordDoubleClickEventArgs<ListControlTypesDto> args)
        {
            var selectedControlType = args.RowData;

            if (selectedControlType != null)
            {
                QualityPlansDataSource.ControlTypesID = selectedControlType.Id;
                QualityPlansDataSource.ControlTypesName = selectedControlType.Name;
                SelectControlTypesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Kontrol Şartı Button Edit

        SfTextBox ControlConditionsCodeButtonEdit;
        SfTextBox ControlConditionsNameButtonEdit;
        bool SelectControlConditionsPopupVisible = false;
        List<ListControlConditionsDto> ControlConditionsList = new List<ListControlConditionsDto>();
        public async Task ControlConditionsCodeOnCreateIcon()
        {
            var ControlConditionsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlConditionsCodeButtonClickEvent);
            await ControlConditionsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlConditionsButtonClick } });
        }

        public async void ControlConditionsCodeButtonClickEvent()
        {
            SelectControlConditionsPopupVisible = true;
            await GetControlConditionsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ControlConditionsNameOnCreateIcon()
        {
            var ControlConditionsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlConditionsNameButtonClickEvent);
            await ControlConditionsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlConditionsButtonClick } });
        }

        public async void ControlConditionsNameButtonClickEvent()
        {
            SelectControlConditionsPopupVisible = true;
            await GetControlConditionsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ControlConditionsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                QualityPlansDataSource.ControlConditionsID = Guid.Empty;
                QualityPlansDataSource.ControlConditionsName = string.Empty;
            }
        }

        public async void ControlConditionsDoubleClickHandler(RecordDoubleClickEventArgs<ListControlConditionsDto> args)
        {
            var selectedControlCondition = args.RowData;

            if (selectedControlCondition != null)
            {
                QualityPlansDataSource.ControlConditionsID = selectedControlCondition.Id;
                QualityPlansDataSource.ControlConditionsName = selectedControlCondition.Name;
                SelectControlConditionsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İstasyon Grubu ButtonEdit

        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            StationGroupList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                QualityPlansDataSource.WorkCenterID = Guid.Empty;
                QualityPlansDataSource.WorkCenterName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                QualityPlansDataSource.WorkCenterID = selectedStationGroup.Id;
                QualityPlansDataSource.WorkCenterName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Ürüne Özel Operasyon Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsOperationsDto()
            {
                IsActive = true
            };

            DataSource.SelectProductsOperationLines = new List<SelectProductsOperationLinesDto>();
            StationGridLineList = DataSource.SelectProductsOperationLines;

            DataSource.SelectProductOperationQualityPlans = new List<SelectProductOperationQualityPlansDto>();
            QulityPlanGridLineList = DataSource.SelectProductOperationQualityPlans;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected async Task StationLineBeforeInsertAsync()
        {
            StationLineDataSource = new SelectProductsOperationLinesDto()
            {
                Alternative = false,
                Priority = StationGridLineList.Count + 1,
                LineNr = StationGridLineList.Count + 1
            };

            await Task.CompletedTask;
        }

        protected void CreateStationLineContextMenuItems()
        {
            if (StationLineGridContextMenu.Count() == 0)
            {
                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductsOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await ProductsOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    StationGridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();
                    QulityPlanGridLineList = DataSource.SelectProductOperationQualityPlans.OrderBy(t => t.MeasureNumberInPicture).ToList();

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

                default:
                    break;
            }
        }

        public async void StationLineContextMenuClick(ContextMenuClickEventArgs<SelectProductsOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    StationLineDataSource = new SelectProductsOperationLinesDto();
                    StationLineCrudPopup = true;
                    await StationLineBeforeInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    StationLineDataSource = args.RowInfo.RowData;
                    StationLineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {

                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectProductsOperationLines.Remove(args.RowInfo.RowData);



                            await _LineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = StationGridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {
                                    int selectedPriority = StationGridLineList[selectedIndex].Priority;


                                    StationGridLineList.Remove(line);

                                    for (int i = 0; i < StationGridLineList.Count; i++)
                                    {
                                        StationGridLineList[i].LineNr = i + 1;
                                        StationGridLineList[i].Priority = i + 1;
                                    }

                                    DataSource.SelectProductsOperationLines = StationGridLineList;

                                    await _LineGrid.Refresh();

                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                DataSource.SelectProductsOperationLines.Remove(line);
                            }
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
        }

        public void HideStationLinesPopup()
        {
            StationLineCrudPopup = false;
        }

        public void HideQualityPlanLinesPopup()
        {
            QualityPlanLineCrudPopup = false;
        }

        protected async Task OnQualityPlanLineSubmit()
        {
            if (QualityPlansDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectProductOperationQualityPlans.Contains(QualityPlansDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductOperationQualityPlans.FindIndex(t => t.LineNr == QualityPlansDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductOperationQualityPlans[selectedLineIndex] = QualityPlansDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductOperationQualityPlans.Add(QualityPlansDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductOperationQualityPlans.FindIndex(t => t.Id == QualityPlansDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductOperationQualityPlans[selectedLineIndex] = QualityPlansDataSource;
                }
            }

            QulityPlanGridLineList = DataSource.SelectProductOperationQualityPlans.OrderBy(t => t.MeasureNumberInPicture).ToList();

            await _QualityPlanGrid.Refresh();

            HideQualityPlanLinesPopup();

            await InvokeAsync(StateHasChanged);
        }

        protected async Task OnStationLineSubmit()
        {
            if (StationLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectProductsOperationLines.Contains(StationLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.LineNr == StationLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductsOperationLines[selectedLineIndex] = StationLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductsOperationLines.Add(StationLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.Id == StationLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductsOperationLines[selectedLineIndex] = StationLineDataSource;
                }
            }

            StationGridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();

            await _LineGrid.Refresh();

            HideStationLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        public async void ArrowUpBtnClicked()
        {
            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == 0))
            {
                StationGridLineList[index].Priority -= 1;
                StationGridLineList[index - 1].Priority += 1;
                StationGridLineList[index].LineNr -= 1;
                StationGridLineList[index - 1].LineNr += 1;

                StationGridLineList = StationGridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = StationGridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == StationGridLineList.Count()))
            {
                StationGridLineList[index].Priority += 1;
                StationGridLineList[index + 1].Priority -= 1;
                StationGridLineList[index].LineNr += 1;
                StationGridLineList[index + 1].LineNr -= 1;

                StationGridLineList = StationGridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = StationGridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Kalite Planı Grid İşlemleri

        protected void CreateQualityPlanContextMenuItems()
        {
            if (QualityPlanGridContextMenu.Count() == 0)
            {
                QualityPlanGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                QualityPlanGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                QualityPlanGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                QualityPlanGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected async Task QualityPlanBeforeInsertAsync()
        {
            QualityPlansDataSource = new SelectProductOperationQualityPlansDto()
            {
                LineNr = QulityPlanGridLineList.Count + 1
            };

            await Task.CompletedTask;
        }

        public async void QualityPlanContextMenuClick(ContextMenuClickEventArgs<SelectProductOperationQualityPlansDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    QualityPlansDataSource = new SelectProductOperationQualityPlansDto();
                    QualityPlanLineCrudPopup = true;
                    await QualityPlanBeforeInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    QualityPlansDataSource = args.RowInfo.RowData;
                    QualityPlanLineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {

                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectProductOperationQualityPlans.Remove(args.RowInfo.RowData);



                            await _QualityPlanGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = QulityPlanGridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {

                                    QulityPlanGridLineList.Remove(line);

                                    for (int i = 0; i < QulityPlanGridLineList.Count; i++)
                                    {
                                        QulityPlanGridLineList[i].LineNr = i + 1;
                                    }

                                    DataSource.SelectProductOperationQualityPlans = QulityPlanGridLineList;

                                    await _QualityPlanGrid.Refresh();

                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                DataSource.SelectProductOperationQualityPlans.Remove(line);
                            }
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _QualityPlanGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetTemplateOperationsList()
        {
            TemplateOperationsList = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.ToList();
        }

        private async Task GetControlTypesList()
        {
            ControlTypesList = (await ControlTypesAppService.GetListAsync(new ListControlTypesParameterDto())).Data.ToList();
        }

        private async Task GetControlConditionsList()
        {
            ControlConditionsList = (await ControlConditionsAppService.GetListAsync(new ListControlConditionsParameterDto())).Data.ToList();
        }

        #endregion
    }
}

