using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.GeneralSkillRecordPriority
{
    public partial class GeneralSkillRecordPrioritiesListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = GeneralSkillRecordPrioritiesService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "GeneralSkillRecordPrioritiesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectGeneralSkillRecordPrioritiesDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("GeneralSkillRecordPrioritiesChildMenu")
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
                        case "GeneralSkillRecordPrioritiesContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["GeneralSkillRecordPrioritiesContextAdd"], Id = "new" }); break;
                        case "GeneralSkillRecordPrioritiesContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["GeneralSkillRecordPrioritiesContextChange"], Id = "changed" }); break;
                        case "GeneralSkillRecordPrioritiesContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["GeneralSkillRecordPrioritiesContextDelete"], Id = "delete" }); break;
                        case "GeneralSkillRecordPrioritiesContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["GeneralSkillRecordPrioritiesContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        protected override async Task OnSubmit()
        {
            if(ListDataSource.Select(t=>t.Score).Sum() + DataSource.Score <= 100)
            {
                SelectGeneralSkillRecordPrioritiesDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectGeneralSkillRecordPrioritiesDto, CreateGeneralSkillRecordPrioritiesDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectGeneralSkillRecordPrioritiesDto, UpdateGeneralSkillRecordPrioritiesDto>(DataSource);

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
            }

            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningScoreTitle"], L["UIWarningScoreMessage"]);
            }
        }


        #region Genel Beceri ButtonEdit

        SfTextBox EmployeeGeneralSkillRecordsButtonEdit;
        bool SelectEmployeeGeneralSkillRecordsPopupVisible = false;
        List<ListEmployeeGeneralSkillRecordsDto> EmployeeGeneralSkillRecordsList = new List<ListEmployeeGeneralSkillRecordsDto>();

        public async Task EmployeeGeneralSkillRecordsOnCreateIcon()
        {
            var EmployeeGeneralSkillRecordsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeeGeneralSkillRecordsButtonClickEvent);
            await EmployeeGeneralSkillRecordsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeeGeneralSkillRecordsButtonClick } });
        }

        public async void EmployeeGeneralSkillRecordsButtonClickEvent()
        {
            SelectEmployeeGeneralSkillRecordsPopupVisible = true;
            EmployeeGeneralSkillRecordsList = (await EmployeeGeneralSkillRecordsAppService.GetListAsync(new ListEmployeeGeneralSkillRecordsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EmployeeGeneralSkillRecordsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.GeneralSkillID = Guid.Empty;
                DataSource.GeneralSkillName = string.Empty;
            }
        }

        public async void EmployeeGeneralSkillRecordsDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeeGeneralSkillRecordsDto> args)
        {
            var selectedEmployeeSenioritie = args.RowData;

            if (selectedEmployeeSenioritie != null)
            {
                DataSource.GeneralSkillID = selectedEmployeeSenioritie.Id;
                DataSource.GeneralSkillName = selectedEmployeeSenioritie.Name;
                SelectEmployeeGeneralSkillRecordsPopupVisible = false;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("GeneralSkillRecordPrioritiesChildMenu");
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
