using Castle.Core.Resource;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.User.Dtos;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;

namespace TsiErp.ErpUI.Pages.User
{
    public partial class UsersListPage
    {

        SfComboBox<string, ListUserGroupsDto> UserGroupComboBox;
        List<ListUserGroupsDto> UserGroupList = new List<ListUserGroupsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = UsersService;
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
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public async Task UserGroupFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await UserGroupComboBox.FilterAsync(UserGroupList, query);
        }

        public async Task UserGroupValueChangeHandler(ChangeEventArgs<string, ListUserGroupsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.GroupID = args.ItemData.Id;
                DataSource.GroupName = args.ItemData.Name;
            }
            else
            {
                DataSource.GroupID = Guid.Empty;
                DataSource.GroupName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
