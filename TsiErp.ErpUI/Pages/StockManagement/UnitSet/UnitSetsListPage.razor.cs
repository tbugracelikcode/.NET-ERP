using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.UnitSet
{
    public partial class UnitSetsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = UnitSetsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "UnitSetsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnitSetsDto()
            {
                IsActive = true
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
                        case "UnitSetContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextAdd"], Id = "new" }); break;
                        case "UnitSetContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextChange"], Id = "changed" }); break;
                        case "UnitSetContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextDelete"], Id = "delete" }); break;
                        case "UnitSetContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        #region Kod ButtonEdit

        //SfTextBox CodeButtonEdit;

        //public async Task CodeOnCreateIcon()
        //{
        //    var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
        //    await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        //}

        //public async void CodeButtonClickEvent()
        //{
        //    DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UnitSetsChildMenu");
        //    await InvokeAsync(StateHasChanged);
        //}
        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
