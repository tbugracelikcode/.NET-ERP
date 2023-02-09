using BlazorInputFile;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Text;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using FluentValidation;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;

namespace TsiErp.ErpUI.Pages.Product
{
    public partial class ProductsListPage
    {

        #region Ürün Grubu ButtonEdit
        SfTextBox ProductGroupsButtonEdit;
        bool SelectProductGroupsPopupVisible = false;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();
        #endregion

        #region Combobox İşlemleri
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

        List<ComboBoxEnumItem<ProductTypeEnum>> ProductTypesList = new List<ComboBoxEnumItem<ProductTypeEnum>>();
        public string[] ProductTypes { get; set; }

        public string[] ProductEnumValues = Enum.GetNames(typeof(ProductTypeEnum));

        #endregion

        public SelectTechnicalDrawingsDto TechnicalDrawingsDataSource { get; set; }
        public SelectProductReferanceNumbersDto ProductReferanceNumbersDataSource { get; set; }
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> TechnicalDrawingGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductReferanceNumberGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectTechnicalDrawingsDto> TechnicalDrawingsList = new List<SelectTechnicalDrawingsDto>();

        public List<SelectProductReferanceNumbersDto> ProductReferanceNumbersList = new List<SelectProductReferanceNumbersDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        private SfGrid<SelectTechnicalDrawingsDto> _TechnicalDrawingGrid;
        private SfGrid<SelectProductReferanceNumbersDto> _ProductReferanceNumberGrid;

        #region Değişkenler

        public bool TechnicalDrawingsCrudPopup = false;
        public bool TechnicalDrawingsPopup = false;

        public bool ProductReferanceNumbersCrudPopup = false;
        public bool ProductReferanceNumbersPopup = false;

        List<string> Drawers = new List<string>();

        List<IFileListEntry> files = new List<IFileListEntry>();

        bool disable;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        #endregion

        //private void SupplyValueChangeHandler(ChangeEventArgs<string, SupplyFormModel> args)
        //{
        //    switch (args.Value)
        //    {
        //        case "Form1":
        //            DataSource.SupplyForm = 1;break;
        //        case "Form2":
        //            DataSource.SupplyForm = 2; break;

        //    }
        //}

        //private void TypeValueChangeHandler(ChangeEventArgs<string, TypeModel> args)
        //{
        //    switch (args.Value)
        //    {
        //        case "TM":
        //            DataSource.ProductType = 1; break;
        //        case "HM":
        //            DataSource.ProductType = 10; break;
        //        case "YM":
        //            DataSource.ProductType = 11; break;
        //        case "MM":
        //            DataSource.ProductType = 12; break;
        //        case "BP":
        //            DataSource.ProductType = 30; break;
        //        case "TK":
        //            DataSource.ProductType = 40; break;
        //        case "KLP":
        //            DataSource.ProductType = 50; break;
        //        case "APRT":
        //            DataSource.ProductType = 60; break;

        //    }
        //}

