using DevExpress.Blazor;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.EquipmentRecord
{
    public partial class EquipmentRecordsListPage
    {
        List<ListEquipmentRecordsDto> Gridlist = new List<ListEquipmentRecordsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;
        DateTime tarih = DateTime.Today;
        bool isCanceled = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await EquipmentRecordsService.GetListAsync(new ListEquipmentRecordsParameterDto() { IsActive = true })).Data.ToList();
        }

        void EquipmentRecordsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
