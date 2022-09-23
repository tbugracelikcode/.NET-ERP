using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;

namespace TsiErp.ErpUI.Pages.ShippingAdress
{
    public partial class ShippingAdressesListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = ShippingAdressesAppService;
        }

      
    }
}