        protected override async Task OnSubmit()
        {
            if (DataSource.ProductType == 0)
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

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Teknik Resimler", Id = "technicaldrawings" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ürün Referans Numaraları", Id = "productreferancenumbers" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        #region Teknik Resim Modalı İşlemleri

        protected void CreateTechnicalDrawingContextMenuItems()
        {
            if (TechnicalDrawingGridContextMenu.Count() == 0)
            {
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void OnTechnicalDrawingContextMenuClick(ContextMenuClickEventArgs<SelectTechnicalDrawingsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    TechnicalDrawingsDataSource = new SelectTechnicalDrawingsDto();
                    TechnicalDrawingsCrudPopup = true;
                    TechnicalDrawingsDataSource.ProductID = DataSource.Id;
                    TechnicalDrawingsDataSource.ProductCode = DataSource.Code;
                    TechnicalDrawingsDataSource.ProductName = DataSource.Name;
                    TechnicalDrawingsDataSource.RevisionDate = DateTime.Today; 
                    Drawers = TechnicalDrawingsList.Select(t => t.Drawer).Distinct().ToList();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    TechnicalDrawingsDataSource = args.RowInfo.RowData;
                    TechnicalDrawingsCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz teknik resim, kalıcı olarak silinecektir.");

                    if (res == true)
                    {

                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();


                        await _TechnicalDrawingGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _TechnicalDrawingGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async Task OnTechnicalDrawingSubmit()
        {
            #region Submit İşlemleri

            SelectTechnicalDrawingsDto result;

            if (TechnicalDrawingsDataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectTechnicalDrawingsDto, CreateTechnicalDrawingsDto>(TechnicalDrawingsDataSource);

                result = (await TechnicalDrawingsAppService.CreateAsync(createInput)).Data;

                if (result != null)
                    TechnicalDrawingsDataSource.Id = result.Id;
            }

            else
            {
                var updateInput = ObjectMapper.Map<SelectTechnicalDrawingsDto, UpdateTechnicalDrawingsDto>(TechnicalDrawingsDataSource);

                result = (await TechnicalDrawingsAppService.UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {
                return;
            }

             await TechnicalDrawingsAppService.GetListAsync(new ListTechnicalDrawingsParameterDto() { ProductId = DataSource.Id});

            var savedEntityIndex = TechnicalDrawingsList.FindIndex(x => x.Id == TechnicalDrawingsDataSource.Id);

            TechnicalDrawingsList = (await TechnicalDrawingsAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();

            await _TechnicalDrawingGrid.Refresh();

            HideTechnicalDrawingCrudPopup();

            if (TechnicalDrawingsDataSource.Id == Guid.Empty)
            {
                TechnicalDrawingsDataSource.Id = result.Id;
            }

            //if (savedEntityIndex > -1)
            //    SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            //else
            //    SelectedItem = ListDataSource.GetEntityById(DataSource.Id);

            #endregion

            #region File Upload İşlemleri

            string productid = TechnicalDrawingsDataSource.ProductID.ToString();
            string productcode = TechnicalDrawingsDataSource.ProductCode;
            string technicaldrawingid = TechnicalDrawingsDataSource.Id.ToString();

            List<string> _result = new List<string>();

            foreach (var file in files)
            {
                disable = true;

                string fileName = file.Name;
                string rootPath = "UploadedFiles/TechnicalDrawings/" + productid + "-" + productcode + "/" + technicaldrawingid;


                _result.Add(await FileUploadService.UploadTechnicalDrawing(file, rootPath, fileName));
                await InvokeAsync(() => StateHasChanged());

            }

            disable = false;

            files.Clear();

            await InvokeAsync(() => StateHasChanged());

            #endregion
        }

        public void HideTechnicalDrawingCrudPopup()
        {
            TechnicalDrawingsCrudPopup = false;
        }

        public void HideTechnicalDrawingPopup()
        {
            TechnicalDrawingsPopup = false;
        }

        #endregion

        #region Ürün Referans Numarası Modalı İşlemleri

        protected void CreateProductReferanceNumberContextMenuItems()
        {
            if (ProductReferanceNumberGridContextMenu.Count() == 0)
            {
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void OnProductReferanceNumberContextMenuClick(ContextMenuClickEventArgs<SelectProductReferanceNumbersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    ProductReferanceNumbersDataSource = new SelectProductReferanceNumbersDto();
                    ProductReferanceNumbersCrudPopup = true;
                    ProductReferanceNumbersDataSource.ProductID = DataSource.Id;
                    ProductReferanceNumbersDataSource.ProductCode = DataSource.Code;
                    ProductReferanceNumbersDataSource.ProductName = DataSource.Name;

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    ProductReferanceNumbersDataSource = args.RowInfo.RowData;
                    ProductReferanceNumbersCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz ürün referans numarası, kalıcı olarak silinecektir.");

                    if (res == true)
                    {

                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();


                        await _ProductReferanceNumberGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _ProductReferanceNumberGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async Task OnProductReferanceNumberSubmit()
        {
            #region Submit İşlemleri

            SelectProductReferanceNumbersDto result;

            if (ProductReferanceNumbersDataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectProductReferanceNumbersDto, CreateProductReferanceNumbersDto>(ProductReferanceNumbersDataSource);

                result = (await ProductReferanceNumbersAppService.CreateAsync(createInput)).Data;

                if (result != null)
                    ProductReferanceNumbersDataSource.Id = result.Id;
            }

            else
            {
                var updateInput = ObjectMapper.Map<SelectProductReferanceNumbersDto, UpdateProductReferanceNumbersDto>(ProductReferanceNumbersDataSource);

                result = (await ProductReferanceNumbersAppService.UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {
                return;
            }

            await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto() { ProductId = DataSource.Id });

            var savedEntityIndex = ProductReferanceNumbersList.FindIndex(x => x.Id == ProductReferanceNumbersDataSource.Id);

            ProductReferanceNumbersList = (await ProductReferanceNumbersAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();

            foreach (var item in ProductReferanceNumbersList)
            {
                item.ProductCode = DataSource.Code;
                item.ProductName = DataSource.Name;
                item.CurrentAccountCardName = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Name;
                item.CurrentAccountCardCode = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Code;
            }

            await _ProductReferanceNumberGrid.Refresh();

            HideProductReferanceNumberCrudPopup();

            if (ProductReferanceNumbersDataSource.Id == Guid.Empty)
            {
                ProductReferanceNumbersDataSource.Id = result.Id;
            }

            #endregion

        }

        public void HideProductReferanceNumberCrudPopup()
        {
            ProductReferanceNumbersCrudPopup = false;
        }

        public void HideProductReferanceNumberPopup()
        {
            ProductReferanceNumbersPopup = false;
        }

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsNameButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCodeButtonClickEvent);
            await CurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task CurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsNameButtonClickEvent);
            await CurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsNameButtonClick } });
        }

        public async void CurrentAccountCardsNameButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ProductReferanceNumbersDataSource.CurrentAccountCardID = Guid.Empty;
                ProductReferanceNumbersDataSource.CurrentAccountCardCode = string.Empty;
                ProductReferanceNumbersDataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                ProductReferanceNumbersDataSource.CurrentAccountCardID = selectedUnitSet.Id;
                ProductReferanceNumbersDataSource.CurrentAccountCardCode = selectedUnitSet.Code;
                ProductReferanceNumbersDataSource.CurrentAccountCardName = selectedUnitSet.Name;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion


        #endregion

        #region Teknik Çizim Upload İşlemleri

        private void HandleFileSelectedTechnicalDrawing(IFileListEntry[] entryFiles)
        {

            foreach (var file in entryFiles)
            {
                files.Add(file);
            }
        }

        private void Remove(IFileListEntry file)
        {
            files.Remove(file);

            InvokeAsync(() => StateHasChanged());
        }

        private async void PreviewImage(IFileListEntry file)
        {
            string format = file.Type;

            if (format == "image/jpg" || format == "image/jpeg" || format == "image/png")
            {

                IFileListEntry imageFile = await file.ToImageFileAsync(format, 1214, 800);

                MemoryStream ms = new MemoryStream();

                await imageFile.Data.CopyToAsync(ms);

                imageDataUri = $"data:{format};base64,{Convert.ToBase64String(ms.ToArray())}";

                previewImagePopupTitle = file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == "application/pdf")
            {
                string rootPath = "tempFiles/";

                PDFrootPath = "wwwroot/" + rootPath + file.Name;

                PDFFileName = file.Name;

                List<string> _result = new List<string>();

                _result.Add(await FileUploadService.UploadTechnicalDrawingPDF(file, rootPath, PDFFileName));

                previewImagePopupTitle = file.Name;

                pdf = true;

                image = false;

                ImagePreviewPopup = true;

            }


            await InvokeAsync(() => StateHasChanged());

        }

        public void HidePreviewPopup()
        {
            ImagePreviewPopup = false;

            if (pdf)
            {
                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }
        }

        #endregion

        #region Ürün Grupları ButtonEdit

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

                case "technicaldrawings":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    TechnicalDrawingsList = (await TechnicalDrawingsAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();
                    TechnicalDrawingsPopup = true;

                    await InvokeAsync(StateHasChanged);


                    break;

                case "productreferancenumbers":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ProductReferanceNumbersList = (await ProductReferanceNumbersAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();

                    foreach (var item in ProductReferanceNumbersList)
                    {
                        item.ProductCode = DataSource.Code;
                        item.ProductName = DataSource.Name;
                        item.CurrentAccountCardName = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Name;
                        item.CurrentAccountCardCode = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Code;
                    }
                    ProductReferanceNumbersPopup = true;

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
            CreateMainContextMenuItems();
            CreateTechnicalDrawingContextMenuItems();
            CreateProductReferanceNumberContextMenuItems();
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

        #region GetList Metotları

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetProductGroupsList()
        {
            ProductGroupsList = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.ToList();
        }

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        #endregion



    }
}
