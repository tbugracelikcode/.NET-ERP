using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.StockManagementParameter
{
    public partial class StockManagementParametersListPage : IDisposable
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = StockManagementParametersService;

            _L = L;

            DataSource = (await StockManagementParametersService.GetStockManagementParametersAsync()).Data;

            foreach (var item in _costCalculationMethodComboBox)
            {
                item.Text = L[item.Text];
            }
        }

        private void StockFichesChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.FutureDateParameter = args.Checked;
        }

        private void AutoCostChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.AutoCostParameter = args.Checked;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextAdd"], Id = "new" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextChange"], Id = "changed" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextDelete"], Id = "delete" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextRefresh"], Id = "refresh" });
            }
        }

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectStockManagementParametersDto, CreateStockManagementParametersDto>(DataSource);

                await StockManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await StockManagementParametersService.GetStockManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectStockManagementParametersDto, UpdateStockManagementParametersDto>(DataSource);

                await StockManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await StockManagementParametersService.GetStockManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
        }

        public class CostCalculationMethodComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<CostCalculationMethodComboBox> _costCalculationMethodComboBox = new List<CostCalculationMethodComboBox>
        {
            new CostCalculationMethodComboBox(){ID = "1", Text="FIFOCombo"},
            new CostCalculationMethodComboBox(){ID = "2", Text="LIFOCombo"},
            new CostCalculationMethodComboBox(){ID = "3", Text="AverageCombo"}
        };

        private void CostCalculationMethodComboBoxValueChangeHandler(ChangeEventArgs<string, CostCalculationMethodComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "1":
                        DataSource.CostCalculationMethod = 1;
                        break;

                    case "2":
                        DataSource.CostCalculationMethod = 2;
                        break;
                    case "3":
                        DataSource.CostCalculationMethod = 3;
                        break;


                    default: break;
                }
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
