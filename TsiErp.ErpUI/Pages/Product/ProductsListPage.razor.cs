using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Product.Dtos;

namespace TsiErp.ErpUI.Pages.Product
{
    public partial class ProductsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = ProductService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}
