namespace TsiErp.ErpUI.Services.Base
{
    public class BaseApplicationService<TGetOutputDto,TGetListOutputDto,TCreateInput,TUpdateInput>
    {
        private readonly HttpClient httpClient;

        public BaseApplicationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TGetListOutputDto>> GetListAsync()
        {
            var list = await httpClient.GetFromJsonAsync<List<TGetListOutputDto>>("Products/getproductsbyrange/");

            return list;
        }
    }
}
