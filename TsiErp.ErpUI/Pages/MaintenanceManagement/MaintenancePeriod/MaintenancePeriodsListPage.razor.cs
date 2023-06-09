using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenancePeriod
{
    public partial class MaintenancePeriodsListPage
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
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
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

    }
}
