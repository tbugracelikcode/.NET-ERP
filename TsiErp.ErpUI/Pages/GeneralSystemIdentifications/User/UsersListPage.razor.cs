using DevExpress.Blazor;
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
using TsiErp.ErpUI.Shared;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.User
{
    public partial class UsersListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        public List<UserMenuPermission> PermissionModalMenusList = new List<UserMenuPermission>();
        public List<Guid> ChangedPermissionsList = new List<Guid>();

        DxTreeView treeview;

        public class UserMenuPermission
        {
            public string MenuName { get; set; }
            public string MenuID { get; set; }
            public string ParentID { get; set; }
            public bool isPermitted { get; set; }
            public bool HasChild { get; set; }
            public bool Expanded { get; set; }
            public Guid Id { get; set; }
        }

        [Inject]
        ModalManager ModalManager { get; set; }

        public bool isPermissionModal = false;

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

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
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

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    var userPermissionList = (await UserPermissionsAppService.GetListAsyncByUserId(DataSource.Id)).Data.ToList();

                    //Layout.LoadingSpinnerVisible = true;

                    var parentList = userPermissionList.Where(t => t.MenuName.Contains("Parent")).Select(t => new UserMenuPermission
                    {
                        //Expanded = false,
                        HasChild = true,
                        isPermitted = t.IsUserPermitted,
                        MenuID = t.MenuId.ToString(),
                        MenuName = L[t.MenuName],
                        Id = t.Id
                    }).ToList();

                    PermissionModalMenusList.AddRange(parentList);

                    var subAmountsParentList = userPermissionList.Where(t => t.MenuName.Contains("AmountsChildMenu")).Select(t => new UserMenuPermission
                    {
                        isPermitted = t.IsUserPermitted,
                        Id = t.Id,
                        MenuID = t.MenuId.ToString(),
                        MenuName = L[t.MenuName],
                        //Expanded = false,
                        //HasChild = false,
                        ParentID = t.ParentID.ToString()
                    }).ToList();

                    PermissionModalMenusList.AddRange(subAmountsParentList);

                    var subParentList = userPermissionList.Where(t => !t.MenuName.Contains("Context") && !t.MenuName.Contains("Parent") && !t.MenuName.Contains("AmountsChildMenu")).Select(t => new UserMenuPermission
                    {
                        isPermitted = t.IsUserPermitted,
                        Id = t.Id,
                        MenuID = t.MenuId.ToString(),
                        MenuName = L[t.MenuName],
                        //Expanded = false,
                        HasChild = true,
                        ParentID = t.ParentID.ToString()
                    }).ToList();

                    PermissionModalMenusList.AddRange(subParentList);

                    var contextList = userPermissionList.Where(t => t.MenuName.Contains("Context")).Select(t => new UserMenuPermission
                    {
                        //Expanded = false,
                        //HasChild = false,
                        isPermitted = t.IsUserPermitted,
                        MenuID = t.MenuId.ToString(),
                        MenuName = L[t.MenuName],
                        Id = t.Id,
                        ParentID = t.ParentID.ToString()
                    }).ToList();

                    PermissionModalMenusList.AddRange(contextList);

                    #region Eski Yetki Kodları

                    //foreach (var permission in userPermissionList)
                    //{
                    //    var menu = (await MenusAppService.GetAsync(permission.MenuId)).Data;

                    //    if (menu != null && menu.Id != Guid.Empty)
                    //    {

                    //        if (menu.MenuName.Contains("Context"))
                    //        {
                    //            UserMenuPermission menuPermissionModel = new UserMenuPermission
                    //            {
                    //                isPermitted = permission.IsUserPermitted,
                    //                PermissionID = permission.Id,
                    //                MenuID = menu.Id.ToString(),
                    //                MenuName = L[menu.MenuName],
                    //                ParentID = menu.ParentMenuId.ToString(),
                    //                Expanded = false,
                    //                HasChild = false
                    //            };
                    //            PermissionModalMenusList.Add(menuPermissionModel);
                    //        }
                    //        else
                    //        {

                    //            if (menu.MenuName.Contains("Parent"))
                    //            {
                    //                UserMenuPermission menuPermissionModel = new UserMenuPermission
                    //                {
                    //                    isPermitted = permission.IsUserPermitted,
                    //                    PermissionID = permission.Id,
                    //                    MenuID = menu.Id.ToString(),
                    //                    MenuName = L[menu.MenuName],
                    //                    Expanded = false,
                    //                    HasChild = true
                    //                };
                    //                PermissionModalMenusList.Add(menuPermissionModel);
                    //            }
                    //            else
                    //            {
                    //                UserMenuPermission menuPermissionModel = new UserMenuPermission
                    //                {
                    //                    isPermitted = permission.IsUserPermitted,
                    //                    PermissionID = permission.Id,
                    //                    MenuID = menu.Id.ToString(),
                    //                    MenuName = L[menu.MenuName],
                    //                    ParentID = menu.ParentMenuId.ToString(),
                    //                    Expanded = false,
                    //                    HasChild = true
                    //                };
                    //                PermissionModalMenusList.Add(menuPermissionModel);
                    //            }

                    //        }
                    //    }


                    //}

                    #endregion

                    isPermissionModal = true;

                    //Layout.LoadingSpinnerVisible = false;

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

        public void NodeCheckingHandler(NodeCheckEventArgs args)
        {
            //Layout.LoadingSpinnerVisible = true;

            //var item = PermissionModalMenusList.Where(t => t.MenuID == args.NodeData.Id).FirstOrDefault();


            //if (!item.MenuName.Contains("Context")) //Context menu dışında, child menü içeren tüm menüler için permission update işlemi
            //{

            //    var updatedChilds = PermissionModalMenusList.Where(t => t.ParentID == item.MenuID).ToList();

            //    if (updatedChilds != null && updatedChilds.Count > 0)
            //    {
            //        #region Child İçeren Parent Menünün İşlemleri

            //        int indexItemContext = PermissionModalMenusList.IndexOf(item);

            //        PermissionModalMenusList[indexItemContext].isPermitted = args.NodeData.IsChecked == "false" ? true : false;

            //        ChangedPermissionsList.Add(new Guid(item.MenuID));

            //        #endregion

            //        foreach (var child in updatedChilds)
            //        {
            //            int indexChild = PermissionModalMenusList.IndexOf(child);

            //            PermissionModalMenusList[indexChild].isPermitted = PermissionModalMenusList[indexItemContext].isPermitted;

            //            ChangedPermissionsList.Add(new Guid(child.MenuID));
            //        }

                   
            //    }
            //    else  // Tutar alanlarının childlerı yok ama Context değiller, bu kısım onlar için
            //    {
            //        int indexItemContext = PermissionModalMenusList.IndexOf(item);

            //        PermissionModalMenusList[indexItemContext].isPermitted = args.NodeData.IsChecked == "false" ? true : false;

            //        ChangedPermissionsList.Add(new Guid(item.MenuID));
            //    }


            //}
            //else // Context menüler için permission update işlemi
            //{
            //    int indexItemContext = PermissionModalMenusList.IndexOf(item);

            //    PermissionModalMenusList[indexItemContext].isPermitted = args.NodeData.IsChecked == "false" ? true : false;

            //    ChangedPermissionsList.Add(new Guid(item.MenuID));
            //}


        }

        void UserPermissionCheckedChanged(TreeViewCheckedChangedEventArgs e)
        {
            var item = PermissionModalMenusList;
            //var firstCheckedNode = e.CheckedItems.FirstOrDefault();
            //FirstChecked = firstCheckedNode != null ? firstCheckedNode.Text : "none";
        }

        public async void OnPermissionsSubmit()
        {
            ////Layout.LoadingSpinnerVisible = true;

            //foreach (var changedPermissionMenuID in ChangedPermissionsList)
            //{
            //    var permission = (await UserPermissionsAppService.GetListAsyncByUserId(DataSource.Id)).Data.Where(t => t.MenuId == changedPermissionMenuID).FirstOrDefault();

            //    if (permission != null && permission.Id != Guid.Empty)
            //    {
            //        bool permissionStatus = PermissionModalMenusList.Where(t => t.MenuID == changedPermissionMenuID.ToString()).Select(t => t.isPermitted).FirstOrDefault();

            //        permission.IsUserPermitted = permissionStatus;

            //        var updatedPermission = ObjectMapper.Map<SelectUserPermissionsDto, UpdateUserPermissionsDto>(permission);

            //        updatedPermission.SelectUserPermissionsList = new List<SelectUserPermissionsDto>();

            //        updatedPermission.SelectUserPermissionsList.Add(permission);

            //        await UserPermissionsAppService.UpdateAsync(updatedPermission);
            //    }
            //}

            //HidePermissionModal();
            ////Layout.LoadingSpinnerVisible = false;
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
