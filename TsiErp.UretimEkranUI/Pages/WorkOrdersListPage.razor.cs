using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.Fatek.CommunicationCore.Base;
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
                HaltReasonTimer = new System.Timers.Timer(1000);
                HaltReasonTimer.Elapsed += HaltReasonTimerTimedEvent;
                HaltReasonTimer.Enabled = true;
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
                    // İş emri değiştir register ı PLC deki verileri temizlemek için set edildi.
                    AppService.FatekCommunication.SetDis(MemoryType.M, 2, RunningCode.Set);

                    OperationDetailDto operationDetail = new OperationDetailDto()
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
                        WorkOrderNo = workOrder.WorkOrderNo
                    };


                    var localWorkOrderId = await OperationDetailLocalDbService.WorkOrderControl(workOrder.Id);

                    if(localWorkOrderId== 0)
                    {
                        await OperationDetailLocalDbService.InsertAsync(operationDetail);
                        operationDetail.Id = localWorkOrderId;
                        AppService.CurrentOperation = operationDetail;
                    }
                    else
                    {
                        AppService.CurrentOperation = await OperationDetailLocalDbService.GetAsync(localWorkOrderId);
                    }

                    NavigationManager.NavigateTo("/before-operation");
                }

                await InvokeAsync(StateHasChanged);
            }
        }


        #region Duruş Kontrol Timer

        System.Timers.Timer HaltReasonTimer;

        public bool HaltReasonModalVisible { get; set; } = false;

        private void HaltReasonTimerTimedEvent(object source, ElapsedEventArgs e)
        {
            if (AppService.FatekCommunication.GetDis(MemoryType.M, 16) == true)
            {
                if (!HaltReasonModalVisible)
                {
                    HaltReasonModalVisible = true;
                    //Duruş bilgisi modalı çıkacak. DR600 deki değer okunacak. Bu değer duruş süresi bilgisidir. Duruş bilgisi modal kapandıktan sonra HaltReasonModalVisible false yapılacak. DR600 0 set edilecek.
                }

            }

            InvokeAsync(StateHasChanged);
        }

        #endregion

        public void Dispose()
        {
            if (HaltReasonTimer != null)
            {
                HaltReasonTimer.Enabled = false;
                HaltReasonTimer.Stop();
                HaltReasonTimer.Dispose();
            }
        }
    }
}
