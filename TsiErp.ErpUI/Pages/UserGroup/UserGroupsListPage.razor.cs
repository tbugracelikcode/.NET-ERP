using TsiErp.Entities.Entities.UserGroup.Dtos;

namespace TsiErp.ErpUI.Pages.UserGroup
{
    public partial class UserGroupsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = UserGroupsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUserGroupsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
