using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Lists;
using TsiErp.Business.Entities.PackageFiche.Services;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.ShippingManagement.PalletRecord.PalletRecordsListPage;

namespace TsiErp.ErpUI.Pages.ShippingManagement.PackingList
{
    public partial class PackingListsListPage
    {

        public class PalletSelectionModal
        {
            public bool SelectedPallet { get; set; }
            public Guid PalletID { get; set; }
            public Guid? CurrentAccountCardID { get; set; }
            public string PalletCode { get; set; }
            public string PalletName { get; set; }
            public int NumberofPackage { get; set; }
            public int Width_ { get; set; }
            public int Length_ { get; set; }
            public int Height_ { get; set; }
            public string PackageType { get; set; }
        }

        private SfGrid<SelectPackingListPalletLinesDto> _LinePalletGrid;
        private SfGrid<SelectPackingListPalletPackageLinesDto> _LinePalletPackageGrid;
        private SfGrid<SelectPackingListPalletCubageLinesDto> _LineCubageGrid;
        private SfGrid<PalletSelectionModal> _LinePalletSelectionGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletSelectionGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PalletPackageGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPackingListPalletLinesDto> GridLinePalletList = new List<SelectPackingListPalletLinesDto>();
        List<SelectPackingListPalletPackageLinesDto> GridLinePalletPackageList = new List<SelectPackingListPalletPackageLinesDto>();
        List<SelectPackingListPalletCubageLinesDto> GridLineCubageList = new List<SelectPackingListPalletCubageLinesDto>();
        List<ListPalletRecordsDto> PalletRecordsList = new List<ListPalletRecordsDto>();
        List<PalletSelectionModal> PalletSelectionList = new List<PalletSelectionModal>();

        public bool ShowPalletsModal = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = PackingListsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreatePalletContextMenuItems();
            CreatePalletPackageContextMenuItems();
            CreatePalletSelectionContextMenuItems();

        }

