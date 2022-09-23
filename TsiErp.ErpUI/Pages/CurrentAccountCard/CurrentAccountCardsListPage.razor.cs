using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;


namespace TsiErp.ErpUI.Pages.CurrentAccountCard
{
    public partial class CurrentAccountCardsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = CurrentAccountCardsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCurrentAccountCardsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}
