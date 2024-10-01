using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using Autofac.Util;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using DevExpress.XtraRichEdit.Import.Html;
using Syncfusion.Blazor.SplitButtons;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine;

namespace TsiErp.ErpUI.Pages.CostManagement.CostPeriod
{
    public partial class CostPeriodsListPage : IDisposable
    {

        private SfGrid<SelectCostPeriodLinesDto> _LineGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        List<SelectCostPeriodLinesDto> GridLineList = new List<SelectCostPeriodLinesDto>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectCostPeriodLinesDto LineDataSource;


        private bool VirtualLineCrudPopup = false;
      

        protected override async void OnInitialized()
        {
            BaseCrudService = CostPeriodsAppService;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CostPeriodsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateLineContextMenuItems();

            _L = L;
        }
        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCostPeriodsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("CostPeriodsChildMenu")
            };

            EditPageVisible = true;

            GridLineList.Clear();

            #region Cost Period Line Properties

            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["IndirectPersonnel"], LineNr=GridLineList.Count+1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["AuxiliaryAndOperatingMaterials"], LineNr = GridLineList.Count + 1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["VariousIndirectMaterials"], LineNr = GridLineList.Count + 1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["NonProductionAreaRent"], LineNr = GridLineList.Count + 1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["GeneralEquipment"], LineNr = GridLineList.Count + 1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["AuxiliaryEquipmentAmortization"], LineNr = GridLineList.Count + 1 });
            GridLineList.Add(new SelectCostPeriodLinesDto { Amount = 0, Title_ = @L["LoadingAndUnloadingStationAmortization"], LineNr = GridLineList.Count + 1 });

            #endregion


            DataSource.SelectCostPeriodLines = GridLineList;

            return Task.CompletedTask;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListCostPeriodsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        IsChanged = true;
                        DataSource = (await CostPeriodsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectCostPeriodLines;
                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {
                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectCostPeriodLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        LineDataSource = args.RowInfo.RowData;
                        VirtualLineCrudPopup = true;
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


            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
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
                            case "CostPeriodLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodLineContextChange"], Id = "changed" }); break;
                            case "CostPeriodLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }
        protected override void CreateContextMenuItems(IStringLocalizer L)
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
                            case "CostPeriodsContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodsContextAdd"], Id = "new" }); break;
                            case "CostPeriodsContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodsContextChange"], Id = "changed" }); break;
                            case "CostPeriodsContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodsContextDelete"], Id = "delete" }); break;
                            case "CostPeriodsContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CostPeriodsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        public async void CalculateButton()
        {

        }


        public void HideLinesPopup()
        {
            VirtualLineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCostPeriodLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCostPeriodLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCostPeriodLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectCostPeriodLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectCostPeriodLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCostPeriodLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectCostPeriodLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);

        }


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CostPeriodsChildMenu");
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
