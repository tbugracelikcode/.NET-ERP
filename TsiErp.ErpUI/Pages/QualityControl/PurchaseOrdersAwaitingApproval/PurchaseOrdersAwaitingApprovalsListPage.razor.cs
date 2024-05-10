using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.PurchaseOrdersAwaitingApproval.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.ErpUI.Helpers;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Enums;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using System.Timers;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using Syncfusion.Blazor.Inputs;
using TsiErp.ErpUI.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;

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

        SelectPurchaseOrdersAwaitingApprovalLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseOrdersAwaitingApprovalLinesDto> GridLineList = new List<SelectPurchaseOrdersAwaitingApprovalLinesDto>();

        public bool LineCrudPopup = false;

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
                    IsChanged = true;

                    foreach (var item in states)
                    {
                        item.PurchaseOrdersAwaitingApprovalStateEnumName = L[item.PurchaseOrdersAwaitingApprovalStateEnumName];
                    }

                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "qualityapprovalcancel":

                    DataSource = (await PurchaseOrdersAwaitingApprovalsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    CancelQualityApproval();

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
            DataSource.PurchaseOrdersAwaitingApprovalStateEnum = PurchaseOrdersAwaitingApprovalStateEnum.KaliteKontrolOnayVerildi;

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
