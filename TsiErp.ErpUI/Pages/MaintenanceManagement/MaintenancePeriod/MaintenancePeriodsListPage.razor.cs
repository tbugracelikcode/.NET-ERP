using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenancePeriod
{
    public partial class MaintenancePeriodsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public bool? isChecked = false;
        public bool periodEnable = true;

        protected override async void OnInitialized()
        {
            BaseCrudService = MaintenancePeriodsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "MainPeriodsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenancePeriodsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MainPeriodsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
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
                            case "MaintenancePeriodContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextAdd"], Id = "new" }); break;
                            case "MaintenancePeriodContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextChange"], Id = "changed" }); break;
                            case "MaintenancePeriodContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextDelete"], Id = "delete" }); break;
                            case "MaintenancePeriodContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        private void Change(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if(args.Checked == true)
            {
                periodEnable = false;
                DataSource.IsDaily = true;
                DataSource.PeriodTime = 0;


            }
            else
            {
                periodEnable = true;
                DataSource.IsDaily = false;

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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MainPeriodsChildMenu");
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
