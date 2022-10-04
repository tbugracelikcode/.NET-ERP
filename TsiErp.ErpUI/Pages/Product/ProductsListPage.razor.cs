using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.Product
{
    public partial class ProductsListPage
    {
        SfComboBox<string, ListUnitSetsDto> UnitSetsComboBox;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        private SfGrid<ListProductsDto> _grid;


        SfComboBox<string, ListProductGroupsDto> ProductGroupsComboBox;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductService;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
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

        #region Birim Setleri ComboBox
        public async Task UnitSetFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await UnitSetsComboBox.FilterAsync(UnitSetsList, query);
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        public async Task UnitSetOpened(PopupEventArgs args)
        {
            if (UnitSetsList.Count == 0)
            {
                await GetUnitSetsList();
            }
        }

        private void UnitSetValueChanged(ChangeEventArgs<string, ListUnitSetsDto> args)
        {
            DataSource.UnitSetID = args.ItemData.Id;
            DataSource.UnitSet = args.ItemData.Name;
        }
        #endregion

        #region Ürün Grupları ComboBox
        public async Task ProductGroupFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductGroupsComboBox.FilterAsync(ProductGroupsList, query);
        }

        private async Task GetProductGroupsList()
        {
            ProductGroupsList = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.ToList();
        }

        public async Task ProductGroupOpened(PopupEventArgs args)
        {
            if (ProductGroupsList.Count == 0)
            {
                await GetProductGroupsList();
            }
        }

        private void ProductGroupValueChanged(ChangeEventArgs<string, ListProductGroupsDto> args)
        {
            DataSource.ProductGrpID = args.ItemData.Id;
            DataSource.ProductGrp = args.ItemData.Name;
        }
        #endregion
    }
}
