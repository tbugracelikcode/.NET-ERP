using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using static TsiErp.ErpUI.Pages.GeneralSystemIdentifications.PurchaseManagementParameter.PurchaseManagementParametersListPage;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.SalesManagementParameter
{
    public partial class SalesManagementParametersListPage : IDisposable
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = SalesManagementParametersService;
            _L = L;

            DataSource = (await SalesManagementParametersService.GetSalesManagementParametersAsync()).Data;

            foreach (var item in _salesOrderExchageTypeComboBox)
            {
                item.Text = L[item.Text];
            }
            foreach (var item in _salesPropositionExchageTypeComboBox)
            {
                item.Text = L[item.Text];
            }
        }

        private void SalesOrderFichesChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.OrderFutureDateParameter = args.Checked;
        }

        private void SalesPropositionFichesChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            DataSource.PropositionFutureDateParameter = args.Checked;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesManagementParameterContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesManagementParameterContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesManagementParameterContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesManagementParameterContextRefresh"], Id = "refresh" });
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

        #region ComboBox İşlemleri

        public class SalesOrderExchageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        public class SalesPropositionExchageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<SalesOrderExchageTypeComboBox> _salesOrderExchageTypeComboBox = new List<SalesOrderExchageTypeComboBox>
        {
            new SalesOrderExchageTypeComboBox(){ID = "0", Text="ForexBuying"},
            new SalesOrderExchageTypeComboBox(){ID = "1", Text="ForexSelling"},
            new SalesOrderExchageTypeComboBox(){ID = "2", Text="BanknoteBuying"},
            new SalesOrderExchageTypeComboBox(){ID = "3", Text="BanknoteSelling"},
        };

        List<SalesPropositionExchageTypeComboBox> _salesPropositionExchageTypeComboBox = new List<SalesPropositionExchageTypeComboBox>
        {
            new SalesPropositionExchageTypeComboBox(){ID = "0", Text="ForexBuying"},
            new SalesPropositionExchageTypeComboBox(){ID = "1", Text="ForexSelling"},
            new SalesPropositionExchageTypeComboBox(){ID = "2", Text="BanknoteBuying"},
            new SalesPropositionExchageTypeComboBox(){ID = "3", Text="BanknoteSelling"},
        };

        private void SalesOrderExchageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, SalesOrderExchageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0":
                        DataSource.SalesOrderExchangeRateType = 0;
                        break;

                    case "1":
                        DataSource.SalesOrderExchangeRateType = 1;
                        break;
                    case "2":
                        DataSource.SalesOrderExchangeRateType = 2;
                        break;
                    case "3":
                        DataSource.SalesOrderExchangeRateType = 3;
                        break;

                    default: break;
                }
            }
        }

        private void SalesPropositionExchageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, SalesPropositionExchageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "0":
                        DataSource.SalesPropositionExchangeRateType = 0;
                        break;

                    case "1":
                        DataSource.SalesPropositionExchangeRateType = 1;
                        break;
                    case "2":
                        DataSource.SalesPropositionExchangeRateType = 2;
                        break;
                    case "3":
                        DataSource.SalesPropositionExchangeRateType = 3;
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
