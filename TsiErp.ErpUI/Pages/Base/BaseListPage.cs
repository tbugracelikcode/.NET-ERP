using Blazored.Modal.Services;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.HeatMap.Internal;
using Syncfusion.Blazor.Lists;
using Syncfusion.Blazor.Navigations;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Blazor.Component.Core.Services;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.ErpUI.Utilities.ModalUtilities.ModalComponents;
using IResult = Tsi.Core.Utilities.Results.IResult;

namespace TsiErp.ErpUI.Pages.Base
{
    public abstract class BaseListPage<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> : ComponentBase,ICoreCommonService
         where TGetOutputDto : class, IEntityDto, new()
         where TGetListOutputDto : class, IEntityDto, new()
         where TGetListInput : class, new()
    {
        public string LoadingCaption { get; set; } = "Lütfen Bekleyin..";
        public string LoadingText { get; set; } = "Kayıtlar Yükleniyor...";
        public bool EditPageVisible { get; set; }
        public bool IsLoaded { get; set; }
        public bool SelectFirstDataRow { get; set; }

        public TGetListOutputDto SelectedItem { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }
        public TGetOutputDto DataSource { get; set; }
        public IList<TGetListOutputDto> ListDataSource { get; set; }

        public List<ContextMenuItemModel> GridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        protected ICrudAppService<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> BaseCrudService { get; set; }

        public ComponentBase ActiveEditComponent { get ; set ; }
        public bool IsPopupListPage { get; set; }
        public Guid PopupListPageFocusedRowId { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            CreateContextMenuItems();
            await GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected virtual void CreateContextMenuItems()
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
        }

        #region Crud Operations
        protected async virtual Task<IDataResult<TGetOutputDto>> GetAsync(Guid id)
        {
            return await BaseCrudService.GetAsync(id);
        }

        protected async virtual Task<IList<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            return (await BaseCrudService.GetListAsync(input)).Data.ToList();
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
                
            })).ToList();

            IsLoaded = true;
        }

        protected virtual async Task OnSubmit()
        {
            TGetOutputDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<TGetOutputDto, TCreateInput>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<TGetOutputDto, TUpdateInput>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null) return;

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
        }

        public virtual void HideEditPage()
        {
            EditPageVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public virtual void ShowEditPage()
        {
            EditPageVisible = true;
            InvokeAsync(StateHasChanged);
        }

        public async virtual void OnContextMenuClick(ContextMenuClickEventArgs<TGetListOutputDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    BeforeInsertAsync();
                    break;

                case "changed":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz ?");

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

                default:
                    break;
            }
        }

        protected virtual async Task BeforeInsertAsync()
        {
            DataSource = new TGetOutputDto();

            ShowEditPage();
        }

    }
}
