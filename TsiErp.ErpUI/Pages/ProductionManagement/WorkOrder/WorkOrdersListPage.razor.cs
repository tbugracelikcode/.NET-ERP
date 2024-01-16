using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionManagement.WorkOrder
{
    public partial class WorkOrdersListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "WorkOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion

        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {

            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "WorkOrderContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextAdd"], Id = "new" }); break;
                        case "WorkOrderContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChange"], Id = "changed" }); break;
                        case "WorkOrderContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextDelete"], Id = "delete" }); break;
                        case "WorkOrderContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
