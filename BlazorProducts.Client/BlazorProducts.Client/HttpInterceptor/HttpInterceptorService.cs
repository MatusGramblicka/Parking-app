using Blazored.Toast.Services;
using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace BlazorProducts.Client.HttpInterceptor;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navManager;
    private readonly IToastService _toastService;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly IAuthenticationService _authenticationService;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<HttpInterceptorService> _logger;

    public HttpInterceptorService(HttpClientInterceptor interceptor,
        NavigationManager navManager, IToastService toastService, IAuthenticationService authenticationService,
        RefreshTokenService refreshTokenService, NavigationManager navigationManager, ILogger<HttpInterceptorService> logger)
    {
        _interceptor = interceptor;
        _navManager = navManager;
        _toastService = toastService;
        _refreshTokenService = refreshTokenService;
        _authenticationService = authenticationService;
        _navigationManager = navigationManager;
        _logger = logger;
    }

    public void RegisterEvent() => _interceptor.AfterSend += HandleResponse;

    public void RegisterBeforeSendEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
        _interceptor.BeforeSendAsync += InterceptBeforeSendAsync;
    }

    public void DisposeEvent()
    {
        _interceptor.AfterSend -= HandleResponse;
        //_interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
    }

    private async Task InterceptBeforeSendAsync(object sender,
        HttpClientInterceptorEventArgs e)
    {
        var absolutePath = e.Request.RequestUri.AbsolutePath;

        if (!absolutePath.Contains("token") && !absolutePath.Contains("account"))
        {
            var authToken = await _refreshTokenService.TryRefreshToken();

            if (authToken.IsAuthSuccessful.HasValue && authToken.IsAuthSuccessful == false)
            {
                _logger.LogInformation("Auth token auth is not successful");
                await _authenticationService.Logout();
                _logger.LogInformation("LogOut performed");
                _navigationManager.NavigateTo("/login");
                _logger.LogInformation("Navigate to login");
                // or _navigationManager.NavigateTo("/logout");

                return;
            }
            _logger.LogInformation("Auth token auth is successful");

            var token = authToken.Token;

            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    private void HandleResponse(object sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response == null)
        {
            _navManager.NavigateTo("/error");
            throw new HttpResponseException("Server not available.");
        }

        var message = "";

        if (!e.Response.IsSuccessStatusCode)
        {
            switch (e.Response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    _navManager.NavigateTo("/404");
                    message = "Resource not found.";
                    break;
                case HttpStatusCode.BadRequest:
                    message = "Invalid request. Please try again.";
                    _toastService.ShowError(message);
                    break;
                case HttpStatusCode.Unauthorized:
                    _navManager.NavigateTo("/unauthorized");
                    message = "Unauthorized access";
                    break;
                default:
                    _navManager.NavigateTo("/error");
                    message = "Something went wrong. Please contact the administrator.";
                    break;
            }

            throw new HttpResponseException(message);
        }
    }
}