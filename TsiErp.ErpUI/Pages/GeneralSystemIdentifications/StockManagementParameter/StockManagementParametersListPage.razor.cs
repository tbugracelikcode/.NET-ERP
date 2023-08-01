using Syncfusion.Blazor.Buttons;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.ModuleConstant;
using TsiErp.Entities.TableConstant;

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
