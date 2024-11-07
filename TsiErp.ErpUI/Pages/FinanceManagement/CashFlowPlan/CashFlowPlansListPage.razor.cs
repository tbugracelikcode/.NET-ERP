using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.PivotView;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
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

        public List<ListBankAccountsDto> BankAccountsList = new List<ListBankAccountsDto>();

        SfPivotView<SelectCashFlowPlanLinesDto> CashFlowPlanLinePivot;
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

            BankAccountsList = (await BankAccountsAppService.GetListAsync(new ListBankAccountsParameterDto())).Data.ToList();

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
                            CurrencyCode = string.Empty,
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            ExpenseAmount_ = 0,
                            LineNr = GridLineList.Count + 1,
                            TransmitterBankAccountName = "AKBANK T.A.Ş.",
                            TransmitterBankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            RecieverBankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            RecieverBankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelIncome2 = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 62500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = string.Empty,
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            ExpenseAmount_ = 0,
                            LineNr = GridLineList.Count + 1,
                            TransmitterBankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            TransmitterBankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            RecieverBankAccountName = "AKBANK T.A.Ş.",
                            RecieverBankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GelenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 55000,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = string.Empty,
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            ExpenseAmount_ = 0,
                            LineNr = GridLineList.Count + 1,
                            TransmitterBankAccountName = "AKBANK T.A.Ş.",
                            TransmitterBankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            TransactionDescription = string.Empty,
                            RecieverBankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            RecieverBankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        SelectCashFlowPlanLinesDto cashFlowPlanLineModelExpenditure2 = new SelectCashFlowPlanLinesDto
                        {
                            Date_ = MinDay,
                            Amount_ = 1500,
                            CurrencyID = Guid.Empty,
                            CurrencyCode = string.Empty,
                            CurrentAccountID = Guid.Empty,
                            CurrentAccountName = string.Empty,
                            ExpenseAmount_ = 0,
                            LineNr = GridLineList.Count + 1,
                            TransmitterBankAccountName = "TÜRKİYE İŞ BANKASI A.Ş.",
                            TransmitterBankAccountID = new Guid("CCF05862-AEAC-D5A6-1BA9-3A15EEE8FA68"),
                            TransactionDescription = string.Empty,
                            RecieverBankAccountName = "AKBANK T.A.Ş.",
                            RecieverBankAccountID = new Guid("62E31707-BFBE-3040-15AA-3A15EF024471"),
                            CashFlowPlansBalanceType = Entities.Enums.CashFlowPlansBalanceTypeEnum.GonderilenOdeme,

                        };

                        GridLineList.Add(cashFlowPlanLineModelIncome);
                        GridLineList.Add(cashFlowPlanLineModelIncome2);
                        GridLineList.Add(cashFlowPlanLineModelExpenditure);
                        GridLineList.Add(cashFlowPlanLineModelExpenditure2);
                    }

                    MinDay = MinDay.AddDays(1);

                }
            }

            DataSource.SelectCashFlowPlanLines = GridLineList;

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
                            case "CashFlowPlanLinesContextAdd":
                                DetailedLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CashFlowPlanLinesContextAdd"], Id = "new" }); break;
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
                case "new":

                    DetailedLineDataSource = new SelectCashFlowPlanLinesDto();
                    DetailedCashFlowCrudPopupVisible = true;
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
