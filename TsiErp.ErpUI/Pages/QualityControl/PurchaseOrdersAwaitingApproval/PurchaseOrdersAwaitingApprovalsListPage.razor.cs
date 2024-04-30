using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.PurchaseManagement.PurchaseOrder.PurchaseOrdersListPage;

namespace TsiErp.ErpUI.Pages.QualityControl.PurchaseOrdersAwaitingApproval
{
    public partial class PurchaseOrdersAwaitingApprovalsListPage : IDisposable
    {
        private SfGrid<SelectPurchaseOrderLinesDto> _LineGrid;
        private SfGrid<SelectPurchaseOrdersDto> _Grid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseOrderLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseOrderLinesDto> GridLineList = new List<SelectPurchaseOrderLinesDto>();

        List<SelectPurchaseOrdersDto> GridList = new List<SelectPurchaseOrdersDto>();

        public bool OrdersAwatingApprovalModalVisible = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PurchaseOrdersAppService;
            _L = L;

            GridList = (await PurchaseOrdersAppService.GetQualityControlSelectListAsync()).Data.ToList();

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseOrdersAwaitingApprovalsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
        }

        #region Sipariş Onaylama İşlemleri

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PurchaseOrdersAwaitingApprovalLineContextApprove":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalLineContextApprove"], Id = "approve" }); break;
                            case "PurchaseOrdersAwaitingApprovalLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PurchaseOrdersAwaitingApprovalContextApprove":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalContextApprove"], Id = "approve" }); break;
                            case "PurchaseOrdersAwaitingApprovalContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrdersAwaitingApprovalContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "approve":
                    DataSource = args.RowInfo.RowData;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    OrdersAwatingApprovalModalVisible = true;

                    await InvokeAsync(StateHasChanged);
                    break;

                case "refresh":
                    GridList = (await PurchaseOrdersAppService.GetQualityControlSelectListAsync()).Data.ToList();
                    await _Grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseOrderLinesDto> args)
        {
            switch (args.Item.Id)
            {
               

                case "approve":
                    LineDataSource = args.RowInfo.RowData;
                    int lineIndex = GridLineList.IndexOf(LineDataSource);

                    GridLineList[lineIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.KaliteKontrolOnayiVerildi;

                    DataSource.SelectPurchaseOrderLinesDto = GridLineList;

                    await _LineGrid.Refresh();

                    await InvokeAsync(StateHasChanged);
                    break;

                case "refresh":
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        protected override async Task OnSubmit()
        {
            SelectPurchaseOrdersDto result;
          
            var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);

            result = (await UpdateAsync(updateInput)).Data;

            if (result == null)
            {

                return;
            }

            GridList = (await PurchaseOrdersAppService.GetQualityControlSelectListAsync()).Data.ToList();

            OrdersAwatingApprovalModalVisible = false;
        }

        public void HideApprovalPage()
        {
            OrdersAwatingApprovalModalVisible = false;
        }


        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
