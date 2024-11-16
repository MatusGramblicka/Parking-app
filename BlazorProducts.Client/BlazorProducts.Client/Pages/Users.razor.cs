using Blazored.LocalStorage;
using BlazorProducts.Client.Features;
using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.Configuration;
using Entities.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages;

public partial class Users : IDisposable
{
    public List<UserLite> UsersList { get; set; } = new();
    public string LoggedUserName { get; set; }
    public PriviledgedUsersConfiguration priviledgedUsersConfiguration { get; set; }

    [Inject] public IUsersHttpRepository UsersHttpRepository { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    [Inject] public ILogger<Users> Logger { get; set; }
    [Inject] public ITenantDayRepo TenantDayRepo { get; set; }
    [Inject] public IOptions<PriviledgedUsersConfiguration> PriviledgedUsersSettings { get; set; }

    private bool _alreadyDisposed;

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Interceptor.RegisterBeforeSendEvent();

        await GetUsers();

        var token = await LocalStorageService.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            LoggedUserName = JwtParser.GetLoggedUserName(token);
            Logger.LogInformation(LoggedUserName);
        }

        priviledgedUsersConfiguration = PriviledgedUsersSettings.Value;
    }

    private async Task GetUsers()
    {
        UsersList = await UsersHttpRepository.GetUsers();

        Logger.LogInformation(JsonConvert.SerializeObject(UsersList));
    }

    private async Task BookAllDaysForTenant(UserLite user)
    {
        if (user.Priviledged)
        {
            // How many users are priviledged ones?				
            var users = await UsersHttpRepository.GetUsers();
            var priviledgedUsersCount = users.Count(w => w.Priviledged);
                
            if (priviledgedUsersCount >= priviledgedUsersConfiguration.MaxCount)
            {
                //todo some pop up to show
                await GetUsers();
                StateHasChanged();
                return;
            }
            //-----------------------------------

            await TenantDayRepo.BookAllDaysFortenant(new TenantSingle
            {
                TenantId = user.Email
            });
            await UsersHttpRepository.UpdatePriviledgeOfUser(user);
        }
        else
        {
            await TenantDayRepo.RemoveAllBookedDaysFromUser(new TenantSingle
            {
                TenantId = user.Email
            });
            await UsersHttpRepository.UpdatePriviledgeOfUser(user);
        }

        await GetUsers();
        StateHasChanged();
    }

    private async Task DeleteUser(UserLite user)
    {
        await UsersHttpRepository.DeleteUser(user);
        await GetUsers();
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_alreadyDisposed)
            return;

        if (disposing)
        {
            Interceptor.DisposeEvent();
            _alreadyDisposed = true;
        }
    }

    ~Users()
    {
        Dispose(disposing: false);
    }
}