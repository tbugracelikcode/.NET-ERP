using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
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

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextRefresh"], Id = "refresh" });
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
