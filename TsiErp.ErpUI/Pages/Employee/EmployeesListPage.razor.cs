using DevExpress.Blazor;
using TsiErp.Entities.Entities.Employee.Dtos;

namespace TsiErp.ErpUI.Pages.Employee
{
    public partial class EmployeesListPage
    {
        List<ListEmployeesDto> Gridlist = new List<ListEmployeesDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;
        DateTime tarih = DateTime.Today;

        protected override async void OnInitialized()
        {
            //Gridlist = (await EmployeesService.GetListAsync(new ListEmployeesParameterDto() { IsActive = true })).Data.ToList();
        }

        void EmployeesPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }

    }
}
