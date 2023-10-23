using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.PaymentPlan.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.TemplateOperation.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ContractTrackingFiche
{
    public partial class ContractTrackingFichesListPage
    {

        private SfGrid<SelectContractTrackingFicheLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectContractTrackingFicheLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectContractTrackingFicheLinesDto> GridLineList = new List<SelectContractTrackingFicheLinesDto>();
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = ContractTrackingFichesAppService;
            _L = L;
            CreateMainContextMenuItems();
            //CreateLineContextMenuItems();

        }


        #region Fason Takip Fişleri Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractTrackingFichesDto()
            {
                FicheDate_ = DateTime.Today,
                EstimatedDate_ = DateTime.Today,
                FicheNr = FicheNumbersAppService.GetFicheNumberAsync("ContractTrackingFichesChildMenu")
            };

            DataSource.SelectContractTrackingFicheLines = new List<SelectContractTrackingFicheLinesDto>();
            GridLineList = DataSource.SelectContractTrackingFicheLines;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractTrackingFicheContextRefresh"], Id = "refresh" });
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
                            DataSource.SelectContractTrackingFicheLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
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

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
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

        #endregion

        
        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public Guid FinishedProductID  { get; set; }

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

                var oprlist = (await ContractQualityPlansAppService.GetAsync(selectedContractQualityPlan.Id)).Data.SelectContractQualityPlanOperations;

                foreach (var opr in oprlist)
                {
                    var operation = (await ProductsOperationsAppService.GetAsync(opr.Id)).Data;
                    var wordorder = (await WorkOrdersAppService.GetbyProductionOrderIdAsync(DataSource.ProductionOrderID)).Data;

                    SelectContractTrackingFicheLinesDto operationModel = new SelectContractTrackingFicheLinesDto
                    {
                        OperationID = operation.Id,
                        OperationCode = operation.Code,
                        OperationName = operation.Name,
                        WorkOrderID = wordorder.Id,
                        WorkOrderNr = wordorder.WorkOrderNo,
                        StationID = wordorder.StationID,
                        StationCode = wordorder.StationCode,
                        StationName = wordorder.StationName,
                        IsSent = false
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
    }
}
