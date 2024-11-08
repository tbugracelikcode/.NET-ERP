using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.PivotView;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.FinanceManagement.CashFlowPlan
{
    public partial class CashFlowPlansListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        SfPivotView<SelectCashFlowPlanLinesDto> _CashFlowPlanLinePivot;
        SfGrid<SelectCashFlowPlanLinesDto> _DetailedLineGrid;

        SelectCashFlowPlanLinesDto DetailedLineDataSource;

        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> DetailedLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectCashFlowPlanLinesDto> GridLineList = new List<SelectCashFlowPlanLinesDto>();

        List<SelectCashFlowPlanLinesDto> GridDetailedLineList = new List<SelectCashFlowPlanLinesDto>();

        public bool DetailedCashFlowPopupVisible = false;
        public bool DetailedCashFlowCrudPopupVisible = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = CashFlowPlansService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CashFlowPlansChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Nakit Akış Planı Satır İşlemleri

        protected override async Task BeforeInsertAsync()
        {

            var now = GetSQLDateAppService.GetDateFromSQL();

            DataSource = new SelectCashFlowPlansDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("CashFlowPlansChildMenu"),
                StartDate = now.Date,
                EndDate = now.Date,
            };

            int thisyear = GetSQLDateAppService.GetDateFromSQL().Date.Year;

            DateTime MinDay = new DateTime(thisyear, 1, 1);

            for (int i = 1; i <= 365; i++)
            {
                if (MinDay.Year == thisyear)
                {
                    if (MinDay.DayOfWeek != DayOfWeek.Sunday && MinDay.DayOfWeek != DayOfWeek.Saturday) // Haftasonu değilse
                    {
                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncome = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "TL",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "AKBANK T.A.Ş.",
                            BankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncomeUSD = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "USD",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "AKBANK T.A.Ş.",
                            BankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncome2 = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 62500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "TL",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            BankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncome2USD = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 62500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "USD",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            BankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 55000,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "TL",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "AKBANK T.A.Ş.",
                            BankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditureUSD = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 55000,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "USD",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "AKBANK T.A.Ş.",
                            BankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure2 = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 1500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "TL",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            BankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure2USD = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 1500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = "USD",
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            LineNr = GridLineList.Count + 1,
                            BankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            BankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        GridLineList.Add(cashFlowPlanLineModelIncome);
                        GridLineList.Add(cashFlowPlanLineModelIncomeUSD);
                        GridLineList.Add(cashFlowPlanLineModelIncome2);
                        GridLineList.Add(cashFlowPlanLineModelIncome2USD);
                        GridLineList.Add(cashFlowPlanLineModelExpenditure);
                        GridLineList.Add(cashFlowPlanLineModelExpenditureUSD);
                        GridLineList.Add(cashFlowPlanLineModelExpenditure2);
                        GridLineList.Add(cashFlowPlanLineModelExpenditure2USD);
                    }

                    MinDay = MinDay.AddDays(1);

                }
            }

            DataSource.SelectCashFlowPlanLines = GridLineList;

            #region Enum Combobox Localization

            foreach (var item in balancetypes)
            {
                item.CashFlowPlansBalanceTypeName = L[item.CashFlowPlansBalanceTypeName];
            }

            foreach(var item in transactiontypes)
            {
                item.CashFlowPlansTransactionTypeName = L[item.CashFlowPlansTransactionTypeName];
            }

            #endregion

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

                    #region Enum Combobox Localization

                    foreach (var item in balancetypes)
                    {
                        item.CashFlowPlansBalanceTypeName = L[item.CashFlowPlansBalanceTypeName];
                    }

                    foreach (var item in transactiontypes)
                    {
                        item.CashFlowPlansTransactionTypeName = L[item.CashFlowPlansTransactionTypeName];
                    }

                    #endregion

                    EditPageVisible = true;

                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void CreateMainContextMenuItems()
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
                            case "CashFlowPlansContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextAdd"], Id = "new" }); break;
                            case "CashFlowPlansContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextChange"], Id = "changed" }); break;
                            case "CashFlowPlansContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextDelete"], Id = "delete" }); break;
                            case "CashFlowPlansContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (DetailedLineGridContextMenu.Count() == 0)
            {
                //LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDelete"], Id = "delete" });

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            //case "CashFlowPlanLinesContextAdd":
                            //    DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextAdd"], Id = "new" }); break;
                            case "CashFlowPlanLinesContextIncomeAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextIncomeAdd"], Id = "incomenew" }); break;
                            case "CashFlowPlanLinesContextPaymentAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextPaymentAdd"], Id = "paymentnew" }); break;
                            case "CashFlowPlanLinesContextTransferAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextTransferAdd"], Id = "transfernew" }); break;
                            case "CashFlowPlanLinesContextExchangePurchaseAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExchangePurchaseAdd"], Id = "expurchasenew" }); break;
                            case "CashFlowPlanLinesContextExchangeSalesAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExchangeSalesAdd"], Id = "salesnew" }); break;
                            case "CashFlowPlanLinesContextExpenseAmountAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExpenseAmountAdd"], Id = "expensenew" }); break;
                            case "CashFlowPlanLinesContextChange":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextChange"], Id = "changed" }); break;
                            case "CashFlowPlanLinesContextDelete":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextDelete"], Id = "delete" }); break;
                            case "CashFlowPlanLinesContextRefresh":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListCashFlowPlansDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                        DataSource = (await CashFlowPlansService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectCashFlowPlanLines;



                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
                        if (res == true)
                        {
                            await DeleteAsync(args.RowInfo.RowData.Id);
                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }


        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectCashFlowPlanLinesDto> args)
        {
            switch (args.Item.Id)
            {
                //case "new":

                //    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                //    DetailedCashFlowCrudPopupVisible = true;
                //    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                //    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                //    await InvokeAsync(StateHasChanged);
                //    break;
                case "incomenew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.GelenOdeme;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "paymentnew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.YapilacakOdeme;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "transfernew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.BankaHesaplarArasiVirman;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "expurchasenew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.DovizAlis;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "salesnew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.DovizSatis;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "expensenew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.MasrafTutari;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;



                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        DetailedLineDataSource = args.RowInfo.RowData;
                        DetailedCashFlowCrudPopupVisible = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                DataSource.SelectCashFlowPlanLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectCashFlowPlanLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectCashFlowPlanLines.Remove(line);
                                }
                            }

                            await _DetailedLineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await _DetailedLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        private async void PivotCellClick(CellClickEventArgs args)
        {
            if (args.Data != null)
            {
                if (args.Data.Axis == "row")
                {
                    return;
                }

                if (args.Data.Axis == "column")
                {
                    return;
                }

                if (args.Data.Axis == "value" && args.Data.RowHeaders == "Grand Total")
                {
                    return;
                }

                if (args.Data.Axis == "value" && args.Data.ColumnHeaders == "Grand Total")
                {
                    return;
                }


                GridDetailedLineList.Clear();


                string DateStr = args.Data.RowHeaders.ToString();

                GridDetailedLineList = GridLineList.Where(t => t.Date_ == DateTime.Parse(DateStr)).ToList();

                DetailedCashFlowPopupVisible = true;

            }
        }



        public void HideDetailedPopup()
        {
            GridDetailedLineList.Clear();
            DetailedCashFlowPopupVisible = false;
        }

        public void HideDetailedCrudPopup()
        {
            DetailedCashFlowCrudPopupVisible = false;
        }

        protected async Task OnLineSubmit()
        {

            if (DetailedLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCashFlowPlanLines.Contains(DetailedLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCashFlowPlanLines.FindIndex(t => t.LineNr == DetailedLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCashFlowPlanLines[selectedLineIndex] = DetailedLineDataSource;
                    }
                }

                else
                {
                    DataSource.SelectCashFlowPlanLines.Add(DetailedLineDataSource);
                }

            }

            else
            {
                int selectedLineIndex = DataSource.SelectCashFlowPlanLines.FindIndex(t => t.Id == DetailedLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCashFlowPlanLines[selectedLineIndex] = DetailedLineDataSource;
                }
            }

            GridLineList = DataSource.SelectCashFlowPlanLines;

            GridDetailedLineList = GridLineList.Where(t => t.Date_ == DetailedLineDataSource.Date_).ToList();

            await _DetailedLineGrid.Refresh();

            await _CashFlowPlanLinePivot.LayoutRefreshAsync();

            await InvokeAsync(StateHasChanged);

            HideDetailedCrudPopup();

        }

        #endregion

        #region ButtonEdit Metotları

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
                DetailedLineDataSource.CurrentAccountID = Guid.Empty;
                DetailedLineDataSource.CurrentAccountName = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DetailedLineDataSource.CurrentAccountID = selectedCurrentAccountCard.Id;
                DetailedLineDataSource.CurrentAccountName = selectedCurrentAccountCard.Name;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Banka ButtonEdit

        SfTextBox BankAccountsButtonEdit;
        bool SelectBankAccountsPopupVisible = false;
        List<ListBankAccountsDto> BankAccountsList = new List<ListBankAccountsDto>();

        public async Task BankAccountOnCreateIcon()
        {
            var BankAccountButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BankAccountsButtonClickEvent);
            await BankAccountsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BankAccountButtonClick } });
        }

        public async void BankAccountsButtonClickEvent()
        {
            SelectBankAccountsPopupVisible = true;
            BankAccountsList = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void BankAccountsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DetailedLineDataSource.BankAccountID = Guid.Empty;
                DetailedLineDataSource.BankAccountName = string.Empty;
                DetailedLineDataSource.CurrencyID = Guid.Empty;
                DetailedLineDataSource.CurrencyCode = string.Empty;
            }
        }

        public async void BankAccountsDoubleClickHandler(RecordDoubleClickEventArgs<ListBankAccountsDto> args)
        {
            var selectedBankAccount = args.RowData;

            if (selectedBankAccount != null)
            {

                DetailedLineDataSource.BankAccountID = selectedBankAccount.Id;
                DetailedLineDataSource.BankAccountName = selectedBankAccount.Name;
                DetailedLineDataSource.CurrencyID = selectedBankAccount.CurrencyID;
                DetailedLineDataSource.CurrencyCode = selectedBankAccount.CurrencyCode;

                SelectBankAccountsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        #region Enum ComboBox İşlemleri

        #region Bakiye Türü ComboBox

        public IEnumerable<SelectCashFlowPlanLinesDto> balancetypes = GetEnumDisplayBalanceTypesNames<CashFlowPlansBalanceTypeEnum>();

        public static List<SelectCashFlowPlanLinesDto> GetEnumDisplayBalanceTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<CashFlowPlansBalanceTypeEnum>()
                       .Select(x => new SelectCashFlowPlanLinesDto
                       {
                           CashFlowPlansBalanceType = x,
                           CashFlowPlansBalanceTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #region İşlem Türü ComboBox

        public IEnumerable<SelectCashFlowPlanLinesDto> transactiontypes = GetEnumDisplayTransactionTypesNames<CashFlowPlansTransactionTypeEnum>();

        public static List<SelectCashFlowPlanLinesDto> GetEnumDisplayTransactionTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<CashFlowPlansTransactionTypeEnum>()
                       .Select(x => new SelectCashFlowPlanLinesDto
                       {
                           CashFlowPlansTransactionType = x,
                           CashFlowPlansTransactionTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CashFlowPlansChildMenu");
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
