using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrdersListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
            }
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


                    OperationDetailTable operationDetail = new OperationDetailTable()
                    {
                        EmployeeID = Guid.Empty,
                        EmployeeName = string.Empty,
                        ApprovedQuantity = 0,
                        Id = 0,
                        PlannedQuantity = workOrder.PlannedQuantity,
                        ProducedQuantity = workOrder.ProducedQuantity,
                        ProductID = workOrder.ProductID.GetValueOrDefault(),
                        ProductName = workOrder.ProductName,
                        ProductsOperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        ProductsOperationName = workOrder.ProductsOperationName,
                        QualitControlApprovalDate = null,
                        ScrapQuantity = 0,
                        StationCode = workOrder.StationCode,
                        TotalQualityControlApprovalTime = 0,
                        WorkOrderID = workOrder.Id,
                        WorkOrderNo = workOrder.WorkOrderNo,
                        ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        StationID = workOrder.StationID.GetValueOrDefault(),
                        WorkOrderState = (int)workOrder.WorkOrderState
                    };


                    var localWorkOrderId = await OperationDetailLocalDbService.WorkOrderControl(workOrder.Id);

                    if (localWorkOrderId == 0)
                    {
                        await OperationDetailLocalDbService.InsertAsync(operationDetail);
                        operationDetail.Id = localWorkOrderId;
                        AppService.CurrentOperation = operationDetail;
                    }
                    else
                    {
                        AppService.CurrentOperation = await OperationDetailLocalDbService.GetAsync(localWorkOrderId);
                    }

                    NavigationManager.NavigateTo("/work-order-detail");
                }

                await InvokeAsync(StateHasChanged);
            }
        }


        public void Dispose()
        {
        }
    }
}
