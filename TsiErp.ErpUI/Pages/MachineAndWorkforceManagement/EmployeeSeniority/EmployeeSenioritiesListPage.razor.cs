using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeSeniority
{
    public partial class EmployeeSenioritiesListPage : IDisposable
    {
        SelectTaskScoringsDto TaskScoringDataSource;
        [Inject]
        ModalManager ModalManager { get; set; }
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeSenioritiesService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EmployeeSenioritiesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeSenioritiesDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeSenioritiesChildMenu")
            };

            TaskScoringDataSource = new SelectTaskScoringsDto()
            {
                Score = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu")
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
                        case "EmployeeSenioritiesContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextAdd"], Id = "new" }); break;
                        case "EmployeeSenioritiesContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextChange"], Id = "changed" }); break;
                        case "EmployeeSenioritiesContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextDelete"], Id = "delete" }); break;
                        case "EmployeeSenioritiesContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextRefresh"], Id = "refresh" }); break;
                        default: break;
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
                    var taskScoringId = (await TaskScoringsAppService.GetListAsync(new ListTaskScoringsParameterDto())).Data.Where(t=>t.SeniorityID == DataSource.Id).Select(t=>t.Id).FirstOrDefault();
                    TaskScoringDataSource = (await TaskScoringsAppService.GetAsync(taskScoringId)).Data;
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected override async Task OnSubmit()
        {

            #region Personel Kıdemleri Kayıt İşlemleri

            SelectEmployeeSenioritiesDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectEmployeeSenioritiesDto, CreateEmployeeSenioritiesDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectEmployeeSenioritiesDto, UpdateEmployeeSenioritiesDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);

            #endregion

            CreateTaskScoringsDto createTaskScoringModel = new CreateTaskScoringsDto
            {
                Code = TaskScoringDataSource.Code,
                SeniorityID = DataSource.Id,
                IsAdjustment = TaskScoringDataSource.IsAdjustment,
                Score = TaskScoringDataSource.Score,
                IsTaskSharing = TaskScoringDataSource.IsTaskSharing,
                IsTaskDone = TaskScoringDataSource.IsTaskDone,
                IsDeveloperIdea = TaskScoringDataSource.IsDeveloperIdea,
                IsDetectFault = TaskScoringDataSource.IsDetectFault,
                Id = Guid.Empty,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            };

            await TaskScoringsAppService.CreateAsync(createTaskScoringModel);


        }


        #region Switch Change Methodları

        private void IsTaskDoneChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (TaskScoringDataSource.IsTaskDone)
            {
                TaskScoringDataSource.Score += 1;
            }
            else
            {
                TaskScoringDataSource.Score -= 1;
            }
        }

        private void IsDetectFaultChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (TaskScoringDataSource.IsDetectFault)
            {
                TaskScoringDataSource.Score += 1;
            }
            else
            {
                TaskScoringDataSource.Score -= 1;
            }
        }
        private void IsAdjustmentChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (TaskScoringDataSource.IsAdjustment)
            {
                TaskScoringDataSource.Score += 1;
            }
            else
            {
                TaskScoringDataSource.Score -= 1;
            }
        }
        private void IsDeveloperIdeaChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (TaskScoringDataSource.IsDeveloperIdea)
            {
                TaskScoringDataSource.Score += 1;
            }
            else
            {
                TaskScoringDataSource.Score -= 1;
            }
        }
        private void IsTaskSharingChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (TaskScoringDataSource.IsTaskSharing)
            {
                TaskScoringDataSource.Score += 1;
            }
            else
            {
                TaskScoringDataSource.Score -= 1;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeSenioritiesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Görev Puanlama Kod ButtonEdit

        SfTextBox TaskScoringCodeButtonEdit = new();

        public async Task TaskScoringCodeOnCreateIcon()
        {
            var TaskScoringCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TaskScoringCodeButtonClickEvent);
            await TaskScoringCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TaskScoringCodesButtonClick } });
        }

        public async void TaskScoringCodeButtonClickEvent()
        {
            TaskScoringDataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu");
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
