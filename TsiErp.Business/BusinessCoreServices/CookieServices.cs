using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Business.BusinessCoreServices
{
    public class CookieServices
    {
        private readonly IJSRuntime _jsRuntime;

        public CookieServices(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async void SetCookie(string name, string value, int days)
        {
            await _jsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", name, value, days);
        }

        public async Task<string> GetCookie(string name)
        {
            return await _jsRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", name);
        }
    }
}
