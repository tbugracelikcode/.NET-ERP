using Microsoft.Extensions.Localization;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;
using Syncfusion.Blazor.Grids;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference
{
    public partial class EmployeeAnnualSeniorityDifferencesListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        DateTime dateYear = DateTime.Today;
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeAnnualSeniorityDifferencesService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EmployeeAnnualSeniorityDifferencesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeAnnualSeniorityDifferencesDto()
            {
                Year_ = GetSQLDateAppService.GetDateFromSQL().Year,
        };

            dateYear = GetSQLDateAppService.GetDateFromSQL();

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
                        case "EmployeeAnnualSeniorityDifferencesContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextAdd"], Id = "new" }); break;
                        case "EmployeeAnnualSeniorityDifferencesContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextChange"], Id = "changed" }); break;
                        case "EmployeeAnnualSeniorityDifferencesContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextDelete"], Id = "delete" }); break;
                        case "EmployeeAnnualSeniorityDifferencesContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        public void DateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            DataSource.Year_ = dateYear.Year;
        }

        #region Kıdem ButtonEdit

        SfTextBox EmployeeSenioritiesButtonEdit;
        bool SelectEmployeeSenioritiesPopupVisible = false;
        List<ListEmployeeSenioritiesDto> EmployeeSenioritiesList = new List<ListEmployeeSenioritiesDto>();

        public async Task EmployeeSenioritiesOnCreateIcon()
        {
            var EmployeeSenioritiesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeeSenioritiesButtonClickEvent);
            await EmployeeSenioritiesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeeSenioritiesButtonClick } });
        }

        public async void EmployeeSenioritiesButtonClickEvent()
        {
            SelectEmployeeSenioritiesPopupVisible = true;
            EmployeeSenioritiesList = (await EmployeeSenioritiesService.GetListAsync(new ListEmployeeSenioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EmployeeSenioritiesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SeniorityID = Guid.Empty;
                DataSource.SeniorityName = string.Empty;
            }
        }

        public async void EmployeeSenioritiesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeeSenioritiesDto> args)
        {
            var selectedEmployeeSenioritie = args.RowData;

            if (selectedEmployeeSenioritie != null)
            {
                DataSource.SeniorityID = selectedEmployeeSenioritie.Id;
                DataSource.SeniorityName = selectedEmployeeSenioritie.Name;
                SelectEmployeeSenioritiesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
