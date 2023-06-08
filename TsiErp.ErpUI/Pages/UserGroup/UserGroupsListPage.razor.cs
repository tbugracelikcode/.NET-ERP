using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;

namespace TsiErp.ErpUI.Pages.UserGroup
{
    public partial class UserGroupsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = UserGroupsService;
            _L = L;
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
