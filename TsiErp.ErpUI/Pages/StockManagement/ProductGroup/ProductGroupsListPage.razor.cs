using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.ProductGroup
{
    public partial class ProductGroupsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductGroupsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductGroupsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "ProductGroupContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextAdd"], Id = "new" }); break;
                        case "ProductGroupContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextChange"], Id = "changed" }); break;
                        case "ProductGroupContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextDelete"], Id = "delete" }); break;
                        case "ProductGroupContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu");
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
