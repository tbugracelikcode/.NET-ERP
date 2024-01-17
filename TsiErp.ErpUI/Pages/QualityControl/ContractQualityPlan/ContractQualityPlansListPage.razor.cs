using BlazorInputFile;
using BoldReports.RDL.DOM;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.ContractQualityPlan
{
    public partial class ContractQualityPlansListPage : IDisposable
    {
        private SfGrid<SelectContractQualityPlanLinesDto> _LineGrid;
        private SfGrid<SelectContractOperationPicturesDto> _OperationPicturesGrid;
        private SfGrid<SelectContractQualityPlanOperationsDto> _ContractOperationsGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectContractQualityPlanLinesDto LineDataSource;
        SelectContractOperationPicturesDto OperationPictureDataSource;
        SelectContractQualityPlanOperationsDto ContractOperationDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> OperationPictureGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ContractOperationGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectContractQualityPlanLinesDto> GridLineList = new List<SelectContractQualityPlanLinesDto>();

        List<SelectContractOperationPicturesDto> GridOperationPictureList = new List<SelectContractOperationPicturesDto>();

        List<SelectContractQualityPlanOperationsDto> GridContractOperationList = new List<SelectContractQualityPlanOperationsDto>();

        List<ListProductsOperationsDto> ProductsOperationList = new List<ListProductsOperationsDto>();

        #region File Upload Değişkenleri

        public List<IFileListEntry> lineFiles = new List<IFileListEntry>();

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

        private bool ContractOperationCrudPopup = false;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ContractQualityPlansAppService;
            _L = L;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateContractOperationContextMenuItems();
            //CreateOperationPictureContextMenuItems();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ContractQualityPlansChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion

        }

        #region File Upload İşlemleri


        private async void HandleFileSelectedOperationPicture(IFileListEntry[] entryFiles)
        {
            if (uploadedfiles != null && uploadedfiles.Count == 0)
            {
                foreach (var file in entryFiles)
                {
                    lineFiles.Add(entryFiles[0]);
                }
            }
            else
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Bu kayıtta yüklenmiş bir teknik resim dosyası mevcut");
            }
        }

        private void Remove(IFileListEntry file)
        {
            lineFiles.Remove(file);

            InvokeAsync(() => StateHasChanged());
        }

        private async void RemoveUploaded(System.IO.FileInfo file)
        {
            string extention = file.Extension;
            string rootpath = FileUploadService.GetRootPath();

            if (extention == ".pdf")
            {
                PDFrootPath = rootpath + @"\UploadedFiles\ContractOperationPictures\" + DataSource.ProductCode + @"\" + DataSource.CurrrentAccountCardCode + @"\"  + file.Name;

                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }

            else
            {
                imageDataUri = rootpath + @"\UploadedFiles\ContractOperationPictures\" + DataSource.ProductCode + @"\" + DataSource.CurrrentAccountCardCode + @"\" + file.Name;

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
                imageDataUri = @"\UploadedFiles\ContractOperationPictures\" + DataSource.ProductCode + @"\" + DataSource.CurrrentAccountCardCode + @"\" + file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = "wwwroot/UploadedFiles/ContractOperationPictures/" + DataSource.ProductCode + "/" + DataSource.CurrrentAccountCardCode + "/" + file.Name;

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

        #region Fason Giriş Kalite Planı Satır İşlemleri

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ContractQualityPlanContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanContextAdd"], Id = "new" }); break;
                            case "ContractQualityPlanContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanContextChange"], Id = "changed" }); break;
                            case "ContractQualityPlanContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanContextDelete"], Id = "delete" }); break;
                            case "ContractQualityPlanContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ContractQualityPlanLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanLineContextAdd"], Id = "new" }); break;
                            case "ContractQualityPlanLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanLineContextChange"], Id = "changed" }); break;
                            case "ContractQualityPlanLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanLineContextDelete"], Id = "delete" }); break;
                            case "ContractQualityPlanLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractQualityPlanLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractQualityPlansDto()
            {
                DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("ContractQualityPlansChildMenu")
            };

            DataSource.SelectContractQualityPlanLines = new List<SelectContractQualityPlanLinesDto>();
            DataSource.SelectContractOperationPictures = new List<SelectContractOperationPicturesDto>();
            DataSource.SelectContractQualityPlanOperations = new List<SelectContractQualityPlanOperationsDto>();
            GridLineList = DataSource.SelectContractQualityPlanLines;
            GridOperationPictureList = DataSource.SelectContractOperationPictures;
            GridContractOperationList = DataSource.SelectContractQualityPlanOperations;
            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListContractQualityPlansDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await ContractQualityPlansAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectContractQualityPlanLines;
                    GridOperationPictureList = DataSource.SelectContractOperationPictures;
                    GridContractOperationList = DataSource.SelectContractQualityPlanOperations;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await ContractQualityPlansAppService.DeleteAsync(args.RowInfo.RowData.Id);
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
            if (DataSource.ProductID == Guid.Empty || DataSource.ProductID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageBase"]);
            }
            else
            {
                LineDataSource = new SelectContractQualityPlanLinesDto
                {
                    ProductID = DataSource.ProductID.GetValueOrDefault(),
                    ProductCode = DataSource.ProductCode,
                    ProductName = DataSource.ProductName,
                    Date_ = DateTime.Now
                };

                LineCrudPopup = true;
                LineDataSource.LineNr = GridLineList.Count + 1;
            }


            await Task.CompletedTask;
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectContractQualityPlanLinesDto> args)
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
                            DataSource.SelectContractQualityPlanLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await ContractQualityPlansAppService.DeleteLineAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectContractQualityPlanLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectContractQualityPlanLines.Remove(line);
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
                if (DataSource.SelectContractQualityPlanLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectContractQualityPlanLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectContractQualityPlanLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectContractQualityPlanLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectContractQualityPlanLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectContractQualityPlanLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectContractQualityPlanLines.OrderBy(t => t.MeasureNumberInPicture).ToList();

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
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["OprPictureContextAdd"], Id = "new" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["OprPictureContextChange"], Id = "changed" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["OprPictureContextDelete"], Id = "delete" });
                OperationPictureGridContextMenu.Add(new ContextMenuItemModel { Text = L["OprPictureContextRefresh"], Id = "refresh" });
            }
        }

        public async Task BeforeOperationPictureInsertAsync()
        {
            if (DataSource.ProductID == Guid.Empty || DataSource.ProductID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageOprPictureBase"]);
            }
            else
            {
                OperationPictureDataSource = new SelectContractOperationPicturesDto
                {
                    CreationDate_ = DateTime.Now
                };

                OperationPictureCrudPopup = true;
                OperationPictureDataSource.LineNr = GridLineList.Count + 1;
            }


            await Task.CompletedTask;
        }

        public async void OnOperationPictureContextMenuClick(ContextMenuClickEventArgs<SelectContractOperationPicturesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    await BeforeOperationPictureInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    OperationPictureDataSource = args.RowInfo.RowData;
                    string rootpath = FileUploadService.GetRootPath();
                    string operationPicturePath = @"\UploadedFiles\ContractOperationPictures\" + DataSource.ProductCode + @"\" + DataSource.CurrrentAccountCardCode + @"\";
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
                            DataSource.SelectContractOperationPictures.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await ContractQualityPlansAppService.DeleteContractPictureAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectContractOperationPictures.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectContractOperationPictures.Remove(line);
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
                if (DataSource.SelectContractOperationPictures.Contains(OperationPictureDataSource))
                {
                    int selectedLineIndex = DataSource.SelectContractOperationPictures.FindIndex(t => t.LineNr == OperationPictureDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectContractOperationPictures[selectedLineIndex] = OperationPictureDataSource;
                    }
                }
                else
                {
                    DataSource.SelectContractOperationPictures.Add(OperationPictureDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectContractOperationPictures.FindIndex(t => t.Id == OperationPictureDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectContractOperationPictures[selectedLineIndex] = OperationPictureDataSource;
                }
            }

            GridOperationPictureList = DataSource.SelectContractOperationPictures;

            await _OperationPicturesGrid.Refresh();


            HideOperationPicturesPopup();

            disable = false;

            uploadedfiles.Clear();

            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnSubmit()
        {

            string productcode = DataSource.ProductCode;
            string currentCode = DataSource.CurrrentAccountCardCode;
            string rootPath = "UploadedFiles/ContractOperationPictures/" + productcode + "/" + currentCode;

            try
            {
                #region File Upload


                foreach (var file in lineFiles)
                {
                    string fileName = file.Name;


                    await FileUploadService.UploadTechnicalDrawing(file, rootPath, fileName);
                    await InvokeAsync(() => StateHasChanged());


                }

                lineFiles.Clear();

                #endregion

                #region Submit İşlemleri

                SelectContractQualityPlansDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectContractQualityPlansDto, CreateContractQualityPlansDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectContractQualityPlansDto, UpdateContractQualityPlansDto>(DataSource);

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

                #endregion

            }
            catch (Exception ex)
            {
                string webHostEnvironment = FileUploadService.GetRootPath();
                string deletedRootPath = webHostEnvironment + "/" + rootPath;
                System.IO.FileInfo pdfFile = new System.IO.FileInfo(rootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }

        }

        #endregion

        #region Fason Operasyonları İşlemleri

        protected void CreateContractOperationContextMenuItems()
        {
            if (ContractOperationGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ContractOprContextAdd":
                                ContractOperationGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractOprContextAdd"], Id = "new" }); break;
                            case "ContractOprContextDelete":
                                ContractOperationGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractOprContextDelete"], Id = "delete" }); break;
                            case "ContractOprContextRefresh":
                                ContractOperationGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractOprContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async Task BeforeContractOperationInsertAsync()
        {
            if (DataSource.ProductID == Guid.Empty || DataSource.ProductID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageContractOperationBase"]);
            }
            else
            {
                ContractOperationDataSource = new SelectContractQualityPlanOperationsDto
                {
                    ContractQualityPlanID = DataSource.Id
                };

                ProductsOperationList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID).ToList();

                ContractOperationCrudPopup = true;
                ContractOperationDataSource.LineNr = GridContractOperationList.Count + 1;
            }


            await Task.CompletedTask;
        }

        public async void OnContractOperationContextMenuClick(ContextMenuClickEventArgs<SelectContractQualityPlanOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    await BeforeContractOperationInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;


                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageContractOperationBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectContractQualityPlanOperations.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await ContractQualityPlansAppService.DeleteContractOperationAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectContractQualityPlanOperations.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectContractQualityPlanOperations.Remove(line);
                            }
                        }

                        await _ContractOperationsGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _ContractOperationsGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideContractOperationsPopup()
        {
            ContractOperationCrudPopup = false;
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

        #region Cari Hesap Button Edit

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsNameButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();
        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCodeButtonClickEvent);
            await CurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsButtonClick } });
        }

        public async void CurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task CurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsNameButtonClickEvent);
            await CurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsButtonClick } });
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
                DataSource.CurrrentAccountCardID = Guid.Empty;
                DataSource.CurrrentAccountCardCode = string.Empty;
                DataSource.CurrrentAccountCardName = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DataSource.CurrrentAccountCardID = selectedCurrentAccountCard.Id;
                DataSource.CurrrentAccountCardCode = selectedCurrentAccountCard.Code;
                DataSource.CurrrentAccountCardName = selectedCurrentAccountCard.Name;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Ürün Operasyon 

        public async void ProductsOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsOperationsDto> args)
        {
            var selectedProductsOperation = args.RowData;

            if (selectedProductsOperation != null)
            {

                ContractOperationDataSource.Name = selectedProductsOperation.Name;
                ContractOperationDataSource.Code = selectedProductsOperation.Code;
                ContractOperationDataSource.OperationID = selectedProductsOperation.Id;
                ContractOperationDataSource.ContractQualityPlanID = DataSource.Id;

                GridContractOperationList.Add(ContractOperationDataSource);
                await _ContractOperationsGrid.Refresh();
                ContractOperationCrudPopup = false;
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

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
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



        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("ContractQualityPlansChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
