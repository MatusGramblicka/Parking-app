using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages;

public partial class WebHooks : IDisposable
{
    public List<WebHookSubscription> WebhookList { get; set; } = new();

    [Inject] public IWebHookRepository WebhookRepo { get; set; }

    [Inject] public HttpInterceptorService Interceptor { get; set; }

    private bool _alreadyDisposed;

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Interceptor.RegisterBeforeSendEvent();
        await GetWebhooks();
    }

    private async Task GetWebhooks()
    {
        WebhookList = await WebhookRepo.GetWebhooks();
    }

    private async Task DeleteWebhook(Guid id)
    {
        await WebhookRepo.DeleteWebhook(id);

        await GetWebhooks();
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

    ~WebHooks()
    {
        Dispose(disposing: false);
    }
}