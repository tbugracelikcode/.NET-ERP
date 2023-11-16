using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.MaintenanceInstruction.Services;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenanceMRP
{
    public partial class MaintenanceMRPsListPage
    {
        private SfGrid<SelectMaintenanceMRPLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMaintenanceMRPLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMaintenanceMRPLinesDto> GridLineList = new List<SelectMaintenanceMRPLinesDto>();


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = MaintenanceMRPsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }


        #region Bakım MRP Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenanceMRPsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MainMatReqPlanningChildMenu"),
                Date_ = DateTime.Today
            };

            DataSource.SelectMaintenanceMRPLines = new List<SelectMaintenanceMRPLinesDto>();
            GridLineList = DataSource.SelectMaintenanceMRPLines;

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextStockUsage"], Id = "stockusage" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPLinesContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceMRPsContextRefresh"], Id = "refresh" });
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
    }
}
