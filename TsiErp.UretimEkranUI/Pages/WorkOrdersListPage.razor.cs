using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Timers;
using TsiErp.Connector.Helpers;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
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

        SelectStationsDto StationDataSource;

        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
        }

        protected override async Task<IList<ListWorkOrdersDto>> GetListAsync(ListWorkOrdersParameterDto input)
        {
            var workOrderstates = new List<WorkOrderStateEnum>() { WorkOrderStateEnum.Durduruldu, WorkOrderStateEnum.DevamEdiyor, WorkOrderStateEnum.Baslamadi, WorkOrderStateEnum.FasonaGonderildi };

            var stationID = ( await SystemGeneralStatusLocalDbService.GetListAsync()).Select(t=>t.StationID).FirstOrDefault();

            if(stationID != Guid.Empty)
            {
                StationDataSource = (await StationsAppService.GetAsync(stationID)).Data;

                ListDataSource = (await WorkOrdersAppService.GetListAsync(input)).Data.Where(t => workOrderstates.Contains(t.WorkOrderState) && t.StationID == stationID).ToList();
            }
            else
            {
                StationDataSource = new SelectStationsDto();

                StationDataSource.IsLoadCell = false;

                ListDataSource = (await WorkOrdersAppService.GetListAsync(input)).Data.Where(t => workOrderstates.Contains(t.WorkOrderState)).ToList();
            }

            

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

                    var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

                    if (generalStatus != null)
                    {
                        generalStatus.isLoadCell = StationDataSource.IsLoadCell;

                        await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);
                    }


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

                    #region İş Emrini Değiştirip Üretim Adedini Sıfırlama Protokolü

                    string result = ProtocolServices.M008W(ProtocolPorts.IPAddress);

                    #endregion

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
