using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Product
{
    public partial class ProductsListPage
    {
        #region Combobox Listeleri

        SfComboBox<string, ListUnitSetsDto> UnitSetsComboBox;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();


        SfComboBox<string, ListProductGroupsDto> ProductGroupsComboBox;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        public class SupplyFormModel
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        public class TypeModel
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<SupplyFormModel> SupplyData = new List<SupplyFormModel> {
      new SupplyFormModel() { ID= "Form1", Text= "Satın Alma" },
      new SupplyFormModel() { ID= "Form2", Text= "Üretim" } };

      List<TypeModel> TypeData = new List<TypeModel> {
      new TypeModel() { ID= "TM", Text= "Ticari Mal" },
      new TypeModel() { ID= "HM", Text= "Hammadde" },
      new TypeModel() { ID= "YM", Text= "Yarı Mamül" },
      new TypeModel() { ID= "MM", Text= "Mamül" },
      new TypeModel() { ID= "BP", Text= "Yedek Parça" },
      new TypeModel() { ID= "TK", Text= "Takım" },
      new TypeModel() { ID= "KLP", Text= "Kalıp" },
      new TypeModel() { ID= "APRT", Text= "Aparat" },
  };

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }
        
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

        private void SupplyValueChangeHandler(ChangeEventArgs<string, SupplyFormModel> args)
        {
            switch (args.Value)
            {
                case "Form1":
                    DataSource.SupplyForm = 1;break;
                case "Form2":
                    DataSource.SupplyForm = 2; break;

            }
        }

        private void TypeValueChangeHandler(ChangeEventArgs<string, TypeModel> args)
        {
            switch (args.Value)
            {
                case "TM":
                    DataSource.ProductType = 1; break;
                case "HM":
                    DataSource.ProductType = 10; break;
                case "YM":
                    DataSource.ProductType = 11; break;
                case "MM":
                    DataSource.ProductType = 12; break;
                case "BP":
                    DataSource.ProductType = 30; break;
                case "TK":
                    DataSource.ProductType = 40; break;
                case "KLP":
                    DataSource.ProductType = 50; break;
                case "APRT":
                    DataSource.ProductType = 60; break;

            }
        }

        protected override async Task OnSubmit()
        {
            if(DataSource.ProductType == 0)
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Stok türü seçilmeden kaydetme işlemi yapılamaz.");
            }
            else
            {
                SelectProductsDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectProductsDto, CreateProductsDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectProductsDto, UpdateProductsDto>(DataSource);

                    result = (await UpdateAsync(updateInput)).Data;
                }

                if (result == null)
                {

                    return;
                }

                await GetListDataSourceAsync();

                var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

                HideEditPage();

                if (DataSource.Id == Guid.Empty)
                {
                    DataSource.Id = result.Id;
                }

                if (savedEntityIndex > -1)
                    SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
                else
                    SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
            }
           
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListProductsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
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
