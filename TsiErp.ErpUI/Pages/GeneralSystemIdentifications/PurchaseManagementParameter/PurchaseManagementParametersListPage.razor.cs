using Syncfusion.Blazor.Buttons;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.PurchaseManagementParameter
{
    public partial class PurchaseManagementParametersListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseManagementParametersService;

            _L = L;

            DataSource = (await PurchaseManagementParametersService.GetPurchaseManagementParametersAsync()).Data;
        }

        private async void PurchaseOrderFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource.OrderFutureDateParameter = args.Checked;
        }

        private async void PurchaseRequestFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource.RequestFutureDateParameter = args.Checked;
        }

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectPurchaseManagementParametersDto, CreatePurchaseManagementParametersDto>(DataSource);

                await PurchaseManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await PurchaseManagementParametersService.GetPurchaseManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectPurchaseManagementParametersDto, UpdatePurchaseManagementParametersDto>(DataSource);

                await PurchaseManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await PurchaseManagementParametersService.GetPurchaseManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
