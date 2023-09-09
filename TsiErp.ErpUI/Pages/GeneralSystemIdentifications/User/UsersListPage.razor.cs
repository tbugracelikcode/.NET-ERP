using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Helpers;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.User
{
    public partial class UsersListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = UsersService;
            _L = L;
            await GetUserGroupsList();
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

                    var menus = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();

                    List<SelectUserPermissionsDto> permissionsList = new List<SelectUserPermissionsDto>();

                    foreach (var menu in menus)
                    {
                        SelectUserPermissionsDto permission = new SelectUserPermissionsDto()
                        {
                            Id = Guid.Empty,
                            IsUserPermitted = true,
                            MenuId = menu.Id,
                            MenuName = menu.MenuName,
                            UserId = DataSource.Id,
                            UserName = DataSource.UserName
                        };

                        permissionsList.Add(permission);
                    }

                    var permissionCreateInput = new CreateUserPermissionsDto
                    {
                        Id = Guid.Empty,
                        IsUserPermitted = false,
                        MenuId = Guid.Empty,
                        SelectUserPermissionsList = permissionsList,
                        UserId = Guid.Empty
                    };

                    var insertedPermissions = (await UserPermissionsService.CreateAsync(permissionCreateInput)).Data;
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
    }
}
