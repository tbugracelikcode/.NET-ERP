using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch.Internal;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.ErpUI.Pages.Widget.StockWidget
{
    public class StockModuleWidgets
    {
        static List<ProductCardsOfProductGroups> ProductCardsOfProductGroupList = new List<ProductCardsOfProductGroups>();
        

        public static async Task<List<ProductCardsOfProductGroups>> GetProductList(IProductsAppService Service)
        {
            var productList = (await Service.GetListAsync(new ListProductsParameterDto())).Data.GroupBy(t => t.ProductGrp).ToList();

            foreach (var product in productList)
            {
                ProductCardsOfProductGroupList.Add(new ProductCardsOfProductGroups
                {
                    ProductGroupName = product.Key,
                    Count = product.Count()
                });
            }

            return ProductCardsOfProductGroupList;
        }
    }

    public class ProductCardsOfProductGroups
    {
        public string ProductGroupName { get; set; }

        public int Count { get; set; }
    }
}
