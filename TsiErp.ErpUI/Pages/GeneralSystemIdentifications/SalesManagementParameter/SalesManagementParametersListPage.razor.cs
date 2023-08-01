using Syncfusion.Blazor.Buttons;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.SalesManagementParameter
{
    public partial class SalesManagementParametersListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = SalesManagementParametersService;

            _L = L;

            DataSource = (await SalesManagementParametersService.GetSalesManagementParametersAsync()).Data;
        }

        private async void SalesOrderFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource.OrderFutureDateParameter = args.Checked;
        }

        private async void SalesPropositionFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource.PropositionFutureDateParameter = args.Checked;
        }

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectSalesManagementParametersDto, CreateSalesManagementParametersDto>(DataSource);

                await SalesManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await SalesManagementParametersService.GetSalesManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectSalesManagementParametersDto, UpdateSalesManagementParametersDto>(DataSource);

                await SalesManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await SalesManagementParametersService.GetSalesManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
