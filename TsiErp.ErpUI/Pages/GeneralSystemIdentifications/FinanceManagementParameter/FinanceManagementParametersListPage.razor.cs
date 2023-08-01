using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.FinanceManagementParameter
{
    public partial class FinanceManagementParametersListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = FinanceManagementParametersService;

            _L = L;

            DataSource = (await FinanceManagementParametersService.GetFinanceManagementParametersAsync()).Data;
        }

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectFinanceManagementParametersDto, CreateFinanceManagementParametersDto>(DataSource);

                await FinanceManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await FinanceManagementParametersService.GetFinanceManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectFinanceManagementParametersDto, UpdateFinanceManagementParametersDto>(DataSource);

                await FinanceManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await FinanceManagementParametersService.GetFinanceManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
