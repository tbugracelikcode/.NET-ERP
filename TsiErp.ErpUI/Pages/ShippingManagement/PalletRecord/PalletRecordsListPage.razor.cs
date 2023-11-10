using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.PalletRecord.Services;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ShippingManagement.PalletRecord
{
    public partial class PalletRecordsListPage
    {
        private SfGrid<SelectPalletRecordLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPalletRecordLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPalletRecordLinesDto> GridLineList = new List<SelectPalletRecordLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = PalletRecordsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Palet Kayıtları Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPalletRecordsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("PalletRecordsChildMenu")
            };

            DataSource.SelectPalletRecordLines = new List<SelectPalletRecordLinesDto>();
            GridLineList = DataSource.SelectPalletRecordLines;


            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

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

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextAddPackageFiche"], Id = "addpackagefiche" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordLinesContextRemovePackageFiche"], Id = "removepackagefiche" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PalletRecordsContextRefresh"], Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPalletRecordsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PalletRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPalletRecordLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPalletRecordLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "addpackagefiche":

                    if (DataSource.CurrentAccountCardID == null || DataSource.CurrentAccountCardID == Guid.Empty)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarninCurrentAccountTitle"], L["UIWarninCurrentAccountMessage"]);
                    }

                    else
                    {
                        LineDataSource = new SelectPalletRecordLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.CurrentAccountCardID = DataSource.CurrentAccountCardID;
                        LineDataSource.CustomerCode = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data.CustomerCode;
                        LineDataSource.LineNr = GridLineList.Count + 1;
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "removepackagefiche":

                    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectPalletRecordLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPalletRecordLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPalletRecordLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

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
                if (DataSource.SelectPalletRecordLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPalletRecordLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPalletRecordLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPalletRecordLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPalletRecordLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPalletRecordLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPalletRecordLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PalletRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

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
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
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
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                LineDataSource.ProductName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsButtonClickEvent);
            await CurrentAccountCardsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DataSource.CurrentAccountCardID = selectedCurrentAccountCard.Id;
                DataSource.CurrentAccountCardCode = selectedCurrentAccountCard.Code;
                DataSource.CurrentAccountCardName = selectedCurrentAccountCard.Name;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Koli Türü ComboBox
        public class PackageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<PackageTypeComboBox> _packageTypeComboBox = new List<PackageTypeComboBox>
        {
            new PackageTypeComboBox(){ID = "Big", Text="BigPackage"},
            new PackageTypeComboBox(){ID = "Small", Text="SmallPackage"}
        };

        private void PackageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, PackageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Big":
                        DataSource.PackageType = L["BigPackage"].Value;
                        break;

                    case "Small":
                        DataSource.PackageType = L["SmallPackage"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region Palet Adı ComboBox
        public class NameComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<NameComboBox> _nameComboBox = new List<NameComboBox>
        {
            new NameComboBox(){ID = "A-1"  , Text="A-1"  },
            new NameComboBox(){ID = "B-2"  , Text="B-2"  },
            new NameComboBox(){ID = "C-3"  , Text="C-3"  },
            new NameComboBox(){ID = "D-4"  , Text="D-4"  },
            new NameComboBox(){ID = "E-5"  , Text="E-5"  },
            new NameComboBox(){ID = "F-6"  , Text="F-6"  },
            new NameComboBox(){ID = "G-7"  , Text="G-7"  },
            new NameComboBox(){ID = "H-8"  , Text="H-8"  },
            new NameComboBox(){ID = "I-9"  , Text="I-9"  },
            new NameComboBox(){ID = "J-10" , Text="J-10" },
            new NameComboBox(){ID = "K-11" , Text="K-11" },
            new NameComboBox(){ID = "L-12" , Text="L-12" },
            new NameComboBox(){ID = "M-13" , Text="M-13" },
            new NameComboBox(){ID = "N-14" , Text="N-14" },
            new NameComboBox(){ID = "O-15" , Text="O-15" },
            new NameComboBox(){ID = "P-16" , Text="P-16" },
            new NameComboBox(){ID = "Q-17" , Text="Q-17" },
            new NameComboBox(){ID = "R-18" , Text="R-18" },
            new NameComboBox(){ID = "S-19" , Text="S-19" },
            new NameComboBox(){ID = "T-20" , Text="T-20" },
            new NameComboBox(){ID = "U-21" , Text="U-21" },
            new NameComboBox(){ID = "V-22" , Text="V-22" },
            new NameComboBox(){ID = "W-23" , Text="W-23" },
            new NameComboBox(){ID = "X-24" , Text="X-24" },
            new NameComboBox(){ID = "Y-25" , Text="Y-25" },
            new NameComboBox(){ID = "Z-26" , Text="Z-26" },
            new NameComboBox(){ID = "AA-27", Text="AA-27"},
            new NameComboBox(){ID = "BB-28", Text="BB-28"},
            new NameComboBox(){ID = "CC-29", Text="CC-29"},
            new NameComboBox(){ID = "DD-30", Text="DD-30"},
            new NameComboBox(){ID = "EE-31", Text="EE-31"},
            new NameComboBox(){ID = "FF-32", Text="FF-32"},
            new NameComboBox(){ID = "GG-33", Text="GG-33"},
            new NameComboBox(){ID = "HH-34", Text="HH-34"},
            new NameComboBox(){ID = "II-35", Text="II-35"},
            new NameComboBox(){ID = "JJ-36", Text="JJ-36"},
            new NameComboBox(){ID = "KK-37", Text="KK-37"},
            new NameComboBox(){ID = "LL-38", Text="LL-38"},
            new NameComboBox(){ID = "MM-39", Text="MM-39"},
            new NameComboBox(){ID = "NN-40", Text="NN-40"},
            new NameComboBox(){ID = "OO-41", Text="OO-41"},
            new NameComboBox(){ID = "PP-42", Text="PP-42"},
            new NameComboBox(){ID = "QQ-43", Text="QQ-43"},
            new NameComboBox(){ID = "RR-44", Text="RR-44"},
            new NameComboBox(){ID = "SS-45", Text="SS-45"},
            new NameComboBox(){ID = "TT-46", Text="TT-46"},
            new NameComboBox(){ID = "UU-47", Text="UU-47"},
            new NameComboBox(){ID = "VV-48", Text="VV-48"},
            new NameComboBox(){ID = "WW-49", Text="WW-49"},
            new NameComboBox(){ID = "XX-50", Text="XX-50"},
            new NameComboBox(){ID = "YY-51", Text="YY-51"},
            new NameComboBox(){ID = "ZZ-52", Text="ZZ-52"},
        };

        private void NameComboBoxValueChangeHandler(ChangeEventArgs<string, NameComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "A-1"  :DataSource.Name="A-1"  ; break;
                    case "B-2"  :DataSource.Name="B-2"  ; break;
                    case "C-3"  :DataSource.Name="C-3"  ; break;
                    case "D-4"  :DataSource.Name="D-4"  ; break;
                    case "E-5"  :DataSource.Name="E-5"  ; break;
                    case "F-6"  :DataSource.Name="F-6"  ; break;
                    case "G-7"  :DataSource.Name="G-7"  ; break;
                    case "H-8"  :DataSource.Name="H-8"  ; break;
                    case "I-9"  :DataSource.Name="I-9"  ; break;
                    case "J-10" :DataSource.Name="J-10" ; break;
                    case "K-11" :DataSource.Name="K-11" ; break;
                    case "L-12" :DataSource.Name="L-12" ; break;
                    case "M-13" :DataSource.Name="M-13" ; break;
                    case "N-14" :DataSource.Name="N-14" ; break;
                    case "O-15" :DataSource.Name="O-15" ; break;
                    case "P-16" :DataSource.Name="P-16" ; break;
                    case "Q-17" :DataSource.Name="Q-17" ; break;
                    case "R-18" :DataSource.Name="R-18" ; break;
                    case "S-19" :DataSource.Name="S-19" ; break;
                    case "T-20" :DataSource.Name="T-20" ; break;
                    case "U-21" :DataSource.Name="U-21" ; break;
                    case "V-22" :DataSource.Name="V-22" ; break;
                    case "W-23" :DataSource.Name="W-23" ; break;
                    case "X-24" :DataSource.Name="X-24" ; break;
                    case "Y-25" :DataSource.Name="Y-25" ; break;
                    case "Z-26" :DataSource.Name="Z-26" ; break;
                    case "AA-27":DataSource.Name="AA-27"; break;
                    case "BB-28":DataSource.Name="BB-28"; break;
                    case "CC-29":DataSource.Name="CC-29"; break;
                    case "DD-30":DataSource.Name="DD-30"; break;
                    case "EE-31":DataSource.Name="EE-31"; break;
                    case "FF-32":DataSource.Name="FF-32"; break;
                    case "GG-33":DataSource.Name="GG-33"; break;
                    case "HH-34":DataSource.Name="HH-34"; break;
                    case "II-35":DataSource.Name="II-35"; break;
                    case "JJ-36":DataSource.Name="JJ-36"; break;
                    case "KK-37":DataSource.Name="KK-37"; break;
                    case "LL-38":DataSource.Name="LL-38"; break;
                    case "MM-39":DataSource.Name="MM-39"; break;
                    case "NN-40":DataSource.Name="NN-40"; break;
                    case "OO-41":DataSource.Name="OO-41"; break;
                    case "PP-42":DataSource.Name="PP-42"; break;
                    case "QQ-43":DataSource.Name="QQ-43"; break;
                    case "RR-44":DataSource.Name="RR-44"; break;
                    case "SS-45":DataSource.Name="SS-45"; break;
                    case "TT-46":DataSource.Name="TT-46"; break;
                    case "UU-47":DataSource.Name="UU-47"; break;
                    case "VV-48":DataSource.Name="VV-48"; break;
                    case "WW-49":DataSource.Name="WW-49"; break;
                    case "XX-50":DataSource.Name="XX-50"; break;
                    case "YY-51":DataSource.Name="YY-51"; break;
                    case "ZZ-52":DataSource.Name= "ZZ-52"; break;


                    default: break;
                }
            }
        }

        #endregion
    }
}
