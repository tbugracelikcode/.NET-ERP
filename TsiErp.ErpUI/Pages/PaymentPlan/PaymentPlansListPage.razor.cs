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

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

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

    }
}
