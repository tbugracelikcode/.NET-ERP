using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationalQualityPlan
{
    public partial class OperationalQualityPlansListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = OperationalQualityPlansAppService;
            _L = L;
        }

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Kontrol Türü Button Edit

        SfTextBox ControlTypesCodeButtonEdit;
        SfTextBox ControlTypesNameButtonEdit;
        bool SelectControlTypesPopupVisible = false;
        List<ListControlTypesDto> ControlTypesList = new List<ListControlTypesDto>();
        public async Task ControlTypesCodeOnCreateIcon()
        {
            var ControlTypesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlTypesCodeButtonClickEvent);
            await ControlTypesCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlTypesButtonClick } });
        }

        public async void ControlTypesCodeButtonClickEvent()
        {
            SelectControlTypesPopupVisible = true;
            await GetControlTypesList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ControlTypesNameOnCreateIcon()
        {
            var ControlTypesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlTypesNameButtonClickEvent);
            await ControlTypesNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlTypesButtonClick } });
        }

        public async void ControlTypesNameButtonClickEvent()
        {
            SelectControlTypesPopupVisible = true;
            await GetControlTypesList();
            await InvokeAsync(StateHasChanged);
        }

        public void ControlTypesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ControlTypesID = Guid.Empty;
                DataSource.ControlTypesName = string.Empty;
            }
        }

        public async void ControlTypesDoubleClickHandler(RecordDoubleClickEventArgs<ListControlTypesDto> args)
        {
            var selectedControlType = args.RowData;

            if (selectedControlType != null)
            {
                DataSource.ControlTypesID = selectedControlType.Id;
                DataSource.ControlTypesName = selectedControlType.Name;
                SelectControlTypesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Kontrol Şartı Button Edit

        SfTextBox ControlConditionsCodeButtonEdit;
        SfTextBox ControlConditionsNameButtonEdit;
        bool SelectControlConditionsPopupVisible = false;
        List<ListControlConditionsDto> ControlConditionsList = new List<ListControlConditionsDto>();
        public async Task ControlConditionsCodeOnCreateIcon()
        {
            var ControlConditionsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlConditionsCodeButtonClickEvent);
            await ControlConditionsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlConditionsButtonClick } });
        }

        public async void ControlConditionsCodeButtonClickEvent()
        {
            SelectControlConditionsPopupVisible = true;
            await GetControlConditionsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ControlConditionsNameOnCreateIcon()
        {
            var ControlConditionsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ControlConditionsNameButtonClickEvent);
            await ControlConditionsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ControlConditionsButtonClick } });
        }

        public async void ControlConditionsNameButtonClickEvent()
        {
            SelectControlConditionsPopupVisible = true;
            await GetControlConditionsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ControlConditionsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ControlConditionsID = Guid.Empty;
                DataSource.ControlConditionsName = string.Empty;
            }
        }

        public async void ControlConditionsDoubleClickHandler(RecordDoubleClickEventArgs<ListControlConditionsDto> args)
        {
            var selectedControlCondition = args.RowData;

            if (selectedControlCondition != null)
            {
                DataSource.ControlConditionsID = selectedControlCondition.Id;
                DataSource.ControlConditionsName = selectedControlCondition.Name;
                SelectControlConditionsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İstasyon Grubu ButtonEdit

        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;
        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            StationGroupList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkCenterID = Guid.Empty;
                DataSource.WorkCenterName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.WorkCenterID = selectedStationGroup.Id;
                DataSource.WorkCenterName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }


        private async Task GetControlTypesList()
        {
            ControlTypesList = (await ControlTypesAppService.GetListAsync(new ListControlTypesParameterDto())).Data.ToList();
        }

        private async Task GetControlConditionsList()
        {
            ControlConditionsList = (await ControlConditionsAppService.GetListAsync(new ListControlConditionsParameterDto())).Data.ToList();
        }

        #endregion

    }
}
