using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Grids;
using Microsoft.Extensions.Localization;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.ErpUI.Pages.ProductionManagement.HaltReason
{
    public partial class HaltReasonsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public bool isMachineDisabled = false;
        public bool isOperatorDisabled = false;
        public bool isManagementDisabled = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = HaltReasonsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "HaltReasonsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectHaltReasonsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("HaltReasonsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
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
                        case "HaltReasonContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextAdd"], Id = "new" }); break;
                        case "HaltReasonContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextChange"], Id = "changed" }); break;
                        case "HaltReasonContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextDelete"], Id = "delete" }); break;
                        case "HaltReasonContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        private void IsMachineSwitchValueChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.IsManagement = false;
                isManagementDisabled = true;
                DataSource.IsOperator = false;
                isOperatorDisabled = true;
                isMachineDisabled = false;
                DataSource.IsMachine = true;
            }
            else
            {
                isManagementDisabled = false;
                isOperatorDisabled = false;
                isMachineDisabled = false;
                DataSource.IsMachine = false;
            }
        }

        private void IsOperatorSwitchValueChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.IsManagement = false;
                isManagementDisabled = true;
                DataSource.IsMachine = false;
                isMachineDisabled = true;
                isOperatorDisabled = false;
                DataSource.IsOperator = true;
            }
            else
            {
                isManagementDisabled = false;
                isMachineDisabled = false;
                isOperatorDisabled = false;
                DataSource.IsOperator = false;
            }
        }

        private void IsManagementSwitchValueChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.IsOperator = false;
                isOperatorDisabled = true;
                DataSource.IsMachine = false;
                isMachineDisabled = true;
                isManagementDisabled = false;
                DataSource.IsManagement = true;
            }
            else
            {
                isOperatorDisabled = false;
                isMachineDisabled = false;
                isManagementDisabled = false;
                DataSource.IsManagement = false;
            }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("HaltReasonsChildMenu");
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
