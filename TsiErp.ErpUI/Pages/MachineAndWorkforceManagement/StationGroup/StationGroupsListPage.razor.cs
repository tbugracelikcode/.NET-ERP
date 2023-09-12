using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.StationGroup
{
    public partial class StationGroupsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = StationGroupsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource= new SelectStationGroupsDto()
            { 
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("StationGroupChildMenu")
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("StationGroupChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
