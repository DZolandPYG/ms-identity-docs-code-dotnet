//using System;
//using System.Timers;
//using System.Threading.Tasks;
//using Microsoft.JSInterop;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
//using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;


//public class SessionManager
//    {
//        private readonly NavigationManager _navigation;
//        private readonly IJSRuntime _jsRuntime;
//        private System.Timers.Timer _logoutTimer = new System.Timers.Timer();

//        public SessionManager(NavigationManager navigation, IJSRuntime jsRuntime)
//        {
//            _navigation = navigation;
//            _jsRuntime = jsRuntime;
//        }

//        public async Task StartSessionTimerAsync(int timeoutMinutes = 1)
//        {
//            // Get session start time from sessionStorage
//            var sessionStartStr = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "sessionStart");

//            if (long.TryParse(sessionStartStr, out var sessionStartMs))
//            {
//                var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//                var elapsedMs = nowMs - sessionStartMs;
//                var timeoutMs = timeoutMinutes * 60 * 1000;
//                var remainingMs = timeoutMs - elapsedMs;

//                if (remainingMs <= 0)
//                {
//                    await ForceLogout();
//                    return;
//                }

//                _logoutTimer = new System.Timers.Timer(remainingMs);
//                _logoutTimer.Elapsed += (s, e) => ForceLogout();
//                _logoutTimer.AutoReset = false;
//                _logoutTimer.Start();
//            }
//            else
//            {
//                // If no session start time is found, set it now and restart timer
//                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "sessionStart", now.ToString());
//                await StartSessionTimerAsync(timeoutMinutes);
//            }
//        }

//        private async Task ForceLogout()
//        {
//            _logoutTimer?.Stop();
//        await Task.Delay(1000); // Wait 1 second
//        _navigation.NavigateToLogout("authentication/logout");
//        //_navigation.Navigate("authentication/logout", true);
//        }
//    }

