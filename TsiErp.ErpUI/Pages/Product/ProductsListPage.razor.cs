using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Branch.Dtos;
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


        SfComboBox<string, ListProductGroupsDto> ProductGroupsComboBox;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductService;
            await GetUnitSetsList();
            await GetProductGroupsList();
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

        public async Task UnitSetValueChangeHandler(ChangeEventArgs<string, ListUnitSetsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.UnitSetID = args.ItemData.Id;
                DataSource.UnitSet = args.ItemData.Name;
            }
            else
            {
                DataSource.UnitSetID = Guid.Empty;
                DataSource.UnitSet = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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

        public async Task ProductGroupValueChangeHandler(ChangeEventArgs<string, ListProductGroupsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductGrpID = args.ItemData.Id;
                DataSource.ProductGrp = args.ItemData.Name;
            }
            else
            {
                DataSource.ProductGrpID = Guid.Empty;
                DataSource.ProductGrp = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
