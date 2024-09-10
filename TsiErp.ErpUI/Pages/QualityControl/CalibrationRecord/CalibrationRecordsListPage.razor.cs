using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.CalibrationRecord
{
    public partial class CalibrationRecordsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        protected override async void OnInitialized()
        {
            _L = L;
            BaseCrudService = CalibrationRecordsService;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CalRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

        }

        #region Ekipman ButtonEdit

        SfTextBox EquipmentRecordButtonEdit;
        bool SelectEquipmentRecordPopupVisible = false;
        List<ListEquipmentRecordsDto> EquipmentRecordList = new List<ListEquipmentRecordsDto>();

        public async Task EquipmentRecordOnCreateIcon()
        {
            var EquipmentRecordButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EquipmentRecordButtonClickEvent);
            await EquipmentRecordButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EquipmentRecordButtonClick } });
        }

        public async void EquipmentRecordButtonClickEvent()
        {
            SelectEquipmentRecordPopupVisible = true;
            EquipmentRecordList = (await EquipmentRecordsService.GetListAsync(new ListEquipmentRecordsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EquipmentRecordOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EquipmentID = Guid.Empty;
                DataSource.Equipment = string.Empty;
            }
        }

        public async void EquipmentRecordDoubleClickHandler(RecordDoubleClickEventArgs<ListEquipmentRecordsDto> args)
        {
            var selectedEquipmentRecord = args.RowData;

            if (selectedEquipmentRecord != null)
            {
                DataSource.EquipmentID = selectedEquipmentRecord.Id;
                DataSource.Equipment = selectedEquipmentRecord.Name;
                SelectEquipmentRecordPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion



        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalibrationRecordsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                NextControl = GetSQLDateAppService.GetDateFromSQL().Date,
                Code = FicheNumbersAppService.GetFicheNumberAsync("CalRecordsChildMenu")
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
                            case "CalibrationRecordContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationRecordContextAdd"], Id = "new" }); break;
                            case "CalibrationRecordContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationRecordContextChange"], Id = "changed" }); break;
                            case "CalibrationRecordContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationRecordContextDelete"], Id = "delete" }); break;
                            case "CalibrationRecordContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationRecordContextRefresh"], Id = "refresh" }); break;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CalRecordsChildMenu");
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
