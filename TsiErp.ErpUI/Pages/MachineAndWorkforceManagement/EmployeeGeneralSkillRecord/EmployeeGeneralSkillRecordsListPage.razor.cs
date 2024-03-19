using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord
{
    public partial class EmployeeGeneralSkillRecordsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeGeneralSkillRecordsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EmployeeGeneralSkillRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeGeneralSkillRecordsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeGeneralSkillRecordsChildMenu")
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
                        case "EmployeeGeneralSkillRecordsContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextAdd"], Id = "new" }); break;
                        case "EmployeeGeneralSkillRecordsContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextChange"], Id = "changed" }); break;
                        case "EmployeeGeneralSkillRecordsContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextDelete"], Id = "delete" }); break;
                        case "EmployeeGeneralSkillRecordsContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeGeneralSkillRecordsChildMenu");
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
