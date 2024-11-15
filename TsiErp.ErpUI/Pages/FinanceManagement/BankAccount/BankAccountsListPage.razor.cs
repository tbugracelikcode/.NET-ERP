using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Linq;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.QualityControl.CustomerComplaintReport.CustomerComplaintReportsListPage;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

namespace TsiErp.ErpUI.Pages.FinanceManagement.BankAccount
{
    public partial class BankAccountsListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        int bankNameIndex = 0;

        [Inject]
        ModalManager ModalManager { get; set; }


        #region ComboBox İşlemleri

        public class BanksComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<BanksComboBox> _banksComboBox = new List<BanksComboBox>
        {
            new BanksComboBox(){ID = "AKBANK", Text="AKBANK T A Ş"},
            new BanksComboBox(){ID = "HSBC", Text="HSBC BANK A Ş"},
            new BanksComboBox(){ID = "QNB", Text="QNB BANK A Ş"},
            new BanksComboBox(){ID = "ZİRAAT", Text="TC ZİRAAT BANKASI A Ş"},
            new BanksComboBox(){ID = "GARANTİ", Text="TÜRKİYE GARANTİ BANKASI A Ş"},
            new BanksComboBox(){ID = "HALK", Text="TÜRKİYE HALK BANKASI A Ş"},
            new BanksComboBox(){ID = "İŞ", Text="TÜRKİYE İŞ BANKASI A Ş"},
            new BanksComboBox(){ID = "YAPIKREDİ", Text="YAPI VE KREDİ BANKASI A Ş"},
            new BanksComboBox(){ID = "VAKIF", Text="TÜRKİYE VAKIFLAR BANKASI T A O"},
            new BanksComboBox(){ID = "DENİZBANK", Text="DENİZBANK A Ş"},
            new BanksComboBox(){ID = "ING", Text="ING BANK A Ş"},
            new BanksComboBox(){ID = "TEB", Text="TÜRK EKONOMİ BANKASI A Ş"},
        };

        private async void BanksComboBoxValueChangeHandler(ChangeEventArgs<string, BanksComboBox> args)
        {

            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "AKBANK":
                        DataSource.Name = "AKBANK T A Ş";
                        DataSource.BankGroupName = "AKBANK T A Ş";
                        bankNameIndex = 0;
                        break;

                    case "HSBC":
                        DataSource.Name = "HSBC BANK A Ş";
                        DataSource.BankGroupName = "HSBC BANK A Ş";
                        bankNameIndex = 1;
                        break;

                    case "QNB":
                        DataSource.Name = "QNB BANK A Ş";
                        DataSource.BankGroupName = "QNB BANK A Ş";
                        bankNameIndex = 2;
                        break;

                    case "ZİRAAT":
                        DataSource.Name = "TC ZİRAAT BANKASI A Ş";
                        DataSource.BankGroupName = "TC ZİRAAT BANKASI A Ş";
                        bankNameIndex = 3;
                        break;

                    case "GARANTİ":
                        DataSource.Name = "TÜRKİYE GARANTİ BANKASI A Ş";
                        DataSource.BankGroupName = "TÜRKİYE GARANTİ BANKASI A Ş";
                        bankNameIndex = 4;
                        break;

                    case "HALK":
                        DataSource.Name = "TÜRKİYE HALK BANKASI A Ş";
                        DataSource.BankGroupName = "TÜRKİYE HALK BANKASI A Ş";
                        bankNameIndex = 5;
                        break;

                    case "İŞ":
                        DataSource.Name = "TÜRKİYE İŞ BANKASI A Ş";
                        DataSource.BankGroupName = "TÜRKİYE İŞ BANKASI A Ş";
                        bankNameIndex = 6;
                        break;

                    case "YAPIKREDİ":
                        DataSource.Name = "YAPI VE KREDİ BANKASI A Ş";
                        DataSource.BankGroupName = "YAPI VE KREDİ BANKASI A Ş";
                        bankNameIndex = 7;
                        break;

                    case "VAKIF":
                        DataSource.Name = "TÜRKİYE VAKIFLAR BANKASI T A O";
                        DataSource.BankGroupName = "TÜRKİYE VAKIFLAR BANKASI T A O";
                        bankNameIndex = 8;
                        break;

                    case "DENİZBANK":
                        DataSource.Name = "DENİZBANK A Ş";
                        DataSource.BankGroupName = "DENİZBANK A Ş";
                        bankNameIndex = 9;
                        break;

                    case "ING":
                        DataSource.Name = "ING BANK A Ş.";
                        DataSource.BankGroupName = "ING BANK A Ş.";
                        bankNameIndex = 10;
                        break;

                    case "TEB":
                        DataSource.Name = "TÜRK EKONOMİ BANKASI A Ş";
                        DataSource.BankGroupName = "TÜRK EKONOMİ BANKASI A Ş";
                        bankNameIndex = 11;
                        break;

                    default: break;
                }

