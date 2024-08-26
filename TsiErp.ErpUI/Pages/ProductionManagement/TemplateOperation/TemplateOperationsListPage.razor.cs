using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.TemplateOperation
{
    public partial class TemplateOperationsListPage : IDisposable
    {
        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private SfGrid<SelectTemplateOperationLinesDto> _LineGrid;
        private SfGrid<SelectTemplateOperationUnsuitabilityItemsDto> _UnsuitabilityItemsLineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectTemplateOperationLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectTemplateOperationLinesDto> GridLineList = new List<SelectTemplateOperationLinesDto>();

        List<SelectTemplateOperationUnsuitabilityItemsDto> UnsuitabilityItemsLineGridList = new List<SelectTemplateOperationUnsuitabilityItemsDto>();

        private bool LineCrudPopup = false;


        protected override async void OnInitialized()
        {
            BaseCrudService = TemplateOperationsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "TempOperationsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region İstasyon Grup ButtonEdit

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkCenterID = Guid.Empty;
                DataSource.WorkCenterName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.WorkCenterID = selectedStationGroup.Id;
                DataSource.WorkCenterName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;

                UnsuitabilityItemsLineGridList.Clear();

                UnsuitabilityItemsLineGridList = (await TemplateOperationsAppService.GetUnsuitabilityItemsAsync(DataSource.WorkCenterID.GetValueOrDefault(), DataSource.Id)).Data.ToList();

                if (UnsuitabilityItemsLineGridList != null)
                {
                    DataSource.SelectTemplateOperationUnsuitabilityItems = UnsuitabilityItemsLineGridList;
                }

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
                LineDataSource.StationID = Guid.Empty;
                LineDataSource.StationCode = string.Empty;
                LineDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                LineDataSource.StationID = selectedUnitSet.Id;
                LineDataSource.StationCode = selectedUnitSet.Code;
                LineDataSource.StationName = selectedUnitSet.Name;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şablon Operasyon Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectTemplateOperationsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("TempOperationsChildMenu"),
                WorkScore = 0
            };
            DataSource.SelectTemplateOperationLines = new List<SelectTemplateOperationLinesDto>();
            GridLineList = DataSource.SelectTemplateOperationLines;

            DataSource.SelectTemplateOperationUnsuitabilityItems = new List<SelectTemplateOperationUnsuitabilityItemsDto>();
            UnsuitabilityItemsLineGridList = DataSource.SelectTemplateOperationUnsuitabilityItems;

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        protected async Task LineBeforeInsertAsync()
        {
            LineDataSource = new SelectTemplateOperationLinesDto()
            {
                Alternative = false,
                Priority = GridLineList.Count + 1,
                LineNr = GridLineList.Count + 1
            };


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "TemplateOperationLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationLineContextAdd"], Id = "new" }); break;
                            case "TemplateOperationLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationLineContextChange"], Id = "changed" }); break;
                            case "TemplateOperationLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationLineContextDelete"], Id = "delete" }); break;
                            case "TemplateOperationLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "TemplateOperationContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationContextAdd"], Id = "new" }); break;
                            case "TemplateOperationContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationContextChange"], Id = "changed" }); break;
                            case "TemplateOperationContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationContextDelete"], Id = "delete" }); break;
                            case "TemplateOperationContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["TemplateOperationContextRefresh"], Id = "refresh" }); break;
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

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListTemplateOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await TemplateOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectTemplateOperationLines.OrderBy(t => t.Priority).ToList();
                    UnsuitabilityItemsLineGridList = DataSource.SelectTemplateOperationUnsuitabilityItems.OrderBy(t => t.LineNr).ToList();
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectTemplateOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectTemplateOperationLinesDto();
                    LineCrudPopup = true;
                    await LineBeforeInsertAsync();
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
                            DataSource.SelectTemplateOperationLines.Remove(args.RowInfo.RowData);



                            await _LineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = GridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {
                                    int selectedPriority = GridLineList[selectedIndex].Priority;


                                    GridLineList.Remove(line);

                                    for (int i = 0; i < GridLineList.Count; i++)
                                    {
                                        GridLineList[i].LineNr = i + 1;
                                        GridLineList[i].Priority = i + 1;
                                    }

                                    DataSource.SelectTemplateOperationLines = GridLineList;

                                    await _LineGrid.Refresh();

                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                DataSource.SelectTemplateOperationLines.Remove(line);
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
                if (DataSource.SelectTemplateOperationLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectTemplateOperationLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectTemplateOperationLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectTemplateOperationLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectTemplateOperationLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectTemplateOperationLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectTemplateOperationLines;
            GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {

        }

        public async void ArrowUpBtnClicked()
        {
            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == 0))
            {
                GridLineList[index].Priority -= 1;
                GridLineList[index - 1].Priority += 1;
                GridLineList[index].LineNr -= 1;
                GridLineList[index - 1].LineNr += 1;

                GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectTemplateOperationLines = GridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == GridLineList.Count()))
            {
                GridLineList[index].Priority += 1;
                GridLineList[index + 1].Priority -= 1;
                GridLineList[index].LineNr += 1;
                GridLineList[index + 1].LineNr -= 1;

                GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectTemplateOperationLines = GridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region GetList Metotları

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        #endregion

        public async void UnsuitabilityItemsRowUpdate(ActionEventArgs<SelectTemplateOperationUnsuitabilityItemsDto> arg)
        {

            if (arg.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                var data = arg.Data;

                DataSource.SelectTemplateOperationUnsuitabilityItems.FirstOrDefault(t => t.LineNr == data.LineNr).ToBeUsed = data.ToBeUsed;
                arg.Cancel = true;
                await _UnsuitabilityItemsLineGrid.CloseEditAsync();
                await _UnsuitabilityItemsLineGrid.Refresh();
            }

        }

        #region Switch İşlemleri

        private void IsCanBeDetectedChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsCanBeDetected)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsCauseExtraCostChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsCauseExtraCost)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsHighRepairCostChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsHighRepairCost)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsLongWorktimeforOperatorChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsLongWorktimeforOperator)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsPhysicallyHardChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsPhysicallyHard)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsRequiresKnowledgeChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsRequiresKnowledge)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsRequiresSkillChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsRequiresSkill)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsRiskyforOperatorChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsRiskyforOperator)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
            }
        }

        private void IsSensitiveChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsSensitive)
            {
                DataSource.WorkScore += 1;
            }
            else
            {
                DataSource.WorkScore -= 1;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("TempOperationsChildMenu");
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
