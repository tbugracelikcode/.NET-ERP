using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Currency
{

    public partial class CurrenciesListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        [Inject]
        ModalManager ModalManager { get; set; }

        protected override async void OnInitialized()
        {
            BaseCrudService = CurrenciesService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CurrenciesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCurrenciesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("CurrenciesChildMenu")
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
                            case "CurrencyContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextAdd"], Id = "new" }); break;
                            case "CurrencyContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextChange"], Id = "changed" }); break;
                            case "CurrencyContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextDelete"], Id = "delete" }); break;
                            case "CurrencyContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task OnSubmit()
        {
            if (DataSource.IsLocalCurrency && ListDataSource.Any(t => t.IsLocalCurrency && t.Id != DataSource.Id))
            {
                await ModalManager.WarningPopupAsync(L["UIWarningLocalCurrencyTitle"], L["UIWarningLocalCurrencyMessage"]);
            }
            else
            {
                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectCurrenciesDto, CreateCurrenciesDto>(DataSource);

                    await CreateAsync(createInput);
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectCurrenciesDto, UpdateCurrenciesDto>(DataSource);

                    await UpdateAsync(updateInput);
                }

                await GetListDataSourceAsync();
                HideEditPage();
            }

        }


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CurrenciesChildMenu");
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
