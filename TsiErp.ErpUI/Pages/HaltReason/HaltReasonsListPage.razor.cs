using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using System.ComponentModel.DataAnnotations;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.HaltReason.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;

namespace TsiErp.ErpUI.Pages.HaltReason
{
    public partial class HaltReasonsListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = HaltReasonsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectHaltReasonsDto(){};

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}
