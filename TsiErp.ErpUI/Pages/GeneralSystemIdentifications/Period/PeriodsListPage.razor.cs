using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Period
{

    public partial class PeriodsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = PeriodsService;
            _L = L;
            await GetBranchsList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PeriodsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPeriodsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("PeriodsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #region Şube ButtonEdit

        SfTextBox BranchButtonEdit;
        bool SelectBranchPopupVisible = false;
        List<ListBranchesDto> BranchList = new List<ListBranchesDto>();

        public async Task BranchOnCreateIcon()
        {
            var BranchButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchButtonClickEvent);
            await BranchButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchButtonClick } });
        }

        public async void BranchButtonClickEvent()
        {
            SelectBranchPopupVisible = true;
            BranchList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void BranchOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchName = string.Empty;
            }
        }

        public async void BranchDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                DataSource.BranchID = selectedBranch.Id;
                DataSource.BranchName = selectedBranch.Name;
                SelectBranchPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {

            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "PeriodContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PeriodContextAdd"], Id = "new" }); break;
                        case "PeriodContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PeriodContextChange"], Id = "changed" }); break;
                        case "PeriodContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PeriodContextDelete"], Id = "delete" }); break;
                        case "PeriodContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PeriodContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }


        private async Task GetBranchsList()
        {
            BranchList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PeriodsChildMenu");
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
