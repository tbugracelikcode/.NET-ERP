using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.ControlType
{
    partial class ControlTypesListPage
    {

        bool _isOperation;
        bool _isPurchase;
        bool _isContract;

        protected override void OnInitialized()
        {
            BaseCrudService = ControlTypesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectControlTypesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ControlTypesChildMenu")
            };


            _isContract = false;
            _isOperation = false;
            _isPurchase = false;

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlTypeContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlTypeContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlTypeContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlTypeContextRefresh"], Id = "refresh" });
        }

        public override void ShowEditPage()
        {
            switch (DataSource.QualityPlanTypes)
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
                DataSource.QualityPlanTypes = "Operation";
                _isPurchase = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void PurchaseChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.QualityPlanTypes = "Purchase";
                _isOperation = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void ContractChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (args.Checked)
            {
                DataSource.QualityPlanTypes = "Contract";
                _isOperation = false;
                _isPurchase = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ControlTypesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
