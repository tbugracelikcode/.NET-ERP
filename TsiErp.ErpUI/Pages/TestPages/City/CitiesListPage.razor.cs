using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.Continent.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;
using TsiErp.Entities.Entities.TestManagement.District.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Entities.TestManagement.City;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Spinner;

namespace TsiErp.ErpUI.Pages.TestPages.City
{
    public partial class CitiesListPage : IDisposable
    {
        private SfGrid<SelectCityLinesDto> _LineGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        #region ComboBox 

        public IEnumerable<SelectCitiesDto> types = GetEnumDisplayTypeNames<CityTypeFormEnum>();

        public static List<SelectCitiesDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<CityTypeFormEnum>()
                       .Select(x => new SelectCitiesDto
                       {
                           CityTypeForm = x,
                           CityTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        public class BigCityComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<BigCityComboBox> bigcityComboBox = new List<BigCityComboBox>
        {
            new BigCityComboBox(){ID = "Yes", Text="YesBig"},
            new BigCityComboBox(){ID = "No", Text="NoBig"}
        };

        private void BigCityComboBoxValueChangeHandler(ChangeEventArgs<string, BigCityComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Yes":
                        DataSource.BigCityIs = L["YesBig"].Value;
                        break;

                    case "No":
                        DataSource.BigCityIs = L["NoBig"].Value;
                        break;


                    default: break;
                }
            }
        }

        #endregion


        [Inject]
        ModalManager ModalManager { get; set; }

        SelectCityLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectCityLinesDto> GridLineList = new List<SelectCityLinesDto>();


        private bool LineCrudPopup = false;

        private bool VisibleProperty { get; set; } = false;
        protected override async Task OnSubmit()
        {

            VisibleProperty = true;

            if (DataSource.Id == Guid.Empty)
            {
                for (int i = 0; i <= 20; i++)
                {

                    DataSource.CityTypeName = i.ToString() + "- Deneme";
                    DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CitiesChildMenu");
                    var createInput = ObjectMapper.Map<SelectCitiesDto, CreateCitiesDto>(DataSource);

                    await CreateAsync(createInput);


                }

            }
            VisibleProperty = false;

            await GetListDataSourceAsync();


            HideEditPage();

            await InvokeAsync(StateHasChanged);






        }

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = CitiesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CitiesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion


            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectCitiesDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("CitiesChildMenu")
            };

            DataSource.SelectCityLines = new List<SelectCityLinesDto>();
            GridLineList = DataSource.SelectCityLines;

            foreach (var item in types)
            {
                item.CityTypeName = L[item.CityTypeName];
            }

            foreach (var item in bigcityComboBox)
            {
                item.ID = L[item.ID];
            }

            foreach (var item in bigcityComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;


            await Task.CompletedTask;
        }
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
                            case "CityLinesContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CityLinesContextAdd"], Id = "new" }); break;
                            case "CityLinesContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CityLinesContextChange"], Id = "changed" }); break;
                            case "CityLinesContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CityLinesContextDelete"], Id = "delete" }); break;
                            case "CityLinesContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CityLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
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
                            case "CitiesContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CitiesContextAdd"], Id = "new" }); break;
                            case "CitiesContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CitiesContextChange"], Id = "changed" }); break;
                            case "CitiesContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CitiesContextDelete"], Id = "delete" }); break;
                            case "CitiesContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CitiesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }
        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListCitiesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await CitiesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectCityLines;


                    ShowEditPage();


                    foreach (var item in bigcityComboBox)
                    {
                        item.ID = L[item.ID];
                    }

                    foreach (var item in bigcityComboBox)
                    {
                        item.Text = L[item.Text];
                    }
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
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
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectCityLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectCityLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectCityLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectCityLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectCityLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();

                        DataSource.Population_ = GridLineList.Sum(t => t.DistrictPopulation);
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCityLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCityLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCityLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectCityLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectCityLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCityLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectCityLines;

            DataSource.Population_ = GridLineList.Sum(t => t.DistrictPopulation);
            await _LineGrid.Refresh();

            HideLinesPopup();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CitiesChildMenu");
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
