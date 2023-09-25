using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
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

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["FinanceManagementParameterContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["FinanceManagementParameterContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["FinanceManagementParameterContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["FinanceManagementParameterContextRefresh"], Id = "refresh" });
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
