using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.UnitSet
{
    public partial class UnitSetsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = UnitSetsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnitSetsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UnitSetsChildMenu")
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UnitSetsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
