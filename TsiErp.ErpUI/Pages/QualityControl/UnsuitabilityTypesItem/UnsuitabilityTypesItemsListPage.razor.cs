using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityTypesItem
{
    partial class UnsuitabilityTypesItemsListPage
    {
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

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
