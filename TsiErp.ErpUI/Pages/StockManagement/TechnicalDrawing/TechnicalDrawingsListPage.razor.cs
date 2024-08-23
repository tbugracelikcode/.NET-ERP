using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.TechnicalDrawing
{
    public partial class TechnicalDrawingsListPage : IDisposable
    {
        List<string> Drawers = new List<string>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        List<IFileListEntry> files = new List<IFileListEntry>();


        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        //bool disable = new();


        bool CustomerCodeEnable = false;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        SfUploader uploader;

        bool SaveOperationPictureLine = false;

        string CurrentRevisionNo = string.Empty;

        protected override async void OnInitialized()
        {
            BaseCrudService = TechnicalDrawingsService;
            _L = L;
            ListDataSource = (await TechnicalDrawingsService.GetListAsync(new ListTechnicalDrawingsParameterDto())).Data.ToList();
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "TechnicalDrawingsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            Drawers = ListDataSource.Select(t => t.Drawer).Distinct().ToList();

            DataSource = new SelectTechnicalDrawingsDto()
            {
                RevisionDate = GetSQLDateAppService.GetDateFromSQL(),

            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }


        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "TechnicalDrawingContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextAdd"], Id = "new" }); break;
                            case "TechnicalDrawingContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextChange"], Id = "changed" }); break;
                            case "TechnicalDrawingContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextDelete"], Id = "delete" }); break;
                            case "TechnicalDrawingContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async override void OnContextMenuClick(ContextMenuClickEventArgs<ListTechnicalDrawingsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    SaveOperationPictureLine = false;
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (!string.IsNullOrEmpty(DataSource.DrawingFilePath))
                    {
                        uploadedfiles.Clear();

                        DirectoryInfo operationPicture = new DirectoryInfo(DataSource.DrawingFilePath);

                        if (operationPicture.Exists)
                        {
                            System.IO.FileInfo[] exactFilesOperationPicture = operationPicture.GetFiles();

                            if (exactFilesOperationPicture.Length > 0)
                            {
                                foreach (System.IO.FileInfo fileinfo in exactFilesOperationPicture)
                                {
                                    uploadedfiles.Add(fileinfo);
                                }
                            }
                        }
                    }
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

        public void HideEditPage()
        {
            if (!SaveOperationPictureLine)
            {
                if (DataSource.Id == Guid.Empty)
                {
                    if (Directory.Exists(DataSource.DrawingFilePath))
                    {
                        Directory.Delete(DataSource.DrawingFilePath, true);
                    }
                }
            }

            EditPageVisible = false;

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
                CustomerCodeEnable = false;
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
                DataSource.ProductType = selectedProduct.ProductType;
                SelectProductsPopupVisible = false;

                if (DataSource.ProductType == Entities.Enums.ProductTypeEnum.MM)
                {
                    CustomerCodeEnable = true;
                }
                else
                {
                    CustomerCodeEnable = false;
                }

                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Cari Hesap Button Edit

        SfTextBox CurrentAccountCardsCodeButtonEdit = new();
        SfTextBox CurrentAccountCardsNameButtonEdit = new();
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
                DataSource.CustomerCurrentAccountCardID = Guid.Empty;
                DataSource.CustomerCode = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DataSource.CustomerCurrentAccountCardID = selectedCurrentAccountCard.Id;
                DataSource.CustomerCode = selectedCurrentAccountCard.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => !string.IsNullOrEmpty(t.CustomerCode)).ToList();
        }

        #endregion

        #region Teknik Çizim Upload İşlemleri

        public async void OnUploadedFileChange(UploadChangeEventArgs args)
        {
            try
            {
                CurrentRevisionNo = DataSource.RevisionNo;

                if (string.IsNullOrEmpty(DataSource.RevisionNo))
                {
                    await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageEmptyRevisionNr"]);
                    await this.uploader.ClearAllAsync();
                    return;
                }

                if ((await TechnicalDrawingsService.GetSelectListAsync(DataSource.ProductID.GetValueOrDefault())).Data.Where(t => t.RevisionNo == DataSource.RevisionNo).Count() > 0)
                {
                    if (DataSource.Id == Guid.Empty)
                    {
                        await ModalManager.WarningPopupAsync(L["UIConfirmationPopupTitleBase"], L["UIWarningPopupMessageRevisionNoError"]);
                        await this.uploader.ClearAllAsync();
                        return;
                    }
                }

                foreach (var file in args.Files)
                {
                    string rootPath =
                        "wwwroot\\UploadedFiles\\TechnicalDrawings\\" +
                        DataSource.ProductCode + "\\" +
                        DataSource.RevisionNo.Replace(" ", "_").Replace("-", "_") + "\\";

                    string fileName = file.FileInfo.Name.Replace(" ", "_").Replace("-", "_");

                    if (!Directory.Exists(rootPath))
                    {
                        Directory.CreateDirectory(rootPath);
                    }

                    DataSource.DrawingDomain = Navigation.BaseUri;
                    DataSource.UploadedFileName = fileName;
                    DataSource.DrawingFilePath = rootPath;

                    FileStream filestream = new FileStream(rootPath + fileName, FileMode.Create, FileAccess.Write);
                    file.Stream.WriteTo(filestream);
                    filestream.Close();
                    file.Stream.Close();
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                await ModalManager.MessagePopupAsync("Bilgi", ex.Message);
                await this.uploader.ClearAllAsync();
            }

        }

        public void OnUploadedFileRemove(RemovingEventArgs args)
        {
            if (File.Exists(DataSource.DrawingFilePath + DataSource.UploadedFileName))
            {
                File.Delete(DataSource.DrawingFilePath + DataSource.UploadedFileName);
            }

            DataSource.DrawingDomain = string.Empty;
            DataSource.UploadedFileName = string.Empty;
            DataSource.DrawingFilePath = string.Empty;
        }

        private async void PreviewUploadedImage(System.IO.FileInfo file)
        {
            string format = file.Extension;

            UploadedFile = true;

            if (format == ".jpg" || format == ".jpeg" || format == ".png")
            {
                imageDataUri = file.FullName;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = file.FullName;

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

        private async void RemoveUploaded(System.IO.FileInfo file)
        {
            if (file.Extension == ".pdf")
            {
                PDFrootPath = file.FullName;

                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }
            else
            {
                imageDataUri = file.FullName;

                System.IO.FileInfo jpgfile = new System.IO.FileInfo(imageDataUri);
                if (jpgfile.Exists)
                {
                    jpgfile.Delete();
                }
            }

            DataSource.DrawingDomain = string.Empty;
            DataSource.UploadedFileName = string.Empty;
            DataSource.DrawingFilePath = string.Empty;

            uploadedfiles.Remove(file);

            await InvokeAsync(() => StateHasChanged());

        }


        #endregion

        protected override async Task OnSubmit()
        {
            #region Submit İşlemleri

            if (DataSource.ProductID == Guid.Empty || DataSource.ProductID == null)
            {
                await ModalManager.MessagePopupAsync(L["Error"], L["ValidatorProductID"]);
                return;
            }

            if (string.IsNullOrEmpty(DataSource.RevisionNo))
            {
                await ModalManager.MessagePopupAsync(L["Error"], L["ValidatorCodeEmpty"]);
                return;
            }

            if (DataSource.RevisionNo.Length > 50)
            {
                await ModalManager.MessagePopupAsync(L["Error"], L["ValidatorCodeMaxLength"]);
                return;
            }

            if (DataSource.ProductType == Entities.Enums.ProductTypeEnum.MM)
            {
                if (DataSource.CustomerCurrentAccountCardID == Guid.Empty || DataSource.CustomerCurrentAccountCardID == null)
                {
                    await ModalManager.MessagePopupAsync(L["Error"], L["ValidatorCurrentCardID"]);
                    return;
                }
            }


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

            HideEditPage();
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
