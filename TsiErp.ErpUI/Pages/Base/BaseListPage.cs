using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.HeatMap.Internal;
using Syncfusion.Blazor.Navigations;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using IResult = Tsi.Core.Utilities.Results.IResult;

namespace TsiErp.ErpUI.Pages.Base
{
    public abstract class BaseListPage<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> : ComponentBase
         where TGetOutputDto : class, new()
         where TGetListOutputDto : class, IEntityDto,new()
         where TGetListInput : class, new()
    {
        public string LoadingCaption { get; set; } = "Lütfen Bekleyin..";
        public string LoadingText { get; set; } = "Kayıtlar Yükleniyor...";
        public bool EditPageVisible { get; set; }
        public bool IsLoaded { get; set; }
        public bool SelectFirstDataRow { get; set; }

        public TGetListOutputDto SelectedItem { get; set; }

        public TGetOutputDto DataSource { get; set; }
        public IList<TGetListOutputDto> ListDataSource { get; set; }

        public List<ContextMenuItemModel> GridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        protected ICrudAppService<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> BaseCrudService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle",Id="new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir",Id="changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Sil",Id="delete" });
            await GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }

        #region Crud Operations
        protected async virtual Task<IDataResult<TGetOutputDto>> GetAsync(Guid id)
        {
            return await BaseCrudService.GetAsync(id);
        }

        protected async virtual Task<IDataResult<IList<TGetListOutputDto>>> GetListAsync(TGetListInput input)
        {
            return await BaseCrudService.GetListAsync(input);
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> CreateAsync(TCreateInput input)
        {
            return await BaseCrudService.CreateAsync(input);
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> UpdateAsync(TUpdateInput input)
        {
            return await BaseCrudService.UpdateAsync(input);
        }

        protected async virtual Task<IResult> DeleteAsync(Guid id)
        {
            return await BaseCrudService.DeleteAsync(id);
        }
        #endregion

        protected virtual async Task GetListDataSourceAsync()
        {
            ListDataSource = (await GetListAsync(new TGetListInput
            {
            })).Data.ToList();

            IsLoaded = true;
        }

        public void HideEditPage()
        {
            EditPageVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public void ShowEditPage()
        {
            EditPageVisible = true;
            InvokeAsync(StateHasChanged);
        }

        public async virtual void OnContextMenuClick(ContextMenuClickEventArgs<TGetListOutputDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    DataSource = new TGetOutputDto();
                    ShowEditPage();
                    break;

                case "changed":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }
    }
}
