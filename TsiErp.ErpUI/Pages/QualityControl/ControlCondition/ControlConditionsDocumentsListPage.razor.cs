using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.ControlCondition
{
    partial class ControlConditionsDocumentsListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = ControlConditionsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectControlConditionsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
