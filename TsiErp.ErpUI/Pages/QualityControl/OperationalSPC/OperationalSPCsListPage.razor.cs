using Syncfusion.Blazor.Grids;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos;
using TsiErp.Business.Entities.SalesPrice.Services;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationalSPC
{
    public partial class OperationalSPCsListPage : IDisposable
    {

        private SfGrid<SelectOperationalSPCLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectOperationalSPCLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectOperationalSPCLinesDto> GridLineList = new List<SelectOperationalSPCLinesDto>();

        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        List<ListProductsOperationsDto> OperationsList = new List<ListProductsOperationsDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private bool LineCrudPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = OperationalSPCsService;
            _L = L;
            CreateMainContextMenuItems();
            //CreateLineContextMenuItems();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "OperationalSPCChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion

        }

        #region Operasyonel SPC Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationalSPCsDto() 
            { 
                Date_ = DateTime.Today, 
                MeasurementEndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month+1,1).AddDays(-1), 
                MeasurementStartDate = DateTime.Today,
                Code = FicheNumbersAppService.GetFicheNumberAsync("OperationalSPCChildMenu")
            };

            DataSource.SelectOperationalSPCLines = new List<SelectOperationalSPCLinesDto>();
            GridLineList = DataSource.SelectOperationalSPCLines;

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
                            case "OperationalSPCLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCLineContextAdd"], Id = "new" }); break;
                            case "OperationalSPCLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCLineContextChange"], Id = "changed" }); break;
                            case "OperationalSPCLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCLineContextDelete"], Id = "delete" }); break;
                            case "OperationalSPCLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCLineContextRefresh"], Id = "refresh" }); break;
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
                            case "OperationalSPCContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCContextAdd"], Id = "new" }); break;
                            case "OperationalSPCContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCContextChange"], Id = "changed" }); break;
                            case "OperationalSPCContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCContextDelete"], Id = "delete" }); break;
                            case "OperationalSPCContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalSPCContextRefresh"], Id = "refresh" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListOperationalSPCsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await OperationalSPCsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectOperationalSPCLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectOperationalSPCLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    LineDataSource = new SelectOperationalSPCLinesDto();
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
                            DataSource.SelectOperationalSPCLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectOperationalSPCLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectOperationalSPCLines.Remove(line);
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
                if (DataSource.SelectOperationalSPCLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectOperationalSPCLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectOperationalSPCLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectOperationalSPCLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectOperationalSPCLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectOperationalSPCLines[selectedLineIndex] = LineDataSource;
                }
            }
            GridLineList = DataSource.SelectOperationalSPCLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }


        public async void Calculate()
        {
            if(DataSource.MeasurementStartDate == DataSource.MeasurementEndDate  )
            {
                await ModalManager.WarningPopupAsync(L["UIWarningDateTitleBase"], L["UIWarningDateMessageBase"]);
            }
            else if(DataSource.MeasurementStartDate > DataSource.MeasurementEndDate)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningDateTitleBase"], L["UIWarningDate2MessageBase"]);

            }
            else
            {
                WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.OccuredStartDate > DataSource.MeasurementStartDate && t.OccuredFinishDate < DataSource.MeasurementEndDate).ToList();

                OperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();

                foreach (var operation in OperationsList)
                {
                    var tempWorkOrderList = WorkOrdersList.Where(t => t.ProductsOperationID == operation.Id).ToList();

                    int workOrdersCount = tempWorkOrderList.Count;

                    var workCenter = (await StationGroupsAppService.GetAsync(operation.WorkCenterID)).Data;

                    #region Uygunsuzluk Raporları Listeleri ve Uygunsuzluk Başlıkları Listesi

                    var contractUnsReportList = (await ContractUnsuitabilityReportsAppService.GetListAsync(new ListContractUnsuitabilityReportsParameterDto())).Data.Where(t => t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).ToList();

                    var purchaseUnsRepostList = (await PurchaseUnsuitabilityReportsAppService.GetListAsync(new ListPurchaseUnsuitabilityReportsParameterDto())).Data.Where(t => t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate && t.IsUnsuitabilityWorkOrder == true).ToList();

                    var operationUnsReportList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.Date_ >= DataSource.MeasurementStartDate && t.Date_ <= DataSource.MeasurementEndDate).ToList();

                    var unsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.ToList();

                    #endregion

                    #region Toplam Üretilen Komponent

                    var totalProducedComponent = tempWorkOrderList.Select(t => t.ProducedQuantity).Sum();

                    #endregion

                    #region Toplam Uygunsuz Komponent ve Rapor

                    int totalUnsuitableComponent = 0;
                    int totalUnsuitableReport = 0;

                    foreach (var workorder in tempWorkOrderList)
                    {
                        var addedAmountComponent = contractUnsReportList.Where(t=>t.WorkOrderID == workorder.Id).Select(t => t.UnsuitableAmount).Sum() +
                           operationUnsReportList.Where(t=>t.WorkOrderID == workorder.Id).Select(t => t.UnsuitableAmount).Sum();

                        var addedAmountReport = contractUnsReportList.Where(t => t.WorkOrderID == workorder.Id).Count() +
                           operationUnsReportList.Where(t => t.WorkOrderID == workorder.Id).Count();

                        totalUnsuitableComponent = totalUnsuitableComponent + Convert.ToInt32(addedAmountComponent);
                        totalUnsuitableReport = totalUnsuitableReport + addedAmountReport;
                    }

                    #endregion

                    #region Toplam Gerçekleşen Operasyon

                    int totalOccuredOperation = tempWorkOrderList.Count;

                    #endregion

                    #region Sıklık Hesaplama

                    int frequency = 0;

                    if(workOrdersCount !=0)
                    {

                        var dataValue = totalUnsuitableReport / workOrdersCount;

                        if (dataValue > 0 && dataValue <= 0.00001)
                        {
                            frequency = 1;
                        }
                        else if (dataValue > 0.00001 && dataValue <= 0.0001)
                        {
                            frequency = 1;
                        }
                        else if (dataValue > 0.0001 && dataValue <= 0.0005)
                        {
                            frequency = 2;
                        }
                        else if (dataValue > 0.0005 && dataValue <= 0.001)
                        {
                            frequency = 3;
                        }
                        else if (dataValue > 0.001 && dataValue <= 0.002)
                        {
                            frequency = 4;
                        }
                        else if (dataValue > 0.002 && dataValue <= 0.005)
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

                    #region Farkedilebilrlik Hesaplama

                    int detectability = 0;

                    if(totalOccuredOperation != 0)
                    {
                        var dataValue = totalUnsuitableComponent / totalOccuredOperation;

                        if (dataValue > 0 && dataValue <= 3)
                        {
                            detectability = 1;
                        }
                        else if (dataValue > 3 && dataValue <= 5)
                        {
                            detectability = 2;
                        }
                        else if (dataValue > 5 && dataValue <= 8)
                        {
                            detectability = 3;
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

                    #region Şiddet Hesaplama

                    var tempOprUnsList = operationUnsReportList.Where(t=>t.OperationID == operation.Id).ToList();

                    int totalSeverity = 0;  

                    foreach(var item in tempOprUnsList)
                    {
                        int tempSeverity = unsuitabilityItemsList.Where(t => t.Id == item.UnsuitabilityItemsID).Select(t => t.IntensityCoefficient).FirstOrDefault() * Convert.ToInt32(item.UnsuitableAmount);

                        totalSeverity = totalSeverity + tempSeverity;
                    }

                    var totalUnsuitableAmount = tempOprUnsList.Sum(t=>t.UnsuitableAmount);

                    int severity = totalUnsuitableAmount == 0 ? 0 : (totalSeverity / Convert.ToInt32(totalUnsuitableAmount));

                    #endregion

                    #region Operasyon Bazlı Ara Kontrol Sıklıkları

                    int rpn = severity * detectability * frequency;

                    int oprBasedFrequency = 0;

                    if(rpn > 0 && rpn <= 50) { oprBasedFrequency = 500; }
                    else if(rpn > 50 && rpn<= 100) { oprBasedFrequency = 400; }
                    else if(rpn > 100 && rpn<= 250) { oprBasedFrequency = 330; }
                    else if(rpn > 250 && rpn<= 500) { oprBasedFrequency = 220; }
                    else if(rpn > 500 && rpn<= 600) { oprBasedFrequency = 110; }
                    else if(rpn > 600 && rpn<= 700) { oprBasedFrequency = 60; }
                    else if(rpn > 700 && rpn<= 800) { oprBasedFrequency = 50; }
                    else if(rpn > 800 && rpn<= 900) { oprBasedFrequency = 40; }
                    else if(rpn > 900) { oprBasedFrequency = 20; }

                    #endregion

                    SelectOperationalSPCLinesDto selectOperationSPCLineModel = new SelectOperationalSPCLinesDto
                    {
                        WorkCenterName = workCenter.Name,
                        OperationName = operation.Name,
                        TotalComponent = Convert.ToInt32(totalProducedComponent),
                        TotalUnsuitableComponent = totalUnsuitableComponent,
                        UnsuitableComponentRate = (totalUnsuitableComponent / Convert.ToInt32(totalProducedComponent)) * 100,
                        TotalOccuredOperation = totalOccuredOperation,
                        TotalUnsuitableOperation = totalUnsuitableReport,
                        UnsuitableOperationRate = (totalUnsuitableReport / totalOccuredOperation) * 100,
                        UnsuitableComponentPerOperation = totalUnsuitableComponent / totalUnsuitableReport,
                        Frequency = frequency,
                        Severity = severity,
                        Detectability = detectability,
                        RPN = rpn,
                        OperationBasedMidControlFrequency = oprBasedFrequency
                    };
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("OperationalSPCChildMenu");
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
