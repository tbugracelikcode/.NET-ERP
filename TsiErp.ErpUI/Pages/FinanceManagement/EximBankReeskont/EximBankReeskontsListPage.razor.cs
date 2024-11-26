using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.FinanceManagement.EximBankReeskont
{
    public partial class EximBankReeskontsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<CurrentDebtAnalysis> CurrentDebtAnalysisList = new List<CurrentDebtAnalysis>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        private bool LineCrudPopup = false;
        private bool CurrentDebtAnalysisPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = EximBankReeskontsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EximBankReeskontsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();

        }

        #region Fiyat Listesi Satır Modalı İşlemleri

        public class CurrentDebtAnalysis
        {
            public string BankName { get; set; }
            public decimal TotalRemaining { get; set; }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectEximBankReeskontsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
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
                            case "EximBankReeskontsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EximBankReeskontsContextAdd"], Id = "new" }); break;
                            case "EximBankReeskontsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EximBankReeskontsContextChange"], Id = "changed" }); break;
                            case "EximBankReeskontsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EximBankReeskontsContextDelete"], Id = "delete" }); break;
                            case "EximBankReeskontsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EximBankReeskontsContextRefresh"], Id = "refresh" }); break;
                            case "EximBankReeskontsContextCurrentDebtAnalysis":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["EximBankReeskontsContextCurrentDebtAnalysis"], Id = "debt" }); break;
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
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListEximBankReeskontsDto> args)
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
                        DataSource = (await EximBankReeskontsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

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

                case "debt":
                    if (args.RowInfo.RowData != null)
                    {

                        var groupedList = ListDataSource.GroupBy(t => t.BankAccountID).ToList();

                        foreach (var group in groupedList)
                        {
                            var selectedList = ListDataSource.Where(t => t.BankAccountID == group.Key).ToList();

                            CurrentDebtAnalysis debt = new CurrentDebtAnalysis
                            {
                                BankName = selectedList.Select(t => t.BankAccountName).FirstOrDefault(),
                                TotalRemaining = selectedList.Sum(t => t.RemainingAmount)
                            };

                            CurrentDebtAnalysisList.Add(debt);
                        }

                        CurrentDebtAnalysisPopup = true;

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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public void HideCurrentDebtAnalysis()
        {
            CurrentDebtAnalysisList.Clear();
            CurrentDebtAnalysisPopup = false;
        }

        public void TotalAmountChange()
        {
            DataSource.TotalAmount = DataSource.MainAmount + DataSource.InterestAmount + DataSource.CommissionAmount;
        }
        public void RemainingAmountChange()
        {
            DataSource.RemainingAmount = DataSource.TotalAmount - DataSource.PaidAmount;
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
                DataSource.BankAccountID = Guid.Empty;
                DataSource.BankAccountName = string.Empty;
            }
        }

        public async void BankAccountsDoubleClickHandler(RecordDoubleClickEventArgs<ListBankAccountsDto> args)
        {
            var selectedBankAccount = args.RowData;

            if (selectedBankAccount != null)
            {

                DataSource.BankAccountID = selectedBankAccount.Id;
                DataSource.BankAccountName = selectedBankAccount.Name;

                SelectBankAccountsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
