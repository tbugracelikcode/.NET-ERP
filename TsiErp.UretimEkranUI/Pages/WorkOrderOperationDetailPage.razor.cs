using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrderOperationDetailPage
    {
        [Parameter] public Guid WorkOrderId { get; set; }

        SelectWorkOrdersDto DataSource = new SelectWorkOrdersDto();

        protected override async void OnInitialized()
        {
            await GetWorkOrderDetail();
        }

        private async Task GetWorkOrderDetail()
        {
            DataSource = (await WorkOrdersAppService.GetAsync(WorkOrderId)).Data;

            if(DataSource.Id != Guid.Empty)
            {

            }
        }
    }
}
