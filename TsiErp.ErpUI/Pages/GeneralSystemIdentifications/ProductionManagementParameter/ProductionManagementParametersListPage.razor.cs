using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
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

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }


}
