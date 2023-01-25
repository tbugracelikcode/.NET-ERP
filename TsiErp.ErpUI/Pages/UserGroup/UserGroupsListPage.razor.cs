using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
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

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
