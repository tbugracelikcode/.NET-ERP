using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.WorkOrder
{
    public partial class WorkOrdersListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        protected override async void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "WorkOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateContextMenuItems();
        }

        protected void CreateContextMenuItems()
        {

            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "WorkOrderContextAdd":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextAdd"], Id = "new" }); break;
                        case "WorkOrderContextChange":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChange"], Id = "changed" }); break;
                        case "WorkOrderContextDelete":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextDelete"], Id = "delete" }); break;
                        case "WorkOrderContextRefresh":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextRefresh"], Id = "refresh" }); break;
                        case "WorkOrderContextProductionTracking":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextProductionTracking"], Id = "prodtracking" }); break;
                        case "WorkOrderContextContractTracking":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextContractTracking"], Id = "contracttracking" }); break;
                        case "WorkOrderContextChangeOperationCriteria":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChangeOperationCriteria"], Id = "changeoprcriteria" }); break;
                        case "WorkOrderContextChangeStation":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChangeStation"], Id = "changestation" }); break;
                        case "WorkOrderContextSplitWorkOrder":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextSplitWorkOrder"], Id = "splitworkorder" }); break;
                        default: break;
                    }
                }
            }
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListWorkOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "prodtracking":

                    break;

                case "contracttracking":

                    break;

                case "changeoprcriteria":

                    break;

                case "changestation":

                    break;

                case "splitworkorder":

                    break;

                default:
                    break;
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
