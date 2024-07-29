using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Business.Entities.User.Services;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.NotificationTemplate
{
    public partial class NotificationTemplatesListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ListMenusDto> MultiMenusList = new List<ListMenusDto>();
        public List<Guid> BindingMenus = new List<Guid>();

        public List<ListDepartmentsDto> MultiDepartmentsList = new List<ListDepartmentsDto>();
        public List<Guid> BindingDepartments = new List<Guid>();

        public List<ListUsersDto> MultiUsersList = new List<ListUsersDto>();
        public List<Guid> BindingUsers = new List<Guid>();


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

            await GetMultiMenusList();
            await GetMultiDepartmentsList();
            await GetMultiUsersList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "NotificationTemplatesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion
        }

        #region ProcessName ComboBox 
        public class ProcessName_ComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ProcessName_ComboBox> processName_ComboBox = new List<ProcessName_ComboBox>
        {
            new ProcessName_ComboBox(){ID = "Add", Text="ComboAdd"},
            new ProcessName_ComboBox(){ID = "Delete", Text="ComboDelete"},
            new ProcessName_ComboBox(){ID = "Refresh", Text="ComboRefresh"}
        };

        private void ProcessName_ComboBoxValueChangeHandler(ChangeEventArgs<string, ProcessName_ComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Add":
                        DataSource.ProcessName_ = L["ComboAdd"].Value;
                        break;

                    case "Delete":
                        DataSource.ProcessName_ = L["ComboDelete"].Value;
                        break;

                    case "Refresh":
                        DataSource.ProcessName_ = L["ComboRefresh"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region ContextMenuName ComboBox 
        public class ContextMenuName_ComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ContextMenuName_ComboBox> contextmenuName_ComboBox = new List<ContextMenuName_ComboBox>
        {
            new ContextMenuName_ComboBox(){ID = "Add", Text="ContextAdd"},
            new ContextMenuName_ComboBox(){ID = "Delete", Text="ContextDelete"},
            new ContextMenuName_ComboBox(){ID = "Refresh", Text="ContextRefresh"}
        };

        private void ContextMenuName_ComboBoxValueChangeHandler(ChangeEventArgs<string, ContextMenuName_ComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Add":
                        DataSource.ContextMenuName_ = L["ContextAdd"].Value;
                        break;

                    case "Delete":
                        DataSource.ContextMenuName_ = L["ContextDelete"].Value;
                        break;

                    case "Refresh":
                        DataSource.ContextMenuName_ = L["ContextRefresh"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

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

        public async virtual void OnContextMenuClick(ContextMenuClickEventArgs<ListNotificationTemplatesDto> args)
        {
            var loc = (IStringLocalizer)_L;

            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;


                case "delete":

                    var res = await ModalManager.ConfirmationAsync(loc["DeleteConfirmationTitleBase"], loc["DeleteConfirmationDescriptionBase"]);


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

                default:
                    break;
            }
        }

        #region GetList Metotları

        public async Task GetMultiMenusList()
        {
            MultiMenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.Where(t => t.MenuName.Contains("ChildMenu")).ToList();
        }
        public async Task GetMultiDepartmentsList()
        {
            MultiDepartmentsList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.Where(t => t.Name.Contains("DepartmentsChildMenu")).ToList();
        }
        public async Task GetMultiUsersList()
        {
            MultiUsersList = (await UsersAppService.GetListAsync(new ListUsersParameterDto())).Data.Where(t => t.UserName.Contains("UsersChildMenu")).ToList();
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
