using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.ErpUI.Pages.PaymentPlan
{
    public partial class PaymentPlansListPage
    {

        private SfGrid<ListPaymentPlansDto> _grid;

        protected override async void OnInitialized()
        {
            BaseCrudService = PaymentPlansService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPaymentPlansDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }
    }
}
