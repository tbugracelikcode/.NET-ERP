using DevExpress.Office.History;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Timers;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.PurchaseOrdersAwaitingApproval
{
    public partial class PurchaseOrdersAwaitingApprovalsListPage : IDisposable
    {
        private SfGrid<SelectPurchaseOrdersAwaitingApprovalLinesDto> _LineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }
        [Inject]
        SpinnerService SpinnerService { get; set; }

        SelectPurchaseOrdersAwaitingApprovalLinesDto LineDataSource;

        SelectPurchaseUnsuitabilityReportsDto PurchaseUnsuitabilityDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseOrdersAwaitingApprovalLinesDto> GridLineList = new List<SelectPurchaseOrdersAwaitingApprovalLinesDto>();

        public bool LineCrudPopup = false;

        public bool PurchaseUnsuitabilityCrudPopup = false;

        public bool PreviewPopup = false;

        string UserName = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PurchaseOrdersAwaitingApprovalsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseOrdersAwaitingApprovalsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            StartTimer();

        }

        #region Operasyon Kalite Planı Satır İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseOrdersAwaitingApprovalsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersAwaitingApprovalsChildMenu")
            };


            EditPageVisible = true;


            await Task.CompletedTask;
        }

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
                            case "PurchaseOrdersAwaitingApprovalsContextQualityApproval":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalsContextQualityApproval"], Id = "qualityapproval" }); break;
                            case "PurchaseOrdersAwaitingApprovalsContextQualityApprovalCancel":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalsContextQualityApprovalCancel"], Id = "qualityapprovalcancel" }); break;
                            case "PurchaseOrdersAwaitingApprovalsContextReview":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalsContextReview"], Id = "review" }); break;
                            case "PurchaseOrdersAwaitingApprovalsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalsContextRefresh"], Id = "refresh" }); break;
                            case "PurchaseOrdersAwaitingApprovalsContextPurchaseUnsuitability":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalsContextPurchaseUnsuitability"], Id = "purchaseunsuitability" }); break;
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
                            case "PurchaseOrdersAwaitingApprovalLinesContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalLinesContextChange"], Id = "changed" }); break;
                            case "PurchaseOrdersAwaitingApprovalLinesContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
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
                    GridLineList.Clear();

                    var qualityPlan = (await PurchaseQualityPlansAppService.GetbyCurrentAccountandProductAsync(DataSource.CurrentAccountCardID.GetValueOrDefault(), DataSource.ProductID.GetValueOrDefault())).Data;

                    if (qualityPlan != null && qualityPlan.Id != Guid.Empty)
                    {
                        var qualityPlanLines = qualityPlan.SelectPurchaseQualityPlanLines;

                        if (qualityPlanLines != null && qualityPlanLines.Count > 0)
                        {
                            foreach (var line in qualityPlanLines)
                            {
                                SelectPurchaseOrdersAwaitingApprovalLinesDto approvalLineModel = new SelectPurchaseOrdersAwaitingApprovalLinesDto
                                {
                                    BottomTolerance = line.BottomTolerance,
                                    ControlFrequency = line.ControlFrequency,
                                    ControlTypesID = line.ControlTypesID,
                                    ControlTypesName = line.ControlTypesName,
                                    IdealMeasure = line.IdealMeasure,
                                    UpperTolerance = line.UpperTolerance,
                                    PurchaseOrdersAwaitingApprovalID = DataSource.Id,
                                    MeasureValue = 0,
                                    LineNr = GridLineList.Count + 1,
                                };

                                GridLineList.Add(approvalLineModel);
                            }

                            if (GridLineList != null && GridLineList.Count > 0)
                            {
                                DataSource.SelectPurchaseOrdersAwaitingApprovalLines = GridLineList;
                            }

                        }
                    }

                    UserName = (await UsersAppService.GetAsync(DataSource.ApproverID.GetValueOrDefault())).Data.UserName;

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseOrdersAwaitingApprovalsDto> args)
        {
            switch (args.Item.Id)
            {
                case "qualityapproval":
                    SpinnerService.Show();
                    await Task.Delay(100);
                    IsChanged = true;

                    foreach (var item in states)
                    {
                        item.PurchaseOrdersAwaitingApprovalStateEnumName = L[item.PurchaseOrdersAwaitingApprovalStateEnumName];
                    }

                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    SpinnerService.Show();
                    await Task.Delay(100);
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "qualityapprovalcancel":

                    SpinnerService.Show();
                    await Task.Delay(100);
                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if(DataSource.PurchaseOrdersAwaitingApprovalStateEnum == PurchaseOrdersAwaitingApprovalStateEnum.SartliOnaylandi || DataSource.PurchaseOrdersAwaitingApprovalStateEnum == PurchaseOrdersAwaitingApprovalStateEnum.KaliteKontrolOnayVerildi)
                    {
                        CancelQualityApproval();
                    }
                    else
                    {
                        SpinnerService.Hide();
                        await ModalManager.WarningPopupAsync(L["UIWarningStateTitle"], L["UIWarningStateMessage"]);
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "review":

                    GridLineList.Clear();

                    foreach (var item in states)
                    {
                        item.PurchaseOrdersAwaitingApprovalStateEnumName = L[item.PurchaseOrdersAwaitingApprovalStateEnumName];
                    }

                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrdersAwaitingApprovalLines;
                    UserName = (await UsersAppService.GetAsync(DataSource.ApproverID.GetValueOrDefault())).Data.UserName;
                    PreviewPopup = true;

                    await InvokeAsync(StateHasChanged);
                    break;

                case "purchaseunsuitability":

                    SpinnerService.Show();
                    await Task.Delay(100);
                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.PurchaseOrdersAwaitingApprovalStateEnum == PurchaseOrdersAwaitingApprovalStateEnum.Red)
                    {
                        foreach (var item in _unsComboBox)
                        {
                            item.Text = L[item.Text];
                        }

                        PurchaseUnsuitabilityDataSource = new SelectPurchaseUnsuitabilityReportsDto
                        {
                            Action_ = string.Empty,
                            CurrentAccountCardCode = DataSource.CurrentAccountCardCode,
                            CurrentAccountCardID = DataSource.CurrentAccountCardID,
                            CurrentAccountCardName = DataSource.CurrentAccountCardName,
                            Date_ = DataSource.QualityApprovalDate,
                            FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchUnsRecordsChildMenu"),
                            Description_ = string.Empty,
                            IsUnsuitabilityWorkOrder = false,
                            UnsuitabilityItemsName = string.Empty,
                            UnsuitabilityItemsID = Guid.Empty,
                            ProductID = DataSource.ProductID,
                            ProductName = DataSource.ProductName,
                            ProductCode = DataSource.ProductCode,
                            PartyNo = string.Empty,
                            OrderID = DataSource.PurchaseOrderID,
                            OrderFicheNo = DataSource.PurchaseOrderFicheNo,
                            UnsuitableAmount = 0

                        };

                        PurchaseUnsuitabilityCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        SpinnerService.Hide();
                        await ModalManager.WarningPopupAsync(L["UIWarningPurchUnsTitle"], L["UIWarningPurchUnsMessage"]);
                    }


                    await InvokeAsync(StateHasChanged);
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
        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseOrdersAwaitingApprovalLinesDto> args)
        {
            switch (args.Item.Id)
            {

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
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

        public void HidePreviewPopPopup()
        {
            PreviewPopup = false;
        }

        public async void CancelQualityApproval()
        {
            #region Depo Giriş Kaydı Durumu Güncelleme

            var productReceiptTransaction = (await ProductReceiptTransactionsAppService.GetAsync(DataSource.ProductReceiptTransactionID.GetValueOrDefault())).Data;

            if (productReceiptTransaction != null && productReceiptTransaction.Id != Guid.Empty)
            {
                productReceiptTransaction.ProductReceiptTransactionStateEnum = ProductReceiptTransactionStateEnum.KaliteKontrolOnayBekliyor;

                var updatedReceiptTransaction = ObjectMapper.Map<SelectProductReceiptTransactionsDto, UpdateProductReceiptTransactionsDto>(productReceiptTransaction);

                await ProductReceiptTransactionsAppService.UpdateAsync(updatedReceiptTransaction);
            }

            #endregion

            #region Satın Alma Sipariş Satırı Durumu Güncelleme

            var purchaseOrder = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data;

            if (purchaseOrder != null && purchaseOrder.Id != Guid.Empty)
            {
                var purchaseOrderLineList = purchaseOrder.SelectPurchaseOrderLinesDto.ToList();

                if (purchaseOrderLineList != null && purchaseOrderLineList.Count > 0)
                {
                    var updatedLine = purchaseOrderLineList.Where(t => t.Id == DataSource.PurchaseOrderLineID.GetValueOrDefault()).FirstOrDefault();

                    int updatedIndex = purchaseOrderLineList.IndexOf(updatedLine);

                    purchaseOrder.SelectPurchaseOrderLinesDto[updatedIndex].PurchaseOrderLineStateEnum = PurchaseOrderLineStateEnum.Beklemede;

                    var updatedPurchaseOrder = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(purchaseOrder);

                    await PurchaseOrdersAppService.UpdateAsync(updatedPurchaseOrder);
                }
            }

            #endregion

            #region Satırları Silme

            foreach (var line in DataSource.SelectPurchaseOrdersAwaitingApprovalLines)
            {
                await PurchaseOrdersAwaitingApprovalsAppService.DeleteAsync(line.Id);
            }

            #endregion

            #region Ana Kayıt Durum Güncellemesi

            DataSource.PurchaseOrdersAwaitingApprovalStateEnum = PurchaseOrdersAwaitingApprovalStateEnum.KaliteKontrolOnayBekliyor;

            var updatedInput = ObjectMapper.Map<SelectPurchaseOrdersAwaitingApprovalsDto, UpdatePurchaseOrdersAwaitingApprovalsDto>(DataSource);

            await PurchaseOrdersAwaitingApprovalsAppService.UpdateCancelQCApprovalAsync(updatedInput);

            #endregion
        }

        public async void GrantedQualityApproval()
        {
            #region Depo Giriş Kaydı Durumu Güncelleme

            var productReceiptTransaction = (await ProductReceiptTransactionsAppService.GetAsync(DataSource.ProductReceiptTransactionID.GetValueOrDefault())).Data;

            if (productReceiptTransaction != null && productReceiptTransaction.Id != Guid.Empty)
            {
                productReceiptTransaction.ProductReceiptTransactionStateEnum = ProductReceiptTransactionStateEnum.KaliteKontrolOnayVerildi;

                var updatedReceiptTransaction = ObjectMapper.Map<SelectProductReceiptTransactionsDto, UpdateProductReceiptTransactionsDto>(productReceiptTransaction);

                await ProductReceiptTransactionsAppService.UpdateAsync(updatedReceiptTransaction);
            }

            #endregion

            #region Satın Alma Sipariş Satırı Durumu Güncelleme

            var purchaseOrder = (await PurchaseOrdersAppService.GetAsync(DataSource.PurchaseOrderID.GetValueOrDefault())).Data;

            if (purchaseOrder != null && purchaseOrder.Id != Guid.Empty)
            {
                var purchaseOrderLineList = purchaseOrder.SelectPurchaseOrderLinesDto.ToList();

                if (purchaseOrderLineList != null && purchaseOrderLineList.Count > 0)
                {
                    var updatedLine = purchaseOrderLineList.Where(t => t.Id == DataSource.PurchaseOrderLineID.GetValueOrDefault()).FirstOrDefault();

                    int updatedIndex = purchaseOrderLineList.IndexOf(updatedLine);

                    purchaseOrder.SelectPurchaseOrderLinesDto[updatedIndex].PurchaseOrderLineStateEnum = PurchaseOrderLineStateEnum.KaliteKontrolOnayiVerildi;

                    var updatedPurchaseOrder = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(purchaseOrder);

                    await PurchaseOrdersAppService.UpdateAsync(updatedPurchaseOrder);
                }
            }

            #endregion

        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectPurchaseOrdersAwaitingApprovalLines.Any(t => t.LineNr == LineDataSource.LineNr))
                {
                    int selectedLineIndex = DataSource.SelectPurchaseOrdersAwaitingApprovalLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseOrdersAwaitingApprovalLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPurchaseOrdersAwaitingApprovalLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPurchaseOrdersAwaitingApprovalLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPurchaseOrdersAwaitingApprovalLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPurchaseOrdersAwaitingApprovalLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnSubmit()
        {
            var updateInput = ObjectMapper.Map<SelectPurchaseOrdersAwaitingApprovalsDto, UpdatePurchaseOrdersAwaitingApprovalsDto>(DataSource);

            await PurchaseOrdersAwaitingApprovalsAppService.UpdateAsync(updateInput);

            GrantedQualityApproval();

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

        }

        public override void HideEditPage()
        {
            if (_Timer.Enabled == false)
            {
                _Timer.Start();
                _Timer.Enabled = true;
            }

            base.HideEditPage();
        }



        #endregion

        #region Teknik Resim Görüntüleme İşlemleri

        #region Değişkenler

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        #endregion

        private async void PreviewUploadedImage()
        {
            string webrootpath = FileUploadService.GetRootPath();
            string qualityPlanPath = @"\UploadedFiles\QualityControl\PurchaseQualityPlan\" + DataSource.CurrentAccountCardName + @"\" + DataSource.ProductCode + @"\";
            DirectoryInfo qualityPlan = new DirectoryInfo(webrootpath + qualityPlanPath);
            if (qualityPlan.Exists)
            {
                System.IO.FileInfo[] exactFilesQualityPlan = qualityPlan.GetFiles();

                if (exactFilesQualityPlan.Length > 0 && exactFilesQualityPlan != null)
                {
                    var file = exactFilesQualityPlan[0];

                    string format = file.Extension;

                    UploadedFile = true;

                    string rootpath = FileUploadService.GetRootPath();

                    if (format == ".jpg" || format == ".jpeg" || format == ".png")
                    {
                        imageDataUri = @"\UploadedFiles\QualityControl\PurchaseQualityPlan\" + DataSource.CurrentAccountCardName + @"\" + DataSource.ProductCode + @"\" + file.Name;

                        image = true;

                        pdf = false;

                        ImagePreviewPopup = true;
                    }

                    else if (format == ".pdf")
                    {

                        PDFrootPath = "wwwroot/UploadedFiles/QualityControl/PurchaseQualityPlan/" + DataSource.CurrentAccountCardName + "/" + DataSource.ProductCode + "/" + file.Name;

                        PDFFileName = file.Name;

                        previewImagePopupTitle = file.Name;

                        pdf = true;

                        image = false;

                        ImagePreviewPopup = true;
                    }

                }

                else
                {
                    await ModalManager.MessagePopupAsync(L["UIInformationFileTitle"], L["UIInformationFileMessage"]);
                }


                await InvokeAsync(() => StateHasChanged());

            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIInformationFileTitle"], L["UIInformationFileMessage"]);
            }
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

        #region Kalite Ekranı Durum Enum Combobox

        public IEnumerable<SelectPurchaseOrdersAwaitingApprovalsDto> states = GetEnumDisplayApprovalStateNames<PurchaseOrdersAwaitingApprovalStateEnum>();

        public static List<SelectPurchaseOrdersAwaitingApprovalsDto> GetEnumDisplayApprovalStateNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PurchaseOrdersAwaitingApprovalStateEnum>()
                       .Select(x => new SelectPurchaseOrdersAwaitingApprovalsDto
                       {
                           PurchaseOrdersAwaitingApprovalStateEnum = x,
                           PurchaseOrdersAwaitingApprovalStateEnumName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }


        #endregion

        #region Satın Alma Uygunsuzluk Ekleme Modalı İşlemleri

        #region Satın Alma Uygunsuzluk Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            PurchaseUnsuitabilityDataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchUnsRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Uygunsuzluk Başlıkları ButtonEdit

        SfTextBox UnsuitabilityItemsButtonEdit;
        bool SelectUnsuitabilityItemsPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemsOnCreateIcon()
        {
            var UnsuitabilityItemsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemsButtonClickEvent);
            await UnsuitabilityItemsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemsButtonClick } });
        }

        public async void UnsuitabilityItemsButtonClickEvent()
        {
            SelectUnsuitabilityItemsPopupVisible = true;
            await GetUnsuitabilityItemsList();
            await InvokeAsync(StateHasChanged);
        }


        public void UnsuitabilityItemsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                PurchaseUnsuitabilityDataSource.UnsuitabilityItemsID = Guid.Empty;
                PurchaseUnsuitabilityDataSource.UnsuitabilityItemsName = string.Empty;
            }
        }

        public async void UnsuitabilityItemsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                PurchaseUnsuitabilityDataSource.UnsuitabilityItemsID = selectedOrder.Id;
                PurchaseUnsuitabilityDataSource.UnsuitabilityItemsName = selectedOrder.Name;
                SelectUnsuitabilityItemsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task GetUnsuitabilityItemsList()
        {
            var unsuitabilityTypesItem = (await UnsuitabilityTypesItemsAppService.GetWithUnsuitabilityItemDescriptionAsync("Purchase")).Data;

            if (unsuitabilityTypesItem != null)
            {
                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.Where(t => t.UnsuitabilityTypesItemsName == unsuitabilityTypesItem.Name).ToList();
            }
        }

        #endregion

        #region Satın Alma Uygunsuzluk ComboBox İşlemleri

        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Rejection", Text="ComboboxRejection"},
            new UnsComboBox(){ID = "Correction", Text="ComboboxCorrection"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="ComboboxToBeUsedAs"},
            new UnsComboBox(){ID = "ContactSupplier", Text="ComboboxContactSupplier"}
        };

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "Rejection":
                        PurchaseUnsuitabilityDataSource.Action_ = L["ComboboxRejection"].Value;
                        break;

                    case "Correction":
                        PurchaseUnsuitabilityDataSource.Action_ = L["ComboboxCorrection"].Value;
                        break;

                    case "ToBeUsedAs":
                        PurchaseUnsuitabilityDataSource.Action_ = L["ComboboxToBeUsedAs"].Value;
                        break;

                    case "ContactSupplier":
                        PurchaseUnsuitabilityDataSource.Action_ = L["ComboboxContactSupplier"].Value;
                        break;

                    default: break;
                }
            }
            else
            {
                PurchaseUnsuitabilityDataSource.Action_ = string.Empty;
            }
        }

        #endregion

        protected async Task OnPurchaseUnsuitabilitySubmit()
        {
            if(PurchaseUnsuitabilityDataSource.UnsuitableAmount > 0 && !string.IsNullOrEmpty(PurchaseUnsuitabilityDataSource.Action_))
            {
                var createInput = ObjectMapper.Map<SelectPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto>(PurchaseUnsuitabilityDataSource);

                await PurchaseUnsuitabilityReportsService.CreateAsync(createInput);

                HidePurchaseUnsuitabilityPopup();
            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningAmountActionTitle"], L["UIWarningAmountActionMessage"]);
            }
            
        }

        public void HidePurchaseUnsuitabilityPopup()
        {
            PurchaseUnsuitabilityCrudPopup = false;
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeMainButtonEdit;

        public async Task CodeMainOnCreateIcon()
        {
            var CodesMainButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeMainButtonClickEvent);
            await CodeMainButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesMainButtonClick } });
        }

        public async void CodeMainButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersAwaitingApprovalsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Timer

        System.Timers.Timer _Timer;

        private void StartTimer()
        {
            _Timer = new System.Timers.Timer(10000);
            _Timer.Elapsed += _TimerTimedEvent;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }

        private async void _TimerTimedEvent(object source, ElapsedEventArgs e)
        {
            await base.GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }
        #endregion



        public void Dispose()
        {

            if (_Timer != null)
            {
                if (_Timer.Enabled)
                {
                    _Timer.Stop();
                    _Timer.Enabled = false;
                    _Timer.Dispose();
                }
            }

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
