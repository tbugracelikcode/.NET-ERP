using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Drawing;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.TechnicalDrawing
{
    public partial class TechnicalDrawingsListPage
    {
        List<string> Drawers = new List<string>();

        [Inject]
        ModalManager ModalManager { get; set; }

        List<IFileListEntry> files = new List<IFileListEntry>();

        private SfGrid<SelectTechnicalDrawingsDto> _TechnicalDrawingChangeGrid;

        public bool TechnicalDrawingsChangedCrudPopup = false;

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        bool disable;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        protected override async void OnInitialized()
        {
            BaseCrudService = TechnicalDrawingsService;
            ListDataSource = (await TechnicalDrawingsService.GetListAsync(new ListTechnicalDrawingsParameterDto())).Data.ToList();
        }

        protected override Task BeforeInsertAsync()
        {
            Drawers = ListDataSource.Select(t => t.Drawer).Distinct().ToList();

            DataSource = new SelectTechnicalDrawingsDto()
            {
                RevisionDate = DateTime.Today,
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public async override void OnContextMenuClick(ContextMenuClickEventArgs<ListTechnicalDrawingsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    string rootpath = FileUploadService.GetRootPath();
                    string technicalDrawingPath = @"\UploadedFiles\TechnicalDrawings\" + DataSource.ProductID + "-" + DataSource.ProductCode + @"\" + DataSource.Id + @"\";
                    DirectoryInfo technicalDrawing = new DirectoryInfo(rootpath + technicalDrawingPath);
                    if (technicalDrawing.Exists)
                    {
                        System.IO.FileInfo[] exactFilesTechnicalDrawing = technicalDrawing.GetFiles();

                        foreach (System.IO.FileInfo fileinfo in exactFilesTechnicalDrawing)
                        {
                            uploadedfiles.Add(fileinfo);
                        }

                    }
                    TechnicalDrawingsChangedCrudPopup = true;
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

        public override void HideEditPage()
        {
            base.EditPageVisible = false;
            files.Clear();
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

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

        #region Teknik Çizim Upload İşlemleri

        private async void HandleFileSelectedTechnicalDrawing(IFileListEntry[] entryFiles)
        {
            if (uploadedfiles != null && uploadedfiles.Count == 0)
            {
                foreach (var file in entryFiles)
                {
                    files.Add(file);
                }
            }
            else
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Bu kayıtta yüklenmiş bir teknik resim dosyası mevcut");
            }
        }

        private void Remove(IFileListEntry file)
        {
            files.Remove(file);

            InvokeAsync(() => StateHasChanged());
        }

        private async void RemoveUploaded(System.IO.FileInfo file)
        {
            string extention = file.Extension;
            string rootpath = FileUploadService.GetRootPath();

            if (extention == ".pdf")
            {
                PDFrootPath = rootpath + @"\UploadedFiles\TechnicalDrawings\" + DataSource.ProductID + "-" + DataSource.ProductCode + @"\" + DataSource.Id + @"\" + file.Name;

                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }

            else
            {
                imageDataUri = rootpath + @"\UploadedFiles\TechnicalDrawings\" + DataSource.ProductID + "-" + DataSource.ProductCode + @"\" + DataSource.Id + @"\" + file.Name;

                System.IO.FileInfo jpgfile = new System.IO.FileInfo(imageDataUri);
                if (jpgfile.Exists)
                {
                    jpgfile.Delete();
                }
            }
            uploadedfiles.Remove(file);

            await InvokeAsync(() => StateHasChanged());

            await ModalManager.MessagePopupAsync("Bilgilendirme", "Yüklenmiş teknik resim dosyası, başarıyla silinmiştir.");
        }

        private async void PreviewImage(IFileListEntry file)
        {
            string format = file.Type;

            if(format == "image/jpg" || format == "image/jpeg" || format == "image/png")
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

            else if(format == "application/pdf")
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

        private async void PreviewUploadedImage(System.IO.FileInfo file)
        {
            string format = file.Extension;

            UploadedFile = true;

            string rootpath = FileUploadService.GetRootPath();

            if (format == ".jpg" || format == ".jpeg" || format == ".png")
            {
                imageDataUri = @"\UploadedFiles\TechnicalDrawings\" + DataSource.ProductID + "-" + DataSource.ProductCode + @"\" + DataSource.Id + @"\" + file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = "wwwroot/UploadedFiles/TechnicalDrawings/" + DataSource.ProductID + "-" + DataSource.ProductCode + "/" + DataSource.Id + "/" + file.Name;

                PDFFileName = file.Name;

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

            if (!UploadedFile)
            {
                if (pdf)
                {
                    System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                    if (pdfFile.Exists)
                    {
                        pdfFile.Delete();
                    }
                }
            }
        }

        public void HideTechnicalDrawingChangedCrudPopup()
        {
            TechnicalDrawingsChangedCrudPopup = false;
            uploadedfiles.Clear();
            InvokeAsync(StateHasChanged);
        }


        #endregion

        protected override async Task OnSubmit()
        {
            #region Submit İşlemleri

            SelectTechnicalDrawingsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectTechnicalDrawingsDto, CreateTechnicalDrawingsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectTechnicalDrawingsDto, UpdateTechnicalDrawingsDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);

            #endregion

            #region File Upload İşlemleri

            string productid = DataSource.ProductID.ToString();
            string productcode = DataSource.ProductCode;
            string technicaldrawingid = DataSource.Id.ToString();

            List<string> _result = new List<string>();

            foreach (var file in files)
            {
                disable = true;

                string fileName = file.Name;
                string rootPath = "UploadedFiles/TechnicalDrawings/" + productid + "-" + productcode + "/" + technicaldrawingid;


                _result.Add(await FileUploadService.UploadTechnicalDrawing(file, rootPath, fileName));
                await InvokeAsync(() => StateHasChanged());

            }

            HideEditPage();
            HideTechnicalDrawingChangedCrudPopup();

            disable = false;

            files.Clear();

            await InvokeAsync(() => StateHasChanged());

            #endregion
        }
    }
}
