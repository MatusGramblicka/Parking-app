using Blazored.Toast.Services;
using BlazorProducts.Client.HttpInterceptor;
using BlazorProducts.Client.HttpRepository;
using Entities.DTO;
using Entities.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages
{
	public partial class CreateWebHook
	{
		private WebHookSubscriptionForCreationDto _webhook = new WebHookSubscriptionForCreationDto();
		private EditContext _editContext;
		private bool formInvalid = true;

		[Inject]
		public IWebHookRepository WebhookRepo { get; set; }

		[Inject]
		public HttpInterceptorService Interceptor { get; set; }

		[Inject]
		public IToastService ToastService { get; set; }

		protected override void OnInitialized()
		{
			_webhook.IsActive = true;
			_webhook.FailureHandlingStrategyFlags = FailureHandlingStrategy.LogFailure;
			_webhook.MaxSendAttemptCount = 3;
			_webhook.SignatureHeaderName = "";
			_webhook.SigningSecret = "";


			_editContext = new EditContext(_webhook);
			_editContext.OnFieldChanged += HandleFieldChanged;
			Interceptor.RegisterEvent();
		}

		private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
		{
			formInvalid = !_editContext.Validate();
			StateHasChanged();
		}

		private async Task Create()
		{
			await WebhookRepo.CreateWebhook(_webhook);

			ToastService.ShowSuccess($"Action successful. " +
				$"Webhook \"{_webhook.WebHookUri}\" successfully added.");
			_webhook = new WebHookSubscriptionForCreationDto();
			_editContext.OnValidationStateChanged += ValidationChanged;
			_editContext.NotifyValidationStateChanged();
		}

		private void ValidationChanged(object sender, ValidationStateChangedEventArgs e)
		{
			formInvalid = true;
			_editContext.OnFieldChanged -= HandleFieldChanged;
			_editContext = new EditContext(_webhook);
			_editContext.OnFieldChanged += HandleFieldChanged;
			_editContext.OnValidationStateChanged -= ValidationChanged;
		}

		public void Dispose()
		{
			Interceptor.DisposeEvent();
			_editContext.OnFieldChanged -= HandleFieldChanged;
			_editContext.OnValidationStateChanged -= ValidationChanged;
		}
	}
}
