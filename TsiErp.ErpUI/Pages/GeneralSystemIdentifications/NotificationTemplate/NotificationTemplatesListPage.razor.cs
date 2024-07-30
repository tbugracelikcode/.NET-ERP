using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Localizations.Resources.NotificationTemplates;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.NotificationTemplate
{
    public partial class NotificationTemplatesListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ListDepartmentsDto> MultiDepartmentsList = new List<ListDepartmentsDto>();
        public List<Guid> BindingDepartments = new List<Guid>();

        public List<ListEmployeesDto> MultiEmployeesList = new List<ListEmployeesDto>();
        public List<Guid> BindingEmployees = new List<Guid>();

        public List<ListMenusDto> ModuleList = new List<ListMenusDto>();
        public List<SelectMenusDto> ModuleContextsList = new List<SelectMenusDto>();
        public List<ListDepartmentsDto> SourceDepartmentList = new List<ListDepartmentsDto>();
        


        protected override async Task BeforeInsertAsync()
        {
            var employee = (await EmployeesAppService.GetAsync(LoginedUserService.UserId)).Data;

            DataSource = new SelectNotificationTemplatesDto()
            {
                IsActive = true,
                SourceDepartmentId = employee.DepartmentID.Value,
                SourceDepartmentName = employee.Department
            };

            EditPageVisible = true;

        }
        protected override async void OnInitialized()
        {
            BaseCrudService = NotificationTemplatesService;
            _L = L;
            await GetMenusList();
            await GetMultiDepartmentsList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "NotificationTemplatesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion
        }

        #region ModuleName ComboBox 

        private async void ModuleNameValueChangeHandler(ChangeEventArgs<string, ListMenusDto> args)
        {
            var parentGuid = new Guid(args.Value);
            ModuleContextsList = (await MenusAppService.GetListbyParentIDAsync(parentGuid)).Data.ToList();
            DataSource.ModuleName_ = ModuleList.Where(t => t.Id == parentGuid).Select(t => t.MenuName).FirstOrDefault();
        }

        #endregion

        #region ProcessName ComboBox 
        public class ProcessName_ComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ProcessName_ComboBox> processName_ComboBox = new List<ProcessName_ComboBox>
        {
            new ProcessName_ComboBox(){ID = "Add", Text="ProcessAdd"},
            new ProcessName_ComboBox(){ID = "Delete", Text="ProcessDelete"},
            new ProcessName_ComboBox(){ID = "Refresh", Text="ProcessRefresh"}
        };

        private void ProcessName_ComboBoxValueChangeHandler(ChangeEventArgs<string, ProcessName_ComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Add":
                        DataSource.ProcessName_ = L["ProcessAdd"].Value;
                        break;

                    case "Delete":
                        DataSource.ProcessName_ = L["ProcessDelete"].Value;
                        break;

                    case "Refresh":
                        DataSource.ProcessName_ = L["ProcessRefresh"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Target Department ComboBox

        private async void TargetDepartmentValueChangeHandler(MultiSelectChangeEventArgs<List<Guid>> args)
        {
            if(args.Value != null && args.Value.Count > 0)
            {
                MultiEmployeesList.Clear();

                foreach (var departmentId in BindingDepartments)
                {
                    var addingEmpList = (await EmployeesAppService.GetListbyDepartmentAsync(departmentId)).Data;
                    MultiEmployeesList.AddRange(addingEmpList);
                }
            }
            else
            {
                MultiEmployeesList.Clear();
            }
            
        }

        #endregion

        #region Contexts ComboBox

        private void ModuleContextValueChangeHandler(ChangeEventArgs<string, SelectMenusDto> args)
        {
            var contextGuid = new Guid(args.Value);
            DataSource.ContextMenuName_ = ModuleContextsList.Where(t=>t.Id == contextGuid).Select(t=>t.MenuName).FirstOrDefault();
        }

        #endregion

        #region Source Department

        private void SourceDepartmentValueChangeHandler(ChangeEventArgs<string, SelectDepartmentsDto> args)
        {
            var sourceDepartmetGuid = new Guid(args.Value);
            DataSource.SourceDepartmentName = SourceDepartmentList.Where(t => t.Id == sourceDepartmetGuid).Select(t => t.Code).FirstOrDefault();
        }


        #endregion

        private void TargetUsersValueChangeHandler(ChangeEventArgs<string, SelectEmployeesDto> args)
        {
            var employeesGuid = new Guid(args.Value);
            DataSource.TargetUsersId = MultiEmployeesList.Where(t => t.Id == employeesGuid).Select(t => t.Code).FirstOrDefault();
        }

        protected override async Task OnSubmit()
        {
            foreach(var departmentId in BindingDepartments)
            {
                if (string.IsNullOrEmpty(DataSource.TargetDepartmentId))
                {
                    DataSource.TargetDepartmentId = departmentId.ToString();
                }
                else
                {
                    DataSource.TargetDepartmentId = DataSource.TargetDepartmentId + "," + departmentId.ToString();
                }
            }

            foreach (var userId in BindingEmployees)
            {
                if (string.IsNullOrEmpty(DataSource.TargetUsersId))
                {
                    DataSource.TargetUsersId = userId.ToString();
                }
                else
                {
                    DataSource.TargetUsersId = DataSource.TargetUsersId + "," + userId.ToString();
                }
            }

            #region Submit 

            SelectNotificationTemplatesDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectNotificationTemplatesDto, CreateNotificationTemplatesDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectNotificationTemplatesDto, UpdateNotificationTemplatesDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

           
            #endregion
        }


        [Inject]
        ModalManager ModalManager { get; set; }

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
                            case "NotificationTemplatesContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextAdd"], Id = "new" }); break;
                            case "NotificationTemplatesContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextDelete"], Id = "delete" }); break;

                            case "NotificationTemplatesContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }

        }


        #region GetList Metotları

        public async Task GetMenusList()
        {
            ModuleList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.Where(t => t.MenuName.Contains("ChildMenu")).ToList();
        }

        public async Task GetMultiDepartmentsList()
        {
            MultiDepartmentsList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
        }
       

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
