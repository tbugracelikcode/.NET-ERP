using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.StockManagementParameter
{
    public partial class StockManagementParametersListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = StockManagementParametersService;

            _L = L;

            DataSource = (await StockManagementParametersService.GetStockManagementParametersAsync()).Data;
        }

        private async void StockFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource.FutureDateParameter = args.Checked;
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
    }
}
