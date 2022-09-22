using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Vsm
{
    public partial class VSMListPage
    {
        List<EmployeeGeneralAnalysis> dataemployee = new List<EmployeeGeneralAnalysis>();
        public List<ContextMenuItemModel> GridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        #region Değişkenler
        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        bool popupVisible = false;
        #endregion

        protected override async void OnInitialized()
        {
            
            dataemployee = await PersonelService.GetEmployeeGeneralAnalysis(startDate, endDate);
            CreateContextMenuItems();
        }

        #region Context Menü - Modal Popup İşlemleri

        protected virtual void CreateContextMenuItems()
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
        }

        public async virtual void OnContextMenuClick(ContextMenuClickEventArgs<EmployeeGeneralAnalysis> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    //BeforeInsertAsync();
                    popupVisible = true;
                    break;

                case "changed":
                    //SelectFirstDataRow = false;
                    //DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    popupVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    //var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz ?");

                    //if (res == true)
                    //{
                    //    SelectFirstDataRow = false;
                    //    await DeleteAsync(args.RowInfo.RowData.Id);
                    //    await GetListDataSourceAsync();
                    //    await InvokeAsync(StateHasChanged);
                    //}

                    break;

                case "refresh":
                    await PersonelService.GetEmployeeGeneralAnalysis(startDate, endDate);
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void OnSubmit()
        {
            popupVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public void HideEditPage()
        {
            popupVisible = false;
            InvokeAsync(StateHasChanged);
        }

        #endregion

    }
}
