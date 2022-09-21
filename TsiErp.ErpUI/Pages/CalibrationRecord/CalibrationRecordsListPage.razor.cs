using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationRecord
{
    public partial class CalibrationRecordsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationRecordsService;
        }
    }
}
