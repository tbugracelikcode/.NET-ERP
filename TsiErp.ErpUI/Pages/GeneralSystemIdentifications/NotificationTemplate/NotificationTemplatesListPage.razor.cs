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
using TsiErp.Business.Entities.ProductProperty.Services;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using Microsoft.Identity.Client;
using static TsiErp.ErpUI.Pages.GeneralSystemIdentifications.User.UsersListPage;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.NotificationTemplate
{
    public partial class NotificationTemplatesListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ListUserGroupsDto> MultiDepartmentsList = new List<ListUserGroupsDto>();
        public List<Guid> BindingDepartments = new List<Guid>();

        public List<ListUsersDto> MultiEmployeesList = new List<ListUsersDto>();
        public List<Guid> BindingEmployees = new List<Guid>();

        public List<ModuleClass> ModuleList = new List<ModuleClass>();
        public List<ModuleContextClass> ModuleContextsList = new List<ModuleContextClass>();
        public List<ListDepartmentsDto> SourceDepartmentList = new List<ListDepartmentsDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public class ModuleClass
        {
            public Guid Id { get; set; }

            public string MenuName { get; set; }

            public Guid ParentMenuId { get; set; }
            public int ContextOrderNo { get; set; }
            public string MenuURL { get; set; }
        }

        public class ModuleContextClass
        {
            public Guid Id { get; set; }

            public string MenuName { get; set; }

            public Guid ParentMenuId { get; set; }
            public int ContextOrderNo { get; set; }
            public string MenuURL { get; set; }
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

        protected override async Task BeforeInsertAsync()
        {
            var employee = (await EmployeesAppService.GetAsync(LoginedUserService.UserId)).Data;

            var user = (await UsersAppService.GetAsync(LoginedUserService.UserId)).Data;

            DataSource = new SelectNotificationTemplatesDto()
            {
                IsActive = true,
                SourceDepartmentId = user.GroupID.Value,
                SourceDepartmentName = user.GroupName
            };

            foreach(var item in processName_ComboBox)
            {
                item.Text = L[item.Text];
            }

            BindingDepartments.Clear();
            BindingEmployees.Clear();
            MultiEmployeesList.Clear();

            EditPageVisible = true;

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
                            case "NotificationTemplatesContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextAdd"], Id = "new" }); break;
                            case "NotificationTemplatesContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextDelete"], Id = "delete" }); break;

                            case "NotificationTemplatesContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextRefresh"], Id = "refresh" }); break;

                            case "NotificationTemplatesContextIsActive":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationTemplatesContextIsActive"], Id = "active" }); break;


                            default: break;
                        }
                    }
                }
            }

        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListNotificationTemplatesDto> args)
        {


            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "active":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    DataSource.IsActive = !DataSource.IsActive;

                    var UpdatedEntity = ObjectMapper.Map<SelectNotificationTemplatesDto, UpdateNotificationTemplatesDto>(DataSource);
                    await NotificationTemplatesService.UpdateAsync(UpdatedEntity);

                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }

        }

        public async void ShowSQLCommandClick()
        {
            if (!string.IsNullOrEmpty(DataSource.ModuleName_) && !string.IsNullOrEmpty(DataSource.ProcessName_) && BindingDepartments.Count > 0 && BindingDepartments != null)
            {
                #region  Departman ve User Seçimi
                foreach (var departmentId in BindingDepartments)
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

                if (BindingEmployees.Count > 0 && BindingEmployees != null)
                {
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
                }

                else
                {
                    if (MultiEmployeesList.Count > 0 && MultiEmployeesList != null)
                    {
                        foreach (var employee in MultiEmployeesList)
                        {
                            if (string.IsNullOrEmpty(DataSource.TargetUsersId))
                            {
                                DataSource.TargetUsersId = employee.Id.ToString();
                            }
                            else
                            {
                                DataSource.TargetUsersId = DataSource.TargetUsersId + "," + employee.Id.ToString();
                            }
                        }
                    }

                }
                #endregion

                var creatingEntity = ObjectMapper.Map<SelectNotificationTemplatesDto, CreateNotificationTemplatesDto>(DataSource);

                DataSource.QueryStr = NotificationTemplatesService.CreateCommandAsync(creatingEntity);
            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSQLCommandTitle"], L["UIWarningSQLCommandMessage"]);
            }
        }

        protected override async Task OnSubmit()
        {
            if (!string.IsNullOrEmpty(DataSource.QueryStr))
            {
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
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningQueryStrTitle"], L["UIWarningQueryStrMessage"]);
            }

        }


        #region GetList Metotları

        public async Task GetMenusList()
        {
            ModuleList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.Where(t => t.MenuName.Contains("ChildMenu")&& !t.MenuName.Contains("ShowAmount")).Select(t => new ModuleClass
            {
                ContextOrderNo = t.ContextOrderNo,
                Id = t.Id,
                MenuURL = t.MenuURL,
                ParentMenuId = t.ParentMenuId,
                MenuName = L[t.MenuName],
            }).ToList();

        }

        public async Task GetMultiDepartmentsList()
        {
            MultiDepartmentsList = (await UserGroupsAppService.GetListAsync(new ListUserGroupsParameterDto())).Data.ToList();
        }


        #endregion

        #region Change Eventleri

        #region ModuleName ComboBox 

        private async void ModuleNameValueChangeHandler(ChangeEventArgs<string, ModuleClass> args)
        {
            var parentGuid = new Guid(args.Value);
            ModuleContextsList = (await MenusAppService.GetListbyParentIDAsync(parentGuid)).Data.Select(t => new ModuleContextClass
            {
                MenuName = L[t.MenuName],
                ParentMenuId = t.ParentMenuId,
                MenuURL = t.MenuURL,
                Id = t.Id,
                ContextOrderNo = t.ContextOrderNo,
            }).ToList();

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
            if (args.Value != null && args.Value.Count > 0)
            {
                MultiEmployeesList.Clear();

                foreach (var departmentId in BindingDepartments)
                {
                    var addingEmpList = (await UsersAppService.GetListAsync(new ListUsersParameterDto())).Data.Where(t=>t.GroupID == departmentId).ToList();
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

        private void ModuleContextValueChangeHandler(ChangeEventArgs<string, ModuleContextClass> args)
        {
            var contextGuid = new Guid(args.Value);
            DataSource.ContextMenuName_ = ModuleContextsList.Where(t => t.Id == contextGuid).Select(t => t.MenuName).FirstOrDefault();
        }

        #endregion

        #region Target Users
        private void TargetUsersValueChangeHandler(ChangeEventArgs<string, SelectEmployeesDto> args)
        {

        }
        #endregion

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
