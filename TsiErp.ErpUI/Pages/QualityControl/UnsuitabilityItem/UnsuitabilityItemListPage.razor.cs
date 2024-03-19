using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.SqlServer.Management.Smo;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItem
{
    partial class UnsuitabilityItemListPage : IDisposable
    {
        private List<SelectionList> SelectionLists = new List<SelectionList>() { new SelectionList { Text = "Evet", Id = 1 }, new SelectionList { Text = "Hayır", Id = 2 } };

        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        List<ListUnsuitabilityTypesItemsDto> UnsuitabilityTypesItemList = new List<ListUnsuitabilityTypesItemsDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = UnsuitabilityItemsService;
            _L = L;
            UnsuitabilityTypesItemList = (await UnsuitabilityTypesItemsService.GetListAsync(new ListUnsuitabilityTypesItemsParameterDto())).Data.ToList();
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "UnsItemsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UnsItemsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #region İş Merkezi ButtonEdit

        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationGroupId = Guid.Empty;
                DataSource.StationGroupName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.StationGroupId = selectedStationGroup.Id;
                DataSource.StationGroupName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
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
                        case "UnsuitabilityItemContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemContextAdd"], Id = "new" }); break;
                        case "UnsuitabilityItemContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemContextChange"], Id = "changed" }); break;
                        case "UnsuitabilityItemContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemContextDelete"], Id = "delete" }); break;
                        case "UnsuitabilityItemContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnsuitabilityItemContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
            }
        }

        private void IntensityRangeCalculate()
        {
            if(DataSource.LifeThreatening=="1")
            {
                DataSource.IntensityRange = 100;
                DataSource.IntensityCoefficient = 10;
                return;
            }
            else
            {
                DataSource.IntensityRange = 0;
                DataSource.IntensityCoefficient = 0;

                if (DataSource.LossOfPrestige == "1")
                {
                    DataSource.IntensityRange = 100;
                    DataSource.IntensityCoefficient = 10;
                    return;
                }

                if (DataSource.ExtraCost == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 50;
                }

                if (DataSource.ProductLifeShortening == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 20;
                }

                if (DataSource.Detectability == "2")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 20;
                }

                if (DataSource.ToBeUsedAs == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 10;
                }
            }

            if (DataSource.LossOfPrestige == "1")
            {
                DataSource.IntensityRange = 100;
                DataSource.IntensityCoefficient = 10;
                return;
            }
            else 
            {
                DataSource.IntensityRange = 0;
                DataSource.IntensityCoefficient = 0;

                if (DataSource.LifeThreatening == "1")
                {
                    DataSource.IntensityRange = 100;
                    DataSource.IntensityCoefficient = 10;
                    return;
                }

                if (DataSource.ExtraCost == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 50;
                }

                if (DataSource.ProductLifeShortening == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 20;
                }

                if (DataSource.Detectability == "2")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 20;
                }

                if (DataSource.ToBeUsedAs == "1")
                {
                    DataSource.IntensityRange = DataSource.IntensityRange + 10;
                }
            }

            if(DataSource.IntensityRange==0)
            {
                DataSource.IntensityCoefficient = 1;
            }
            else
            {
                DataSource.IntensityCoefficient = DataSource.IntensityRange / 10;
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UnsItemsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }


    public class SelectionList
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }
}
