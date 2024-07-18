using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.PurchaseManagementParameter
{
    public partial class PurchaseManagementParametersListPage : IDisposable
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseManagementParametersService;

            _L = L;

            DataSource = (await PurchaseManagementParametersService.GetPurchaseManagementParametersAsync()).Data;

            foreach (var item in _purchaseOrderExchageTypeComboBox)
            {
                item.Text = L[item.Text];
            }
            foreach (var item in _purchaseRequestExchageTypeComboBox)
            {
                item.Text = L[item.Text];
            }
        }

        private void PurchaseOrderFichesChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.OrderFutureDateParameter = args.Checked;
        }

        private void PurchaseRequestFichesChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.RequestFutureDateParameter = args.Checked;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextAdd"], Id = "new" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextChange"], Id = "changed" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextDelete"], Id = "delete" });
                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseManagementParameterContextRefresh"], Id = "refresh" });
            }
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

        #region Depo ButtonEdit

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehousesOnCreateIcon()
        {
            var WarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehousesButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WarehouseID = Guid.Empty;
                DataSource.WarehouseCode = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                DataSource.WarehouseID = selectedWarehouse.Id;
                DataSource.WarehouseCode = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchesOnCreateIcon()
        {
            var BranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchesButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region ComboBox İşlemleri

        public class PurchaseOrderExchageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        public class PurchaseRequestExchageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<PurchaseOrderExchageTypeComboBox> _purchaseOrderExchageTypeComboBox = new List<PurchaseOrderExchageTypeComboBox>
        {
            new PurchaseOrderExchageTypeComboBox(){ID = "0", Text="ForexBuying"},
            new PurchaseOrderExchageTypeComboBox(){ID = "1", Text="ForexSelling"},
            new PurchaseOrderExchageTypeComboBox(){ID = "2", Text="BanknoteBuying"},
            new PurchaseOrderExchageTypeComboBox(){ID = "3", Text="BanknoteSelling"},
        };

        List<PurchaseRequestExchageTypeComboBox> _purchaseRequestExchageTypeComboBox = new List<PurchaseRequestExchageTypeComboBox>
        {
            new PurchaseRequestExchageTypeComboBox(){ID = "0", Text="ForexBuying"},
            new PurchaseRequestExchageTypeComboBox(){ID = "1", Text="ForexSelling"},
            new PurchaseRequestExchageTypeComboBox(){ID = "2", Text="BanknoteBuying"},
            new PurchaseRequestExchageTypeComboBox(){ID = "3", Text="BanknoteSelling"},
        };

        private void PurchaseOrderExchageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, PurchaseOrderExchageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0":
                        DataSource.PurchaseOrderExchangeRateType = 0;
                        break;

                    case "1":
                        DataSource.PurchaseOrderExchangeRateType = 1;
                        break;
                    case "2":
                        DataSource.PurchaseOrderExchangeRateType = 2;
                        break;
                    case "3":
                        DataSource.PurchaseOrderExchangeRateType = 3;
                        break;

                    default: break;
                }
            }
        }

        private void PurchaseRequestExchageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, PurchaseRequestExchageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0":
                        DataSource.PurchaseRequestExchangeRateType = 0;
                        break;

                    case "1":
                        DataSource.PurchaseRequestExchangeRateType = 1;
                        break;
                    case "2":
                        DataSource.PurchaseRequestExchangeRateType = 2;
                        break;
                    case "3":
                        DataSource.PurchaseRequestExchangeRateType = 3;
                        break;

                    default: break;
                }
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
