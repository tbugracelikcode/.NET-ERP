using Autofac.Features.OwnedInstances;
using DevExpress.Blazor;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System.Text;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Shared;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;
using IResult = Tsi.Core.Utilities.Results.IResult;

namespace TsiErp.ErpUI.Pages.Base
{
    public abstract class BaseListPage<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> : ComponentBase
         where TGetOutputDto : class, IEntityDto, new()
         where TGetListOutputDto : class, IEntityDto, new()
         where TGetListInput : class, new()
    {

        public string LoadingCaption { get; set; }
        public string LoadingText { get; set; }
        public bool EditPageVisible { get; set; }
        public bool IsLoaded { get; set; }

        public bool IsChanged { get; set; } = false;
        public bool SelectFirstDataRow { get; set; }

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        public string toolbarSearchKey = string.Empty;

        public TGetListOutputDto SelectedItem { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }

        [CascadingParameter]
        public MainLayout Layout { get; set; }

        [Inject]
        IJSRuntime JsRuntime { get; set; }

        public TGetOutputDto DataSource { get; set; }
        public IList<TGetListOutputDto> ListDataSource { get; set; }

        public SfGrid<TGetListOutputDto> _grid;

        public object _L { get; set; }

        public List<ContextMenuItemModel> GridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<ItemModel> GridToolbarItems { get; set; } = new List<ItemModel>();

        public string GridSearchText { get; set; }

        protected ICrudAppService<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> BaseCrudService { get; set; }



        protected async override Task OnParametersSetAsync()
        {
            var loc = (IStringLocalizer)_L;

            if (loc != null)
            {
                LoadingCaption = loc["LoadingCaption"];
                LoadingText = loc["LoadingText"];


                CreateContextMenuItems(loc);
                CreateGridToolbar();
            }

            await GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected virtual void CreateContextMenuItems(IStringLocalizer loc)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = loc["ContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = loc["ContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = loc["ContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = loc["ContextRefresh"], Id = "refresh" });
        }

        #region Crud Operations

        protected async virtual Task<IDataResult<TGetOutputDto>> GetAsync(Guid id)
        {
            var loc = (IStringLocalizer)_L;

            try
            {
                return await BaseCrudService.GetAsync(id);
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message);
                }

                return default(IDataResult<TGetOutputDto>);
            }
        }

        protected async virtual Task<IList<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            var loc = (IStringLocalizer)_L;

            try
            {
                return (await BaseCrudService.GetListAsync(input)).Data.ToList();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message);
                }

