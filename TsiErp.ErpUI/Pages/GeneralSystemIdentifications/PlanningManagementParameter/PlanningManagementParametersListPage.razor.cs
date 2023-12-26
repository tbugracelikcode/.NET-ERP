using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.PlanningManagementParameter
{
    public partial class PlanningManagementParametersListPage : IDisposable
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = PlanningManagementParametersService;

            _L = L;

            DataSource = (await PlanningManagementParametersService.GetPlanningManagementParametersAsync()).Data;

            foreach (var item in _mrpPurchaseComboBox)
            {
                item.Text = L[item.Text];
            }
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextRefresh"], Id = "refresh" });
        }

        #region ComboBox İşlemleri

        public class MRPPurchaseComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<MRPPurchaseComboBox> _mrpPurchaseComboBox = new List<MRPPurchaseComboBox>
        {
            new MRPPurchaseComboBox(){ID = "salesorder", Text="PurchaseOrder"},
            new MRPPurchaseComboBox(){ID = "salesrequest", Text="PurchaseRequest"},
        };

        private void MRPPurchaseComboBoxValueChangeHandler(ChangeEventArgs<string, MRPPurchaseComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "salesorder":
                        DataSource.MRPPurchaseTransaction = 1;
                        break;

                    case "salesrequest":
                        DataSource.MRPPurchaseTransaction = 2;
                        break;

                    default: break;
                }
            }
        }

        #endregion

        private async void OnClick()
        {
            if (DataSource.Id == Guid.Empty)
            {
                var createdEntity = ObjectMapper.Map<SelectPlanningManagementParametersDto, CreatePlanningManagementParametersDto>(DataSource);

                await PlanningManagementParametersService.CreateAsync(createdEntity);

                DataSource = (await PlanningManagementParametersService.GetPlanningManagementParametersAsync()).Data;

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var updatedEntity = ObjectMapper.Map<SelectPlanningManagementParametersDto, UpdatePlanningManagementParametersDto>(DataSource);

                await PlanningManagementParametersService.UpdateAsync(updatedEntity);

                DataSource = (await PlanningManagementParametersService.GetPlanningManagementParametersAsync()).Data;

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
