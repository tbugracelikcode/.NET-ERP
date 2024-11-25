using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.FinanceManagement.CompanyCheck
{
    public partial class CompanyChecksListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService Spinner { get; set; }

        protected override async void OnInitialized()
        {
            BaseCrudService = CompanyChecksAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CompanyChecksChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion

            foreach(var item in states)
            {
                item.CompanyChecksStateName = L[item.CompanyChecksStateName];
            }

            foreach (var item in paymentstates)
            {
                item.CompanyChecksPaymentStateName = L[item.CompanyChecksPaymentStateName];
            }
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCompanyChecksDto()
            {
                CompanyChecksPaymentState = Entities.Enums.CompanyChecksPaymentStateEnum.Odenmedi,
                CompanyChecksState = Entities.Enums.CompanyChecksStateEnum.KendiCekimiz,
                DueDate = GetSQLDateAppService.GetDateFromSQL().Date
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
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
                            case "CompanyChecksContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CompanyChecksContextAdd"], Id = "new" }); break;
                            case "CompanyChecksContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CompanyChecksContextChange"], Id = "changed" }); break;
                            case "CompanyChecksContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CompanyChecksContextDelete"], Id = "delete" }); break;
                            case "CompanyChecksContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CompanyChecksContextRefresh"], Id = "refresh" }); break;
                            case "CompanyChecksContextStatus":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "CompanyChecksContextPaid":
                                                subMenus.Add(new MenuItem { Text = L["CompanyChecksContextPaid"], Id = "paid" }); break;

                                            case "CompanyChecksContextNotPaid":
                                                subMenus.Add(new MenuItem { Text = L["CompanyChecksContextNotPaid"], Id = "notpaid" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CompanyChecksContextStatus"], Id = "status", Items = subMenus }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListCompanyChecksDto> args)
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
                        SelectFirstDataRow = false;
                        DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
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
                            SelectFirstDataRow = false;
                            await DeleteAsync(args.RowInfo.RowData.Id);
                            await GetListDataSourceAsync();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "paid":
                    if (args.RowInfo.RowData != null)
                    {
                        Spinner.Show();
                        await Task.Delay(100);

                        DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.CompanyChecksPaymentState = CompanyChecksPaymentStateEnum.Odendi;

                        var updatedInput = ObjectMapper.Map<SelectCompanyChecksDto, UpdateCompanyChecksDto>(DataSource);

                        await CompanyChecksAppService.UpdateAsync(updatedInput);

                        await GetListDataSourceAsync();

                        await InvokeAsync(StateHasChanged);

                        Spinner.Hide();
                    }

                    break;

                case "notpaid":
                    if (args.RowInfo.RowData != null)
                    {
                        Spinner.Show();
                        await Task.Delay(100);

                        DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.CompanyChecksPaymentState = CompanyChecksPaymentStateEnum.Odenmedi;

                        var updatedInput = ObjectMapper.Map<SelectCompanyChecksDto, UpdateCompanyChecksDto>(DataSource);

                        await CompanyChecksAppService.UpdateAsync(updatedInput);

                        await GetListDataSourceAsync();

                        await InvokeAsync(StateHasChanged);

                        Spinner.Hide();

                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
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

        #region ButtonEdit Metotları

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit = new();
        SfTextBox CurrentAccountCardsNameButtonEdit = new();
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
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void CurrentAccountCardsCustomerCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
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
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;

            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id;
                DataSource.CurrentAccountCardName = selectedUnitSet.Name;
                DataSource.CurrentAccountCardCode = selectedUnitSet.Code;
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
                DataSource.BankAccountID   = Guid.Empty;
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

        #endregion

        #region Combobox İşlemleri

        public IEnumerable<SelectCompanyChecksDto> states = GetEnumDisplayStatesNames<CompanyChecksStateEnum>();

        public static List<SelectCompanyChecksDto> GetEnumDisplayStatesNames<T>()
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<CompanyChecksStateEnum>()
                       .Select(x => new SelectCompanyChecksDto
                       {
                           CompanyChecksState = x,
                           CompanyChecksStateName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();

        }

        public IEnumerable<SelectCompanyChecksDto> paymentstates = GetEnumDisplayPaymentStatesNames<CompanyChecksPaymentStateEnum>();

        public static List<SelectCompanyChecksDto> GetEnumDisplayPaymentStatesNames<T>()
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<CompanyChecksPaymentStateEnum>()
                       .Select(x => new SelectCompanyChecksDto
                       {
                           CompanyChecksPaymentState = x,
                           CompanyChecksPaymentStateName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();

        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
