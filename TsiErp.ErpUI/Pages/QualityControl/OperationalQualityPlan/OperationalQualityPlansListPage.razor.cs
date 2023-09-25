using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Management.Smo;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.IO;
using System.Text;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationalQualityPlan
{
    public partial class OperationalQualityPlansListPage
    {
        private SfGrid<SelectOperationalQualityPlanLinesDto> _LineGrid;
        private SfGrid<SelectOperationPicturesDto> _OperationPicturesGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectOperationalQualityPlanLinesDto LineDataSource;
        SelectOperationPicturesDto OperationPictureDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> OperationPictureGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectOperationalQualityPlanLinesDto> GridLineList = new List<SelectOperationalQualityPlanLinesDto>();

        List<SelectOperationPicturesDto> GridOperationPictureList = new List<SelectOperationPicturesDto>();

        #region File Upload Değişkenleri

        //List<IFileListEntry> files = new List<IFileListEntry>();

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        public bool OperationPicturesChangedCrudPopup = false;

        #endregion

        #region Değişkenler

        private bool LineCrudPopup = false;

        private bool OperationPictureCrudPopup = false;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = OperationalQualityPlansAppService;
            _L = L;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateOperationPictureContextMenuItems();

        }

        #region File Upload İşlemleri


        private async void HandleFileSelectedOperationPicture(IFileListEntry[] entryFiles)
        {
            if (uploadedfiles != null && uploadedfiles.Count == 0)
            {
                OperationPictureDataSource.UploadedFiles = entryFiles[0];
                OperationPictureDataSource.DrawingDomain = Navigation.BaseUri;
                OperationPictureDataSource.DrawingFilePath = "UploadedFiles\\ProductOperationPictures\\" + DataSource.ProductCode + "\\" + DataSource.OperationName + "\\" + "Line-" + OperationPictureDataSource.LineNr + "\\";

                byte[] data;
                using (MemoryStream stream = new MemoryStream())
                {
                    await entryFiles[0].Data.CopyToAsync(stream);
                    data = stream.ToArray();
                }

                OperationPictureDataSource.FileByteArray = data.ToArray();
                OperationPictureDataSource.UploadedFileName = entryFiles[0].Name;
            }
            else
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Bu kayıtta yüklenmiş bir teknik resim dosyası mevcut");
            }
        }

        private void Remove(SelectOperationPicturesDto file)
        {
            OperationPictureDataSource = new SelectOperationPicturesDto();
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

                _result.Add(await FileUploadService.UploadOperationPicturePDF(file, rootPath, PDFFileName));

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
                imageDataUri = "\\UploadedFiles\\ProductOperationPictures\\" + DataSource.ProductCode + "\\" + DataSource.OperationName + "\\" + "Line-" + OperationPictureDataSource.LineNr + "\\" + file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = "wwwroot/UploadedFiles/ProductOperationPictures/" + DataSource.ProductCode + "/" + DataSource.OperationName + "/" + "Line-" + OperationPictureDataSource.LineNr + "/" + file.Name;

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


        #endregion

        #region Operasyon Kalite Planı Satır İşlemleri

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanLineContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanLineContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationalQualityPlanLineContextRefresh"], Id = "refresh" });
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationalQualityPlansDto()
            {
                DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("OperationalQualityPlansChildMenu")
            };

            DataSource.SelectOperationalQualityPlanLines = new List<SelectOperationalQualityPlanLinesDto>();
            DataSource.SelectOperationPictures = new List<SelectOperationPicturesDto>();
            GridLineList = DataSource.SelectOperationalQualityPlanLines;
            GridOperationPictureList = DataSource.SelectOperationPictures;

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListOperationalQualityPlansDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await OperationalQualityPlansAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectOperationalQualityPlanLines;
                    GridOperationPictureList = DataSource.SelectOperationPictures;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await OperationalQualityPlansAppService.DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async Task BeforeLineInsertAsync()
        {
            if (DataSource.ProductID == Guid.Empty || DataSource.ProductsOperationID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageBase"]);
            }
            else
            {
                LineDataSource = new SelectOperationalQualityPlanLinesDto
                {
                    ProductID = DataSource.ProductID,
                    ProductCode = DataSource.ProductCode,
                    ProductName = DataSource.ProductName,
                    ProductsOperationID = DataSource.ProductsOperationID,
                    OperationCode = DataSource.OperationCode,
                    OperationName = DataSource.OperationName,
                    Date_ = DateTime.Now
                };

                LineCrudPopup = true;
                LineDataSource.LineNr = GridLineList.Count + 1;
            }


            await Task.CompletedTask;
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectOperationalQualityPlanLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    await BeforeLineInsertAsync();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectOperationalQualityPlanLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await OperationalQualityPlansAppService.DeleteLineAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectOperationalQualityPlanLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectOperationalQualityPlanLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectOperationalQualityPlanLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectOperationalQualityPlanLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectOperationalQualityPlanLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectOperationalQualityPlanLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectOperationalQualityPlanLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectOperationalQualityPlanLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectOperationalQualityPlanLines.OrderBy(t => t.MeasureNumberInPicture).ToList();

            await _LineGrid.Refresh();

            HideLinesPopup();

            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Operasyon Resimleri İşlemleri

        protected void CreateOperationPictureContextMenuItems()
        {
            if (OperationPictureGridContextMenu.Count() == 0)
            {
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        public async Task BeforeOperationPictureInsertAsync()
        {
            if (DataSource.ProductID == Guid.Empty || DataSource.ProductsOperationID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageOprPictureBase"]);
            }
            else
            {
                OperationPictureDataSource = new SelectOperationPicturesDto
                {
                    CreationDate_ = DateTime.Now
                };

                OperationPictureCrudPopup = true;
                OperationPictureDataSource.LineNr = GridOperationPictureList.Count + 1;
            }


            await Task.CompletedTask;
        }

        public async void OnOperationPictureContextMenuClick(ContextMenuClickEventArgs<SelectOperationPicturesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    await BeforeOperationPictureInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":

                    uploadedfiles.Clear();

                    OperationPictureDataSource = args.RowInfo.RowData;

                    if (!OperationPictureDataSource.IsDeleted)
                    {
                        string rootpath = FileUploadService.GetRootPath();
                        string operationPicturePath = "\\UploadedFiles\\ProductOperationPictures\\" + DataSource.ProductCode + "\\" + DataSource.OperationName + "\\" + "Line-" + OperationPictureDataSource.LineNr + "\\";
                        DirectoryInfo operationPicture = new DirectoryInfo(rootpath + operationPicturePath);
                        if (operationPicture.Exists)
                        {
                            System.IO.FileInfo[] exactFilesOperationPicture = operationPicture.GetFiles();

                            foreach (System.IO.FileInfo fileinfo in exactFilesOperationPicture)
                            {
                                uploadedfiles.Add(fileinfo);
                            }

                        }
                    }

                    OperationPictureCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageOprPictureBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectOperationPictures.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await OperationalQualityPlansAppService.DeleteOperationPictureAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectOperationPictures.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectOperationPictures.Remove(line);
                            }
                        }

                        await _OperationPicturesGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _OperationPicturesGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideOperationPicturesPopup()
        {
            uploadedfiles.Clear();
            OperationPictureCrudPopup = false;
        }

        protected async Task OnOperationPictureSubmit()
        {
            if (OperationPictureDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectOperationPictures.Contains(OperationPictureDataSource))
                {
                    int selectedLineIndex = DataSource.SelectOperationPictures.FindIndex(t => t.LineNr == OperationPictureDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectOperationPictures[selectedLineIndex] = OperationPictureDataSource;
                    }
                }
                else
                {
                    DataSource.SelectOperationPictures.Add(OperationPictureDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectOperationPictures.FindIndex(t => t.Id == OperationPictureDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectOperationPictures[selectedLineIndex] = OperationPictureDataSource;
                }
            }

            GridOperationPictureList = DataSource.SelectOperationPictures;

            //files.Clear();

            await _OperationPicturesGrid.Refresh();

            HideOperationPicturesPopup();

            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Button Edit Metotları

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

        #region Ürün Operasyon Button Edit

        SfTextBox ProductsOperationsButtonEdit;
        bool SelectProductsOperationsPopupVisible = false;
        List<ListProductsOperationsDto> ProductsOperationsList = new List<ListProductsOperationsDto>();
        public async Task ProductsOperationsOnCreateIcon()
        {
            var ProductsOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsOperationsButtonClickEvent);
            await ProductsOperationsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsOperationsButtonClick } });
        }

        public async void ProductsOperationsButtonClickEvent()
        {
            if (DataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningProductIdTitleBase"], L["UIWarningProductIdMessageBase"]);
            }
            else
            {
                SelectProductsOperationsPopupVisible = true;
                await GetProductsOperationsList();
            }

            await InvokeAsync(StateHasChanged);
        }


        public void ProductsOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductsOperationID = Guid.Empty;
                DataSource.OperationCode = string.Empty;
                DataSource.OperationName = string.Empty;
            }
        }

        public async void ProductsOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsOperationsDto> args)
        {
            var selectedProductsOperation = args.RowData;

            if (selectedProductsOperation != null)
            {
                DataSource.ProductsOperationID = selectedProductsOperation.Id;
                DataSource.OperationCode = selectedProductsOperation.Code;
                DataSource.OperationName = selectedProductsOperation.Name;
                SelectProductsOperationsPopupVisible = false;
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
                LineDataSource.ControlTypesID = Guid.Empty;
                LineDataSource.ControlTypesName = string.Empty;
            }
        }

        public async void ControlTypesDoubleClickHandler(RecordDoubleClickEventArgs<ListControlTypesDto> args)
        {
            var selectedControlType = args.RowData;

            if (selectedControlType != null)
            {
                LineDataSource.ControlTypesID = selectedControlType.Id;
                LineDataSource.ControlTypesName = selectedControlType.Name;
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
                LineDataSource.ControlConditionsID = Guid.Empty;
                LineDataSource.ControlConditionsName = string.Empty;
            }
        }

        public async void ControlConditionsDoubleClickHandler(RecordDoubleClickEventArgs<ListControlConditionsDto> args)
        {
            var selectedControlCondition = args.RowData;

            if (selectedControlCondition != null)
            {
                LineDataSource.ControlConditionsID = selectedControlCondition.Id;
                LineDataSource.ControlConditionsName = selectedControlCondition.Name;
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
                LineDataSource.WorkCenterID = Guid.Empty;
                LineDataSource.WorkCenterName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                LineDataSource.WorkCenterID = selectedStationGroup.Id;
                LineDataSource.WorkCenterName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetProductsOperationsList()
        {
            ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID).ToList();
        }

        private async Task GetControlTypesList()
        {
            ControlTypesList = (await IControlTypesAppService.GetListAsync(new ListControlTypesParameterDto())).Data.ToList();
        }

        private async Task GetControlConditionsList()
        {
            ControlConditionsList = (await ControlConditionsAppService.GetListAsync(new ListControlConditionsParameterDto())).Data.ToList();
        }

        #endregion

        protected override async Task OnSubmit()
        {
            SelectOperationalQualityPlansDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectOperationalQualityPlansDto, CreateOperationalQualityPlansDto>(DataSource);

                result = (await OperationalQualityPlansAppService.CreateAsync(createInput)).Data;

                if (result != null)
                {
                    DataSource.Id = result.Id;

                    foreach (var item in DataSource.SelectOperationPictures)
                    {
                        string rootPath = Path.Combine(FileUploadService.GetRootPath(), item.DrawingFilePath);

                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }

                        var path = Path.Combine(rootPath, item.UploadedFileName);

                        File.WriteAllBytes(path, item.FileByteArray);

                        var fileInfo = new System.IO.FileInfo(path);

                        if (fileInfo.Length == item.UploadedFiles.Size)
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectOperationalQualityPlansDto, UpdateOperationalQualityPlansDto>(DataSource);

                result = (await OperationalQualityPlansAppService.UpdateAsync(updateInput)).Data;

                foreach (var item in DataSource.SelectOperationPictures)
                {
                    if (item.Id == Guid.Empty)
                    {
                        string rootPath = Path.Combine(FileUploadService.GetRootPath(), item.DrawingFilePath);

                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }

                        var path = Path.Combine(rootPath, item.UploadedFileName);

                        File.WriteAllBytes(path, item.FileByteArray);

                        var fileInfo = new System.IO.FileInfo(path);

                        if (fileInfo.Length == item.UploadedFiles.Size)
                        {
                            continue;
                        }
                    }
                }
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



        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("OperationalQualityPlansChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
