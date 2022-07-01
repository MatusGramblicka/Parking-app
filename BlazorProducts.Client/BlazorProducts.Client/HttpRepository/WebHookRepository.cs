using Entities.DTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository
{
    public class WebHookRepository : IWebHookRepository
    {
        private readonly HttpClient _client;

        public WebHookRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<WebHookSubscription>> GetWebhooks()
        {
            var webhooksResult = await _client.GetFromJsonAsync<List<WebHookSubscription>>("webhooks");

            return webhooksResult;
        }

        public async Task<WebHookSubscription> GetWebhook(Guid id)
        {
            var person = await _client.GetFromJsonAsync<WebHookSubscription>($"webhooks/{id}");

            return person;
        }

        public async Task CreateWebhook(WebHookSubscriptionForCreationDto webhook)
            => await _client.PostAsJsonAsync("webhooks", webhook);

        public async Task DeleteWebhook(Guid webhookId)
            => await _client.DeleteAsync(Path.Combine("webhooks", webhookId.ToString()));

        public async Task UpdateWebhook(Guid webhookId, WebHookSubscriptionForUpdateDto webhook)
            => await _client.PutAsJsonAsync(Path.Combine("webhooks",
                webhookId.ToString()), webhook);

    }
}