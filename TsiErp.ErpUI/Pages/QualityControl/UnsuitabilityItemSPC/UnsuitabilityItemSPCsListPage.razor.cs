using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.ContractUnsuitabilityReport.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
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
    public partial class UnsuitabilityItemSPCsListPage : IDisposable
    {
        private SfGrid<SelectUnsuitabilityItemSPCLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectUnsuitabilityItemSPCLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectUnsuitabilityItemSPCLinesDto> GridLineList = new List<SelectUnsuitabilityItemSPCLinesDto>();

        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        //bool LineCrudPopup = new();

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = UnsuitabilityItemSPCsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "UnsuitabilityItemSPSChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            //CreateLineContextMenuItems();

        }

        #region Operasyonel SPC Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemSPCsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                MeasurementEndDate = new DateTime(GetSQLDateAppService.GetDateFromSQL().Year, GetSQLDateAppService.GetDateFromSQL().Month + 1, 1).AddDays(-1),
                MeasurementStartDate = GetSQLDateAppService.GetDateFromSQL(),
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

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "UnsuitabilityItemSPCLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCLineContextAdd"], Id = "new" }); break;
                            case "UnsuitabilityItemSPCLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCLineContextChange"], Id = "changed" }); break;
                            case "UnsuitabilityItemSPCLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCLineContextDelete"], Id = "delete" }); break;
                            case "UnsuitabilityItemSPCLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCLineContextRefresh"], Id = "refresh" }); break;
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
                            case "UnsuitabilityItemSPCContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCContextAdd"], Id = "new" }); break;
                            case "UnsuitabilityItemSPCContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCContextChange"], Id = "changed" }); break;
                            case "UnsuitabilityItemSPCContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCContextDelete"], Id = "delete" }); break;
                            case "UnsuitabilityItemSPCContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemSPCContextRefresh"], Id = "refresh" }); break;
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
            //LineCrudPopup = false;
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

                WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t=>t.OccuredStartDate >= DataSource.MeasurementStartDate && t.OccuredFinishDate <= DataSource.MeasurementEndDate).ToList();


                foreach (var unsuitabilityItem in UnsuitabilityItemsList)
                {
                    var unsuitabilityType = (await UnsuitabilityTypesItemsAppService.GetAsync(unsuitabilityItem.UnsuitabilityTypesItemsId)).Data;

                    var workCenter = (await StationGroupsAppService.GetAsync(unsuitabilityItem.StationGroupId)).Data;

                    var tempWorkOrdersList = WorkOrdersList.Where(t => t.StationGroupID == workCenter.Id).ToList();

                    int workOrdersCount = tempWorkOrdersList.Count;

                    #region Uygunsuzluk Raporları Listeleri

                    var contractUnsReportList = (await ContractUnsuitabilityReportsAppService.GetListAsync(new ListContractUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).ToList();

                    var purchaseUnsReportList = (await PurchaseUnsuitabilityReportsAppService.GetListAsync(new ListPurchaseUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).ToList();

                    var operationUnsReportList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.UnsuitabilityItemsID == unsuitabilityItem.Id && t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).ToList();

                    #endregion

                    #region Toplam Uygunsuz Komponent Hesaplama

                    var totalUnsuitableComponent = contractUnsReportList.Select(t => t.UnsuitableAmount).Sum() +operationUnsReportList.Select(t => t.UnsuitableAmount).Sum() + purchaseUnsReportList.Select(t => t.UnsuitableAmount).Sum();

                    #endregion

                    #region Toplam Uygunsuz Rapor Hesaplama

                    int totalUnsuitableReport = operationUnsReportList.Count() + contractUnsReportList.Count() + purchaseUnsReportList.Count();

                    #endregion

                    #region Sıklık Hesaplama

                    int frequency = 0;


                    if (workOrdersCount != 0)
                    {
                        var dataValue = (totalUnsuitableReport / workOrdersCount);

                        if(dataValue > 0 && dataValue <= 0.00001)
                        {
                            frequency = 1;
                        }
                        else if (dataValue > 0.00001 && dataValue <= 0.0001)
                        {
                            frequency = 1;
                        }
                        else if(dataValue > 0.0001 && dataValue <= 0.0005)
                        {
                            frequency = 2;
                        }
                        else if(dataValue > 0.0005 && dataValue <= 0.001)
                        {
                            frequency = 3;
                        }
                        else if (dataValue > 0.001 && dataValue <= 0.002)
                        {
                            frequency = 4;
                        }
                        else if ( dataValue > 0.002 && dataValue <= 0.005)
                        {
                            frequency = 5;
                        }
                        else if (dataValue > 0.005 && dataValue <= 0.01)
                        {
                            frequency = 6;
                        }
                        else if (dataValue > 0.01 && dataValue <= 0.025)
                        {
                            frequency = 7;
                        }
                        else if (dataValue > 0.025 && dataValue <= 0.05)
                        {
                            frequency = 8;
                        }
                        else if (dataValue > 0.05 && dataValue <= 0.1)
                        {
                            frequency = 9;
                        }
                        else if (dataValue > 0.1)
                        {
                            frequency = 10;
                        }
                    }
                  

                    #endregion

                    #region Keşfedilebilirlik Hesaplama

                    decimal detectability = 0;

                    if(totalUnsuitableReport != 0)
                    {
                        int dataValue = Convert.ToInt32(totalUnsuitableComponent / totalUnsuitableReport);
                        if(dataValue > 0 && dataValue <= 3)
                        {
                            detectability = 1;
                        }
                        else if(dataValue > 3 && dataValue <= 5)
                        {
                            detectability= 2;
                        }
                        else if (dataValue > 5 && dataValue <= 8)
                        {
                            detectability= 3;
                        }
                        else if (dataValue > 8 && dataValue <= 10)
                        {
                            detectability = 4;
                        }
                        else if (dataValue > 10 && dataValue <= 15)
                        {
                            detectability = 5;
                        }
                        else if (dataValue > 15 && dataValue <= 20)
                        {
                            detectability = 6;
                        }
                        else if (dataValue > 20 && dataValue <= 30)
                        {
                            detectability = 7;
                        }
                        else if (dataValue > 30 && dataValue <= 40)
                        {
                            detectability = 8;
                        }
                        else if (dataValue > 40 && dataValue <= 50)
                        {
                            detectability = 9;
                        }
                        else if (dataValue > 50)
                        {
                            detectability = 10;
                        }
                    }
                   
                    #endregion

                    SelectUnsuitabilityItemSPCLinesDto selectSPCLineModel = new SelectUnsuitabilityItemSPCLinesDto
                    {
                        Detectability = Convert.ToInt32(detectability),
                        Frequency = frequency,
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


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
