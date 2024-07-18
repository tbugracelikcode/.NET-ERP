using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.ProductGroup
{
    public partial class ProductGroupsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private SfGrid<SelectProductPropertyLinesDto> _PropertyLineGrid;
        private SfGrid<ListProductPropertiesDto> _PropertyGrid;
        SelectProductPropertyLinesDto PropertyLineDataSource;
        SelectProductPropertiesDto PropertyDataSource;
        public List<ContextMenuItemModel> PropertyLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> PropertyMainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductPropertyLinesDto> PropertyGridLineList = new List<SelectProductPropertyLinesDto>();
        List<ListProductPropertiesDto> PropertyGridList = new List<ListProductPropertiesDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public bool ProductPropertiesModalVisible = false;
        public bool ProductPropertiesCrudModalVisible = false;
        public bool ProductPropertyLinesModalVisible = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductGroupsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateProductPropertyContextMenuItems();
            CreateProductPropertyLineContextMenuItems();
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
                        case "ProductGroupContextProductProperties":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextProductProperties"], Id = "productproperties" }); break;
                        default: break;
                    }
                }
            }
        }

        protected void CreateProductPropertyContextMenuItems()
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
                            case "ProductPropertyContextAdd":
                                PropertyMainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextAdd"], Id = "new" }); break;
                            case "ProductPropertyContextChange":
                                PropertyMainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextChange"], Id = "changed" }); break;
                            case "ProductPropertyContextDelete":
                                PropertyMainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextDelete"], Id = "delete" }); break;
                            case "ProductPropertyContextRefresh":
                                PropertyMainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateProductPropertyLineContextMenuItems()
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
                            case "ProductPropertyLineContextAdd":
                                PropertyLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyLineContextAdd"], Id = "new" }); break;
                            case "ProductPropertyLineContextChange":
                                PropertyLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyLineContextChange"], Id = "changed" }); break;
                            case "ProductPropertyLineContextDelete":
                                PropertyLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyLineContextDelete"], Id = "delete" }); break;
                            case "ProductPropertyLineContextRefresh":
                                PropertyLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductPropertyLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListProductGroupsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "productproperties":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();

                    ProductPropertiesModalVisible = true;

                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnProductPropertyContextMenuClick(ContextMenuClickEventArgs<ListProductPropertiesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    PropertyDataSource = new SelectProductPropertiesDto()
                    { ProductGroupID = DataSource.Id };

                    PropertyGridLineList = new List<SelectProductPropertyLinesDto>();

                    ProductPropertiesCrudModalVisible = true;

                    break;

                case "changed":
                    PropertyDataSource = (await ProductPropertiesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    PropertyGridLineList = PropertyDataSource.SelectProductPropertyLines;

                    if (PropertyDataSource != null)
                    {
                        bool? dataOpenStatus = PropertyDataSource.DataOpenStatus;

                        if (dataOpenStatus == true && dataOpenStatus != null)
                        {
                            ProductPropertiesCrudModalVisible = false;
                            await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            ProductPropertiesCrudModalVisible = true;
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        await ProductPropertiesAppService.DeleteAsync(args.RowInfo.RowData.Id);

                        PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();

                        await InvokeAsync(StateHasChanged);
                    }
                    await _PropertyGrid.Refresh();

                    break;

                case "refresh":
                    PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();
                    await _PropertyGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnProductPropertyLineContextMenuClick(ContextMenuClickEventArgs<SelectProductPropertyLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    PropertyLineDataSource = new SelectProductPropertyLinesDto();
                    ProductPropertyLinesModalVisible = true;
                    PropertyLineDataSource.LineNr = PropertyGridLineList.Count + 1;
                    PropertyLineDataSource.ProductGroupID = PropertyDataSource.ProductGroupID;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    PropertyLineDataSource = args.RowInfo.RowData;
                    ProductPropertyLinesModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            PropertyDataSource.SelectProductPropertyLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await ProductPropertiesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                                PropertyDataSource.SelectProductPropertyLines.Remove(line);
                                PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();
                            }
                            else
                            {
                                PropertyDataSource.SelectProductPropertyLines.Remove(line);
                            }
                        }

                        await _PropertyLineGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();
                    await _PropertyLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideProductPropertyPopup()
        {
            ProductPropertiesModalVisible = false;
            PropertyGridList.Clear();
            PropertyGridLineList.Clear();
        }

        public void HideProductPropertyCrudPopup()
        {
            ProductPropertiesCrudModalVisible = false;
        }

        public void HideProductPropertyLinePopup()
        {
            ProductPropertyLinesModalVisible = false;
        }

        public async void OnPropertySubmit()
        {

            if(string.IsNullOrEmpty(PropertyDataSource.Code))
            {
                await ModalManager.WarningPopupAsync(L["Error"], L["ValidatorProductPropertyCodeEmpty"]);
                return;
            }

            if (PropertyDataSource.Code.Length>17)
            {
                await ModalManager.WarningPopupAsync(L["Error"], L["ValidatorProductPropertyCodeMaxLenght"]);
                return;
            }

            if(string.IsNullOrEmpty(PropertyDataSource.Name))
            {
                await ModalManager.WarningPopupAsync(L["Error"], L["ValidatorProductPropertyNameEmpty"]);
                return;
            }

            if (PropertyDataSource.Name.Length>17)
            {
                await ModalManager.WarningPopupAsync(L["Error"], L["ValidatorProductPropertyNameMaxLenght"]);
                return;
            }


            if (PropertyDataSource.isChooseFromList)
            {
                if (PropertyDataSource.SelectProductPropertyLines.Count == 0 || PropertyDataSource.SelectProductPropertyLines == null)
                {
                    await ModalManager.WarningPopupAsync("UIWarningChooseFromListTitle", "UIWarningChooseFromListMessage");
                }
                else
                {
                    await InsertUpdateMappingProductProperty();
                }
            }
            else
            {
                await InsertUpdateMappingProductProperty();
            }

            PropertyGridList = (await ProductPropertiesAppService.GetListAsync(new ListProductPropertiesParameterDto())).Data.Where(t => t.ProductGroupID == DataSource.Id).ToList();

            HideProductPropertyCrudPopup();
        }

        public async Task InsertUpdateMappingProductProperty()
        {
            if (PropertyDataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectProductPropertiesDto, CreateProductPropertiesDto>(PropertyDataSource);

                await ProductPropertiesAppService.CreateAsync(createInput);
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectProductPropertiesDto, UpdateProductPropertiesDto>(PropertyDataSource);

                await ProductPropertiesAppService.UpdateAsync(updateInput);
            }
        }

        protected async Task OnPropertyLineSubmit()
        {
            if (PropertyLineDataSource.Id == Guid.Empty)
            {
                if (PropertyDataSource.SelectProductPropertyLines == null)
                {
                    PropertyDataSource.SelectProductPropertyLines = new List<SelectProductPropertyLinesDto>();
                }

                if (PropertyDataSource.SelectProductPropertyLines.Contains(PropertyLineDataSource))
                {
                    int selectedLineIndex = PropertyDataSource.SelectProductPropertyLines.FindIndex(t => t.LineNr == PropertyLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        PropertyDataSource.SelectProductPropertyLines[selectedLineIndex] = PropertyLineDataSource;
                    }
                }
                else
                {
                    PropertyDataSource.SelectProductPropertyLines.Add(PropertyLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = PropertyDataSource.SelectProductPropertyLines.FindIndex(t => t.Id == PropertyLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    PropertyDataSource.SelectProductPropertyLines[selectedLineIndex] = PropertyLineDataSource;
                }
            }

            PropertyGridLineList = PropertyDataSource.SelectProductPropertyLines;
            await _PropertyLineGrid.Refresh();

            HideProductPropertyLinePopup();
            await InvokeAsync(StateHasChanged);
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
