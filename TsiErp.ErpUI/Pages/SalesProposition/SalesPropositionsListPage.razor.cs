using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesProposition
{
    public partial class SalesPropositionsListPage
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        List<SelectSalesPropositionLinesDto> LineList = new List<SelectSalesPropositionLinesDto>();
        List<ListSalesPropositionLinesDto> GridLineList = new List<ListSalesPropositionLinesDto>();

        private bool LineEditPageVisible = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = SalesPropositionsAppService;
        }


        public async override void OnContextMenuClick(ContextMenuClickEventArgs<ListSalesPropositionsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    DataSource = new SelectSalesPropositionsDto();
                    DataSource.SelectSalesPropositionLines = new List<SelectSalesPropositionLinesDto>();
                    ShowEditPage();
                    break;

                case "changed":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz ?");

                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

    }
}
