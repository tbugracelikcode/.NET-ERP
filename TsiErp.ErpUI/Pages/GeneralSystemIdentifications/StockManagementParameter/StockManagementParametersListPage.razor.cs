using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

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
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StockManagementParameterContextRefresh"], Id = "refresh" });
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


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
