using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

namespace TsiErp.ErpUI.Pages.OperationUnsuitabilityReport
{
    public partial class OperationUnsuitabilityReportsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListWorkOrdersDto> WorkOrdersComboBox;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        SfComboBox<string, ListStationsDto> StationsComboBox;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        SfComboBox<string, ListStationGroupsDto> StationGroupsComboBox;
        List<ListStationGroupsDto> StationGroupsList = new List<ListStationGroupsDto>();

        SfComboBox<string, ListEmployeesDto> EmployeesComboBox;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        SfComboBox<string, ListProductionOrdersDto> ProductionOrdersComboBox;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListProductsOperationsDto> ProductsOperationsComboBox;
        List<ListProductsOperationsDto> ProductsOperationsList = new List<ListProductsOperationsDto>();

        #endregion

        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Scrap", Text="Hurda"},
            new UnsComboBox(){ID = "Correction", Text="Düzeltme"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="Olduğu Gibi Kullanılacak"}
        };

        protected override async void OnInitialized()
        {
            BaseCrudService = OperationUnsuitabilityReportsService;

            await GetProductsList();
            await GetWorkOrdersList();
            await GetStationsList();
            await GetStationGroupsList();
            await GetEmployeesList();
            await GetProductionOrdersList();
            await GetProductsOperationsList();
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationUnsuitabilityReportsDto()
            {
                Date_ = DateTime.Today
            };

            ShowEditPage();

            await Task.CompletedTask;
        }

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            switch(args.ItemData.ID)
            {
                case "Scrap": 
                    DataSource.IsScrap = true; 
                    DataSource.IsCorrection = false; 
                    DataSource.IsToBeUsedAs = false;
                    break;

                case "Correction":
                    DataSource.IsScrap = false;
                    DataSource.IsCorrection = true;
                    DataSource.IsToBeUsedAs = false;
                    break;

                case "ToBeUsedAs":
                    DataSource.IsScrap = false;
                    DataSource.IsCorrection = false;
                    DataSource.IsToBeUsedAs = true;
                    break;

                default:break;
            }
        }

        #region Ürünler
        public async Task ProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductsComboBox.FilterAsync(ProductsList, query);
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task ProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductID = args.ItemData.Id;
                DataSource.ProductName = args.ItemData.Name;
                DataSource.ProductCode = args.ItemData.Code;
            }
            else
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.ProductCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İş Emirleri
        public async Task WorkOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await WorkOrdersComboBox.FilterAsync(WorkOrdersList, query);
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
        }

        public async Task WorkOrderValueChangeHandler(ChangeEventArgs<string, ListWorkOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.WorkOrderID = args.ItemData.Id;
                DataSource.WorkOrderNo = args.ItemData.WorkOrderNo;
            }
            else
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.WorkOrderNo = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İstasyonlar
        public async Task StationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await StationsComboBox.FilterAsync(StationsList, query);
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        public async Task StationValueChangeHandler(ChangeEventArgs<string, ListStationsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.StationID = args.ItemData.Id;
                DataSource.StationName = args.ItemData.Name;
                DataSource.StationCode = args.ItemData.Code;
            }
            else
            {
                DataSource.StationID = Guid.Empty;
                DataSource.StationName = string.Empty;
                DataSource.StationCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İstasyon Grupları
        public async Task StationGroupFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await StationGroupsComboBox.FilterAsync(StationGroupsList, query);
        }

        private async Task GetStationGroupsList()
        {
            StationGroupsList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
        }

        public async Task StationGroupValueChangeHandler(ChangeEventArgs<string, ListStationGroupsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.StationGroupID = args.ItemData.Id;
                DataSource.StationGroupName = args.ItemData.Name;
                DataSource.StationGroupCode = args.ItemData.Code;
            }
            else
            {
                DataSource.StationGroupID = Guid.Empty;
                DataSource.StationGroupCode = string.Empty;
                DataSource.StationGroupName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Çalışanlar
        public async Task EmployeeFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await EmployeesComboBox.FilterAsync(EmployeesList, query);
        }

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        public async Task EmployeeValueChangeHandler(ChangeEventArgs<string, ListEmployeesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.EmployeeID = args.ItemData.Id;
                DataSource.EmployeeName = args.ItemData.Name;
            }
            else
            {
                DataSource.EmployeeID = Guid.Empty;
                DataSource.EmployeeName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Üretim Emirleri
        public async Task ProductionOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductionOrdersComboBox.FilterAsync(ProductionOrdersList, query);
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        public async Task ProductionOrderValueChangeHandler(ChangeEventArgs<string, ListProductionOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductionOrderID = args.ItemData.Id;
                DataSource.ProductionOrderFicheNo = args.ItemData.FicheNo;
            }
            else
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Operasyonlar
        public async Task ProductsOperationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductsOperationsComboBox.FilterAsync(ProductsOperationsList, query);
        }

        private async Task GetProductsOperationsList()
        {
            ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();
        }

        public async Task ProductsOperationValueChangeHandler(ChangeEventArgs<string, ListProductsOperationsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.OperationID = args.ItemData.Id;
                DataSource.OperationName = args.ItemData.Name;
                DataSource.OperationCode = args.ItemData.Code;
            }
            else
            {
                DataSource.OperationID = Guid.Empty;
                DataSource.OperationName = string.Empty;
                DataSource.OperationCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
