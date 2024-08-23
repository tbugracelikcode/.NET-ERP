using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
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
            foreach (var item in _mrpIISourceModuleComboBox)
            {
                item.Text = L[item.Text];
            }
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextAdd"], Id = "new" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextChange"], Id = "changed" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextDelete"], Id = "delete" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PlanningManagementParameterContextRefresh"], Id = "refresh" });
            }
        }

        #region ComboBox İşlemleri

        public class MRPPurchaseComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        public class MRPIISourceModule
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<MRPPurchaseComboBox> _mrpPurchaseComboBox = new List<MRPPurchaseComboBox>
        {
            new MRPPurchaseComboBox(){ID = "1", Text="PurchaseOrder"},
            new MRPPurchaseComboBox(){ID = "2", Text="PurchaseRequest"},
        };

        private void MRPPurchaseComboBoxValueChangeHandler(ChangeEventArgs<string, MRPPurchaseComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "1":
                        DataSource.MRPPurchaseTransaction = 1;
                        break;

                    case "2":
                        DataSource.MRPPurchaseTransaction = 2;
                        break;

                    default: break;
                }
            }
        }

        List<MRPIISourceModule> _mrpIISourceModuleComboBox = new List<MRPIISourceModule>
        {
            new MRPIISourceModule(){ID = "orderacceptance", Text="OrderAcceptance"},
            new MRPIISourceModule(){ID = "salesorder", Text="SalesOrder"},
        };

        private void MRPIISourceModuleComboBoxValueChangeHandler(ChangeEventArgs<string, MRPIISourceModule> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "orderacceptance":
                        DataSource.MRPIISourceModule = 1;
                        break;

                    case "salesorder":
                        DataSource.MRPIISourceModule = 2;
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

        #region Varsayılan Depo ButtonEdit

        SfTextBox DefaultWarehousesButtonEdit;
        bool SelectDefaultWarehousesPopupVisible = false;
        List<ListWarehousesDto> DefaultWarehousesList = new List<ListWarehousesDto>();

        public async Task DefaultWarehousesOnCreateIcon()
        {
            var DefaultWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DefaultWarehousesButtonClickEvent);
            await DefaultWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DefaultWarehousesButtonClick } });
        }

        public async void DefaultWarehousesButtonClickEvent()
        {
            SelectDefaultWarehousesPopupVisible = true;
            DefaultWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DefaultWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.DefaultWarehouseID = Guid.Empty;
                DataSource.DefaultWarehouseCode = string.Empty;
            }
        }

        public async void DefaultWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedDefaultWarehouse = args.RowData;

            if (selectedDefaultWarehouse != null)
            {
                DataSource.DefaultWarehouseID = selectedDefaultWarehouse.Id;
                DataSource.DefaultWarehouseCode = selectedDefaultWarehouse.Code;
                SelectDefaultWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Varsayılan Şube ButtonEdit

        SfTextBox DefaultBranchesButtonEdit;
        bool SelectDefaultBranchesPopupVisible = false;
        List<ListBranchesDto> DefaultBranchesList = new List<ListBranchesDto>();

        public async Task DefaultBranchesOnCreateIcon()
        {
            var DefaultBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DefaultBranchesButtonClickEvent);
            await DefaultBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DefaultBranchesButtonClick } });
        }

        public async void DefaultBranchesButtonClickEvent()
        {
            SelectDefaultBranchesPopupVisible = true;
            DefaultBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DefaultBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.DefaultBranchID = Guid.Empty;
                DataSource.DefaultBranchCode = string.Empty;
            }
        }

        public async void DefaultBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.DefaultBranchID = selectedUnitSet.Id;
                DataSource.DefaultBranchCode = selectedUnitSet.Code;
                SelectDefaultBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
