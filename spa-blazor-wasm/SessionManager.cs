using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

public class SessionManager
{
    private readonly NavigationManager _navigation;
    private readonly IJSRuntime _jsRuntime;
    private System.Timers.Timer _logoutTimer = new System.Timers.Timer();
    private readonly IAccessTokenProvider _tokenProvider;
    private ElapsedEventHandler? _logoutHandler;



    public SessionManager(NavigationManager navigation, IJSRuntime jsRuntime, IAccessTokenProvider tokenProvider)
    {
        _navigation = navigation;
        _jsRuntime = jsRuntime;
        _tokenProvider = tokenProvider;
    }


    public async Task StartSessionTimerAsync(int timeoutMinutes = 5)
    {
        _logoutTimer?.Stop();
        if (_logoutHandler != null)
            _logoutTimer.Elapsed -= _logoutHandler;
        _logoutTimer?.Dispose();

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "sessionStart", now.ToString());

        var timeoutMs = timeoutMinutes * 60 * 1000;


        _logoutTimer = new System.Timers.Timer(timeoutMs);
        _logoutTimer.Elapsed += OnLogoutTimerElapsed;
        _logoutTimer.AutoReset = false;
        _logoutTimer.Start();

    }

    private async Task ForceLogoutAsync()
    {
        _logoutTimer?.Stop();
        _logoutTimer?.Dispose();

        await Task.Delay(1000); // Avoid MSAL race conditions

        var tokenResult = await _tokenProvider.RequestAccessToken(new AccessTokenRequestOptions
        {
            Scopes = new[] { "openid", "profile", "email" }
        });

        if (tokenResult.TryGetToken(out var token))
        {
            var idToken = token.Value;

            string loginHint = JwtHelper.ExtractLoginHint(idToken);
            string tenantId = "7d0e67e6-a37b-445e-82f0-6cc16260055e";

            await _jsRuntime.InvokeVoidAsync("alert", $"First ForceLogoutAsync triggered at {DateTime.Now}; loginHint: {loginHint}");

            var logoutUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/logout?id_token_hint={idToken}&logout_hint={loginHint}&post_logout_redirect_uri=http://localhost:5000/logged-out";

            //_navigation.NavigateTo(logoutUrl, forceLoad: true);
            await _jsRuntime.InvokeVoidAsync("revokeRefreshToken");
            _navigation.NavigateToLogout(logoutUrl);


        }
        else
        {
            Console.WriteLine("Failed to grab token!");
            await _jsRuntime.InvokeVoidAsync("alert", $"Second ForceLogoutAsync triggered at {DateTime.Now}");
            // Fallback if token retrieval fails
            _navigation.NavigateTo("authentication/logout");
        }
    }



    private async void OnLogoutTimerElapsed(object sender, ElapsedEventArgs e)
    {
        await ForceLogoutAsync();
    }


    public static class JwtHelper
    {
        public static string? ExtractLoginHint(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            // Try common claim types for login hint
            var loginHint = token.Claims.FirstOrDefault(c =>
                c.Type == "preferred_username" || c.Type == "upn" || c.Type == "email")?.Value;

            return loginHint;
        }
    }



}