                ListBankAccountsDto LastBankAccountsName = (await BankAccountsService.GetListAsync(new ListBankAccountsParameterDto())).Data.Where(t => t.Name.Contains(DataSource.Name) && t.Id != DataSource.Id).LastOrDefault();

                if (LastBankAccountsName == null)
                {
                    DataSource.BankOrderNo = 1;
                }
                else
                {
                    DataSource.BankOrderNo = LastBankAccountsName.BankOrderNo + 1;
                }


            }
        }

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = BankAccountsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "BankAccountsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();

            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBankAccountsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("BankAccountsChildMenu"),
                Name = "AKBANK T.A.Ş."
            };

            bankNameIndex = 0;

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public override async void ShowEditPage()
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
                    #region Banka Combobox Index Ataması

                    switch (DataSource.Name)
                    {
                        case "AKBANK T.A.Ş.": bankNameIndex = 0; break;
                        case "HSBC BANK A.Ş.": bankNameIndex = 1; break;
                        case "QNB BANK A.Ş.": bankNameIndex = 2; break;
                        case "T.C. ZİRAAT BANKASI A.Ş.": bankNameIndex = 3; break;
                        case "TÜRKİYE GARANTİ BANKASI A.Ş.": bankNameIndex = 4; break;
                        case "TÜRKİYE HALK BANKASI A.Ş.": bankNameIndex = 5; break;
                        case "TÜRKİYE İŞ BANKASI A.Ş.": bankNameIndex = 6; break;
                        case "YAPI VE KREDİ BANKASI A.Ş.": bankNameIndex = 7; break;
                        case "TÜRKİYE VAKIFLAR BANKASI T.A.O.": bankNameIndex = 8; break;
                        case "DENİZBANK A.Ş.": bankNameIndex = 9; break;
                        case "ING BANK A.Ş.": bankNameIndex = 10; break;
                        case "TÜRK EKONOMİ BANKASI A.Ş.": bankNameIndex = 11; break;
                        default: break;
                    }

                    #endregion

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
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
                            case "BankAccountsContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["BankAccountsContextAdd"], Id = "new" }); break;
                            case "BankAccountsContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["BankAccountsContextChange"], Id = "changed" }); break;
                            case "BankAccountsContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["BankAccountsContextDelete"], Id = "delete" }); break;
                            case "BankAccountsContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["BankAccountsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task OnSubmit()
        {
            if (DataSource.Name.Contains("."))
            {
                await ModalManager.WarningPopupAsync(L["UIWarningDotTitle"], L["UIWarningDotMessage"]);
            }
            else
            {
                #region OnSubmit

                SelectBankAccountsDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectBankAccountsDto, CreateBankAccountsDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectBankAccountsDto, UpdateBankAccountsDto>(DataSource);

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
                #endregion
            }

        }


        #region Para Birimleri ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrencyPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.CurrencyID = selectedCurrency.Id;
                DataSource.CurrencyCode = selectedCurrency.CurrencySymbol;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("BankAccountsChildMenu");
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
