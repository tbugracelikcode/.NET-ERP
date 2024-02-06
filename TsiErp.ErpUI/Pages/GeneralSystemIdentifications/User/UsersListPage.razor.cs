using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.User
{
    public partial class UsersListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        public List<UserMenuPermission> PermissionModalMenusList = new List<UserMenuPermission>();

        public class UserMenuPermission
        {
            public string MenuName { get; set; }
            public string MenuID { get; set; }
            public string ParentID { get; set; }
            public bool isPermitted { get; set; }
            public bool HasChild { get; set; }
            public bool Expanded { get; set; }
            public Guid PermissionID { get; set; }
        }

        [Inject]
        ModalManager ModalManager { get; set; }

        public bool isPermissionModal = false;

        public bool isLoadingPermissions = false;



        protected override async void OnInitialized()
        {
            BaseCrudService = UsersService;
            _L = L;
            await GetUserGroupsList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "UsersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
        }

        private async Task GetUserGroupsList()
        {
            UserGroupList = (await UserGroupsService.GetListAsync(new ListUserGroupsParameterDto())).Data.ToList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUsersDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UsersChildMenu")
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
                        case "UserContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UserContextAdd"], Id = "new" }); break;
                        case "UserContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UserContextChange"], Id = "changed" }); break;
                        case "UserContextPermission":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UserContextPermission"], Id = "permission" }); break;
                        case "UserContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UserContextDelete"], Id = "delete" }); break;
                        case "UserContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UserContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        protected override async Task OnSubmit()
        {
            SelectUsersDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createdInput = ObjectMapper.Map<SelectUsersDto, CreateUsersDto>(DataSource);

                result = (await CreateAsync(createdInput)).Data;

                if (result != null)
                {
                    DataSource.Id = result.Id;
                }
            }
            else
            {
                var updatedInput = ObjectMapper.Map<SelectUsersDto, UpdateUsersDto>(DataSource);

                result = (await UpdateAsync(updatedInput)).Data;
            }

            if (result == null)
            {
                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            //await ModalManager.MessagePopupAsync("LoadingCaption", "LoadingText");

            HideEditPage();

            //BaseModalComponent.Co

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
        }

        #region Kullanıcı Grubu ButtonEdit

        SfTextBox UserGroupButtonEdit;
        bool SelectUserGroupPopupVisible = false;
        List<ListUserGroupsDto> UserGroupList = new List<ListUserGroupsDto>();

        public async Task UserGroupOnCreateIcon()
        {
            var UserGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UserGroupButtonClickEvent);
            await UserGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UserGroupButtonClick } });
        }

        public async void UserGroupButtonClickEvent()
        {
            SelectUserGroupPopupVisible = true;
            UserGroupList = (await UserGroupsService.GetListAsync(new ListUserGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void UserGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.GroupID = Guid.Empty;
                DataSource.GroupName = string.Empty;
            }
        }

        public async void UserGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListUserGroupsDto> args)
        {
            var selectedUserGroup = args.RowData;

            if (selectedUserGroup != null)
            {
                DataSource.GroupID = selectedUserGroup.Id;
                DataSource.GroupName = selectedUserGroup.Name;
                SelectUserGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListUsersDto> args)
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

                case "permission":

                    //ShowLoadingPermisssionsModal();
                    //await InvokeAsync(StateHasChanged);

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    var userPermissionList = (await UserPermissionsAppService.GetListAsyncByUserId(DataSource.Id)).Data.ToList();


                    foreach (var permission in userPermissionList)
                    {
                        var menu = (await MenusAppService.GetAsync(permission.MenuId)).Data;

                        if (menu != null && menu.Id != Guid.Empty)
                        {

                            if (menu.MenuName.Contains("Context"))
                            {
                                UserMenuPermission menuPermissionModel = new UserMenuPermission
                                {
                                    isPermitted = permission.IsUserPermitted,
                                    PermissionID = permission.Id,
                                    MenuID = menu.Id.ToString(),
                                    MenuName = L[menu.MenuName],
                                    ParentID = menu.ParentMenuId.ToString(),
                                    Expanded = false,
                                    HasChild = false
                                };
                                PermissionModalMenusList.Add(menuPermissionModel);
                            }
                            else
                            {

                                if (menu.MenuName.Contains("Parent"))
                                {
                                    UserMenuPermission menuPermissionModel = new UserMenuPermission
                                    {
                                        isPermitted = permission.IsUserPermitted,
                                        PermissionID = permission.Id,
                                        MenuID = menu.Id.ToString(),
                                        MenuName = L[menu.MenuName],
                                        Expanded = false,
                                        HasChild = true
                                    };
                                    PermissionModalMenusList.Add(menuPermissionModel);
                                }
                                else
                                {
                                    UserMenuPermission menuPermissionModel = new UserMenuPermission
                                    {
                                        isPermitted = permission.IsUserPermitted,
                                        PermissionID = permission.Id,
                                        MenuID = menu.Id.ToString(),
                                        MenuName = L[menu.MenuName],
                                        ParentID = menu.ParentMenuId.ToString(),
                                        Expanded = false,
                                        HasChild = true
                                    };
                                    PermissionModalMenusList.Add(menuPermissionModel);
                                }

                            }
                        }


                    }

                    isPermissionModal = true;

                    //if (isPermissionModal)
                    //{
                    //    isLoadingPermissions = false;
                    //}


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

                default:
                    break;
            }
        }

        public void HidePermissionModal()
        {
            PermissionModalMenusList.Clear();
            isPermissionModal = false;
        }

        public void ShowLoadingPermisssionsModal()
        {
            isLoadingPermissions = true;
        }

        public async void NodeCheckedHandler(NodeCheckEventArgs args)
        {
            var menuID = new Guid(args.NodeData.Id);
            var userID = DataSource.Id;

            var menu = (await MenusAppService.GetAsync(menuID)).Data;

            if (!menu.MenuName.Contains("Context")) //Context menu dışında, child menü içeren tüm menüler için permission update işlemi
            {
                var permission = (await UserPermissionsAppService.GetListAsyncByUserId(userID)).Data.Where(t => t.MenuId == menuID).FirstOrDefault();

                permission.IsUserPermitted = args.NodeData.IsChecked == "false" ? true : false;

                var updateInput = ObjectMapper.Map<SelectUserPermissionsDto, UpdateUserPermissionsDto>(permission);

                updateInput.SelectUserPermissionsList = new List<SelectUserPermissionsDto>();

                updateInput.SelectUserPermissionsList.Add(permission);

                await UserPermissionsAppService.UpdateAsync(updateInput);

                var childMenus = (await MenusAppService.GetListbyParentIDAsync(menuID)).Data.ToList();

                if (childMenus != null && childMenus.Count > 0)
                {
                    foreach (var childmenu in childMenus)
                    {
                        var permissionChild = (await UserPermissionsAppService.GetListAsyncByUserId(userID)).Data.Where(t => t.MenuId == childmenu.Id).FirstOrDefault();

                        permissionChild.IsUserPermitted = permission.IsUserPermitted;

                        var updateInputChild = ObjectMapper.Map<SelectUserPermissionsDto, UpdateUserPermissionsDto>(permissionChild);

                        updateInputChild.SelectUserPermissionsList = new List<SelectUserPermissionsDto>();

                        updateInputChild.SelectUserPermissionsList.Add(permissionChild);

                        await UserPermissionsAppService.UpdateAsync(updateInputChild);

                    }
                }
            }
            else // Context menüler için permission update işlemi
            {
                var permission = (await UserPermissionsAppService.GetListAsyncByUserId(userID)).Data.Where(t => t.MenuId == menuID).FirstOrDefault();

                permission.IsUserPermitted = args.NodeData.IsChecked == "false" ? true : false;

                var updateInput = ObjectMapper.Map<SelectUserPermissionsDto, UpdateUserPermissionsDto>(permission);

                updateInput.SelectUserPermissionsList = new List<SelectUserPermissionsDto>();

                updateInput.SelectUserPermissionsList.Add(permission);

                await UserPermissionsAppService.UpdateAsync(updateInput);
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UsersChildMenu");
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
