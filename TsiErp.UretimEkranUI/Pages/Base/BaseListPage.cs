using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.Text;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.UretimEkranUI.Helpers;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages.Base
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

        [Inject]
        IJSRuntime JsRuntime { get; set; }

        public TGetOutputDto DataSource { get; set; }
        public IList<TGetListOutputDto> ListDataSource { get; set; }

        public SfGrid<TGetListOutputDto> _grid;

        public object _L { get; set; }


        public List<ItemModel> GridToolbarItems { get; set; } = new List<ItemModel>();

        public string GridSearchText { get; set; }

        protected ICrudAppService<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> BaseCrudService { get; set; }



        protected async override Task OnParametersSetAsync()
        {
            CreateGridToolbar();


            await GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);
        }


        #region Crud Operations

        protected async virtual Task<IDataResult<TGetOutputDto>> GetAsync(Guid id)
        {
            try
            {
                return await BaseCrudService.GetAsync(id);
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message);
                }

                return default(IDataResult<TGetOutputDto>);
            }
        }

        protected async virtual Task<IList<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {

            try
            {
                return (await BaseCrudService.GetListAsync(input)).Data.ToList();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message);
                }

                return default(IList<TGetListOutputDto>);
            }
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> CreateAsync(TCreateInput input)
        {
            try
            {
                return await BaseCrudService.CreateAsync(input);
            }
            catch (DuplicateCodeException exp)
            {
                await ModalManager.MessagePopupAsync("Bilgi", exp.Message);
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (ValidationException exp)
            {
                var errorList = exp.Errors.ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<ul>");

                for (int i = 0; i < errorList.Count; i++)
                {
                    sb.AppendLine("<li>" + errorList[i].ErrorMessage + "</li>");
                }

                sb.AppendLine("</ul>");

                await ModalManager.MessagePopupAsync("Bilgi", sb.ToString());
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message);
                }

                return new ErrorDataResult<TGetOutputDto>();
            }
        }

        protected async virtual Task<IDataResult<TGetOutputDto>> UpdateAsync(TUpdateInput input)
        {
            try
            {
                return await BaseCrudService.UpdateAsync(input);
            }
            catch (DuplicateCodeException exp)
            {
                await ModalManager.MessagePopupAsync("Bilgi", exp.Message);
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (ValidationException exp)
            {
                var errorList = exp.Errors.ToList();

                StringBuilder sb = new();
                sb.AppendLine("<ul>");

                for (int i = 0; i < errorList.Count; i++)
                {
                    sb.AppendLine("<li>" + errorList[i].ErrorMessage + "</li>");
                }

                sb.AppendLine("</ul>");

                await ModalManager.MessagePopupAsync("Bilgi", sb.ToString());
                return new ErrorDataResult<TGetOutputDto>();
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message);
                }
                return new ErrorDataResult<TGetOutputDto>();
            }
        }

        protected async virtual Task<IResult> DeleteAsync(Guid id)
        {
            try
            {
                return await BaseCrudService.DeleteAsync(id);
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message + "\n" + exp.InnerException.Message);
                }
                else
                {
                    await ModalManager.MessagePopupAsync("Hata", exp.Message);
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
        }

        public virtual void HideEditPage()
        {
            EditPageVisible = false;
            InvokeAsync(StateHasChanged);
        }

        public async virtual void ShowEditPage()
        {

            EditPageVisible = true;
            await InvokeAsync(StateHasChanged);

        }


        protected virtual async Task BeforeInsertAsync()
        {
            DataSource = new TGetOutputDto();

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public virtual void LineCalculate() { }

        public virtual void GetTotal() { }



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


            //GridToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = "Excel Aktar" });


            //GridToolbarItems.Add(new ItemModel() { Id = "PDFExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIPdfIcon", TooltipText = "PDF Aktar" });


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
