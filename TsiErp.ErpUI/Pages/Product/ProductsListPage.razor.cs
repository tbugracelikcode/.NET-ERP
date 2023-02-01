using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.Product
{
    public partial class ProductsListPage
    {

        #region Ürün Grubu ButtonEdit
        SfTextBox ProductGroupsButtonEdit;
        bool SelectProductGroupsPopupVisible = false;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        public async Task ProductGroupsOnCreateIcon()
        {
            var ProductGroupsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductGroupsButtonClickEvent);
            await ProductGroupsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductGroupsButtonClick } });
        }

        public async void ProductGroupsButtonClickEvent()
        {
            SelectProductGroupsPopupVisible = true;
            await GetProductGroupsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductGroupsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductGrpID = Guid.Empty;
                DataSource.ProductGrp = string.Empty;
            }
        }

        public async void ProductGroupsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductGroupsDto> args)
        {
            var selectedProductGroup = args.RowData;

            if (selectedProductGroup != null)
            {
                DataSource.ProductGrpID = selectedProductGroup.Id;
                DataSource.ProductGrp = selectedProductGroup.Name;
                SelectProductGroupsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Birim Setleri ButtonEdit
        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.UnitSetID = Guid.Empty;
                DataSource.UnitSet = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.UnitSetID = selectedUnitSet.Id;
                DataSource.UnitSet = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductService;
            await GetUnitSetsList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetProductGroupsList()
        {
            ProductGroupsList = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.ToList();
        }

    }
}
