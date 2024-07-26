using BlazorInputFile;
using DevExpress.Blazor;
using DevExpress.ClipboardSource.SpreadsheetML;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.Product
{
    public partial class ProductsListPage : IDisposable
    {

        #region Ürün Grubu ButtonEdit
        SfTextBox ProductGroupsButtonEdit;
        bool SelectProductGroupsPopupVisible = false;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        #endregion

        #region Combobox İşlemleri

        public IEnumerable<SelectProductsDto> types = GetEnumDisplayTypeNames<ProductTypeEnum>();

        public IEnumerable<SelectProductsDto> supplyforms = GetEnumDisplaySupplyFormNames<ProductSupplyFormEnum>();
        public IEnumerable<SelectProductsDto> rawmaterialtypes = GetEnumDisplayRowMaterialTypeNames<RowMaterialTypeEnum>();



        public static List<SelectProductsDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<ProductTypeEnum>()
                       .Select(x => new SelectProductsDto
                       {
                           ProductType = x,
                           ProductTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        public static List<SelectProductsDto> GetEnumDisplaySupplyFormNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<ProductSupplyFormEnum>()
                       .Select(x => new SelectProductsDto
                       {
                           SupplyForm = x,
                           SupplyFormName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        public static List<SelectProductsDto> GetEnumDisplayRowMaterialTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<RowMaterialTypeEnum>()
                       .Select(x => new SelectProductsDto
                       {
                           RawMaterialType = x,
                           RawMaterialTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private void ProductTypeValueChangeHandler(ChangeEventArgs<ProductTypeEnum, SelectProductsDto> args)
        {
            if (args.ItemData.ProductType == ProductTypeEnum.HM)
            {
                productSizeVisible = true;
            }
            else
            {
                productSizeVisible = false;
            }
        }
        private async void RawMaterialTypeValueChangeHandler(ChangeEventArgs<RowMaterialTypeEnum, SelectProductsDto> args)
        {
            if (args.ItemData.RawMaterialType == RowMaterialTypeEnum.BoruHammadde)
            {
                isHBRaw = true;
                isHMRaw = false;
                isHSRaw = false;
                //DataSource.Width_ = 0;
                //DataSource.Tickness_ = 0;
                //DataSource.RadiusValue = 0;
            }
            else if (args.ItemData.RawMaterialType == RowMaterialTypeEnum.MilHammadde)
            {
                isHBRaw = false;
                isHMRaw = true;
                isHSRaw = false;
                //DataSource.Width_ = 0;
                //DataSource.Tickness_ = 0;
                //DataSource.ExternalRadius = 0;
                //DataSource.InternalRadius = 0;
            }
            else if (args.ItemData.RawMaterialType == RowMaterialTypeEnum.SacHammadde)
            {
                isHBRaw = false;
                isHMRaw = false;
                isHSRaw = true;
                //DataSource.ExternalRadius = 0;
                //DataSource.InternalRadius = 0;
                //DataSource.RadiusValue = 0;
            }

            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Listeler ve DataSource

        public SelectTechnicalDrawingsDto TechnicalDrawingsDataSource { get; set; }

        SelectProductRelatedProductPropertiesDto LineDataSource;
        public SelectProductReferanceNumbersDto ProductReferanceNumbersDataSource { get; set; }
        public SelectBillsofMaterialsDto BillsofMaterialsDataSource { get; set; }
        public SelectRoutesDto RoutesDataSource { get; set; }
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> TechnicalDrawingGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductReferanceNumberGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> BillsofMaterialGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> RouteGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductRelatedProductPropertiesContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectTechnicalDrawingsDto> TechnicalDrawingsList = new List<SelectTechnicalDrawingsDto>();

        public List<SelectProductReferanceNumbersDto> ProductReferanceNumbersList = new List<SelectProductReferanceNumbersDto>();

        public List<SelectSalesPriceLinesDto> SalesPriceLinesList = new List<SelectSalesPriceLinesDto>();

        public List<SelectPurchasePriceLinesDto> PurchasePriceLinesList = new List<SelectPurchasePriceLinesDto>();

        public List<ListBillsofMaterialsDto> BillsofMaterialsList = new List<ListBillsofMaterialsDto>();

        public List<ListRoutesDto> RoutesList = new List<ListRoutesDto>();

        public List<SelectBillsofMaterialLinesDto> BillsofMaterialLinesList = new List<SelectBillsofMaterialLinesDto>();

        public List<SelectRouteLinesDto> RouteLinesList = new List<SelectRouteLinesDto>();

        public List<SelectContractProductionTrackingsDto> ContractProductionTrackingsList = new List<SelectContractProductionTrackingsDto>();

        public List<SelectProductPropertyLinesDto> ProductPropertyLineList = new List<SelectProductPropertyLinesDto>();

        public List<ListGrandTotalStockMovementsDto> StockAmountsList = new List<ListGrandTotalStockMovementsDto>();

        public List<ListStockAddressesDto> StockAddressesList = new List<ListStockAddressesDto>();

        public List<SelectStockAddressLinesDto> StockAddressLinesList = new List<SelectStockAddressLinesDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();

        public List<SelectProductRelatedProductPropertiesDto> ProductRelatedProductPropertiesList = new List<SelectProductRelatedProductPropertiesDto>();

        public List<ListMenusDto> MenusList = new List<ListMenusDto>();

        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #endregion


        [Inject]
        ModalManager ModalManager { get; set; }

        #region Grid

        private SfGrid<SelectTechnicalDrawingsDto> _TechnicalDrawingGrid;
        //private SfGrid<SelectTechnicalDrawingsDto> _TechnicalDrawingChangeGrid;
        private SfGrid<SelectProductReferanceNumbersDto> _ProductReferanceNumberGrid;
        private SfGrid<SelectSalesPriceLinesDto> _SalesPriceLineGrid;
        private SfGrid<SelectPurchasePriceLinesDto> _PurchasePriceLineGrid;
        private SfGrid<ListBillsofMaterialsDto> _BillsofMaterialGrid;
        private SfGrid<SelectBillsofMaterialLinesDto> _BillsofMaterialLineGrid;
        private SfGrid<ListRoutesDto> _RouteGrid;
        private SfGrid<SelectRouteLinesDto> _RouteLineGrid;
        private SfGrid<SelectContractProductionTrackingsDto> _ContractProductionTrackingGrid;
        private SfGrid<ListGrandTotalStockMovementsDto> _StockAmountsGrid;
        private SfGrid<SelectProductRelatedProductPropertiesDto> _LineGrid;

        #endregion

        #region Değişkenler

        public bool TechnicalDrawingsCrudPopup = false;
        public bool TechnicalDrawingsChangedCrudPopup = false;
        public bool TechnicalDrawingsPopup = false;

        public bool ProductReferanceNumbersCrudPopup = false;
        public bool ProductReferanceNumbersPopup = false;

        public bool SalesPriceLinesPopup = false;

        public bool PurchasePriceLinesPopup = false;

        public bool BillsofMaterialsPopup = false;
        public bool BillsofMaterialsCrudPopup = false;

        public bool RoutesPopup = false;
        public bool RoutesCrudPopup = false;

        public bool ContractProductionTrackingsPopup = false;
        public bool TechnicalDrawingIsChange = false;
        public bool ProductReferanceNumberIsChange = false;

        public bool StockAmountsPopup = false;
        public bool isHM = false;
        public bool isYM = false;
        public bool isMM = false;
        public bool isTM = false;

        public bool isHSRaw = false;
        public bool isHBRaw = false;
        public bool isHMRaw = false;

        public bool StockAddressPopupVisible = false;

        List<string> Drawers = new List<string>();

        List<IFileListEntry> files = new List<IFileListEntry>();

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        //bool disable = new();

        bool ImagePreviewPopup = false;

        bool UploadedFile = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        public bool productSizeVisible = false;

        public bool LineCrudPopup = false;

        #endregion

        protected override async Task OnSubmit()
        {
            if (DataSource.ProductType == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);
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

        public override void HideEditPage()
        {
            isHMRaw = false;
            isHSRaw = false;
            isHBRaw = false;
            base.HideEditPage();
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
                            case "ProductContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextAdd"], Id = "new" }); break;
                            case "ProductContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextChange"], Id = "changed" }); break;
                            case "ProductContextTechnicalDrawings":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextTechnicalDrawings"], Id = "technicaldrawings" }); break;
                            case "ProductContextProductRefNr":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextProductRefNr"], Id = "productreferancenumbers" }); break;
                            case "ProductContextPurchasePrices":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextPurchasePrices"], Id = "purchaseprices" }); break;
                            case "ProductContextSalesPrices":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextSalesPrices"], Id = "salesprices" }); break;
                            case "ProductContextBOMs":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextBOMs"], Id = "billsofmaterials" }); break;
                            case "ProductContextRoutes":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextRoutes"], Id = "routes" }); break;
                            case "ProductContextContProdTrackings":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextContProdTrackings"], Id = "contractproductiontrackings" }); break;
                            case "ProductContextStockAmounts":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextStockAmounts"], Id = "stockamounts" }); break;
                            case "ProductContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextDelete"], Id = "delete" }); break;
                            case "ProductContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextRefresh"], Id = "refresh" }); break;
                            case "ProductContextStockAddress":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductContextStockAddress"], Id = "stockaddress" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        protected void CreateLineContextMenuItems()
        {
            if (ProductRelatedProductPropertiesContextMenu.Count() == 0)
            {
                ProductRelatedProductPropertiesContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextChange"], Id = "changed" });
                ProductRelatedProductPropertiesContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextRefresh"], Id = "refresh" });

            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectProductRelatedProductPropertiesDto> args)
        {
            switch (args.Item.Id)
            {


                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    ProductPropertyLineList = (await ProductPropertiesAppService.GetAsync(LineDataSource.ProductPropertyID)).Data.SelectProductPropertyLines.ToList();
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
            ProductPropertyLineList.Clear();
        }


        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectProductRelatedProductProperties.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductRelatedProductProperties.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductRelatedProductProperties[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductRelatedProductProperties.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductRelatedProductProperties.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductRelatedProductProperties[selectedLineIndex] = LineDataSource;
                }
            }

            ProductRelatedProductPropertiesList = DataSource.SelectProductRelatedProductProperties;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }


        #region Teknik Resim Modalı İşlemleri

        public async Task TechnicalDrawingBeforeInsertAsync()
        {
            TechnicalDrawingsDataSource = new SelectTechnicalDrawingsDto()
            {
                ProductID = DataSource.Id,
                ProductCode = DataSource.Code,
                ProductName = DataSource.Name,
                RevisionDate = GetSQLDateAppService.GetDateFromSQL()
            };

            Drawers = TechnicalDrawingsList.Select(t => t.Drawer).Distinct().ToList();

            TechnicalDrawingsCrudPopup = true;

            await Task.CompletedTask;
        }

        public async void TechnicalDrawingShowEditPage()
        {

            if (TechnicalDrawingsDataSource != null)
            {

                if (TechnicalDrawingsDataSource.DataOpenStatus == true && TechnicalDrawingsDataSource.DataOpenStatus != null)
                {
                    TechnicalDrawingsChangedCrudPopup = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    TechnicalDrawingsChangedCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void CreateTechnicalDrawingContextMenuItems()
        {
            if (TechnicalDrawingGridContextMenu.Count() == 0)
            {

                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextAdd"], Id = "new" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextChange"], Id = "changed" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextDelete"], Id = "delete" });
                TechnicalDrawingGridContextMenu.Add(new ContextMenuItemModel { Text = L["TechnicalDrawingContextRefresh"], Id = "refresh" });

            }
        }

        public async void OnTechnicalDrawingContextMenuClick(ContextMenuClickEventArgs<SelectTechnicalDrawingsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await TechnicalDrawingBeforeInsertAsync();

                    await InvokeAsync(StateHasChanged);

                    break;

                case "changed":
                    TechnicalDrawingIsChange = true;
                    TechnicalDrawingsDataSource = args.RowInfo.RowData;
                    string rootpath = FileUploadService.GetRootPath();
                    string technicalDrawingPath = @"\UploadedFiles\TechnicalDrawings\" + DataSource.Id + "-" + DataSource.Code + @"\" + TechnicalDrawingsDataSource.Id + @"\";
                    DirectoryInfo technicalDrawing = new DirectoryInfo(rootpath + technicalDrawingPath);
                    if (technicalDrawing.Exists)
                    {
                        System.IO.FileInfo[] exactFilesTechnicalDrawing = technicalDrawing.GetFiles();

                        foreach (System.IO.FileInfo fileinfo in exactFilesTechnicalDrawing)
                        {
                            uploadedfiles.Add(fileinfo);
                        }

                    }
                    TechnicalDrawingShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);

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

            await TechnicalDrawingsAppService.GetListAsync(new ListTechnicalDrawingsParameterDto() { ProductId = DataSource.Id });

            var savedEntityIndex = TechnicalDrawingsList.FindIndex(x => x.Id == TechnicalDrawingsDataSource.Id);

            TechnicalDrawingsList = (await TechnicalDrawingsAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();

            await _TechnicalDrawingGrid.Refresh();


            if (TechnicalDrawingsDataSource.Id == Guid.Empty)
            {
                TechnicalDrawingsDataSource.Id = result.Id;
            }


            #endregion

            #region File Upload İşlemleri

            string productid = TechnicalDrawingsDataSource.ProductID.ToString();
            string productcode = TechnicalDrawingsDataSource.ProductCode;
            string technicaldrawingid = TechnicalDrawingsDataSource.Id.ToString();

            List<string> _result = new List<string>();

            foreach (var file in files)
            {
                //disable = true;

                string fileName = file.Name;
                string rootPath = "UploadedFiles/TechnicalDrawings/" + productid + "-" + productcode + "/" + technicaldrawingid;


                _result.Add(await FileUploadService.UploadTechnicalDrawing(file, rootPath, fileName));
                await InvokeAsync(() => StateHasChanged());

            }


            HideTechnicalDrawingCrudPopup();
            HideTechnicalDrawingChangedCrudPopup();

            //disable = false;

            files.Clear();

            await InvokeAsync(() => StateHasChanged());

            #endregion
        }

        public void HideTechnicalDrawingCrudPopup()
        {
            TechnicalDrawingsCrudPopup = false;
        }

        public void HideTechnicalDrawingChangedCrudPopup()
        {
            TechnicalDrawingsChangedCrudPopup = false;
            uploadedfiles.Clear();
            InvokeAsync(StateHasChanged);
        }

        public void HideTechnicalDrawingPopup()
        {
            TechnicalDrawingsPopup = false;
        }

        public virtual async void TechnicalDrawingCrudModalShowing(PopupShowingEventArgs args)
        {
            if (TechnicalDrawingsDataSource.Id != Guid.Empty)
            {
                await TechnicalDrawingsAppService.UpdateConcurrencyFieldsAsync(TechnicalDrawingsDataSource.Id, true, Guid.NewGuid());
            }
        }

        public virtual async void TechnicalDrawingCrudModalClosing(PopupClosingEventArgs args)
        {
            if (TechnicalDrawingIsChange)
            {
                await TechnicalDrawingsAppService.UpdateConcurrencyFieldsAsync(TechnicalDrawingsDataSource.Id, false, Guid.Empty);
                TechnicalDrawingIsChange = false;
            }
        }

        #endregion

        #region Ürün Referans Numarası Modalı İşlemleri

        public async Task ProductReferanceNumberBeforeInsertAsync()
        {
            ProductReferanceNumbersDataSource = new SelectProductReferanceNumbersDto()
            {
                ProductID = DataSource.Id,
                ProductCode = DataSource.Code,
                ProductName = DataSource.Name
            };


            ProductReferanceNumbersCrudPopup = true;

            await Task.CompletedTask;
        }

        public async void ProductReferanceNumberShowEditPage()
        {

            if (ProductReferanceNumbersDataSource != null)
            {

                if (ProductReferanceNumbersDataSource.DataOpenStatus == true && ProductReferanceNumbersDataSource.DataOpenStatus != null)
                {
                    ProductReferanceNumbersCrudPopup = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    ProductReferanceNumbersCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void CreateProductReferanceNumberContextMenuItems()
        {
            if (ProductReferanceNumberGridContextMenu.Count() == 0)
            {
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReferanceNumberContextAdd"], Id = "new" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReferanceNumberContextChange"], Id = "changed" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReferanceNumberContextDelete"], Id = "delete" });
                ProductReferanceNumberGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductReferanceNumberContextRefresh"], Id = "refresh" });

            }
        }

        public async void OnProductReferanceNumberContextMenuClick(ContextMenuClickEventArgs<SelectProductReferanceNumbersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await ProductReferanceNumberBeforeInsertAsync();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    ProductReferanceNumbersDataSource = args.RowInfo.RowData;
                    ProductReferanceNumberShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBaseProdRefNr"]);

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

        public virtual async void ProductReferanceNumberCrudModalShowing(PopupShowingEventArgs args)
        {
            if (ProductReferanceNumbersDataSource.Id != Guid.Empty)
            {
                await ProductReferanceNumbersAppService.UpdateConcurrencyFieldsAsync(ProductReferanceNumbersDataSource.Id, true, Guid.NewGuid());
            }
        }

        public virtual async void ProductReferanceNumberCrudModalClosing(PopupClosingEventArgs args)
        {
            if (ProductReferanceNumberIsChange)
            {
                await ProductReferanceNumbersAppService.UpdateConcurrencyFieldsAsync(ProductReferanceNumbersDataSource.Id, false, Guid.Empty);
                ProductReferanceNumberIsChange = false;
            }
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

        #region Reçete Modalı İşlemleri

        protected void CreateBillsofMaterialsContextMenuItems()
        {
            if (BillsofMaterialGridContextMenu.Count() == 0)
            {
                var contextID = contextsList.Where(t => t.MenuName == "BoMContextExamine").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    BillsofMaterialGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMContextExamine"], Id = "examine" });
                }
            }
        }

        public async void OnBillsofMaterialContextMenuClick(ContextMenuClickEventArgs<ListBillsofMaterialsDto> args)
        {
            switch (args.Item.Id)
            {


                case "examine":
                    BillsofMaterialsDataSource = (await BillsofMaterialsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    BillsofMaterialLinesList = BillsofMaterialsDataSource.SelectBillsofMaterialLines;

                    foreach (SelectBillsofMaterialLinesDto item in BillsofMaterialLinesList)
                    {
                        item.FinishedProductCode = BillsofMaterialsDataSource.FinishedProductCode;
                        item.ProductCode = (await ProductService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    BillsofMaterialsCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;


                default:
                    break;
            }
        }

        public void HideBillsofMaterialCrudPopup()
        {
            BillsofMaterialsCrudPopup = false;
        }

        public void HideBillsofMaterialPopup()
        {
            BillsofMaterialsPopup = false;
        }

        #endregion

        #region Rota Modalı İşlemleri

        protected void CreateRoutesContextMenuItems()
        {
            if (RouteGridContextMenu.Count() == 0)
            {
                var contextID = contextsList.Where(t => t.MenuName == "RouteContextExamine").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    RouteGridContextMenu.Add(new ContextMenuItemModel { Text = L["RouteContextExamine"], Id = "examine" });
                }
            }
        }

        public async void OnRouteContextMenuClick(ContextMenuClickEventArgs<ListRoutesDto> args)
        {
            switch (args.Item.Id)
            {


                case "examine":
                    RoutesDataSource = (await RoutesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    RouteLinesList = RoutesDataSource.SelectRouteLines;

                    foreach (SelectRouteLinesDto item in RouteLinesList)
                    {
                        item.ProductCode = DataSource.Code;
                        item.ProductName = DataSource.Name;
                        item.OperationCode = (await ProductsOperationsAppService.GetAsync(item.ProductsOperationID)).Data.Code;
                        item.OperationName = (await ProductsOperationsAppService.GetAsync(item.ProductsOperationID)).Data.Name;
                    }

                    RoutesCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;


                default:
                    break;
            }
        }

        public void HideRouteCrudPopup()
        {
            RoutesCrudPopup = false;
        }

        public void HideRoutePopup()
        {
            RoutesPopup = false;
        }

        #endregion

        #region Fiyat Listeleri Modalları İşlemleri

        public void HideSalesPricesPopup()
        {
            SalesPriceLinesPopup = false;
        }

        public void HidePurchasePricesPopup()
        {
            PurchasePriceLinesPopup = false;
        }

        #endregion

        #region Fason Üretim Takip Fişleri Modalı İşlemleri

        public void HideContractProductionTrackingsPopup()
        {
            ContractProductionTrackingsPopup = false;
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
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBaseTechDraw"]);
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
                PDFrootPath = rootpath + @"\UploadedFiles\TechnicalDrawings\" + DataSource.Id + "-" + DataSource.Code + @"\" + TechnicalDrawingsDataSource.Id + @"\" + file.Name;

                System.IO.FileInfo pdfFile = new System.IO.FileInfo(PDFrootPath);
                if (pdfFile.Exists)
                {
                    pdfFile.Delete();
                }
            }

            else
            {
                imageDataUri = rootpath + @"\UploadedFiles\TechnicalDrawings\" + DataSource.Id + "-" + DataSource.Code + @"\" + TechnicalDrawingsDataSource.Id + @"\" + file.Name;

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

            UploadedFile = false;

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
                string tempPath = "tempFiles/";

                PDFrootPath = "wwwroot/" + tempPath + file.Name;

                PDFFileName = file.Name;

                List<string> _result = new List<string>();

                _result.Add(await FileUploadService.UploadTechnicalDrawingPDF(file, tempPath, PDFFileName));

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
                imageDataUri = @"\UploadedFiles\TechnicalDrawings\" + DataSource.Id + "-" + DataSource.Code + @"\" + TechnicalDrawingsDataSource.Id + @"\" + file.Name;

                image = true;

                pdf = false;

                ImagePreviewPopup = true;
            }

            else if (format == ".pdf")
            {

                PDFrootPath = "wwwroot/UploadedFiles/TechnicalDrawings/" + DataSource.Id + "-" + DataSource.Code + "/" + TechnicalDrawingsDataSource.Id + "/" + file.Name;

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

                var productPropertyList = (await ProductPropertiesAppService.GetListByProductGroupAsync(selectedProductGroup.Id)).Data.ToList();

                if (productPropertyList != null && productPropertyList.Count > 0)
                {
                    foreach (var item in productPropertyList)
                    {
                        var productProperty = (await ProductPropertiesAppService.GetAsync(item.Id)).Data;


                        SelectProductRelatedProductPropertiesDto lineModel = new SelectProductRelatedProductPropertiesDto
                        {
                            isPurchaseBreakdown = false,
                            IsQualityControlCriterion = false,
                            LineNr = ProductRelatedProductPropertiesList.Count + 1,
                            ProductGroupID = selectedProductGroup.Id,
                            ProductPropertyID = item.Id,
                            PropertyValue = string.Empty,
                            PropertyName = productProperty.Name
                        };

                        ProductRelatedProductPropertiesList.Add(lineModel);

                    }
                }

                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Depo Toplamları Modalı İşlemleri

        public void HideStockAmountsPopup()
        {
            isHM = false;
            isMM = false;
            isTM = false;
            isYM = false;
            StockAmountsPopup = false;
        }

        #endregion

        #region Stok Adresleri Modalı İşlemler

        public void HideStockAddressButtonClicked()
        {
            StockAddressesList.Clear();
            StockAddressPopupVisible = false;
        }

        #endregion

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListProductsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    DataSource.ProductType = ProductTypeEnum.TM;
                    DataSource.SupplyForm = ProductSupplyFormEnum.Üretim;
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    foreach (var item in supplyforms)
                    {
                        item.SupplyFormName = L[item.SupplyFormName];
                    }

                    foreach (var item in types)
                    {
                        item.ProductTypeName = L[item.ProductTypeName];
                    }
                    ShowEditPage();
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

                case "purchaseprices":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    PurchasePriceLinesList = (await PurchasePricesAppService.GetSelectLineListAsync(DataSource.Id)).Data.ToList();

                    foreach (var item in PurchasePriceLinesList)
                    {
                        item.ProductCode = DataSource.Code;
                        item.ProductName = DataSource.Name;
                        item.CurrentAccountCardName = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Name;
                        item.CurrencyCode = (await CurrenciesAppService.GetAsync(item.CurrencyID.GetValueOrDefault())).Data.Code;
                    }
                    PurchasePriceLinesPopup = true;

                    await InvokeAsync(StateHasChanged);


                    break;

                case "salesprices":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    SalesPriceLinesList = (await SalesPricesAppService.GetSelectLineListAsync(DataSource.Id)).Data.ToList();

                    foreach (var item in SalesPriceLinesList)
                    {
                        item.ProductCode = DataSource.Code;
                        item.ProductName = DataSource.Name;
                        item.CurrentAccountCardName = (await CurrentAccountCardsAppService.GetAsync(item.CurrentAccountCardID.GetValueOrDefault())).Data.Name;
                        item.CurrencyCode = (await CurrenciesAppService.GetAsync(item.CurrencyID.GetValueOrDefault())).Data.Code;
                    }
                    SalesPriceLinesPopup = true;

                    await InvokeAsync(StateHasChanged);


                    break;

                case "billsofmaterials":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    BillsofMaterialsList = (await BillsofMaterialsAppService.GetListAsync(new ListBillsofMaterialsParameterDto())).Data.Where(t => t.FinishedProductID == DataSource.Id).ToList();

                    BillsofMaterialsPopup = true;

                    await InvokeAsync(StateHasChanged);


                    break;

                case "stockamounts":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    isHM = false;
                    isYM = false;
                    isMM = false;
                    isTM = false;

                    StockAmountsList = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == DataSource.Id).ToList();

                    switch (DataSource.ProductType)
                    {
                        case ProductTypeEnum.TM: isTM = true; break;
                        case ProductTypeEnum.MM: isMM = true; break;
                        case ProductTypeEnum.HM: isHM = true; break;
                        case ProductTypeEnum.YM: isYM = true; break;

                    }

                    StockAmountsPopup = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                case "routes":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    RoutesList = (await RoutesAppService.GetListAsync(new ListRoutesParameterDto())).Data.Where(t => t.ProductID == DataSource.Id).ToList();

                    RoutesPopup = true;

                    await InvokeAsync(StateHasChanged);


                    break;

                case "contractproductiontrackings":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ContractProductionTrackingsList = (await ContractProductionTrackingsAppService.GetSelectListAsync(DataSource.Id)).Data.ToList();

                    ContractProductionTrackingsPopup = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                case "stockaddress":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    StockAddressesList = (await StockAddressesService.GetListAsync(new ListStockAddressesParameterDto())).Data.Where(t => t.ProductID == DataSource.Id).ToList();

                    if (StockAddressesList != null && StockAddressesList.Count != 0)
                    {
                        foreach (var stockaddress in StockAddressesList)
                        {
                            var lineList = (await StockAddressesService.GetAsync(stockaddress.Id)).Data.SelectStockAddressLines.ToList();

                            if (lineList != null && lineList.Count != 0)
                            {
                                foreach (var line in lineList)
                                {
                                    StockAddressLinesList.Add(line);
                                }
                            }
                        }
                    }


                    StockAddressPopupVisible = true;

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
            _L = L;

            #region Context Menü Yetkilendirmesi
            
            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion
            CreateMainContextMenuItems();
            CreateTechnicalDrawingContextMenuItems();
            CreateProductReferanceNumberContextMenuItems();
            CreateBillsofMaterialsContextMenuItems();
            CreateRoutesContextMenuItems();
            CreateLineContextMenuItems();


        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsDto()
            {
                IsActive = true
            };


            DataSource.SelectProductRelatedProductProperties = new List<SelectProductRelatedProductPropertiesDto>();
            ProductRelatedProductPropertiesList = DataSource.SelectProductRelatedProductProperties;

            foreach (var item in supplyforms)
            {
                item.SupplyFormName = L[item.SupplyFormName];
            }

            foreach (var item in types)
            {
                item.ProductTypeName = L[item.ProductTypeName];
            }
            foreach (var item in rawmaterialtypes)
            {
                item.RawMaterialTypeName = L[item.RawMaterialTypeName];
            }

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {
            if (DataSource != null)
            {
                bool? dataOpenStatus = DataSource.DataOpenStatus;

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    foreach (var item in supplyforms)
                    {
                        item.SupplyFormName = L[item.SupplyFormName];
                    }

                    foreach (var item in types)
                    {
                        item.ProductTypeName = L[item.ProductTypeName];
                    }
                    foreach (var item in rawmaterialtypes)
                    {
                        item.RawMaterialTypeName = L[item.RawMaterialTypeName];
                    }

                    switch (DataSource.RawMaterialType)
                    {
                        case RowMaterialTypeEnum.MilHammadde:
                            isHMRaw = true;
                            break;
                        case RowMaterialTypeEnum.SacHammadde:
                            isHSRaw = true;
                            break;
                        case RowMaterialTypeEnum.BoruHammadde:
                            isHBRaw = true;
                            break;
                        default:
                            isHMRaw= false;
                            isHSRaw=false;
                            isHBRaw=false;
                            break;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        #region Teknik Resim Cari Hesap Button Edit

        SfTextBox TechDrawingCurrentAccountCardsCodeButtonEdit = new();
        SfTextBox TechDrawingCurrentAccountCardsNameButtonEdit = new();
        List<ListCurrentAccountCardsDto> TechDrawingCurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();
        public async Task TechDrawingCurrentAccountCardsCodeOnCreateIcon()
        {
            var TechDrawingCurrentAccountCardsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TechDrawingCurrentAccountCardsCodeButtonClickEvent);
            await TechDrawingCurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TechDrawingCurrentAccountCardsButtonClick } });
        }

        public async void TechDrawingCurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetTechDrawingCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task TechDrawingCurrentAccountCardsNameOnCreateIcon()
        {
            var TechDrawingCurrentAccountCardsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TechDrawingCurrentAccountCardsNameButtonClickEvent);
            await TechDrawingCurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TechDrawingCurrentAccountCardsButtonClick } });
        }

        public async void TechDrawingCurrentAccountCardsNameButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetTechDrawingCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public void TechDrawingCurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                TechnicalDrawingsDataSource.CustomerCurrentAccountCardID = Guid.Empty;
                TechnicalDrawingsDataSource.CustomerCode = string.Empty;
            }
        }

        public async void TechDrawingCurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedTechDrawingCurrentAccountCard = args.RowData;

            if (selectedTechDrawingCurrentAccountCard != null)
            {
                TechnicalDrawingsDataSource.CustomerCurrentAccountCardID = selectedTechDrawingCurrentAccountCard.Id;
                TechnicalDrawingsDataSource.CustomerCode = selectedTechDrawingCurrentAccountCard.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

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

        private async Task GetTechDrawingCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        #endregion



        #region Kod ButtonEdit

        //SfTextBox CodeButtonEdit;

        //public async Task CodeOnCreateIcon()
        //{
        //    var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
        //    await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        //}

        //public async void CodeButtonClickEvent()
        //{
        //    DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProductsChildMenu");
        //    await InvokeAsync(StateHasChanged);
        //}
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
