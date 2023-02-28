using Microsoft.Extensions.Localization;

namespace TsiErp.ErpUI.Shared
{
    public class PageLocalizer : IStringLocalizer
    {
        public LocalizedString this[string name] => throw new NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }
}
