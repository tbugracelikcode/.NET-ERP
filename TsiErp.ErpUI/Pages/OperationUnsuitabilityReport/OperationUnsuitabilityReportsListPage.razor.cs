using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
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
            _L = L;
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

            EditPageVisible = true;

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

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Operasyon Button Edit

        SfTextBox ProductsOperationsCodeButtonEdit;
        SfTextBox ProductsOperationsNameButtonEdit;
        bool SelectProductsOperationsPopupVisible = false;
        List<ListProductsOperationsDto> ProductsOperationsList = new List<ListProductsOperationsDto>();
        public async Task ProductsOperationsCodeOnCreateIcon()
        {
            var ProductsOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsOperationsCodeButtonClickEvent);
            await ProductsOperationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsOperationsButtonClick } });
        }

        public async void ProductsOperationsCodeButtonClickEvent()
        {
            SelectProductsOperationsPopupVisible = true;
            await GetProductsOperationsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsOperationsNameOnCreateIcon()
        {
            var ProductsOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsOperationsNameButtonClickEvent);
            await ProductsOperationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsOperationsButtonClick } });
        }

        public async void ProductsOperationsNameButtonClickEvent()
        {
            SelectProductsOperationsPopupVisible = true;
            await GetProductsOperationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.OperationID = Guid.Empty;
                DataSource.OperationCode = string.Empty;
                DataSource.OperationName = string.Empty;
            }
        }

        public async void ProductsOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsOperationsDto> args)
        {
            var selectedProductsOperation = args.RowData;

            if (selectedProductsOperation != null)
            {
                DataSource.OperationID = selectedProductsOperation.Id;
                DataSource.OperationCode = selectedProductsOperation.Code;
                DataSource.OperationName = selectedProductsOperation.Name;
                SelectProductsOperationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İş Emri ButtonEdit

        SfTextBox WorkOrdersButtonEdit;
        bool SelectWorkOrdersPopupVisible = false;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        public async Task WorkOrdersOnCreateIcon()
        {
            var WorkOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WorkOrdersButtonClickEvent);
            await WorkOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WorkOrdersButtonClick } });
        }

        public async void WorkOrdersButtonClickEvent()
        {
            SelectWorkOrdersPopupVisible = true;
            await GetWorkOrdersList();
            await InvokeAsync(StateHasChanged);
        }


        public void WorkOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.WorkOrderNo = string.Empty;
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.WorkOrderID = selectedOrder.Id;
                DataSource.WorkOrderNo = selectedOrder.WorkOrderNo;
                SelectWorkOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationID = Guid.Empty;
                DataSource.StationCode = string.Empty;
                DataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.StationID = selectedUnitSet.Id;
                DataSource.StationCode = selectedUnitSet.Code;
                DataSource.StationName = selectedUnitSet.Name;
                DataSource.StationGroupID = selectedUnitSet.GroupID;
                DataSource.StationGroupName = selectedUnitSet.StationGroup;
                DataSource.StationGroupCode = selectedUnitSet.StationGroupCode;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İstasyon Grup Button Edit

        SfTextBox StationGroupsCodeButtonEdit;
        SfTextBox StationGroupsNameButtonEdit;
        bool SelectStationGroupsPopupVisible = false;
        List<ListStationGroupsDto> StationGroupsList = new List<ListStationGroupsDto>();
        public async Task StationGroupsCodeOnCreateIcon()
        {
            var StationGroupsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupsCodeButtonClickEvent);
            await StationGroupsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupsButtonClick } });
        }

        public async void StationGroupsCodeButtonClickEvent()
        {
            SelectStationGroupsPopupVisible = true;
            await GetStationGroupsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task StationGroupsNameOnCreateIcon()
        {
            var StationGroupsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupsNameButtonClickEvent);
            await StationGroupsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupsButtonClick } });
        }

        public async void StationGroupsNameButtonClickEvent()
        {
            SelectStationGroupsPopupVisible = true;
            await GetStationGroupsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationGroupID = Guid.Empty;
                DataSource.StationGroupCode = string.Empty;
                DataSource.StationGroupName = string.Empty;
            }
        }

        public async void StationGroupsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.StationGroupID = selectedStationGroup.Id;
                DataSource.StationGroupCode = selectedStationGroup.Code;
                DataSource.StationGroupName = selectedStationGroup.Name;
                SelectStationGroupsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Personel ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        public async Task EmployeesOnCreateIcon()
        {
            var EmployeesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            SelectEmployeesPopupVisible = true;
            await GetEmployeesList();
            await InvokeAsync(StateHasChanged);
        }


        public void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EmployeeID = Guid.Empty;
                DataSource.EmployeeName = string.Empty;
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.EmployeeID = selectedOrder.Id;
                DataSource.EmployeeName = selectedOrder.Name;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Üretim Emri Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            await GetProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }


        public void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.ProductionOrderID = selectedOrder.Id;
                DataSource.ProductionOrderFicheNo = selectedOrder.FicheNo;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region GetList Metotları

        private async Task GetProductsOperationsList()
        {
            ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        private async Task GetStationGroupsList()
        {
            StationGroupsList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion
    }
}
