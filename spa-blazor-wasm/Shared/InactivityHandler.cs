using System;
using System.Threading.Tasks;

namespace BlazorWasm // Match your project namespace
{
    public static class InactivityHandler
    {
        public static async Task HandleInactivityLogout()
        {
            Console.WriteLine("Logging out due to inactivity...");
        }
    }
}
