using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.WorkOrder
{
    public partial class WorkOrdersListPage
    {
        #region ComboBox Listeleri

        //SfComboBox<string, ListProductionOrdersDto> ProductionOrdersComboBox;
        //List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        //SfComboBox<string, ListSalesPropositionsDto> PropositionsComboBox;
        //List<ListSalesPropositionsDto> PropositionsList = new List<ListSalesPropositionsDto>();

        //SfComboBox<string, ListRoutesDto> RoutesComboBox;
        //List<ListRoutesDto> RoutesList = new List<ListRoutesDto>();

        //SfComboBox<string, ListProductsOperationsDto> ProductsOperationsComboBox;
        //List<ListProductsOperationsDto> ProductsOperationsList = new List<ListProductsOperationsDto>();

        //SfComboBox<string, ListStationsDto> StationsComboBox;
        //List<ListStationsDto> StationsList = new List<ListStationsDto>();

        //SfComboBox<string, ListStationGroupsDto> StationGroupsComboBox;
        //List<ListStationGroupsDto> StationGroupsList = new List<ListStationGroupsDto>();

        //SfComboBox<string, ListProductsDto> ProductsComboBox;
        //List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        //SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        //List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        #endregion

        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;

            //await GetProductionOrdersList();
            //await GetProductsOperationsList();
        }
    }
}
