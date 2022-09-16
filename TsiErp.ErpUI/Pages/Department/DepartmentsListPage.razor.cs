using DevExpress.Blazor;
using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.ErpUI.Pages.Department
{
    public partial class DepartmentsListPage
    {
        List<ListDepartmentsDto> Gridlist = new List<ListDepartmentsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await DepartmentsService.GetListAsync(new ListDepartmentsParameterDto() { IsActive = true })).Data.ToList();
        }

        void DepartmentsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