        #region Çeki Listeleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPackingListsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("PackingListsChildMenu"),
            };

            DataSource.SelectPackingListPalletCubageLines = new List<SelectPackingListPalletCubageLinesDto>();
            GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;

            DataSource.SelectPackingListPalletLines = new List<SelectPackingListPalletLinesDto>();
            GridLinePalletList = DataSource.SelectPackingListPalletLines;

            DataSource.SelectPackingListPalletPackageLines = new List<SelectPackingListPalletPackageLinesDto>();
            GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

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

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreatePalletContextMenuItems()
        {
            if (PalletGridContextMenu.Count() == 0)
            {
                PalletGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletLineContextSelectPallet"], Id = "selectpallet" });
            }
        }

        protected void CreatePalletPackageContextMenuItems()
        {
            if (PalletPackageGridContextMenu.Count() == 0)
            {
                PalletPackageGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletPackageLineContextEnumarate"], Id = "enumarate" });
            }
        }

        protected void CreatePalletSelectionContextMenuItems()
        {
            if (PalletSelectionGridContextMenu.Count() == 0)
            {
                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextSelect"], Id = "select" });
                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextSelectAll"], Id = "selectall" });
                PalletSelectionGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackingListsPalletSelectionContextRemove"], Id = "remove" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPackingListsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PackingListsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineCubageList = DataSource.SelectPackingListPalletCubageLines;
                    GridLinePalletList = DataSource.SelectPackingListPalletLines;
                    GridLinePalletPackageList = DataSource.SelectPackingListPalletPackageLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
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

        public async void PalletLineContextMenuClick(ContextMenuClickEventArgs<SelectPackingListPalletLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "selectpallet":

                    if (DataSource.Id == Guid.Empty)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningPalletSelectionTitle"], L["UIWarningPalletSelectionMessage"]);
                    }
                    else
                    {

                        PalletRecordsList = (await PalletRecordsAppService.GetListAsync(new ListPalletRecordsParameterDto())).Data.Where(t => t.PackingListID == DataSource.Id).ToList();

                        PalletRecordsList = PalletRecordsList.OrderBy(t => t.Name).ToList();

                        foreach (var pallet in PalletRecordsList)
                        {
                            if (DataSource.SelectPackingListPalletLines.Where(t => t.PalletID == pallet.Id).Count() == 0)
                            {
                                PalletSelectionModal palletSelectionModel = new PalletSelectionModal
                                {
                                    PalletCode = pallet.Code,
                                    PalletID = pallet.Id,
                                    PalletName = pallet.Name,
                                    NumberofPackage = pallet.PalletPackageNumber,
                                    Height_ = pallet.Height_,
                                    Length_ = pallet.Lenght_,
                                    PackageType = pallet.PackageType,
                                    CurrentAccountCardID = pallet.CurrentAccountCardID,
                                    Width_ = pallet.Width_,
                                    SelectedPallet = false
                                };

                                PalletSelectionList.Add(palletSelectionModel);
                            }

                        }

                        ShowPalletsModal = true;
                    }


                    break;
                default:
                    break;
            }
        }

        public async void PalletSelectionLineContextMenuClick(ContextMenuClickEventArgs<PalletSelectionModal> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    var pallet = args.RowInfo.RowData;
                    int selectedPalletIndex = PalletSelectionList.IndexOf(pallet);

                    bool notLastSelectedLine = false;

                    for (int i = selectedPalletIndex; i <= 0; i--)
                    {
                        if (!PalletSelectionList[i].SelectedPallet)
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningSelectedIndexTitle"], L["UIWarningSelectedIndexMessage"]);
                            notLastSelectedLine = true;
                        }
                        if (notLastSelectedLine)
                        {
                            break;
                        }
                    }
                    if (!notLastSelectedLine)
                    {

                        PalletSelectionList[selectedPalletIndex].SelectedPallet = true;
                        await _LinePalletSelectionGrid.Refresh();
                    }


                    break;

                case "selectall":

                    foreach (var line in PalletSelectionList)
                    {
                        int lineIndex = PalletSelectionList.IndexOf(line);
                        PalletSelectionList[lineIndex].SelectedPallet = true;
                    }

                    await _LinePalletSelectionGrid.Refresh();

                    break;

                case "remove":
                    var palletRemove = args.RowInfo.RowData;
                    int selectedRemovePalletIndex = PalletSelectionList.IndexOf(palletRemove);

                    PalletSelectionList[selectedRemovePalletIndex].SelectedPallet = false;

                    await _LinePalletSelectionGrid.Refresh();

                    break;

                default:
                    break;
            }
        }

        public async void OnTransferSelectedPalletButtonClicked()
        {
            PalletSelectionList = PalletSelectionList.OrderBy(t => t.PalletName).ToList();

            foreach (var pallet in PalletSelectionList)
            {
                if (pallet.SelectedPallet)
                {
                    #region Palet Line İşlemleri

                    int packageNo = 1;

                    SelectPackingListPalletLinesDto palletLineModel = new SelectPackingListPalletLinesDto
                    {
                        LineNr = GridLinePalletList.Count + 1,
                        PalletID = pallet.PalletID,
                        PalletName = pallet.PalletName.Split("-")[0],
                        NumberofPackage = pallet.NumberofPackage,
                        FirstPackageNo = packageNo.ToString(),
                        LastPackageNo = (packageNo + pallet.NumberofPackage - 1).ToString(),
                    };

                    packageNo += pallet.NumberofPackage;

                    GridLinePalletList.Add(palletLineModel);

                    #endregion

                    #region Palet Paket Line İşlemleri

                    var palletLineList = (await PalletRecordsAppService.GetAsync(pallet.PalletID)).Data.SelectPalletRecordLines;
                    decimal packageKG = 0;
                    var currentAccountDataSource = (await CurrentAccountCardsAppService.GetAsync(pallet.CurrentAccountCardID.GetValueOrDefault())).Data;

                    if (pallet.PackageType == L["BigPackage"].Value)
                    {
                        packageKG = currentAccountDataSource.BigPackageKG;
                    }
                    else if (pallet.PackageType == L["SmallPackage"].Value)
                    {
                        packageKG = currentAccountDataSource.SmallPackageKG;
                    }

                    foreach (var palletLine in palletLineList)
                    {
                        var unitKG = (await ProductsAppService.GetAsync(palletLine.ProductID.GetValueOrDefault())).Data.UnitWeight;

                        decimal onePackageNetKG = palletLine.PackageContent * unitKG;
                        decimal onePackageGrossKG = onePackageNetKG + packageKG;

                        SelectPackingListPalletPackageLinesDto palletPackageLineModel = new SelectPackingListPalletPackageLinesDto
                        {
                            PackageType = palletLine.PackageType,
                            ProductID = palletLine.ProductID,
                            ProductCode = palletLine.ProductCode,
                            ProductName = palletLine.ProductName,
                            CustomerCode = palletLine.CustomerCode,
                            CustomerID = palletLine.CurrentAccountCardID,
                            NumberofPackage = palletLine.NumberofPackage,
                            TotalAmount = palletLine.TotalAmount,
                            TotalGrossKG = palletLine.TotalGrossKG,
                            TotalNetKG = palletLine.TotalNetKG,
                            OnePackageGrossKG = onePackageGrossKG,
                            OnePackageNetKG = onePackageNetKG,
                            PackageContent = palletLine.PackageContent,
                            PackageNo = string.Empty
                        };
                    }


                    #endregion


                }
            }

            #region Palet Kübaj İşlemleri

            var palletList = PalletSelectionList.GroupBy(t => t.PackageType).Select(t => new { PackageType = t.Key, Pallet = t.ToList() }).ToList();

            foreach (var pallet in palletList)
            {
                int height = pallet.Pallet.Select(t => t.Height_).FirstOrDefault();
                int width = pallet.Pallet.Select(t => t.Width_).FirstOrDefault();
                int lenght = pallet.Pallet.Select(t => t.Length_).FirstOrDefault();

                SelectPackingListPalletCubageLinesDto palletCubageLineModel = new SelectPackingListPalletCubageLinesDto
                {
                    NumberofPallet = pallet.Pallet.Count,
                    Height_ = height,
                    Width_ = width,
                    Load_ = lenght,
                    Cubage = (height * width * lenght* pallet.Pallet.Count) / 1000000
                };

                GridLineCubageList.Add(palletCubageLineModel);
            }

            await _LineCubageGrid.Refresh();

            #endregion

            PalletSelectionList.Clear();

            ShowPalletsModal = false;
        }

        public async void PalletPackageLineContextMenuClick(ContextMenuClickEventArgs<SelectPackingListPalletPackageLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "enumarate":

                    if(GridLinePalletPackageList != null && GridLinePalletPackageList.Count > 0)
                    {
                        int packageNo = 1;

                        foreach(var line in GridLinePalletPackageList)
                        {
                            int lineIndex = GridLinePalletPackageList.IndexOf(line);
                            if(GridLinePalletPackageList[lineIndex].NumberofPackage != 1)
                            {
                                GridLinePalletPackageList[lineIndex].PackageNo = packageNo.ToString() + "-" + (packageNo + line.NumberofPackage - 1).ToString();
                            }
                            else
                            {
                                GridLinePalletPackageList[lineIndex].PackageNo = packageNo.ToString();
                            }
                            packageNo = packageNo + line.NumberofPackage ;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
