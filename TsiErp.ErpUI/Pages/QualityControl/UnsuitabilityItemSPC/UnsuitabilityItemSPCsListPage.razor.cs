using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.ContractUnsuitabilityReport.Services;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItemSPC
{
    public partial class UnsuitabilityItemSPCsListPage
    {
        private SfGrid<SelectUnsuitabilityItemSPCLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectUnsuitabilityItemSPCLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectUnsuitabilityItemSPCLinesDto> GridLineList = new List<SelectUnsuitabilityItemSPCLinesDto>();

        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        private bool LineCrudPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = UnsuitabilityItemSPCsService;
            _L = L;
            CreateMainContextMenuItems();
            //CreateLineContextMenuItems();

        }

        #region Operasyonel SPC Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemSPCsDto()
            {
                Date_ = DateTime.Today,
                MeasurementEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1),
                MeasurementStartDate = DateTime.Today,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UnsuitabilityItemSPSChildMenu")
            };

            DataSource.SelectUnsuitabilityItemSPCLines = new List<SelectUnsuitabilityItemSPCLinesDto>();
            GridLineList = DataSource.SelectUnsuitabilityItemSPCLines;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListUnsuitabilityItemSPCsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await UnsuitabilityItemSPCsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectUnsuitabilityItemSPCLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectUnsuitabilityItemSPCLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    LineDataSource = new SelectUnsuitabilityItemSPCLinesDto();
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

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectUnsuitabilityItemSPCLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectUnsuitabilityItemSPCLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectUnsuitabilityItemSPCLines.Remove(line);
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

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectUnsuitabilityItemSPCLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectUnsuitabilityItemSPCLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectUnsuitabilityItemSPCLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectUnsuitabilityItemSPCLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectUnsuitabilityItemSPCLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectUnsuitabilityItemSPCLines[selectedLineIndex] = LineDataSource;
                }
            }
            GridLineList = DataSource.SelectUnsuitabilityItemSPCLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }


        public async void Calculate()
        {

            if (DataSource.MeasurementStartDate == DataSource.MeasurementEndDate)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningDateTitleBase"], L["UIWarningDateMessageBase"]);
            }
            else if (DataSource.MeasurementStartDate > DataSource.MeasurementEndDate)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningDateTitleBase"], L["UIWarningDate2MessageBase"]);

            }
            else
            {

                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.ToList();

                foreach (var unsuitabilityItem in UnsuitabilityItemsList)
                {
                    var unsuitabilityType = (await UnsuitabilityTypesItemsAppService.GetAsync(unsuitabilityItem.UnsuitabilityTypesItemsId)).Data;

                    var workCenter = (await StationGroupsAppService.GetAsync(unsuitabilityItem.StationGroupId)).Data;

                    #region Toplam Uygunsuz Komponent Hesaplama

                    var totalUnsuitableComponent = (await ContractUnsuitabilityReportsAppService.GetListAsync(new ListContractUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Select(t => t.UnsuitableAmount).Sum() + (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Select(t => t.UnsuitableAmount).Sum() + (await PurchaseUnsuitabilityReportsAppService.GetListAsync(new ListPurchaseUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Select(t => t.UnsuitableAmount).Sum();

                    #endregion

                    #region Toplam Uygunsuz Rapor Hesaplama

                    var totalUnsuitableReport = (await ContractUnsuitabilityReportsAppService.GetListAsync(new ListContractUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Count() + (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Count() + (await PurchaseUnsuitabilityReportsAppService.GetListAsync(new ListPurchaseUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).Count();

                    #endregion

                    SelectUnsuitabilityItemSPCLinesDto selectSPCLineModel = new SelectUnsuitabilityItemSPCLinesDto
                    {
                        Detectability = 10,
                        Frequency = 10,
                        LineNr = GridLineList.Count + 1,
                        WorkCenterID = workCenter.Id,
                        UnsuitabilityTypeName = unsuitabilityType.Name,
                        WorkCenterName = workCenter.Name,
                        UnsuitabilityTypeID = unsuitabilityType.Id,
                        UnsuitabilityItemIntensityCoefficient = unsuitabilityItem.IntensityCoefficient,
                        TotalUnsuitableComponent = Convert.ToInt32(totalUnsuitableComponent),
                        TotalUnsuitableReport = totalUnsuitableReport,
                        UnsuitableComponentPerReport = (Convert.ToInt32(totalUnsuitableComponent) / totalUnsuitableReport),
                        RPN = 10 * 10 * unsuitabilityItem.IntensityCoefficient
                    };

                    GridLineList.Add(selectSPCLineModel);
                }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UnsuitabilityItemSPSChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
