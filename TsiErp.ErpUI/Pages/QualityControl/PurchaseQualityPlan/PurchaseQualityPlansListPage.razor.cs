using BlazorInputFile;
using DevExpress.XtraCharts.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.PurchaseQualityPlan
{
    public partial class PurchaseQualityPlansListPage : IDisposable
    {
        private SfGrid<SelectPurchaseQualityPlanLinesDto> _LineGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseQualityPlanLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseQualityPlanLinesDto> GridLineList = new List<SelectPurchaseQualityPlanLinesDto>();

        List<IFileListEntry> files = new List<IFileListEntry>();

        //public bool TechnicalDrawingsChangedCrudPopup = false;

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        bool SaveOperationPictureLine = false;

        string CurrentRevisionNo = string.Empty;

        #region Değişkenler

        private bool LineCrudPopup = false;

        SfUploader uploader;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PurchaseQualityPlansAppService;
            _L = L;


            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseQualityPlansChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Satın Alma Giriş Kalite Planı Satır İşlemleri

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
                            case "PurchaseQualityPlanContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanContextAdd"], Id = "new" }); break;
                            case "PurchaseQualityPlanContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanContextChange"], Id = "changed" }); break;
                            case "PurchaseQualityPlanContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanContextDelete"], Id = "delete" }); break;
                            case "PurchaseQualityPlanContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanContextRefresh"], Id = "refresh" }); break;
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
                            case "PurchaseQualityPlanLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanLineContextAdd"], Id = "new" }); break;
                            case "PurchaseQualityPlanLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanLineContextChange"], Id = "changed" }); break;
                            case "PurchaseQualityPlanLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanLineContextDelete"], Id = "delete" }); break;
                            case "PurchaseQualityPlanLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseQualityPlanLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseQualityPlansDto()
            {
                DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("PurchaseQualityPlansChildMenu")
            };

            DataSource.SelectPurchaseQualityPlanLines = new List<SelectPurchaseQualityPlanLinesDto>();
            GridLineList = DataSource.SelectPurchaseQualityPlanLines;
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

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseQualityPlansDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    SaveOperationPictureLine = false;
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PurchaseQualityPlansAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseQualityPlanLines;

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
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await PurchaseQualityPlansAppService.DeleteAsync(args.RowInfo.RowData.Id);

                        if (Directory.Exists(DataSource.DrawingFilePath))
                        {
                            Directory.Delete(DataSource.DrawingFilePath, true);
                        }

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
                LineDataSource = new SelectPurchaseQualityPlanLinesDto
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseQualityPlanLinesDto> args)
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
                            DataSource.SelectPurchaseQualityPlanLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await PurchaseQualityPlansAppService.DeleteLineAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPurchaseQualityPlanLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPurchaseQualityPlanLines.Remove(line);
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
                if (DataSource.SelectPurchaseQualityPlanLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPurchaseQualityPlanLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseQualityPlanLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPurchaseQualityPlanLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPurchaseQualityPlanLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPurchaseQualityPlanLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPurchaseQualityPlanLines.OrderBy(t => t.MeasureNumberInPicture).ToList();

            await _LineGrid.Refresh();

            HideLinesPopup();

            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnSubmit()
        {
            #region Submit İşlemi

            SelectPurchaseQualityPlansDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectPurchaseQualityPlansDto, CreatePurchaseQualityPlansDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectPurchaseQualityPlansDto, UpdatePurchaseQualityPlansDto>(DataSource);

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

            await InvokeAsync(() => StateHasChanged());

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

        #region Kontrol Türü Button Edit

        SfTextBox ControlTypesCodeButtonEdit = new();
        SfTextBox ControlTypesNameButtonEdit = new();
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

        SfTextBox ControlConditionsCodeButtonEdit = new();
        SfTextBox ControlConditionsNameButtonEdit = new();
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

        #region Kalite Planı Teknik Çizim Upload İşlemleri

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

                if (await PurchaseQualityPlansAppService.RevisionNoControlAsync(DataSource.Id, DataSource.RevisionNo) > 0)
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
                        "wwwroot\\UploadedFiles\\QualityControl\\PurchaseQualityPlans\\" +
                        DataSource.ProductCode + "\\" +
                        DataSource.CurrrentAccountCardName.Replace(" ", "_").Replace("-", "_") + "\\" +
                        DataSource.RevisionNo + "\\";

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

        //public void HideTechnicalDrawingChangedCrudPopup()
        //{
        //    TechnicalDrawingsChangedCrudPopup = false;
        //    uploadedfiles.Clear();
        //    InvokeAsync(StateHasChanged);
        //}


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
            DataSource.DocumentNumber = FicheNumbersAppService.GetFicheNumberAsync("PurchaseQualityPlansChildMenu");
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
