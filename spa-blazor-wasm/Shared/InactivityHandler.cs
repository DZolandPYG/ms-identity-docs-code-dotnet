using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWasm // Match your project namespace
{
    public static class InactivityHandler
    {
        [JSInvokable("HandleInactivityLogout")]
        public static async Task HandleInactivityLogout()
        {
            Console.WriteLine("Logging out due to inactivity...");
            await JsInteropContext.JSRuntime.InvokeVoidAsync("msalInstance.logoutRedirect");
        }
    }
}
