using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenancePeriod
{
    public partial class MaintenancePeriodsListPage : IDisposable
    {

        public bool? isChecked = false;
        public bool periodEnable = true;

        protected override async void OnInitialized()
        {
            BaseCrudService = MaintenancePeriodsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenancePeriodsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("MainPeriodsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenancePeriodContextRefresh"], Id = "refresh" });
        }

        private void Change(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if(args.Checked == true)
            {
                periodEnable = false;
                DataSource.IsDaily = true;
                DataSource.PeriodTime = 0;


            }
            else
            {
                periodEnable = true;
                DataSource.IsDaily = false;

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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MainPeriodsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
