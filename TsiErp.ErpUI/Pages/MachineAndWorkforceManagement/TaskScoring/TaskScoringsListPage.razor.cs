using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.TaskScoring
{
    public partial class TaskScoringsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = TaskScoringsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "TaskScoringsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectTaskScoringsDto()
            {
                Score = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu")
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
                            case "TaskScoringsContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextAdd"], Id = "new" }); break;
                            case "TaskScoringsContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextChange"], Id = "changed" }); break;
                            case "TaskScoringsContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextDelete"], Id = "delete" }); break;
                            case "TaskScoringsContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        #region Switch Change Methodları

        private void IsTaskDoneChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsTaskDone)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }

        private void IsDetectFaultChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsDetectFault)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsAdjustmentChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsAdjustment)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsDeveloperIdeaChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsDeveloperIdea)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsTaskSharingChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsTaskSharing)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }

        #endregion


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


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu");
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
