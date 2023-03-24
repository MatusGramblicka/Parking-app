using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Components;

public partial class WebhookTable
{
    [Parameter] public List<WebHookSubscription> Webhooks { get; set; }

    [Parameter] public EventCallback<Guid> OnDelete { get; set; }

    private Confirmation _confirmation;
    private Guid _webhookIdToDelete;

    private void CallConfirmationModal(Guid id)
    {
        _webhookIdToDelete = id;
        _confirmation.Show();
    }

    private async Task DeleteWebhook()
    {
        _confirmation.Hide();
        await OnDelete.InvokeAsync(_webhookIdToDelete);
    }
}