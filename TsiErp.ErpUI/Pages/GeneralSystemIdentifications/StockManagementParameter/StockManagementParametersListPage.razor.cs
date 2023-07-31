using Syncfusion.Blazor.Buttons;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.ModuleConstant;
using TsiErp.Entities.TableConstant;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.StockManagementParameter
{
    public partial class StockManagementParametersListPage
    {
        #region Değişkenler 

        public bool stockFiches = false;
        public Guid stockFichesId;

        #endregion

        List<ListStockManagementParametersDto> StockManagementParametersList = new List<ListStockManagementParametersDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = StockManagementParametersService;
            _L = L;


            StockManagementParametersList = (await StockManagementParametersService.GetListAsync(new ListStockManagementParametersParameterDto())).Data.ToList();

            stockFiches = StockManagementParametersList.Where(t => t.PageName == Tables.StockFiches).Select(t => t.FutureDateParameter).FirstOrDefault();
            stockFichesId = StockManagementParametersList.Where(t => t.PageName == Tables.StockFiches).Select(t => t.Id).FirstOrDefault();

        }

        private async void StockFichesChange(ChangeEventArgs<bool> args)
        {
            DataSource = (await StockManagementParametersService.GetAsync(stockFichesId)).Data;

            DataSource.FutureDateParameter = args.Checked;

            var updateInput = ObjectMapper.Map<SelectStockManagementParametersDto, UpdateStockManagementParametersDto>(DataSource);

            var result = (await UpdateAsync(updateInput)).Data;
        }
    }
}
