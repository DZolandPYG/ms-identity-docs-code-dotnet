using Microsoft.JSInterop;

namespace BlazorWasm // Use your actual root namespace
{
    public static class JsInteropContext
    {
        public static IJSRuntime JSRuntime { get; set; }
    }
}
