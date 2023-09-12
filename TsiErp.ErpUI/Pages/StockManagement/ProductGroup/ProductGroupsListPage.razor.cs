using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.ProductGroup
{
    public partial class ProductGroupsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductGroupsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu")
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
