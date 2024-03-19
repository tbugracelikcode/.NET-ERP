using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.ControlCondition
{
    partial class ControlConditionsDocumentsListPage:IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        bool _isOperation;
        bool _isPurchase;
        bool _isContract;

        protected override async void OnInitialized()
        {
            BaseCrudService = ControlConditionsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ControlConditionsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectControlConditionsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ControlConditionsChildMenu")
            };

            _isContract = false;
            _isOperation = false;
            _isPurchase = false;

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
                        case "ControlConditionContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlConditionContextAdd"], Id = "new" }); break;
                        case "ControlConditionContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlConditionContextChange"], Id = "changed" }); break;
                        case "ControlConditionContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlConditionContextDelete"], Id = "delete" }); break;
                        case "ControlConditionContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ControlConditionContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        public override void ShowEditPage()
        {
            switch (DataSource.QualityPlanTypes)
            {
                case "Operation":
                    _isOperation = true;
                    _isPurchase = false;
                    _isContract = false;
                    break;

                case "Purchase":
                    _isOperation = false;
                    _isPurchase = true;
                    _isContract = false;
                    break;

                case "Contract":
                    _isOperation = false;
                    _isPurchase = false;
                    _isContract = true;
                    break;
                default:
                    break;
            }

            base.ShowEditPage();
        }

        private async void OperationChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.QualityPlanTypes = "Operation";
                _isPurchase = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void PurchaseChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (args.Checked)
            {
                DataSource.QualityPlanTypes = "Purchase";
                _isOperation = false;
                _isContract = false;
                await (InvokeAsync(StateHasChanged));
            }
        }

        private async void ContractChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {

            if (args.Checked)
            {
                DataSource.QualityPlanTypes = "Contract";
                _isOperation = false;
                _isPurchase = false;
                await (InvokeAsync(StateHasChanged));
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ControlConditionsChildMenu");
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
