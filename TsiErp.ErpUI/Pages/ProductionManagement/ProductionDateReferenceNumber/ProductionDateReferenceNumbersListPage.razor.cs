using Microsoft.AspNetCore.Components;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos;
using Syncfusion.Blazor.Navigations;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.Business.Entities.PalletRecord.Services;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionDateReferenceNumber
{
    public partial class ProductionDateReferenceNumbersListPage : IDisposable
    {
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionDateReferenceNumbersAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductionDateReferenceNumbersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();

        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionDateReferenceNumbersDto()
            {

            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public async override void ShowEditPage()
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
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }


        protected void CreateMainContextMenuItems()
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
                            case "ProdDateReferenceNoContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProdDateReferenceNoContextAdd"], Id = "new" }); break;
                            case "ProdDateReferenceNoContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProdDateReferenceNoContextChange"], Id = "changed" }); break;
                            case "ProdDateReferenceNoContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProdDateReferenceNoContextDelete"], Id = "delete" }); break;
                            case "ProdDateReferenceNoContextState":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "ProdDateReferenceNoContextStateApproved":
                                                subMenus.Add(new MenuItem { Text = L["ProdDateReferenceNoContextStateApproved"], Id = "approved" }); break;
                                            case "ProdDateReferenceNoContextStateNotApproved":
                                                subMenus.Add(new MenuItem { Text = L["ProdDateReferenceNoContextStateNotApproved"], Id = "notapproved" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProdDateReferenceNoContextState"], Id = "state", Items = subMenus }); break;
                            case "ProdDateReferenceNoContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProdDateReferenceNoContextRefresh"], Id = "refresh" }); break;

                            default: break;
                        }
                    }
                }
            }
        }


        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionDateReferenceNumbersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                        DataSource = (await ProductionDateReferenceNumbersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
                        if (res == true)
                        {
                            await DeleteAsync(args.RowInfo.RowData.Id);
                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }
                    break;

                case "approved":
                    if (args.RowInfo.RowData != null)
                    {
                        SpinnerService.Show();
                        await Task.Delay(100);
                        DataSource = (await ProductionDateReferenceNumbersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        SpinnerService.Hide();
                        DataSource.Confirmation = true;
                        var updateInput = ObjectMapper.Map<SelectProductionDateReferenceNumbersDto, UpdateProductionDateReferenceNumbersDto>(DataSource);
                        await ProductionDateReferenceNumbersAppService.UpdateAsync(updateInput);

                        await GetListDataSourceAsync();
                        await _grid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "notapproved":
                    if (args.RowInfo.RowData != null)
                    {
                        SpinnerService.Show();
                        await Task.Delay(100);
                        DataSource = (await ProductionDateReferenceNumbersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;


                        SpinnerService.Hide();
                        DataSource.Confirmation = false;
                        var updateInput = ObjectMapper.Map<SelectProductionDateReferenceNumbersDto, UpdateProductionDateReferenceNumbersDto>(DataSource);
                        await ProductionDateReferenceNumbersAppService.UpdateAsync(updateInput);

                        await GetListDataSourceAsync();
                        await _grid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;
                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
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
            DataSource.ProductionDateReferenceNo = FicheNumbersAppService.GetFicheNumberAsync("ProductionDateReferenceNumbersChildMenu");
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
