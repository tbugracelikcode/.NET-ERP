using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.EquipmentRecord
{
    public partial class EquipmentRecordsListPage : IDisposable
    {
        bool cancelReasonVisible = false;


        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = EquipmentRecordsService;
            _L = L;
            await GetDepartmentsList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EquipmentRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        void CancelValueChanged(ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);

            if (argsValue)
            {
                DataSource.CancellationDate = GetSQLDateAppService.GetDateFromSQL();
                cancelReasonVisible = true;
            }
            else
            {
                DataSource.CancellationDate = null;
                cancelReasonVisible = false;
            }
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEquipmentRecordsDto()
            {
                IsActive = true,
                CancellationDate = null,
                RecordDate = GetSQLDateAppService.GetDateFromSQL(),
                Code = FicheNumbersAppService.GetFicheNumberAsync("EquipmentRecordsChildMenu")
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
                        case "EquipmentRecordContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EquipmentRecordContextAdd"], Id = "new" }); break;
                        case "EquipmentRecordContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EquipmentRecordContextChange"], Id = "changed" }); break;
                        case "EquipmentRecordContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EquipmentRecordContextDelete"], Id = "delete" }); break;
                        case "EquipmentRecordContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EquipmentRecordContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        #region Departman ButtonEdit

        SfTextBox DepartmentButtonEdit;
        bool SelectDepartmentPopupVisible = false;
        List<ListDepartmentsDto> DepartmentList = new List<ListDepartmentsDto>();

        public async Task DepartmentOnCreateIcon()
        {
            var DepartmentButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DepartmentButtonClickEvent);
            await DepartmentButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DepartmentButtonClick } });
        }

        public async void DepartmentButtonClickEvent()
        {
            SelectDepartmentPopupVisible = true;
            DepartmentList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DepartmentOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.Department = Guid.Empty;
                DataSource.DepartmentName = string.Empty;
            }
        }

        public async void DepartmentDoubleClickHandler(RecordDoubleClickEventArgs<ListDepartmentsDto> args)
        {
            var selectedDepartment = args.RowData;

            if (selectedDepartment != null)
            {
                DataSource.Department = selectedDepartment.Id;
                DataSource.DepartmentName = selectedDepartment.Name;
                SelectDepartmentPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        private async Task GetDepartmentsList()
        {
            DepartmentList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EquipmentRecordsChildMenu");
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
