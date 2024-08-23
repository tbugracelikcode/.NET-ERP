using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using static TsiErp.ErpUI.Pages.GeneralSystemIdentifications.ProductionManagementParameter.ProductionManagementParametersListPage;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.ProductionManagementParameter
{
    public partial class ProductionManagementParametersListPage : IDisposable
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionManagementParametersService;
            _L = L;

            DataSource = (await ProductionManagementParametersService.GetProductionManagementParametersAsync()).Data;

           
        }

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectProductionManagementParametersDto, CreateProductionManagementParametersDto>(DataSource);

                await ProductionManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await ProductionManagementParametersService.GetProductionManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectProductionManagementParametersDto, UpdateProductionManagementParametersDto>(DataSource);

                await ProductionManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await ProductionManagementParametersService.GetProductionManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
        }

        #region Varsayılan Depo ButtonEdit

        SfTextBox DefaultWarehousesButtonEdit;
        bool SelectDefaultWarehousesPopupVisible = false;
        List<ListWarehousesDto> DefaultWarehousesList = new List<ListWarehousesDto>();

        public async Task DefaultWarehousesOnCreateIcon()
        {
            var DefaultWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DefaultWarehousesButtonClickEvent);
            await DefaultWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DefaultWarehousesButtonClick } });
        }

        public async void DefaultWarehousesButtonClickEvent()
        {
            SelectDefaultWarehousesPopupVisible = true;
            DefaultWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DefaultWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.DefaultWarehouseID = Guid.Empty;
                DataSource.DefaultWarehouseCode = string.Empty;
            }
        }

        public async void DefaultWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedDefaultWarehouse = args.RowData;

            if (selectedDefaultWarehouse != null)
            {
                DataSource.DefaultWarehouseID = selectedDefaultWarehouse.Id;
                DataSource.DefaultWarehouseCode = selectedDefaultWarehouse.Code;
                SelectDefaultWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Varsayılan Şube ButtonEdit

        SfTextBox DefaultBranchesButtonEdit;
        bool SelectDefaultBranchesPopupVisible = false;
        List<ListBranchesDto> DefaultBranchesList = new List<ListBranchesDto>();

        public async Task DefaultBranchesOnCreateIcon()
        {
            var DefaultBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DefaultBranchesButtonClickEvent);
            await DefaultBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DefaultBranchesButtonClick } });
        }

        public async void DefaultBranchesButtonClickEvent()
        {
            SelectDefaultBranchesPopupVisible = true;
            DefaultBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DefaultBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.DefaultBranchID = Guid.Empty;
                DataSource.DefaultBranchCode = string.Empty;
            }
        }

        public async void DefaultBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.DefaultBranchID = selectedUnitSet.Id;
                DataSource.DefaultBranchCode = selectedUnitSet.Code;
                SelectDefaultBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }


}
