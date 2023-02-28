using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StationGroup
{
    public partial class StationGroupsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = StationGroupsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource= new SelectStationGroupsDto()
            { 
                IsActive = true 
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
