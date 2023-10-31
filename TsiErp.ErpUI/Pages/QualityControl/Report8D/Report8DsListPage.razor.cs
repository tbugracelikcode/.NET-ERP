using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.Report8D
{
    public partial class Report8DsListPage
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        #region Değişkenler

        string Ca1 = "CA1";
        string Ca2 = "CA2";
        string Ca3 = "CA3";
        string Ro1 = "RO1";
        string Ro2 = "RO2";
        string Rn1 = "RN1";
        string Rn2 = "RN2";
        string Pa1 = "PA1";
        string Pa2 = "PA2";
        string Pa3 = "PA3";
        string Pa4 = "PA4";
        string Pa5 = "PA5";
        string Ia1 = "IA1";
        string Ia2 = "IA2";
        string Ia3 = "IA3";
        string Ia4 = "IA4";
        string Ia5 = "IA5";
        bool isCustomer = false;
        bool isSupplier = false;

        #endregion
        protected override async void OnInitialized()
        {
            _L = L;
            BaseCrudService = Report8DsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectReport8DsDto()
            {
                ClaimOpeningDate = DateTime.Now,
                Code = FicheNumbersAppService.GetFicheNumberAsync("Report8DChildMenu"),
                State_ = L["WaitingState"]
            };

            #region Combobox Localization İşlemleri

            foreach (var item in _d2ComplaintComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _d4FailureOccuranceComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA1ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA2ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA3ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA4ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA5ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA1ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA2ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA3ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA4ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA5ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IDFMEAProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IDFMEARelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IPFMEAProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IPFMEARelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IControlPlanProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IControlPlanRelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision1ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision1RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision2ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision2RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision3ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision3RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7ILessonLearnedProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7ILessonLearnedRelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _ReportAcceptedComboBox)
            {
                item.Text = L[item.Text];
            }

            #endregion

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {
            #region Combobox Localization İşlemleri

            foreach (var item in _d2ComplaintComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _d4FailureOccuranceComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA1ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA2ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA3ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA4ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D5PA5ImplementedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA1ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA2ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA3ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA4ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D6IA5ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IDFMEAProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IDFMEARelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IPFMEAProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IPFMEARelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IControlPlanProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IControlPlanRelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision1ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision1RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision2ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision2RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision3ProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7IRevision3RelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7ILessonLearnedProofAttachedComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _D7ILessonLearnedRelevantComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _ReportAcceptedComboBox)
            {
                item.Text = L[item.Text];
            }

            #endregion

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextAddforSupplier"], Id = "newsupplier" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextAddforCustomer"], Id = "newcustomer" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextState"], Id = "state" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["8DContextRefresh"], Id = "refresh" });
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListReport8DsDto> args)
        {
            switch (args.Item.Id)
            {
                case "newsupplier":
                    await BeforeInsertAsync();
                    var customer = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => t.IsSoftwareCompanyInformation == true).FirstOrDefault();
                    DataSource.CustomerID = customer.Id;
                    DataSource.CustomerName = customer.Name;
                    DataSource.CustomerCode = customer.Code;
                    isCustomer = false;
                    isSupplier = true;
                    break;

                case "newcustomer":
                    await BeforeInsertAsync();
                    var supplier = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => t.IsSoftwareCompanyInformation == true).FirstOrDefault();
                    DataSource.SupplierID = supplier.Id;
                    DataSource.SupplierCode = supplier.Code;
                    DataSource.SupplierName = supplier.Name;
                    DataSource.SupplierNo = supplier.SupplierNo;
                    isCustomer = true;
                    isSupplier = false;
                    break;

                case "state":
                    SelectReport8DsDto result;

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    DataSource.State_ = L["CompletedState"];
                    var updateInput = ObjectMapper.Map<SelectReport8DsDto, UpdateReport8DsDto>(DataSource);
                    result = (await UpdateAsync(updateInput)).Data;

                    if (result == null)
                    {
                        return;
                    }
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


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

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("Report8DChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

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
        public async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
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

        #region Müşteri ButtonEdit

        SfTextBox CustomersCodeButtonEdit;
        SfTextBox CustomersNameButtonEdit;
        bool SelectCustomersPopupVisible = false;
        List<ListCurrentAccountCardsDto> CustomersList = new List<ListCurrentAccountCardsDto>();

        public async Task CustomersCodeOnCreateIcon()
        {
            var CustomersCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CustomersCodeButtonClickEvent);
            await CustomersCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CustomersCodeButtonClick } });
        }

        public async void CustomersCodeButtonClickEvent()
        {
            SelectCustomersPopupVisible = true;
            await GetCustomersList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task GetCustomersList()
        {
            CustomersList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        public async Task CustomersNameOnCreateIcon()
        {
            var CustomersNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CustomersNameButtonClickEvent);
            await CustomersNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CustomersNameButtonClick } });
        }

        public async void CustomersNameButtonClickEvent()
        {
            SelectCustomersPopupVisible = true;
            await GetCustomersList();
            await InvokeAsync(StateHasChanged);
        }


        public void CustomersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CustomerID = Guid.Empty;
                DataSource.CustomerCode = string.Empty;
                DataSource.CustomerName = string.Empty;
            }
        }

        public async void CustomersDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCustomer = args.RowData;

            if (selectedCustomer != null)
            {
                DataSource.CustomerID = selectedCustomer.Id;
                DataSource.CustomerCode = selectedCustomer.Code;
                DataSource.CustomerName = selectedCustomer.Name;
                SelectCustomersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Tedarikçi ButtonEdit

        SfTextBox SuppliersCodeButtonEdit;
        SfTextBox SuppliersNameButtonEdit;
        bool SelectSuppliersPopupVisible = false;
        List<ListCurrentAccountCardsDto> SuppliersList = new List<ListCurrentAccountCardsDto>();

        public async Task SuppliersCodeOnCreateIcon()
        {
            var SuppliersCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SuppliersCodeButtonClickEvent);
            await SuppliersCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SuppliersCodeButtonClick } });
        }

        public async void SuppliersCodeButtonClickEvent()
        {
            SelectSuppliersPopupVisible = true;
            await GetSuppliersList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task GetSuppliersList()
        {
            SuppliersList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        public async Task SuppliersNameOnCreateIcon()
        {
            var SuppliersNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SuppliersNameButtonClickEvent);
            await SuppliersNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SuppliersNameButtonClick } });
        }

        public async void SuppliersNameButtonClickEvent()
        {
            SelectSuppliersPopupVisible = true;
            await GetSuppliersList();
            await InvokeAsync(StateHasChanged);
        }


        public void SuppliersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SupplierID = Guid.Empty;
                DataSource.SupplierCode = string.Empty;
                DataSource.SupplierName = string.Empty;
                DataSource.SupplierNo = string.Empty;
            }
        }

        public async void SuppliersDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedSupplier = args.RowData;

            if (selectedSupplier != null)
            {
                DataSource.SupplierID = selectedSupplier.Id;
                DataSource.SupplierCode = selectedSupplier.Code;
                DataSource.SupplierName = selectedSupplier.Name;
                DataSource.SupplierNo = selectedSupplier.SupplierNo;
                SelectSuppliersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Teknik Resim ButtonEdit

        SfTextBox TechnicalDrawingButtonEdit;
        bool SelectTechnicalDrawingPopupVisible = false;
        List<ListTechnicalDrawingsDto> TechnicalDrawingList = new List<ListTechnicalDrawingsDto>();

        public async Task TechnicalDrawingOnCreateIcon()
        {
            var TechnicalDrawingButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TechnicalDrawingButtonClickEvent);
            await TechnicalDrawingButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TechnicalDrawingButtonClick } });
        }

        public async void TechnicalDrawingButtonClickEvent()
        {
            if (DataSource.ProductID == null || DataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningProductTitle"], L["UIWarningProductMessage"]);
            }
            else
            {

                SelectTechnicalDrawingPopupVisible = true;
                TechnicalDrawingList = (await TechnicalDrawingsAppService.GetListAsync(new ListTechnicalDrawingsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID).ToList();
            }
            await InvokeAsync(StateHasChanged);
        }

        public void TechnicalDrawingOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TechnicalDrawingID = Guid.Empty;
                DataSource.TechnicalDrawingNo = string.Empty;
            }
        }

        public async void TechnicalDrawingDoubleClickHandler(RecordDoubleClickEventArgs<ListTechnicalDrawingsDto> args)
        {
            var selectedTechnicalDrawing = args.RowData;

            if (selectedTechnicalDrawing != null)
            {
                DataSource.TechnicalDrawingID = selectedTechnicalDrawing.Id;
                DataSource.TechnicalDrawingNo = selectedTechnicalDrawing.DrawingNo;
                SelectTechnicalDrawingPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Combobox İşlemleri

        #region D2
        public class D2ComplaintComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D2ComplaintComboBox> _d2ComplaintComboBox = new List<D2ComplaintComboBox>
        {
            new D2ComplaintComboBox(){ID = "Yes", Text="YesD2"},
            new D2ComplaintComboBox(){ID = "No", Text="NoD2"}
        };

        private void D2ComplaintComboBoxValueChangeHandler(ChangeEventArgs<string, D2ComplaintComboBox> args)
        {
            if(args.ItemData != null)
            {

            
            switch (args.ItemData.ID)
            {
                case "Yes":
                    DataSource.ComplaintJustified = L["YesD2"].Value;
                    break;

                case "No":
                    DataSource.ComplaintJustified = L["NoD2"].Value;
                    break;


                default: break;
            }}
        }

        #endregion

        #region D4

        public class D4FailureOccuranceComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D4FailureOccuranceComboBox> _d4FailureOccuranceComboBox = new List<D4FailureOccuranceComboBox>
        {
            new D4FailureOccuranceComboBox(){ID = "First", Text="FirstD4"},
            new D4FailureOccuranceComboBox(){ID = "Repetitive", Text="RepetitiveD4"}
        };

        private void D4FailureOccuranceComboBoxValueChangeHandler(ChangeEventArgs<string, D4FailureOccuranceComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "First":
                        DataSource.FailureOccurance = L["FirstD4"].Value;
                        break;

                    case "Repetitive":
                        DataSource.FailureOccurance = L["RepetitiveD4"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region D5

        public class D5PA1ImplementedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D5PA1ImplementedComboBox> _D5PA1ImplementedComboBox = new List<D5PA1ImplementedComboBox>
        {
            new D5PA1ImplementedComboBox(){ID = "Yes", Text="YesD5"},
            new D5PA1ImplementedComboBox(){ID = "No", Text="NoD5"}
        };

        private void D5PA1ImplementedComboBoxValueChangeHandler(ChangeEventArgs<string, D5PA1ImplementedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PA1ToBeImplemented = L["YesD5"].Value;
                        break;

                    case "No":
                        DataSource.PA1ToBeImplemented = L["NoD5"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D5PA2ImplementedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D5PA2ImplementedComboBox> _D5PA2ImplementedComboBox = new List<D5PA2ImplementedComboBox>
        {
            new D5PA2ImplementedComboBox(){ID = "Yes", Text="YesD5"},
            new D5PA2ImplementedComboBox(){ID = "No", Text="NoD5"}
        };

        private void D5PA2ImplementedComboBoxValueChangeHandler(ChangeEventArgs<string, D5PA2ImplementedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PA2ToBeImplemented = L["YesD5"].Value;
                        break;

                    case "No":
                        DataSource.PA2ToBeImplemented = L["NoD5"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D5PA3ImplementedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D5PA3ImplementedComboBox> _D5PA3ImplementedComboBox = new List<D5PA3ImplementedComboBox>
        {
            new D5PA3ImplementedComboBox(){ID = "Yes", Text="YesD5"},
            new D5PA3ImplementedComboBox(){ID = "No", Text="NoD5"}
        };

        private void D5PA3ImplementedComboBoxValueChangeHandler(ChangeEventArgs<string, D5PA3ImplementedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PA3ToBeImplemented = L["YesD5"].Value;
                        break;

                    case "No":
                        DataSource.PA3ToBeImplemented = L["NoD5"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D5PA4ImplementedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D5PA4ImplementedComboBox> _D5PA4ImplementedComboBox = new List<D5PA4ImplementedComboBox>
        {
            new D5PA4ImplementedComboBox(){ID = "Yes", Text="YesD5"},
            new D5PA4ImplementedComboBox(){ID = "No", Text="NoD5"}
        };

        private void D5PA4ImplementedComboBoxValueChangeHandler(ChangeEventArgs<string, D5PA4ImplementedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PA4ToBeImplemented = L["YesD5"].Value;
                        break;

                    case "No":
                        DataSource.PA4ToBeImplemented = L["NoD5"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D5PA5ImplementedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D5PA5ImplementedComboBox> _D5PA5ImplementedComboBox = new List<D5PA5ImplementedComboBox>
        {
            new D5PA5ImplementedComboBox(){ID = "Yes", Text="YesD5"},
            new D5PA5ImplementedComboBox(){ID = "No", Text="NoD5"}
        };

        private void D5PA5ImplementedComboBoxValueChangeHandler(ChangeEventArgs<string, D5PA5ImplementedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PA5ToBeImplemented = L["YesD5"].Value;
                        break;

                    case "No":
                        DataSource.PA5ToBeImplemented = L["NoD5"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region D6

        public class D6IA1ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D6IA1ProofAttachedComboBox> _D6IA1ProofAttachedComboBox = new List<D6IA1ProofAttachedComboBox>
        {
            new D6IA1ProofAttachedComboBox(){ID = "Yes", Text="YesD6"},
            new D6IA1ProofAttachedComboBox(){ID = "No", Text="NoD6"}
        };

        private void D6IA1ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D6IA1ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.IA1ProofAttached = L["YesD6"].Value;
                        break;

                    case "No":
                        DataSource.IA1ProofAttached = L["NoD6"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D6IA2ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D6IA2ProofAttachedComboBox> _D6IA2ProofAttachedComboBox = new List<D6IA2ProofAttachedComboBox>
        {
            new D6IA2ProofAttachedComboBox(){ID = "Yes", Text="YesD6"},
            new D6IA2ProofAttachedComboBox(){ID = "No", Text="NoD6"}
        };

        private void D6IA2ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D6IA2ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.IA2ProofAttached = L["YesD6"].Value;
                        break;

                    case "No":
                        DataSource.IA2ProofAttached = L["NoD6"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D6IA3ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D6IA3ProofAttachedComboBox> _D6IA3ProofAttachedComboBox = new List<D6IA3ProofAttachedComboBox>
        {
            new D6IA3ProofAttachedComboBox(){ID = "Yes", Text="YesD6"},
            new D6IA3ProofAttachedComboBox(){ID = "No", Text="NoD6"}
        };

        private void D6IA3ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D6IA3ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.IA3ProofAttached = L["YesD6"].Value;
                        break;

                    case "No":
                        DataSource.IA3ProofAttached = L["NoD6"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D6IA4ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D6IA4ProofAttachedComboBox> _D6IA4ProofAttachedComboBox = new List<D6IA4ProofAttachedComboBox>
        {
            new D6IA4ProofAttachedComboBox(){ID = "Yes", Text="YesD6"},
            new D6IA4ProofAttachedComboBox(){ID = "No", Text="NoD6"}
        };

        private void D6IA4ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D6IA4ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.IA4ProofAttached = L["YesD6"].Value;
                        break;

                    case "No":
                        DataSource.IA4ProofAttached = L["NoD6"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D6IA5ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D6IA5ProofAttachedComboBox> _D6IA5ProofAttachedComboBox = new List<D6IA5ProofAttachedComboBox>
        {
            new D6IA5ProofAttachedComboBox(){ID = "Yes", Text="YesD6"},
            new D6IA5ProofAttachedComboBox(){ID = "No", Text="NoD6"}
        };

        private void D6IA5ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D6IA5ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.IA5ProofAttached = L["YesD6"].Value;
                        break;

                    case "No":
                        DataSource.IA5ProofAttached = L["NoD6"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region D7

        #region DFMEA Revision

        public class D7IDFMEAProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IDFMEAProofAttachedComboBox> _D7IDFMEAProofAttachedComboBox = new List<D7IDFMEAProofAttachedComboBox>
        {
            new D7IDFMEAProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IDFMEAProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IDFMEAProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IDFMEAProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.DFMEARevisionProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.DFMEARevisionProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IDFMEARelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IDFMEARelevantComboBox> _D7IDFMEARelevantComboBox = new List<D7IDFMEARelevantComboBox>
        {
            new D7IDFMEARelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IDFMEARelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IDFMEARelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IDFMEARelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.DFMEARevisionRelevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.DFMEARevisionRelevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region PFMEA Revision

        public class D7IPFMEAProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IPFMEAProofAttachedComboBox> _D7IPFMEAProofAttachedComboBox = new List<D7IPFMEAProofAttachedComboBox>
        {
            new D7IPFMEAProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IPFMEAProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IPFMEAProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IPFMEAProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PFMEARevisionProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.PFMEARevisionProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IPFMEARelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IPFMEARelevantComboBox> _D7IPFMEARelevantComboBox = new List<D7IPFMEARelevantComboBox>
        {
            new D7IPFMEARelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IPFMEARelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IPFMEARelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IPFMEARelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.PFMEARevisionRelevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.PFMEARevisionRelevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region ControlPlan Revision

        public class D7IControlPlanProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IControlPlanProofAttachedComboBox> _D7IControlPlanProofAttachedComboBox = new List<D7IControlPlanProofAttachedComboBox>
        {
            new D7IControlPlanProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IControlPlanProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IControlPlanProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IControlPlanProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.ControlPlanRevisionProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.ControlPlanRevisionProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IControlPlanRelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IControlPlanRelevantComboBox> _D7IControlPlanRelevantComboBox = new List<D7IControlPlanRelevantComboBox>
        {
            new D7IControlPlanRelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IControlPlanRelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IControlPlanRelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IControlPlanRelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.ControlPlanRevisionRelevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.ControlPlanRevisionRelevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Revision1 Revision

        public class D7IRevision1ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision1ProofAttachedComboBox> _D7IRevision1ProofAttachedComboBox = new List<D7IRevision1ProofAttachedComboBox>
        {
            new D7IRevision1ProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision1ProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision1ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision1ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision1ProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision1ProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IRevision1RelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision1RelevantComboBox> _D7IRevision1RelevantComboBox = new List<D7IRevision1RelevantComboBox>
        {
            new D7IRevision1RelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision1RelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision1RelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision1RelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision1Relevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision1Relevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Revision2 Revision

        public class D7IRevision2ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision2ProofAttachedComboBox> _D7IRevision2ProofAttachedComboBox = new List<D7IRevision2ProofAttachedComboBox>
        {
            new D7IRevision2ProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision2ProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision2ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision2ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision2ProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision2ProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IRevision2RelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision2RelevantComboBox> _D7IRevision2RelevantComboBox = new List<D7IRevision2RelevantComboBox>
        {
            new D7IRevision2RelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision2RelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision2RelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision2RelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision2Relevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision2Relevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Revision3 Revision

        public class D7IRevision3ProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision3ProofAttachedComboBox> _D7IRevision3ProofAttachedComboBox = new List<D7IRevision3ProofAttachedComboBox>
        {
            new D7IRevision3ProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision3ProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision3ProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision3ProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision3ProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision3ProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7IRevision3RelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7IRevision3RelevantComboBox> _D7IRevision3RelevantComboBox = new List<D7IRevision3RelevantComboBox>
        {
            new D7IRevision3RelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7IRevision3RelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7IRevision3RelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7IRevision3RelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Revision3Relevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.Revision3Relevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region LessonLearned Revision

        public class D7ILessonLearnedProofAttachedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7ILessonLearnedProofAttachedComboBox> _D7ILessonLearnedProofAttachedComboBox = new List<D7ILessonLearnedProofAttachedComboBox>
        {
            new D7ILessonLearnedProofAttachedComboBox(){ID = "Yes", Text="YesD7"},
            new D7ILessonLearnedProofAttachedComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7ILessonLearnedProofAttachedComboBoxValueChangeHandler(ChangeEventArgs<string, D7ILessonLearnedProofAttachedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.LessonsLearnedProofAttached = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.LessonsLearnedProofAttached = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        public class D7ILessonLearnedRelevantComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<D7ILessonLearnedRelevantComboBox> _D7ILessonLearnedRelevantComboBox = new List<D7ILessonLearnedRelevantComboBox>
        {
            new D7ILessonLearnedRelevantComboBox(){ID = "Yes", Text="YesD7"},
            new D7ILessonLearnedRelevantComboBox(){ID = "No", Text="NoD7"}
        };

        private void D7ILessonLearnedRelevantComboBoxValueChangeHandler(ChangeEventArgs<string, D7ILessonLearnedRelevantComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.LessonsLearnedRelevant = L["YesD7"].Value;
                        break;

                    case "No":
                        DataSource.LessonsLearnedRelevant = L["NoD7"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #endregion

        #region Desicion

        public class ReportAcceptedComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ReportAcceptedComboBox> _ReportAcceptedComboBox = new List<ReportAcceptedComboBox>
        {
            new ReportAcceptedComboBox(){ID = "Yes", Text="YesD8"},
            new ReportAcceptedComboBox(){ID = "No", Text="NoD8"}
        };

        private void ReportAcceptedComboBoxValueChangeHandler(ChangeEventArgs<string, ReportAcceptedComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.Report8DAccepted = L["YesD8"].Value;
                        break;

                    case "No":
                        DataSource.Report8DAccepted = L["NoD8"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #endregion
    }
}
