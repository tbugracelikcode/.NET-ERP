using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        private SfGrid<ListBranchesDto> _grid;
        protected override async void OnInitialized()
        {
            BaseCrudService = BranchesService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBranchesDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }

        //protected override void CreateContextMenu()
        //{
        //    base.CreateContextMenu();

        //    //base.GridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
        //}

        //public async override void OnContextMenuClick(ContextMenuClickEventArgs<ListBranchesDto> args)
        //{

        //    switch (args.Item.Id)
        //    {
        //        case "refresh":
        //            ListDataSource = (await GetListAsync(new ListBranchesParameterDto { IsActive = true })).Data.ToList();
        //            break;

        //        default:
        //            break;
        //    }

        //    base.OnContextMenuClick(args);
        //}
    }
}