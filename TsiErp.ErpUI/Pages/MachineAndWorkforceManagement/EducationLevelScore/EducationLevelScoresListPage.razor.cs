﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EducationLevelScore
{
    public partial class EducationLevelScoresListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = EducationLevelScoresService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "EducationLevelScoresChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEducationLevelScoresDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EducationLevelScoresChildMenu")
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
                            case "EducationLevelScoresContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextAdd"], Id = "new" }); break;
                            case "EducationLevelScoresContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextChange"], Id = "changed" }); break;
                            case "EducationLevelScoresContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextDelete"], Id = "delete" }); break;
                            case "EducationLevelScoresContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EducationLevelScoresChildMenu");
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
