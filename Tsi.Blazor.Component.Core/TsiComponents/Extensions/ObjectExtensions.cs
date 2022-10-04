using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Blazor.Component.Core.TsiComponents.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsEqual<T>(this T x, T y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return EqualityComparer<T>.Default.Equals(x, y);
        }

        public static async Task SafeDisposeAsync(this IAsyncDisposable disposable)
        {
            var disposableTask = disposable.DisposeAsync();

            try
            {
                await disposableTask;
            }
            catch when (disposableTask.IsCanceled)
            {
            }
            catch (Microsoft.JSInterop.JSDisconnectedException)
            {
            }
        }
    }
}
