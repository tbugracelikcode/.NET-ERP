using Microsoft.AspNetCore.Components;

namespace TsiErp.ErpUI.Pages.Base
{
    public class BaseListPage<TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> : ComponentBase
    {
        public HttpClient HttpClient { get; set; }

        public virtual async Task<List<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            var list = await HttpClient.GetFromJsonAsync<List<TGetListOutputDto>>("https://localhost:7258/Branches/GetListAsync/" + input);

            return list;
        }
    }
}
