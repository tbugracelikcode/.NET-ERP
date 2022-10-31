using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.ErpUI.Pages.ScheduleModal
{
    public partial class SchedulerModalExample
    {

        private bool modal1visible = false;
        private bool modal2visible = false;
        DateTime today = DateTime.Today;
        string imageURL = "images/Stations/";

        protected override async void OnInitialized()
        {
            BaseCrudService = StationsService;
            await GetListDataSourceAsync();
        }

        private void ShowModal1()
        {
            modal1visible = true;
        }

        private void HideModal1()
        {
            modal1visible = false;
        }

        private void ShowModal2()
        {
            modal2visible = true;
        }

        private void HideModal2()
        {
            modal2visible = false;
        }

        private void Modal2Save()
        {
            HideModal2();
        }


    }
}
