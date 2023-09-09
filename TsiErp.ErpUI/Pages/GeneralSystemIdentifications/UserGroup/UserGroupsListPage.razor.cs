using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.UserGroup
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
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UserGrpChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UserGrpChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