                return default(IList<TGetListOutputDto>);
            }
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> CreateAsync(TCreateInput input)
        {
            var loc = (IStringLocalizer)_L;

            try
            {
                return await BaseCrudService.CreateAsync(input);
            }
            catch (DuplicateCodeException exp)
            {
                await ModalManager.MessagePopupAsync(loc["MessagePopupInformationTitleBase"], exp.Message);
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (ValidationException exp)
            {
                var errorList = exp.Errors.ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<ul>");

                for (int i = 0; i < errorList.Count; i++)
                {
                    sb.AppendLine("<li>" + loc.GetString(errorList[i].ErrorMessage) + "</li>");
                }

                sb.AppendLine("</ul>");

                await ModalManager.MessagePopupAsync(loc["MessagePopupInformationTitleBase"], sb.ToString());
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message);
                }

                return new ErrorDataResult<TGetOutputDto>();
            }
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> UpdateAsync(TUpdateInput input)
        {
            var loc = (IStringLocalizer)_L;
            try
            {
                return await BaseCrudService.UpdateAsync(input);
            }
            catch (DuplicateCodeException exp)
            {
                await ModalManager.MessagePopupAsync(loc["MessagePopupInformationTitleBase"], exp.Message);
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (ValidationException exp)
            {
                var errorList = exp.Errors.ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<ul>");

                for (int i = 0; i < errorList.Count; i++)
                {
                    sb.AppendLine("<li>" + loc.GetString(errorList[i].ErrorMessage) + "</li>");
                }

                sb.AppendLine("</ul>");

                await ModalManager.MessagePopupAsync(loc["MessagePopupInformationTitleBase"], sb.ToString());
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message);
                }
                return new ErrorDataResult<TGetOutputDto>();
            }
        }

        protected async virtual Task<IResult> DeleteAsync(Guid id)
        {
            var loc = (IStringLocalizer)_L;
            try
            {
                return await BaseCrudService.DeleteAsync(id);
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync(loc["Error"], exp.Message);
                }
                return new ErrorDataResult<TGetOutputDto>();
            }
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
            //Layout.LoadingSpinnerVisible = true;
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

            if (result == null)
            {

                return;
            }

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


            //Layout.LoadingSpinnerVisible = false;
        }

        public virtual void HideEditPage()
        {
            EditPageVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public async virtual void ShowEditPage()
        {
            var loc = (IStringLocalizer)_L;

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync(loc["MessagePopupInformationTitleBase"], loc["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async virtual void OnContextMenuClick(ContextMenuClickEventArgs<TGetListOutputDto> args)
        {
            var loc = (IStringLocalizer)_L;

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

                    var res = await ModalManager.ConfirmationAsync(loc["DeleteConfirmationTitleBase"], loc["DeleteConfirmationDescriptionBase"]);


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

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public virtual void LineCalculate() { }

        public virtual void GetTotal() { }

        public virtual async void CrudModalShowing(PopupShowingEventArgs args)
        {
            if (DataSource.Id != Guid.Empty)
            {
                await BaseCrudService.UpdateConcurrencyFieldsAsync(DataSource.Id, true, LoginedUserService.UserId);
            }
        }

        public virtual async void CrudModalClosing(PopupClosingEventArgs args)
        {
            if (IsChanged)
            {
                await BaseCrudService.UpdateConcurrencyFieldsAsync(DataSource.Id, false, Guid.Empty);
                IsChanged = false;
            }
        }

        #region Grid Toolbar Methods


        public async void OnToolbarSearchChange(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                if (!string.IsNullOrEmpty(GridSearchText))
                {
                    await _grid.SearchAsync(GridSearchText.ToLower());

                    await JsRuntime.InvokeVoidAsync("focusInput", "srcText");
                }
                else
                {
                    await _grid.SearchAsync("");

                    await JsRuntime.InvokeVoidAsync("focusInput", "srcText");
                }
            }
        }

        public void CreateGridToolbar()
        {
            var loc = (IStringLocalizer)_L;

            RenderFragment search = (builder) =>
            {
                builder.OpenComponent(0, typeof(SfTextBox));
                builder.AddAttribute(1, "ID", "srcText");
                builder.AddAttribute(2, "CssClass", "TSITxtBox");
                builder.AddAttribute(3, "Value", BindConverter.FormatValue(GridSearchText));
                builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => GridSearchText = __value, GridSearchText));
                builder.AddAttribute(5, "onkeydown", OnToolbarSearchChange);
                builder.AddAttribute(6, "ShowClearButton", true);
                builder.CloseComponent();
            };


            GridToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = @loc["UIExportFileName"] });


            GridToolbarItems.Add(new ItemModel() { Id = "PDFExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIPdfIcon", TooltipText = @loc["UIExportFileName"] });


            GridToolbarItems.Add(new ItemModel() { Id = "Search", CssClass = "TSIToolbarTxtBox", Type = ItemType.Input, Template = search, Text = GridSearchText });


        }

        public async void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "ExcelExport")
            {
                ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();
                ExcelExportProperties.FileName = args.Item.TooltipText + ".xlsx";
                await this._grid.ExportToExcelAsync(ExcelExportProperties);
            }

            if (args.Item.Id == "PDFExport")
            {
                PdfExportProperties PdfExportProperties = new PdfExportProperties();
                PdfExportProperties.PageOrientation = PageOrientation.Landscape;
                PdfExportProperties.FileName = args.Item.TooltipText + ".pdf";
                await this._grid.ExportToPdfAsync(PdfExportProperties);
            }

        }
        #endregion
    }
}
