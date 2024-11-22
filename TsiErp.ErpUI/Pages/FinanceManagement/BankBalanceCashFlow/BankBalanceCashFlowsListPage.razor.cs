using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.CodeAnalysis;
using Microsoft.SqlServer.Management.Smo;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.PivotView;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.BankAccount.Services;
using TsiErp.Business.Entities.BankBalance.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.FinanceManagement.CashFlowPlan.CashFlowPlansListPage;

namespace TsiErp.ErpUI.Pages.FinanceManagement.BankBalanceCashFlow
{
    public partial class BankBalanceCashFlowsListPage : IDisposable
    {
        #region Yetki Listeleri

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #endregion

        #region Grid Tanımlamaları

        SfGrid<SelectBankBalanceCashFlowLinesLinesDto> _DetailedLineGrid;
        SfGrid<SelectBankBalanceCashFlowLinesDto> _LineGrid;
        SfGrid<TotalbyMonths> _TotalbyMonthsGrid;

        #endregion

        #region DataSource Tanımlamaları

        SelectBankBalanceCashFlowLinesLinesDto DetailedLineDataSource;
        SelectBankBalanceCashFlowLinesDto LineDataSource;
        TotalbyMonths TotalbyMonthsDataSource;
        TotalbyMonthsDetail TotalbyMonthsDetailDataSource;

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }
        [Inject]
        SpinnerService Spinner { get; set; }

        #region Listeler ve Context Menüler
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> DetailedLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> TotalMonthsGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectBankBalanceCashFlowLinesDto> GridLineList = new List<SelectBankBalanceCashFlowLinesDto>();

        List<SelectBankBalanceCashFlowLinesLinesDto> GridDetailedLineList = new List<SelectBankBalanceCashFlowLinesLinesDto>();

        List<DateTime> BankBalancesLastDatesList = new List<DateTime>();

        List<TotalbyMonths> TotalbyMonthsList = new List<TotalbyMonths>();

        #endregion

        #region Değişkenler

        public bool DetailedCashFlowPopupVisible = false;
        public bool DetailedCashFlowCrudPopupVisible = false;
        public bool IncomePaymentExpensePopupVisible = false;
        public bool CreditPaymentPopupVisible = false;
        public bool TransferPopupVisible = false;
        public string TransactionPopupTitle = string.Empty;
        string DetailedGridPopupTitle = string.Empty;
        public bool ExchangeSalesPopupVisible = false;
        decimal BankTL = 0;
        string CurrencyStr = string.Empty;
        string BankTLName = string.Empty;
        Guid? RecieverBankID = Guid.Empty;
        string RecieverBankName = string.Empty;
        Guid? RecieverBankCurrencyID = Guid.Empty;
        string RecieverBankCurrencyCode = string.Empty;
        bool TotalsbyMonthsModalVisible = false;
        Guid? SelectedBankID = Guid.Empty;
        public bool TotalbyMonthsDetailPopupVisible = false;
        DateTime MaxRecurrentDate = DateTime.Now;
        bool RecurrentTimeEnabled = false;
        string TotalbyMonthsTitle = string.Empty;

        #endregion

        public class TotalbyMonths
        {
            public string MonthYear { get; set; }
            public decimal AmountAkbankTL { get; set; }
            public decimal AmountAkbankEUR { get; set; }
            public decimal AmountIsBankTL { get; set; }
            public decimal AmountIsBankEUR { get; set; }
        }

        public class TotalbyMonthsDetail
        {
            public string MonthYear { get; set; }
            public decimal AmountTL { get; set; }
            public decimal AmountEUR { get; set; }
            public decimal AmountUSD { get; set; }
            public decimal ExchangetUSD { get; set; }
            public decimal ExchangetEUR { get; set; }
            public decimal GrandTotalTL { get; set; }
        }

        protected override async void OnInitialized()
        {
            //Spinner.Show();
            //await Task.Delay(100);

            BaseCrudService = BankBalanceCashFlowsAppService;
            _L = L;

            int thisyear = GetSQLDateAppService.GetDateFromSQL().Date.Year;
            MaxRecurrentDate = new DateTime(thisyear, 12, 31);

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "BankBalanceCashFlowPlansChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateLinesLineContextMenuItems();
            CreateTotalbyMonthsContextMenuItems();

        }

