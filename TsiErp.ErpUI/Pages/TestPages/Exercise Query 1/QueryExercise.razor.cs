using DevExpress.Utils.Internal;
using Syncfusion.Blazor.Inputs;
using System.Numerics;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.FirstProductApproval.Services;
using TsiErp.Business.Entities.HaltReason.Services;
using TsiErp.Business.Entities.MaintenancePeriod.Services;
using TsiErp.Business.Entities.OperationalQualityPlan.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.Period.Services;
using TsiErp.Business.Entities.PlannedMaintenance.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.StationGroup.Services;
using TsiErp.Business.Entities.TemplateOperation.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
namespace TsiErp.ErpUI.Pages.TestPages.Exercise_Query_1
{
    public partial class QueryExercise
    {
        public async void Query1()
        {
            Guid productId = Guid.Parse("5F17B091-770F-4D67-040F-3A0D57B5A098");

            var OperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == productId).ToList();

        }
        public async void Query2()
        {
            Guid productId = Guid.Parse("5F17B091-770F-4D67-040F-3A0D57B5A098");

            var workCenterList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == productId).Select(t => t.WorkCenterID).ToList();



        }
        public async void Query3()
        {
            Guid workCenterId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871");

            var UnsuitabilityItemList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.Where(t => t.StationGroupId == workCenterId).Select(t => t.UnsuitabilityTypesItemsId).ToList();



        }
        public async void Query4()
        {
            string istasyon = "CTT01";

            var stationGroupsList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.StationCode == istasyon).Select(t => t.WorkOrderID).ToList();

        }
        //Seçilen üretim emrine ait iş emirleri listesi
        public async void Query5()
        {
            Guid productionOrderId = Guid.Parse("AABB7848");

            var workOrderList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == productionOrderId).Select(t => t.WorkOrderNo).ToList();



        }
        //Seçilen ürün grubuna ait stok kartları listesi
        public async void Query6()
        {
            Guid productionGroupId = Guid.Parse("CTB02");

            var productsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.ProductGrpID == productionGroupId).Select(t => t.Code).ToList();

        }
        //Seçilen iş emrine ait üretim takip kayıtları listesi
        public async void Query7()
        {
            Guid workOrderId = Guid.Parse("CTA23");

            var trackingRecordsList = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.WorkOrderID == workOrderId).ToList();

        }
        //Seçilen çalışana ait üretim takip kayıtları listesi
        public async void Query8()
        {
            Guid employeeId = Guid.Parse("AFGHJ2");

            var trackingRecordsList = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.EmployeeID == employeeId).ToList();
        }
        //Seçilen iş merkezine ait şablon operasyon kayıtları listesi

        public async void Query9()
        {
            Guid workCenterId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871");

            var templateOperationsList = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.Where(t => t.WorkCenterID  == workCenterId).Select(t => t.Code).ToList();



        }
        //Eklenen son iş merkezi kaydı
        public async void Query10()
        {
        }
        // Eklenen ilk stok kartı kaydı
        public async void Query11()
        {

        }

        //Seçilen iş merkezine ait ürün operasyonları listesi
        public async void Query12()
        {
            Guid workCenterId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871");

            var productOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.WorkCenterID == workCenterId).Select(t => t.Code).ToList();
        }

        // Seçilen stok kartına ait ürün reçeteleri listesi

        public async void Query13()
        {
            Guid productId = Guid.Parse("ABC234");

            var billOfMaterialsList = (await BillsofMaterialsAppService.GetListAsync(new ListBillsofMaterialsParameterDto())).Data.Where(t=> t.FinishedProductID == productId).Select(t => t.Code).ToList();
        }

        //Teknik onay verilmemiş ürün rotaları listesi
        public async void Query14()
        {
            bool technicalApproval = false;

            var productionRoutesList = (await RoutesAppService.GetListAsync(new ListRoutesParameterDto())).Data.Where(t => t.TechnicalApproval == technicalApproval).Select(t => t.ProductID).ToList();

        }

        //Makine kaynaklı duruş kodları listesi
        public async void Query15()
        {
            bool isMachine = true;

            var haltReasonsList = (await HaltReasonsAppService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsMachine == isMachine).Select(t => t.Code).ToList();

        }

        //Seçilen iş istasyonunun Önleyici/Planlı Bakım Kayıtları Listesi
        public async void Query16()
        {
            Guid stationId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871"); ;

            var plannedMaintenanceRecordsList = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.StationID == stationId).Select(t => t.RegistrationNo).ToList();

        }

        //Seçilen iş istasyonunun 6 Aylık Bakım Önleyici/Planlı Bakım Kayıtları Listesi

        public async void Query17()
        {
            string stationId = "93CA4DEF-2EEB-439B-0916-3A07F9BC8871"; 

            var maintenancePeriodsList = (await MaintenancePeriodsAppService.GetListAsync(new ListMaintenancePeriodsParameterDto())).Data.Where(t => t.Code == stationId && t.Name == "6 Aylık Bakım").ToList();

        }

        //Seçilen cari hesabın satış siparişleri listesi
        public async void Query18()
        {
            Guid currentAccoundCardId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871"); ;

            var salesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.CurrentAccountCardID == currentAccoundCardId).Select(t => t.FicheNo).ToList();

        }

        //Seçilen cari hesabın seçilen şubeye göre satış siparişleri listesi
        public async void Query19()
        {
            Guid currentAccoundCardId = Guid.Parse("93CA4DEF-2EEB-439B-0916-3A07F9BC8871"); 
            string branchId = "FFGH0916"; 

            var salesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.CurrentAccountCardID == currentAccoundCardId && t.BranchCode== branchId).ToList();

        }

        //Seçilen iş emrinin ilk ürün onay kayıtları listesi
        public async void Query20()
        {
            Guid workOrderId = Guid.Parse("CTA23");

            var firstProductApprovalsList = (await FirstProductApprovalsAppService.GetListAsync(new ListFirstProductApprovalsParameterDto())).Data.Where(t => t.WorkOrderID == workOrderId).ToList();
        }

        // Seçilen iş emrinin durumu Onay Bekliyor olan ilk ürün onay kayıtları listesi
        public async void Query21()
        {
            Guid workOrderId = Guid.Parse("SKNJSA23");

            var firstProductApprovalsList = (await FirstProductApprovalsAppService.GetListAsync(new ListFirstProductApprovalsParameterDto())).Data.Where(t => t.WorkOrderID == workOrderId && t.IsApproval == false).ToList();
        }

        //Seçilen operasyonun operasyon kalite planı listesi
        public async void Query22()
        {
            string operationId = "CTA239876";

            var operationalQualityPlansList = (await OperationalQualityPlansAppService.GetListAsync(new ListOperationalQualityPlansParameterDto())).Data.Where(t => t.OperationCode == operationId).ToList();
        }

        //Seçilen satış siparişinin bağlı olduğu üretim emirleri listesi
        public async void Query23()
        {
            Guid salesOrderId = Guid.Parse("CTA239876");

            var productionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.OrderID == salesOrderId).Select(t => t.LinkedProductionOrderID).ToList();
        }






    }
}
