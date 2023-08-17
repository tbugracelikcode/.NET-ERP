using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using BlazorInputFile;
using AutoMapper.Internal.Mappers;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.ErpUI.Helpers;

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

        List<IFileListEntry> files = new List<IFileListEntry>();

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
                PDFrootPath = rootpath + @"\UploadedFiles\OperationPictures\" + DataSource.ProductID + "-" + DataSource.ProductsOperationID  + @"\" + file.Name;

                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }

            else
            {
                imageDataUri = rootpath + @"\UploadedFiles\OperationPictures\" + DataSource.ProductID + "-" + DataSource.ProductsOperationID  + @"\" + file.Name;

                System.IO.FileInfo jpgfile = new System.IO.FileInfo(imageDataUri);
                if (jpgfile.Exists)
                {
                    jpgfile.Delete();
                }
            }
            uploadedfiles.Remove(file);

            await InvokeAsync(() => StateHasChanged());

            await ModalManager.MessagePopupAsync(L["UIInformationPopupTitleBase"], L["UIInformationPopupMessageBase"]);
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
                imageDataUri = @"\UploadedFiles\OperationPictures\" + DataSource.ProductID + "-" + DataSource.ProductsOperationID  + @"\" + file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = "wwwroot/UploadedFiles/OperationPictures/" + DataSource.ProductID + "-" + DataSource.ProductsOperationID + "/" + file.Name;

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
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationalQualityPlansDto()
            {
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
            if(DataSource.ProductID == Guid.Empty || DataSource.ProductsOperationID == Guid.Empty)
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

        public async void OnOperationPictureContextMenuClick(ContextMenuClickEventArgs<SelectOperationPicturesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    OperationPictureDataSource = new SelectOperationPicturesDto();
                    OperationPictureCrudPopup = true;
                    OperationPictureDataSource.LineNr = GridOperationPictureList.Count + 1;
                    OperationPictureDataSource.CreationDate_ = DateTime.Now;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    OperationPictureDataSource = args.RowInfo.RowData;
                    string rootpath = FileUploadService.GetRootPath();
                    string operationPicturePath = @"\UploadedFiles\OperationPictures\" + DataSource.ProductID.ToString() + "-" + DataSource.ProductsOperationID.ToString()  + @"\";
                    DirectoryInfo operationPicture = new DirectoryInfo(rootpath + operationPicturePath);
                    if (operationPicture.Exists)
                    {
                        System.IO.FileInfo[] exactFilesOperationPicture = operationPicture.GetFiles();

                        foreach (System.IO.FileInfo fileinfo in exactFilesOperationPicture)
                        {
                            uploadedfiles.Add(fileinfo);
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
            SelectProductsOperationsPopupVisible = true;
            await GetProductsOperationsList();
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
            ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();
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
            #region Submit İşlemleri

            SelectOperationalQualityPlansDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectOperationalQualityPlansDto, CreateOperationalQualityPlansDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectOperationalQualityPlansDto, UpdateOperationalQualityPlansDto>(DataSource);

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
            string operationid = DataSource.ProductsOperationID.ToString();

            List<string> _result = new List<string>();

            foreach (var file in files)
            {
                disable = true;

                string fileName = file.Name;
                string rootPath = "UploadedFiles/OperationPictures/" + productid + "-" + operationid ;


                _result.Add(await FileUploadService.UploadOperationPicture(file, rootPath, fileName));
                await InvokeAsync(() => StateHasChanged());

            }

            HideEditPage();

            disable = false;

            files.Clear();

            uploadedfiles.Clear();

            await InvokeAsync(() => StateHasChanged());

            #endregion
        }
    }
}