        protected override async Task BeforeInsertAsync()
        {

            int thisyear = GetSQLDateAppService.GetDateFromSQL().Date.Year;

            DateTime MinDay = new DateTime(thisyear, 1, 1);

            DataSource = new SelectBankBalanceCashFlowsDto()
            {
                Year_ = thisyear
            };

            GridLineList = new List<SelectBankBalanceCashFlowLinesDto>();
            GridDetailedLineList = new List<SelectBankBalanceCashFlowLinesLinesDto>();

            for (int i = 1; i <= 365; i++)
            {
                if (MinDay.Year == thisyear)
                {

                    string monthyear = string.Empty;

                    switch (MinDay.Date.Month)
                    {
                        case 1: monthyear = L["1Month"]; break;
                        case 2: monthyear = L["2Month"]; break;
                        case 3: monthyear = L["3Month"]; break;
                        case 4: monthyear = L["4Month"]; break;
                        case 5: monthyear = L["5Month"]; break;
                        case 6: monthyear = L["6Month"]; break;
                        case 7: monthyear = L["7Month"]; break;
                        case 8: monthyear = L["8Month"]; break;
                        case 9: monthyear = L["9Month"]; break;
                        case 10: monthyear = L["10Month"]; break;
                        case 11: monthyear = L["11Month"]; break;
                        case 12: monthyear = L["12Month"]; break;
                        default: break;
                    }

                    SelectBankBalanceCashFlowLinesDto bankBalance = new SelectBankBalanceCashFlowLinesDto
                    {
                        Date_ = MinDay.Date,
                        Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate(),
                        AmountIsBankTL = 0,
                        AmountIsBankEUR = 0,
                        AmountAkbankTL = 0,
                        AmountAkbankEUR = 0,
                        MonthYear = monthyear + " " + MinDay.Date.Year.ToString(),

                    };

                    GridLineList.Add(bankBalance);

                    MinDay = MinDay.AddDays(1);

                }
            }

            DataSource.SelectBankBalanceCashFlowLinesDto = GridLineList;

            var groupedBankBalanceList = GridLineList.GroupBy(t => t.MonthYear).ToList();

            foreach (var group in groupedBankBalanceList)
            {
                DateTime lastDate = GridLineList.Where(t => t.MonthYear == group.Key).Select(t => t.Date_).LastOrDefault();

                BankBalancesLastDatesList.Add(lastDate);
            }

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


            await Task.CompletedTask;
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count == 0)
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
            if (LineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "CashFlowPlanLinesContextTotalbyMonths":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextTotalbyMonths"], Id = "totalbymonths" }); break;
                            case "CashFlowPlanLinesContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextChange"], Id = "changed" }); break;
                            case "CashFlowPlanLinesContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlansContextRefresh"], Id = "refresh" }); break;

                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateLinesLineContextMenuItems()
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
                            case "CashFlowPlanLinesContextIncomeAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextIncomeAdd"], Id = "incomenew" }); break;
                            case "CashFlowPlanLinesContextPaymentAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextPaymentAdd"], Id = "paymentnew" }); break;
                            case "CashFlowPlanLinesContextTransferAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextTransferAdd"], Id = "transfernew" }); break;
                            case "CashFlowPlanLinesContextExchangeSalesAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExchangeSalesAdd"], Id = "salesnew" }); break;
                            case "CashFlowPlanLinesContextExpenseAmountAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExpenseAmountAdd"], Id = "expensenew" }); break;
                            case "CashFlowPlanLinesContextCreditPaymentAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextCreditPaymentAdd"], Id = "creditpayment" }); break;
                            case "CashFlowPlanLineLinesContextChange":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextChange"], Id = "changed" }); break;
                            case "CashFlowPlanLinesContextDelete":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextDelete"], Id = "delete" }); break;
                            case "CashFlowPlanLineLinesContextRefresh":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateTotalbyMonthsContextMenuItems()
        {
            if (TotalMonthsGridContextMenu.Count() == 0)
            {
                TotalMonthsGridContextMenu.Add(new ContextMenuItemModel { Text = L["TotalbyMonthsContextBalanceAnalysis"], Id = "balanceanalysis" });
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListBankBalanceCashFlowsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        Spinner.Show();
                        await Task.Delay(100);

                        IsChanged = true;
                        DataSource = (await BankBalanceCashFlowsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectBankBalanceCashFlowLinesDto;

                        var groupedBankBalanceList = GridLineList.GroupBy(t => t.MonthYear).ToList();

                        foreach (var group in groupedBankBalanceList)
                        {
                            DateTime lastDate = GridLineList.Where(t => t.MonthYear == group.Key).Select(t => t.Date_).LastOrDefault();

                            BankBalancesLastDatesList.Add(lastDate);
                        }

                        ShowEditPage();

                        Spinner.Hide();

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectBankBalanceCashFlowLinesDto> args)
        {
            switch (args.Item.Id)
            {

                case "changed":

                    LineDataSource = DataSource.SelectBankBalanceCashFlowLinesDto.Where(t => t.Id == args.RowInfo.RowData.Id).FirstOrDefault();

                    string header = args.Column.HeaderText;

                    string DateStr = LineDataSource.Date_.ToShortDateString();
                    string BankStr = header;

                    ListBankAccountsDto bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == header).FirstOrDefault();

                    SelectedBankID = bank.Id;

                    DetailedGridPopupTitle = DateStr + " > " + BankStr;

                    if (LineDataSource.SelectBankBalanceCashFlowLinesLines == null)
                    {
                        LineDataSource.SelectBankBalanceCashFlowLinesLines = new List<SelectBankBalanceCashFlowLinesLinesDto>();
                    }

                    GridDetailedLineList = LineDataSource.SelectBankBalanceCashFlowLinesLines.Where(t => t.Date_ == LineDataSource.Date_ && t.CurrencyID == bank.CurrencyID && t.BankAccountID == SelectedBankID).ToList();

                    DetailedCashFlowPopupVisible = true;

                    break;

                case "totalbymonths":

                    TotalbyMonthsList.Clear();

                    var groupedBankBalanceList = DataSource.SelectBankBalanceCashFlowLinesDto.GroupBy(t => t.MonthYear).ToList();

                    foreach (var item in groupedBankBalanceList)
                    {
                        var lastBankBalance = DataSource.SelectBankBalanceCashFlowLinesDto.Where(t => t.MonthYear == item.Key).LastOrDefault();

                        TotalbyMonths totalbyMonthModel = new TotalbyMonths
                        {
                            MonthYear = item.Select(t => t.MonthYear).FirstOrDefault(),
                            AmountAkbankEUR = lastBankBalance.AmountAkbankEUR,
                            AmountAkbankTL = lastBankBalance.AmountAkbankTL,
                            AmountIsBankEUR = lastBankBalance.AmountIsBankEUR,
                            AmountIsBankTL = lastBankBalance.AmountIsBankTL,
                        };

                        TotalbyMonthsList.Add(totalbyMonthModel);
                    }

                    TotalsbyMonthsModalVisible = true;

                    break;

                case "refresh":

                    await _LineGrid.Refresh();
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

        public async void TotalbyMonthsContextMenuClick(ContextMenuClickEventArgs<TotalbyMonths> args)
        {
            switch (args.Item.Id)
            {

                case "balanceanalysis":

                    TotalbyMonthsDataSource = args.RowInfo.RowData;

                    TotalbyMonthsDetailDataSource = new TotalbyMonthsDetail
                    {
                        AmountEUR = TotalbyMonthsDataSource.AmountAkbankEUR + TotalbyMonthsDataSource.AmountIsBankEUR,
                        AmountTL = TotalbyMonthsDataSource.AmountAkbankTL + TotalbyMonthsDataSource.AmountIsBankTL,
                        AmountUSD = 0,
                        ExchangetEUR = 0,
                        ExchangetUSD = 0,
                        GrandTotalTL = 0,
                        MonthYear = TotalbyMonthsDataSource.MonthYear
                    };

                    TotalbyMonthsTitle = TotalbyMonthsDetailDataSource.MonthYear;

                    TotalbyMonthsDetailPopupVisible = true;

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

        public async void OnLinesLineContextMenuClick(ContextMenuClickEventArgs<SelectBankBalanceCashFlowLinesLinesDto> args)
        {
            SelectBankAccountsDto bank = new SelectBankAccountsDto();

            switch (args.Item.Id)
            {
                #region Gelen Ödeme
                case "incomenew":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.GelenOdeme;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GelenOdeme;
                    TransactionPopupTitle = L["TransactionTypeIncome"];
                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    RecurrentTimeEnabled = false;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;
                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;
                    CurrencyStr = bank.CurrencyCode;

                    #endregion

                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Yapılacak Ödeme
                case "paymentnew":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.YapilacakOdeme;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    TransactionPopupTitle = L["TransactionTypePayment"];
                    RecurrentTimeEnabled = false;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    CurrencyStr = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Bankalar Arası Virman
                case "transfernew":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    TransferPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.BankaHesaplarArasiVirman;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    RecurrentTimeEnabled = false;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    CurrencyStr = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    RecieverBankID = Guid.Empty;
                    RecieverBankName = string.Empty;
                    RecieverBankCurrencyID = Guid.Empty;
                    RecieverBankCurrencyCode = string.Empty;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Döviz Alış Yorum
                //case "expurchasenew":

                //    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                //    DetailedCashFlowCrudPopupVisible = true;
                //    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.DovizAlis;
                //    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                //    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                //    await InvokeAsync(StateHasChanged);
                //    break; 
                #endregion

                #region Döviz Satış
                case "salesnew":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    ExchangeSalesPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.DovizSatis;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    RecurrentTimeEnabled = false;
                    BankTL = 0;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    CurrencyStr = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    int removeStartIndex = bank.Name.Length - 3;

                    string bankName = bank.Name.Remove(removeStartIndex) + "TRY";

                    BankTLName = bankName;

                    #endregion

                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Masraf Tutarı
                case "expensenew":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.MasrafTutari;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    TransactionPopupTitle = L["TransactionTypeExpenseAmount"];
                    RecurrentTimeEnabled = false;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    CurrencyStr = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.TransactionDescription = L["TransactionTypeExpense"];
                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Kredi Ödemesi
                case "creditpayment":

                    DetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto();
                    CreditPaymentPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.KrediOdemesi;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    TransactionPopupTitle = L["TransactionTypeCreditPayment"];
                    RecurrentTimeEnabled = false;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetAsync(SelectedBankID.GetValueOrDefault())).Data;

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    CurrencyStr = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.Date_ = LineDataSource.Date_;
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                #region Değiştir
                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        DetailedLineDataSource = args.RowInfo.RowData;

                        CashFlowPlansTransactionTypeEnum transactionType = args.RowInfo.RowData.CashFlowPlansTransactionType;

                        BankTL = DetailedLineDataSource.ExchangeAmount_ * DetailedLineDataSource.Amount_;

                        switch (transactionType)
                        {
                            case CashFlowPlansTransactionTypeEnum.GelenOdeme: IncomePaymentExpensePopupVisible = true; break;
                            case CashFlowPlansTransactionTypeEnum.YapilacakOdeme: IncomePaymentExpensePopupVisible = true; break;
                            case CashFlowPlansTransactionTypeEnum.MasrafTutari: IncomePaymentExpensePopupVisible = true; break;
                            case CashFlowPlansTransactionTypeEnum.DovizSatis: ExchangeSalesPopupVisible = true; break;
                            case CashFlowPlansTransactionTypeEnum.BankaHesaplarArasiVirman: TransferPopupVisible = true; break;
                            default: break;
                        }



                        await InvokeAsync(StateHasChanged);
                    }
                    break;
                #endregion

                #region Sil
                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= line.Date_))
                                {
                                    switch (line.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += line.Amount_; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += line.Amount_; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += line.Amount_; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += line.Amount_; break;
                                    }
                                }
                            }
                            else if (line.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= line.Date_))
                                {
                                    switch (line.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= line.Amount_; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= line.Amount_; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= line.Amount_; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= line.Amount_; break;
                                    }
                                }
                            }

                            await _LineGrid.Refresh();

                            if (line.Id == Guid.Empty)
                            {
                                LineDataSource.SelectBankBalanceCashFlowLinesLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await BankBalanceCashFlowsAppService.DeleteLinesLineAsync(args.RowInfo.RowData.Id);
                                    LineDataSource.SelectBankBalanceCashFlowLinesLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    LineDataSource.SelectBankBalanceCashFlowLinesLines.Remove(line);
                                }
                            }



                            await _DetailedLineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;
                #endregion

                #region Güncelle
                case "refresh":
                    await _DetailedLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;
                #endregion

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public void CalculateTotalbyMonthsDetail()
        {
            TotalbyMonthsDetailDataSource.GrandTotalTL = TotalbyMonthsDetailDataSource.AmountTL + (TotalbyMonthsDetailDataSource.AmountEUR * TotalbyMonthsDetailDataSource.ExchangetEUR) + (TotalbyMonthsDetailDataSource.AmountUSD * TotalbyMonthsDetailDataSource.ExchangetUSD);
        }

        public void HideTotalbyMonthsDetailPopup()
        {
            TotalbyMonthsDetailPopupVisible = false;
        }

        public async Task BankBalanceCashFlowLineOnSubmit()
        {
            Spinner.Show();
            await Task.Delay(100);

            if (DetailedLineDataSource.Id == Guid.Empty)
            {
                DetailedLineDataSource.Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate();
                DetailedLineDataSource.BankAccountID = SelectedBankID;



                if (LineDataSource.SelectBankBalanceCashFlowLinesLines.Contains(DetailedLineDataSource))
                {
                    int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.LineNr == DetailedLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                    }
                }
                else
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines.Add(DetailedLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.Id == DetailedLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                }
            }

            GridDetailedLineList = LineDataSource.SelectBankBalanceCashFlowLinesLines.Where(t => t.Date_ == LineDataSource.Date_ && t.CurrencyID == DetailedLineDataSource.CurrencyID && t.BankAccountID == SelectedBankID).ToList();

            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
            {
                foreach (var item in GridLineList.Where(t => t.Date_ >= DetailedLineDataSource.Date_))
                {
                    switch (DetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += DetailedLineDataSource.Amount_; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += DetailedLineDataSource.Amount_; break;
                    }


                }
            }
            else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
            {
                foreach (var item in GridLineList.Where(t => t.Date_ >= DetailedLineDataSource.Date_))
                {
                    switch (DetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                    }
                }
            }

            if (DetailedLineDataSource.isRecurrent)
            {
                List<DateTime> PeriodicDates = new List<DateTime>();
                DateTime addedDate = DetailedLineDataSource.Date_;

                for (int i = 0; i <= 12; i++)
                {
                    addedDate = addedDate.AddDays(30);

                    if (addedDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        addedDate = addedDate.AddDays(-2);
                    }
                    else if (addedDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        addedDate = addedDate.AddDays(-1);
                    }

                    if (addedDate < DetailedLineDataSource.RecurrentEndTime)
                    {
                        PeriodicDates.Add(addedDate);
                    }
                }

                foreach (DateTime date in PeriodicDates)
                {

                    SelectBankBalanceCashFlowLinesDto SelectedLine = GridLineList.Where(t => t.Date_ == date).FirstOrDefault();

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines == null)
                    {
                        SelectedLine.SelectBankBalanceCashFlowLinesLines = new List<SelectBankBalanceCashFlowLinesLinesDto>();
                    }

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).Count() > 0)
                    {
                        var SelectedLinesLine = SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).FirstOrDefault();

                        int index = SelectedLine.SelectBankBalanceCashFlowLinesLines.IndexOf(SelectedLinesLine);

                        decimal diff = DetailedLineDataSource.Amount_ - SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_;

                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_ = DetailedLineDataSource.Amount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountID = DetailedLineDataSource.BankAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountName = DetailedLineDataSource.BankAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyID = DetailedLineDataSource.CurrencyID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyCode = DetailedLineDataSource.CurrencyCode;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountID = DetailedLineDataSource.CurrentAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountName = DetailedLineDataSource.CurrentAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].TransactionDescription = DetailedLineDataSource.TransactionDescription;

                        if (diff != 0)
                        {
                            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += diff; break;
                                    }


                                }
                            }
                            else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= diff; break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectBankBalanceCashFlowLinesLinesDto addedLinesLineModel = new SelectBankBalanceCashFlowLinesLinesDto
                        {
                            Date_ = date,
                            Amount_ = DetailedLineDataSource.Amount_,
                            BankAccountID = DetailedLineDataSource.BankAccountID,
                            BankAccountName = DetailedLineDataSource.BankAccountName,
                            BankBalanceCashFlowID = DetailedLineDataSource.BankBalanceCashFlowID,
                            BankBalanceCashFlowLineID = GridLineList.Where(t => t.Date_ == date).Select(t => t.Id).FirstOrDefault(),
                            CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType,
                            RecurrentEndTime = null,
                            LinkedBankBalanceCashFlowLinesLineID = DetailedLineDataSource.Id,
                            CurrencyID = DetailedLineDataSource.CurrencyID,
                            CurrencyCode = DetailedLineDataSource.CurrencyCode,
                            CurrentAccountID = DetailedLineDataSource.CurrentAccountID,
                            CurrentAccountName = DetailedLineDataSource.CurrentAccountName,
                            ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_,
                            isRecurrent = false,
                            LineNr = SelectedLine.SelectBankBalanceCashFlowLinesLines.Count + 1,
                            TransactionDescription = DetailedLineDataSource.TransactionDescription,

                        };

                        SelectedLine.SelectBankBalanceCashFlowLinesLines.Add(addedLinesLineModel);

                        if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL += DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += DetailedLineDataSource.Amount_; break;
                                }


                            }
                        }
                        else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                                }
                            }
                        }
                    }



                }
            }





            await _LineGrid.Refresh();

            await _DetailedLineGrid.Refresh();


            HideDetailedCrudPopup();

            Spinner.Hide();

            await InvokeAsync(StateHasChanged);
        }

        public async Task BankBalanceCashFlowLineOnExchangeSalesSubmit()
        {
            Spinner.Show();
            await Task.Delay(100);

            if (DetailedLineDataSource.Id == Guid.Empty)
            {

                DetailedLineDataSource.Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate();
                DetailedLineDataSource.BankAccountID = SelectedBankID;

                if (LineDataSource.SelectBankBalanceCashFlowLinesLines.Contains(DetailedLineDataSource))
                {
                    int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.LineNr == DetailedLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                    }
                }
                else
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines.Add(DetailedLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.Id == DetailedLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                }
            }


            int thisyear = GetSQLDateAppService.GetDateFromSQL().Date.Year;

            int removeStartIndex = DetailedLineDataSource.BankAccountName.Length - 3;

            string bankName = DetailedLineDataSource.BankAccountName.Remove(removeStartIndex) + "TRY";

            ListBankAccountsDto bankTl = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == bankName).FirstOrDefault();

            SelectBankBalanceCashFlowLinesLinesDto TLDetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto
            {
                Amount_ = BankTL,
                Date_ = DetailedLineDataSource.Date_,
                ExchangeAmount_ = 0,
                BankAccountID = bankTl.Id,
                CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GelenOdeme,
                CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.GelenOdeme,
                CurrencyID = bankTl.CurrencyID,
                CurrentAccountID = Guid.Empty,
                TransactionDescription = string.Empty,
                LineNr = GridLineList.Count + 1,
                BankAccountName = bankTl.Name,
                CurrencyCode = bankTl.CurrencyCode,
                LinkedBankBalanceCashFlowLinesLineID = Guid.Empty,
                isRecurrent = DetailedLineDataSource.isRecurrent,
                Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate(),
                RecurrentEndTime = DetailedLineDataSource.RecurrentEndTime
            };

            LineDataSource.SelectBankBalanceCashFlowLinesLines.Add(TLDetailedLineDataSource);



            ListBankAccountsDto bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Id == DetailedLineDataSource.BankAccountID.GetValueOrDefault()).FirstOrDefault();

            GridDetailedLineList = LineDataSource.SelectBankBalanceCashFlowLinesLines.Where(t => t.Date_ == LineDataSource.Date_ && t.CurrencyID == bank.CurrencyID && t.BankAccountID == SelectedBankID).ToList();


            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
            {
                foreach (var item in GridLineList.Where(t => t.Date_ >= DetailedLineDataSource.Date_))
                {
                    switch (DetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                    }

                    switch (TLDetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += BankTL; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += BankTL; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += BankTL; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += BankTL; break;
                    }
                }
            }


            if (DetailedLineDataSource.isRecurrent)
            {
                List<DateTime> PeriodicDates = new List<DateTime>();
                DateTime addedDate = DetailedLineDataSource.Date_;

                for (int i = 0; i <= 12; i++)
                {
                    addedDate = addedDate.AddDays(30);

                    if (addedDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        addedDate = addedDate.AddDays(-2);
                    }
                    else if (addedDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        addedDate = addedDate.AddDays(-1);
                    }

                    if (addedDate < DetailedLineDataSource.RecurrentEndTime)
                    {
                        PeriodicDates.Add(addedDate);
                    }
                }

                foreach (DateTime date in PeriodicDates)
                {

                    SelectBankBalanceCashFlowLinesDto SelectedLine = GridLineList.Where(t => t.Date_ == date).FirstOrDefault();

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines == null)
                    {
                        SelectedLine.SelectBankBalanceCashFlowLinesLines = new List<SelectBankBalanceCashFlowLinesLinesDto>();
                    }

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).Count() > 0)
                    {
                        var SelectedLinesLine = SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).FirstOrDefault();

                        int index = SelectedLine.SelectBankBalanceCashFlowLinesLines.IndexOf(SelectedLinesLine);

                        decimal diff = DetailedLineDataSource.Amount_ - SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_;

                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_ = DetailedLineDataSource.Amount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountID = DetailedLineDataSource.BankAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountName = DetailedLineDataSource.BankAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyID = DetailedLineDataSource.CurrencyID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyCode = DetailedLineDataSource.CurrencyCode;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountID = DetailedLineDataSource.CurrentAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountName = DetailedLineDataSource.CurrentAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].TransactionDescription = DetailedLineDataSource.TransactionDescription;

                        if (diff != 0)
                        {
                            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += diff; break;
                                    }


                                }
                            }
                            else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= diff; break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectBankBalanceCashFlowLinesLinesDto addedLinesLineModel = new SelectBankBalanceCashFlowLinesLinesDto
                        {
                            Date_ = date,
                            Amount_ = DetailedLineDataSource.Amount_,
                            BankAccountID = DetailedLineDataSource.BankAccountID,
                            BankAccountName = DetailedLineDataSource.BankAccountName,
                            BankBalanceCashFlowID = DetailedLineDataSource.BankBalanceCashFlowID,
                            BankBalanceCashFlowLineID = GridLineList.Where(t => t.Date_ == date).Select(t => t.Id).FirstOrDefault(),
                            CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType,
                            RecurrentEndTime = null,
                            LinkedBankBalanceCashFlowLinesLineID = DetailedLineDataSource.Id,
                            CurrencyID = DetailedLineDataSource.CurrencyID,
                            CurrencyCode = DetailedLineDataSource.CurrencyCode,
                            CurrentAccountID = DetailedLineDataSource.CurrentAccountID,
                            CurrentAccountName = DetailedLineDataSource.CurrentAccountName,
                            ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_,
                            isRecurrent = false,
                            LineNr = SelectedLine.SelectBankBalanceCashFlowLinesLines.Count + 1,
                            TransactionDescription = DetailedLineDataSource.TransactionDescription,

                        };

                        SelectedLine.SelectBankBalanceCashFlowLinesLines.Add(addedLinesLineModel);

                        if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL += DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += DetailedLineDataSource.Amount_; break;
                                }


                            }
                        }
                        else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                                }
                            }
                        }
                    }

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == TLDetailedLineDataSource.Id).Count() > 0)
                    {
                        var SelectedLinesLine = SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == TLDetailedLineDataSource.Id).FirstOrDefault();

                        int index = SelectedLine.SelectBankBalanceCashFlowLinesLines.IndexOf(SelectedLinesLine);

                        decimal diff = TLDetailedLineDataSource.Amount_ - SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_;

                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_ = TLDetailedLineDataSource.Amount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountID = TLDetailedLineDataSource.BankAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountName = TLDetailedLineDataSource.BankAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansBalanceType = TLDetailedLineDataSource.CashFlowPlansBalanceType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansTransactionType = TLDetailedLineDataSource.CashFlowPlansTransactionType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyID = TLDetailedLineDataSource.CurrencyID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyCode = TLDetailedLineDataSource.CurrencyCode;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountID = TLDetailedLineDataSource.CurrentAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountName = TLDetailedLineDataSource.CurrentAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].ExchangeAmount_ = TLDetailedLineDataSource.ExchangeAmount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].TransactionDescription = TLDetailedLineDataSource.TransactionDescription;

                        if (diff != 0)
                        {
                            if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (TLDetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += diff; break;
                                    }


                                }
                            }
                            else if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (TLDetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= diff; break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectBankBalanceCashFlowLinesLinesDto addedLinesLineModel = new SelectBankBalanceCashFlowLinesLinesDto
                        {
                            Date_ = date,
                            Amount_ = TLDetailedLineDataSource.Amount_,
                            BankAccountID = TLDetailedLineDataSource.BankAccountID,
                            BankAccountName = TLDetailedLineDataSource.BankAccountName,
                            BankBalanceCashFlowID = TLDetailedLineDataSource.BankBalanceCashFlowID,
                            BankBalanceCashFlowLineID = GridLineList.Where(t => t.Date_ == date).Select(t => t.Id).FirstOrDefault(),
                            CashFlowPlansBalanceType = TLDetailedLineDataSource.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = TLDetailedLineDataSource.CashFlowPlansTransactionType,
                            RecurrentEndTime = null,
                            LinkedBankBalanceCashFlowLinesLineID = TLDetailedLineDataSource.Id,
                            CurrencyID = TLDetailedLineDataSource.CurrencyID,
                            CurrencyCode = TLDetailedLineDataSource.CurrencyCode,
                            CurrentAccountID = TLDetailedLineDataSource.CurrentAccountID,
                            CurrentAccountName = TLDetailedLineDataSource.CurrentAccountName,
                            ExchangeAmount_ = TLDetailedLineDataSource.ExchangeAmount_,
                            isRecurrent = false,
                            LineNr = SelectedLine.SelectBankBalanceCashFlowLinesLines.Count + 1,
                            TransactionDescription = TLDetailedLineDataSource.TransactionDescription,

                        };

                        SelectedLine.SelectBankBalanceCashFlowLinesLines.Add(addedLinesLineModel);

                        if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (TLDetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL += TLDetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR += TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += TLDetailedLineDataSource.Amount_; break;
                                }


                            }
                        }
                        else if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (TLDetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL -= TLDetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= TLDetailedLineDataSource.Amount_; break;
                                }
                            }
                        }
                    }


                }
            }

            await _LineGrid.Refresh();
            await _DetailedLineGrid.Refresh();

            Spinner.Hide();

            HideDetailedCrudPopup();

            await InvokeAsync(StateHasChanged);
        }

        public async Task BankBalanceCashFlowLineOnTransferSubmit()
        {
            Spinner.Show();
            await Task.Delay(100);



            if (DetailedLineDataSource.Id == Guid.Empty)
            {
                DetailedLineDataSource.Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate();
                DetailedLineDataSource.BankAccountID = SelectedBankID;

                if (LineDataSource.SelectBankBalanceCashFlowLinesLines.Contains(DetailedLineDataSource))
                {
                    int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.LineNr == DetailedLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                    }
                }
                else
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines.Add(DetailedLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = LineDataSource.SelectBankBalanceCashFlowLinesLines.FindIndex(t => t.Id == DetailedLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    LineDataSource.SelectBankBalanceCashFlowLinesLines[selectedLineIndex] = DetailedLineDataSource;
                }
            }

            int thisyear = GetSQLDateAppService.GetDateFromSQL().Date.Year;

            SelectBankBalanceCashFlowLinesLinesDto TLDetailedLineDataSource = new SelectBankBalanceCashFlowLinesLinesDto
            {
                Amount_ = DetailedLineDataSource.Amount_,
                Date_ = DetailedLineDataSource.Date_,
                ExchangeAmount_ = 0,
                BankAccountID = RecieverBankID,
                CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GelenOdeme,
                CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.GelenOdeme,
                CurrencyID = RecieverBankCurrencyID,
                CurrentAccountID = Guid.Empty,
                TransactionDescription = string.Empty,
                LineNr = GridLineList.Count + 1,
                BankAccountName = RecieverBankName,
                CurrencyCode = RecieverBankCurrencyCode,
                RecurrentEndTime = null,
                isRecurrent = false,
                LinkedBankBalanceCashFlowLinesLineID = Guid.Empty,
                Id = BankBalanceCashFlowsAppService.BankBalanceCashFlowLineGuidGenerate()
            };

            LineDataSource.SelectBankBalanceCashFlowLinesLines.Add(TLDetailedLineDataSource);

            ListBankAccountsDto bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Id == DetailedLineDataSource.BankAccountID.GetValueOrDefault()).FirstOrDefault();

            GridDetailedLineList = LineDataSource.SelectBankBalanceCashFlowLinesLines.Where(t => t.Date_ == LineDataSource.Date_ && t.CurrencyID == bank.CurrencyID && t.BankAccountID == SelectedBankID).ToList();
            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
            {
                foreach (var item in GridLineList.Where(t => t.Date_ >= DetailedLineDataSource.Date_))
                {
                    switch (DetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                    }

                    switch (TLDetailedLineDataSource.BankAccountName)
                    {
                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += DetailedLineDataSource.Amount_; break;
                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += DetailedLineDataSource.Amount_; break;
                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += DetailedLineDataSource.Amount_; break;
                    }
                }
            }

            if (DetailedLineDataSource.isRecurrent)
            {
                List<DateTime> PeriodicDates = new List<DateTime>();
                DateTime addedDate = DetailedLineDataSource.Date_;

                for (int i = 0; i <= 12; i++)
                {
                    addedDate = addedDate.AddDays(30);

                    if (addedDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        addedDate = addedDate.AddDays(-2);
                    }
                    else if (addedDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        addedDate = addedDate.AddDays(-1);
                    }

                    if (addedDate < DetailedLineDataSource.RecurrentEndTime)
                    {
                        PeriodicDates.Add(addedDate);
                    }
                }

                foreach (DateTime date in PeriodicDates)
                {

                    SelectBankBalanceCashFlowLinesDto SelectedLine = GridLineList.Where(t => t.Date_ == date).FirstOrDefault();

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines == null)
                    {
                        SelectedLine.SelectBankBalanceCashFlowLinesLines = new List<SelectBankBalanceCashFlowLinesLinesDto>();
                    }

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).Count() > 0)
                    {
                        var SelectedLinesLine = SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == DetailedLineDataSource.Id).FirstOrDefault();

                        int index = SelectedLine.SelectBankBalanceCashFlowLinesLines.IndexOf(SelectedLinesLine);

                        decimal diff = DetailedLineDataSource.Amount_ - SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_;

                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_ = DetailedLineDataSource.Amount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountID = DetailedLineDataSource.BankAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountName = DetailedLineDataSource.BankAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyID = DetailedLineDataSource.CurrencyID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyCode = DetailedLineDataSource.CurrencyCode;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountID = DetailedLineDataSource.CurrentAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountName = DetailedLineDataSource.CurrentAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].TransactionDescription = DetailedLineDataSource.TransactionDescription;

                        if (diff != 0)
                        {
                            if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += diff; break;
                                    }


                                }
                            }
                            else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (DetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= diff; break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectBankBalanceCashFlowLinesLinesDto addedLinesLineModel = new SelectBankBalanceCashFlowLinesLinesDto
                        {
                            Date_ = date,
                            Amount_ = DetailedLineDataSource.Amount_,
                            BankAccountID = DetailedLineDataSource.BankAccountID,
                            BankAccountName = DetailedLineDataSource.BankAccountName,
                            BankBalanceCashFlowID = DetailedLineDataSource.BankBalanceCashFlowID,
                            BankBalanceCashFlowLineID = GridLineList.Where(t => t.Date_ == date).Select(t => t.Id).FirstOrDefault(),
                            CashFlowPlansBalanceType = DetailedLineDataSource.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = DetailedLineDataSource.CashFlowPlansTransactionType,
                            RecurrentEndTime = null,
                            LinkedBankBalanceCashFlowLinesLineID = DetailedLineDataSource.Id,
                            CurrencyID = DetailedLineDataSource.CurrencyID,
                            CurrencyCode = DetailedLineDataSource.CurrencyCode,
                            CurrentAccountID = DetailedLineDataSource.CurrentAccountID,
                            CurrentAccountName = DetailedLineDataSource.CurrentAccountName,
                            ExchangeAmount_ = DetailedLineDataSource.ExchangeAmount_,
                            isRecurrent = false,
                            LineNr = SelectedLine.SelectBankBalanceCashFlowLinesLines.Count + 1,
                            TransactionDescription = DetailedLineDataSource.TransactionDescription,

                        };

                        SelectedLine.SelectBankBalanceCashFlowLinesLines.Add(addedLinesLineModel);

                        if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL += DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += DetailedLineDataSource.Amount_; break;
                                }


                            }
                        }
                        else if (DetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (DetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL -= DetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= DetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= DetailedLineDataSource.Amount_; break;
                                }
                            }
                        }
                    }

                    if (SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == TLDetailedLineDataSource.Id).Count() > 0)
                    {
                        var SelectedLinesLine = SelectedLine.SelectBankBalanceCashFlowLinesLines.Where(t => t.LinkedBankBalanceCashFlowLinesLineID == TLDetailedLineDataSource.Id).FirstOrDefault();

                        int index = SelectedLine.SelectBankBalanceCashFlowLinesLines.IndexOf(SelectedLinesLine);

                        decimal diff = TLDetailedLineDataSource.Amount_ - SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_;

                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].Amount_ = TLDetailedLineDataSource.Amount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountID = TLDetailedLineDataSource.BankAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].BankAccountName = TLDetailedLineDataSource.BankAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansBalanceType = TLDetailedLineDataSource.CashFlowPlansBalanceType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CashFlowPlansTransactionType = TLDetailedLineDataSource.CashFlowPlansTransactionType;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyID = TLDetailedLineDataSource.CurrencyID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrencyCode = TLDetailedLineDataSource.CurrencyCode;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountID = TLDetailedLineDataSource.CurrentAccountID;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].CurrentAccountName = TLDetailedLineDataSource.CurrentAccountName;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].ExchangeAmount_ = TLDetailedLineDataSource.ExchangeAmount_;
                        SelectedLine.SelectBankBalanceCashFlowLinesLines[index].TransactionDescription = TLDetailedLineDataSource.TransactionDescription;

                        if (diff != 0)
                        {
                            if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (TLDetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL += diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += diff; break;
                                    }


                                }
                            }
                            else if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                            {
                                foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                                {
                                    switch (TLDetailedLineDataSource.BankAccountName)
                                    {
                                        case "AKBANK T A Ş TRY": item.AmountAkbankTL -= diff; break;
                                        case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= diff; break;
                                        case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= diff; break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SelectBankBalanceCashFlowLinesLinesDto addedLinesLineModel = new SelectBankBalanceCashFlowLinesLinesDto
                        {
                            Date_ = date,
                            Amount_ = TLDetailedLineDataSource.Amount_,
                            BankAccountID = TLDetailedLineDataSource.BankAccountID,
                            BankAccountName = TLDetailedLineDataSource.BankAccountName,
                            BankBalanceCashFlowID = TLDetailedLineDataSource.BankBalanceCashFlowID,
                            BankBalanceCashFlowLineID = GridLineList.Where(t => t.Date_ == date).Select(t => t.Id).FirstOrDefault(),
                            CashFlowPlansBalanceType = TLDetailedLineDataSource.CashFlowPlansBalanceType,
                            CashFlowPlansTransactionType = TLDetailedLineDataSource.CashFlowPlansTransactionType,
                            RecurrentEndTime = null,
                            LinkedBankBalanceCashFlowLinesLineID = TLDetailedLineDataSource.Id,
                            CurrencyID = TLDetailedLineDataSource.CurrencyID,
                            CurrencyCode = TLDetailedLineDataSource.CurrencyCode,
                            CurrentAccountID = TLDetailedLineDataSource.CurrentAccountID,
                            CurrentAccountName = TLDetailedLineDataSource.CurrentAccountName,
                            ExchangeAmount_ = TLDetailedLineDataSource.ExchangeAmount_,
                            isRecurrent = false,
                            LineNr = SelectedLine.SelectBankBalanceCashFlowLinesLines.Count + 1,
                            TransactionDescription = TLDetailedLineDataSource.TransactionDescription,

                        };

                        SelectedLine.SelectBankBalanceCashFlowLinesLines.Add(addedLinesLineModel);

                        if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GelenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (TLDetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL += TLDetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR += TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL += TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR += TLDetailedLineDataSource.Amount_; break;
                                }


                            }
                        }
                        else if (TLDetailedLineDataSource.CashFlowPlansBalanceType == CashFlowPlansBalanceTypeEnum.GonderilenOdeme)
                        {
                            foreach (var item in GridLineList.Where(t => t.Date_ >= SelectedLine.Date_))
                            {
                                switch (TLDetailedLineDataSource.BankAccountName)
                                {
                                    case "AKBANK T A Ş TRY": item.AmountAkbankTL -= TLDetailedLineDataSource.Amount_; break;
                                    case "AKBANK T A Ş EUR": item.AmountAkbankEUR -= TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş TRY": item.AmountIsBankTL -= TLDetailedLineDataSource.Amount_; break;
                                    case "TÜRKİYE İŞ BANKASI A Ş EUR": item.AmountIsBankEUR -= TLDetailedLineDataSource.Amount_; break;
                                }
                            }
                        }
                    }


                }
            }

            await _LineGrid.Refresh();
            await _DetailedLineGrid.Refresh();

            Spinner.Hide();

            HideDetailedCrudPopup();


            await InvokeAsync(StateHasChanged);
        }

        public void HideTotalbyMonthsModal()
        {
            TotalsbyMonthsModalVisible = false;
        }

        public void HideDetailedPopup()
        {
            GridDetailedLineList.Clear();
            DetailedCashFlowPopupVisible = false;
        }

        public void HideDetailedCrudPopup()
        {
            DetailedCashFlowCrudPopupVisible = false;
            IncomePaymentExpensePopupVisible = false;
            ExchangeSalesPopupVisible = false;
            TransferPopupVisible = false;
            CreditPaymentPopupVisible = false;
        }

        public void CellInfoHandler(Syncfusion.Blazor.Grids.QueryCellInfoEventArgs<SelectBankBalanceCashFlowLinesDto> Args)
        {
            switch (Args.Column.Field)
            {
                case "AmountAkbankTL":
                    if (Args.Data.AmountAkbankTL < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white;font-weight: bold; font-size: 18px !important; " });
                    }
                    else
                    {
                        if (BankBalancesLastDatesList.Contains(Args.Data.Date_))
                        {
                            Args.Cell.AddStyle(new string[] { "background-color: #626262; color: white;font-weight: bold; font-size: 20px !important;" });
                        }
                        else
                        {
                            Args.Cell.AddStyle(new string[] { "font-weight: bold; font-size: 18px !important; " });
                        }
                    }
                    break;

                case "AmountAkbankEUR":

                    if (Args.Data.AmountAkbankEUR < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white; font-weight: bold; font-size: 18px !important;" });
                    }
                    else
                    {
                        if (BankBalancesLastDatesList.Contains(Args.Data.Date_))
                        {
                            Args.Cell.AddStyle(new string[] { "background-color: #626262; color: white;font-weight: bold; font-size: 20px !important;" });
                        }
                        else
                        {
                            Args.Cell.AddStyle(new string[] { "font-weight: bold; font-size: 18px !important; " });
                        }
                    }

                    break;

                case "AmountIsBankTL":

                    if (Args.Data.AmountIsBankTL < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white;font-weight: bold; font-size: 18px !important; " });
                    }
                    else
                    {
                        if (BankBalancesLastDatesList.Contains(Args.Data.Date_))
                        {
                            Args.Cell.AddStyle(new string[] { "background-color: #626262; color: white;font-weight: bold; font-size: 20px !important;" });
                        }
                        else
                        {
                            Args.Cell.AddStyle(new string[] { "font-weight: bold; font-size: 18px !important; " });
                        }
                    }
                    break;

                case "AmountIsBankEUR":
                    if (Args.Data.AmountIsBankEUR < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white;font-weight: bold; font-size: 18px !important; " });
                    }
                    else
                    {
                        if (BankBalancesLastDatesList.Contains(Args.Data.Date_))
                        {
                            Args.Cell.AddStyle(new string[] { "background-color: #626262; color: white;font-weight: bold; font-size: 20px !important;" });
                        }
                        else
                        {
                            Args.Cell.AddStyle(new string[] { "font-weight: bold; font-size: 18px !important; " });
                        }
                    }
                    break;

                case "Date_":
                    if (BankBalancesLastDatesList.Contains(Args.Data.Date_))
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #626262; color: white;font-weight: bold; font-size: 20px !important; " });
                    }
                    else
                    {
                        Args.Cell.AddStyle(new string[] { "font-weight: bold; font-size: 18px !important; " });

                    }

                    break;
            }


            StateHasChanged();
        }

        public void TotalMonthsCellInfoHandler(Syncfusion.Blazor.Grids.QueryCellInfoEventArgs<TotalbyMonths> Args)
        {
            switch (Args.Column.Field)
            {
                case "AmountAkbankTL":
                    if (Args.Data.AmountAkbankTL < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white; " });
                    }
                    break;

                case "AmountAkbankEUR":
                    if (Args.Data.AmountAkbankEUR < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white; " });
                    }
                    break;

                case "AmountIsBankTL":
                    if (Args.Data.AmountIsBankTL < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white; " });
                    }
                    break;

                case "AmountIsBankEUR":
                    if (Args.Data.AmountIsBankEUR < 0)
                    {
                        Args.Cell.AddStyle(new string[] { "background-color: #e24545; color: white; " });
                    }
                    break;
            }


            StateHasChanged();
        }

        public void OnAmountsChange()
        {
            BankTL = DetailedLineDataSource.ExchangeAmount_ * DetailedLineDataSource.Amount_;
        }

        private async void RecurrentChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (!DetailedLineDataSource.isRecurrent)
            {
                DetailedLineDataSource.RecurrentEndTime = null;
                RecurrentTimeEnabled = false;
            }
            else
            {
                RecurrentTimeEnabled = true;

            }

            await InvokeAsync(StateHasChanged);
        }

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
            BankAccountsList = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.CurrencyCode == CurrencyStr && t.Id != DetailedLineDataSource.BankAccountID.GetValueOrDefault()).ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void BankAccountsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                RecieverBankCurrencyID = Guid.Empty;
                RecieverBankCurrencyCode = string.Empty;
                RecieverBankID = Guid.Empty;
                RecieverBankName = string.Empty;
            }
        }

        public async void BankAccountsDoubleClickHandler(RecordDoubleClickEventArgs<ListBankAccountsDto> args)
        {
            var selectedBankAccount = args.RowData;

            if (selectedBankAccount != null)
            {

                RecieverBankID = selectedBankAccount.Id;
                RecieverBankName = selectedBankAccount.Name;
                RecieverBankCurrencyID = selectedBankAccount.CurrencyID;
                RecieverBankCurrencyCode = selectedBankAccount.CurrencyCode;

                SelectBankAccountsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        #region Enum ComboBox İşlemleri

        #region Bakiye Türü ComboBox

        public IEnumerable<SelectBankBalanceCashFlowLinesLinesDto> balancetypes = GetEnumDisplayBalanceTypesNames<CashFlowPlansBalanceTypeEnum>();

        public static List<SelectBankBalanceCashFlowLinesLinesDto> GetEnumDisplayBalanceTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<CashFlowPlansBalanceTypeEnum>()
                       .Select(x => new SelectBankBalanceCashFlowLinesLinesDto
                       {
                           CashFlowPlansBalanceType = x,
                           CashFlowPlansBalanceTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #region İşlem Türü ComboBox

        public IEnumerable<SelectBankBalanceCashFlowLinesLinesDto> transactiontypes = GetEnumDisplayTransactionTypesNames<CashFlowPlansTransactionTypeEnum>();

        public static List<SelectBankBalanceCashFlowLinesLinesDto> GetEnumDisplayTransactionTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<CashFlowPlansTransactionTypeEnum>()
                       .Select(x => new SelectBankBalanceCashFlowLinesLinesDto
                       {
                           CashFlowPlansTransactionType = x,
                           CashFlowPlansTransactionTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
