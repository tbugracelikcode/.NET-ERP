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
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
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
        public bool IncomePaymentExpensePopupVisible = false;
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

            List<ListBankAccountsDto> banksList =  (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.ToList();

            #region Yorum
            for (int i = 1; i <= 365; i++)
            {
                if (MinDay.Year == thisyear)
                {
                    if (MinDay.DayOfWeek != DayOfWeek.Sunday && MinDay.DayOfWeek != DayOfWeek.Saturday) // Haftasonu değilse
                    {

                        foreach(var bank in banksList)
                        {
                            SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncome = new SelectCashFlowPlanLinesDto
                            {
                                Date_ = MinDay,
                                Amount_ = 0,
                                CurrencyID = bank.CurrencyID,
                                CurrencyCode = bank.CurrencyCode,
                                CurrentAccountID = Guid.Empty,
                                CurrentAccountName = string.Empty,
                                LineNr = GridLineList.Count + 1,
                                BankAccountName = bank.Name,
                                BankAccountID = bank.Id,
                                TransactionDescription = string.Empty,
                                CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                            };

                            SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure = new SelectCashFlowPlanLinesDto
                            {
                                Date_ = MinDay,
                                Amount_ = 0,
                                CurrencyID = bank.CurrencyID,
                                CurrencyCode = bank.CurrencyCode,
                                CurrentAccountID = Guid.Empty,
                                CurrentAccountName = string.Empty,
                                LineNr = GridLineList.Count + 1,
                                BankAccountName = bank.Name,
                                BankAccountID = bank.Id,
                                TransactionDescription = string.Empty,
                                CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                            };

                            GridLineList.Add(cashFlowPlanLineModelIncome);
                            GridLineList.Add(cashFlowPlanLineModelExpenditure);
                        }
                    }

                    MinDay = MinDay.AddDays(1);

                }
            }
            #endregion

            DataSource.SelectCashFlowPlanLines = GridLineList;

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
                            //case "CashFlowPlanLinesContextExchangePurchaseAdd":
                            //    DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextExchangePurchaseAdd"], Id = "expurchasenew" }); break;
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
            ListCurrenciesDto currency = new ListCurrenciesDto();
            ListBankAccountsDto bank = new ListBankAccountsDto();

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
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.GelenOdeme;
                    TransactionPopupTitle = L["TransactionTypeIncome"];
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == GridDetailedLineList.Select(t => t.BankAccountName).FirstOrDefault() && t.CurrencyCode == CurrencyStr).FirstOrDefault();

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "paymentnew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.YapilacakOdeme;
                    TransactionPopupTitle = L["TransactionTypePayment"];

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == GridDetailedLineList.Select(t => t.BankAccountName).FirstOrDefault() && t.CurrencyCode == CurrencyStr).FirstOrDefault();

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "transfernew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    TransferPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.BankaHesaplarArasiVirman;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == GridDetailedLineList.Select(t => t.BankAccountName).FirstOrDefault() && t.CurrencyCode == CurrencyStr).FirstOrDefault();

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    RecieverBankID = Guid.Empty;
                    RecieverBankName = string.Empty;
                    RecieverBankCurrencyID = Guid.Empty;
                    RecieverBankCurrencyCode = string.Empty;
                    await InvokeAsync(StateHasChanged);
                    break;

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

                case "salesnew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    ExchangeSalesPopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.DovizSatis;
                    DetailedLineDataSource.CashFlowPlansBalanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    BankTL = 0;

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == GridDetailedLineList.Select(t => t.BankAccountName).FirstOrDefault() && t.CurrencyCode == CurrencyStr).FirstOrDefault();

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;
                    BankTLName = bank.Name + " TL";

                    #endregion

                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;
                case "expensenew":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    IncomePaymentExpensePopupVisible = true;
                    DetailedLineDataSource.CashFlowPlansTransactionType = CashFlowPlansTransactionTypeEnum.MasrafTutari;
                    TransactionPopupTitle = L["TransactionTypeExpenseAmount"];

                    #region Banka ve Para Birimi Eşleşmesi

                    bank = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == GridDetailedLineList.Select(t => t.BankAccountName).FirstOrDefault() && t.CurrencyCode == CurrencyStr).FirstOrDefault();

                    DetailedLineDataSource.BankAccountName = bank.Name;
                    DetailedLineDataSource.BankAccountID = bank.Id;
                    DetailedLineDataSource.CurrencyCode = bank.CurrencyCode;
                    DetailedLineDataSource.CurrencyID = bank.CurrencyID;

                    #endregion

                    DetailedLineDataSource.TransactionDescription = L["TransactionTypeExpense"];
                    DetailedLineDataSource.Date_ = GridDetailedLineList.Select(t => t.Date_).FirstOrDefault();
                    DetailedLineDataSource.LineNr = GridDetailedLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;



                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        BankTL = DetailedLineDataSource.ExchangeAmount_ * DetailedLineDataSource.Amount_;
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

                string[] arr = args.Data.ColumnHeaders.ToString().Split('.');

                CurrencyStr = arr[2];
                string BankStr = arr[1];
                string balanceStr = arr[0];
                CashFlowPlansBalanceTypeEnum balanceType = CashFlowPlansBalanceTypeEnum.GelenOdeme;

                if (balanceStr == "GelenOdeme")
                {
                    balanceType = CashFlowPlansBalanceTypeEnum.GelenOdeme;
                }
                else
                {
                    balanceType = CashFlowPlansBalanceTypeEnum.GonderilenOdeme;
                }

                string DateStr = args.Data.RowHeaders.ToString();

                DetailedGridPopupTitle = DateStr + " > " + BankStr + " > " + CurrencyStr;

                GridDetailedLineList = GridLineList.Where(t => t.Date_ == DateTime.Parse(DateStr) && t.CurrencyCode == CurrencyStr && t.BankAccountName == BankStr && t.CashFlowPlansBalanceType == balanceType).ToList();

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
            IncomePaymentExpensePopupVisible = false;
            ExchangeSalesPopupVisible = false;
            TransferPopupVisible = false;
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

            GridDetailedLineList = GridLineList.Where(t => t.Date_ == DetailedLineDataSource.Date_ && t.CurrencyCode == DetailedLineDataSource.CurrencyCode && t.BankAccountName == DetailedLineDataSource.BankAccountName).ToList();

            await _DetailedLineGrid.Refresh();

            await _CashFlowPlanLinePivot.LayoutRefreshAsync();

            await InvokeAsync(StateHasChanged);

            HideDetailedCrudPopup();

        }

        public async Task OnLineExchangeSalesSubmit()
        {

            ListBankAccountsDto bankTl = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name == DetailedLineDataSource.BankAccountName && t.CurrencyCode == "TRY").FirstOrDefault();

            SelectCashFlowPlanLinesDto TLDetailedLineDataSource = new SelectCashFlowPlanLinesDto
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
            };

            DataSource.SelectCashFlowPlanLines.Add(TLDetailedLineDataSource);

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

            GridDetailedLineList = GridLineList.Where(t => t.Date_ == DetailedLineDataSource.Date_ && t.CurrencyCode == DetailedLineDataSource.CurrencyCode && t.BankAccountName == DetailedLineDataSource.BankAccountName && t.CashFlowPlansBalanceType == DetailedLineDataSource.CashFlowPlansBalanceType).ToList();

            await _DetailedLineGrid.Refresh();

            await _CashFlowPlanLinePivot.LayoutRefreshAsync();

            await InvokeAsync(StateHasChanged);

            HideDetailedCrudPopup();

        }

        public async Task OnLineTransferSubmit()
        {

            SelectCashFlowPlanLinesDto TLDetailedLineDataSource = new SelectCashFlowPlanLinesDto
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
            };

            DataSource.SelectCashFlowPlanLines.Add(TLDetailedLineDataSource);

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

            GridDetailedLineList = GridLineList.Where(t => t.Date_ == DetailedLineDataSource.Date_ && t.CurrencyCode == DetailedLineDataSource.CurrencyCode && t.BankAccountName == DetailedLineDataSource.BankAccountName && t.CashFlowPlansBalanceType == DetailedLineDataSource.CashFlowPlansBalanceType).ToList();

            await _DetailedLineGrid.Refresh();

            await _CashFlowPlanLinePivot.LayoutRefreshAsync();

            await InvokeAsync(StateHasChanged);

            HideDetailedCrudPopup();

        }

        public void OnAmountsChange()
        {
            BankTL = DetailedLineDataSource.ExchangeAmount_ * DetailedLineDataSource.Amount_;
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
            BankAccountsList = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t=>t.CurrencyCode == CurrencyStr).ToList();
            await InvokeAsync(StateHasChanged);
        }


        public void BankAccountsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                RecieverBankCurrencyID = Guid.Empty;
                RecieverBankCurrencyCode= string.Empty;
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
