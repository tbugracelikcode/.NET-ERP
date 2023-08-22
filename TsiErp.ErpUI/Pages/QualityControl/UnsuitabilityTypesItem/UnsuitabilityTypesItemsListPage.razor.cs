using Syncfusion.Blazor.Buttons;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityTypesItem
{
    partial class UnsuitabilityTypesItemsListPage
    {
        bool _isOperation;
        bool _isPurchase;
        bool _isContract;

        protected override void OnInitialized()
        {
            BaseCrudService = UnsuitabilityTypesItemsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityTypesItemsDto()
            {
                IsActive = true
            };

            _isContract = false;
            _isOperation = false;
            _isPurchase = false;

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public override void ShowEditPage()
        {
            switch (DataSource.UnsuitabilityTypesDescription)
            {
                case "Operation":
                    _isOperation = true;
                    _isPurchase = false;
                    _isContract = false;
                    break;

                case "Purchase":
                    _isOperation = false;
                    _isPurchase = true;
                    _isContract = false;
                    break;

                case "Contract":
                    _isOperation = false;
                    _isPurchase = false;
                    _isContract = true;
                    break;
                default:
                    break;
            }

            base.ShowEditPage();
        }

        private async void OperationChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.UnsuitabilityTypesDescription = "Operation";
                _isPurchase = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void PurchaseChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.UnsuitabilityTypesDescription = "Purchase";
                _isOperation = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void ContractChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (args.Checked)
            {
                DataSource.UnsuitabilityTypesDescription = "Contract";
                _isOperation = false;
                _isPurchase = false;
                await (InvokeAsync(StateHasChanged));
            }
        }
    }
}
