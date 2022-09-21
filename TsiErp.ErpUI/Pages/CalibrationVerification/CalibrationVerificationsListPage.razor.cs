using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationVerification
{
    public partial class CalibrationVerificationsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationVerificationsService;
        }
    }
}
