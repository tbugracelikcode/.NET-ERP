using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrdersListPage
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
        }

        protected override async Task<IList<ListWorkOrdersDto>> GetListAsync(ListWorkOrdersParameterDto input)
        {
            var workOrderstates = new List<WorkOrderStateEnum>() { WorkOrderStateEnum.Durduruldu, WorkOrderStateEnum.DevamEdiyor, WorkOrderStateEnum.Baslamadi, WorkOrderStateEnum.FasonaGonderildi };

            ListDataSource = (await WorkOrdersAppService.GetListAsync(input)).Data.Where(t => workOrderstates.Contains(t.WorkOrderState)).ToList();

            return ListDataSource;
        }

        public async void WorkOrderDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var workOrder = args.RowData;

            if (workOrder != null)
            {
                var res = await ModalManager.ConfirmationAsync("Bilgi", "Bunu seçmek istiyor musunuz?");

                if (res)
                {

                    OperationDetailDto operationDetail = new OperationDetailDto();

                    operationDetail.EmployeeID = Guid.Empty;
                    operationDetail.EmployeeName = string.Empty;
                    operationDetail.WorkOrder = workOrder;
                    operationDetail.OperationAdjustment = new OperationAdjustmentDto();


                    AppService.CurrentOperation = operationDetail;

                    NavigationManager.NavigateTo("/work-order-detail");
                }

                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
