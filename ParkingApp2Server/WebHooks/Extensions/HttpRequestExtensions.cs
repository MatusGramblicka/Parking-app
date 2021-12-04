using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace WebHooks.Extensions
{
    public static class HttpRequestExtensions
    {
        // ReSharper disable once InconsistentNaming
        private const string SignatureHeaderKey = "sha256";

        // ReSharper disable once InconsistentNaming
        private const string SignatureHeaderValueTemplate = SignatureHeaderKey + "={0}";

        public static void AddAdditionalHeaders(this HttpRequestMessage request, Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                if (request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    continue;
                }

                if (request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    continue;
                }

                throw new Exception($"Invalid Header. Header: {header.Key}:{header.Value}");
            }
        }

        //https://docs.microsoft.com/en-us/azure/media-services/previous/media-services-dotnet-check-job-progress-with-webhooks
        public static void SignRequest(this HttpRequestMessage request, string serializedBody, string signingSecret,
            string signatureHeaderName, string mediaType = "application/json")
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(serializedBody))
            {
                throw new ArgumentNullException(nameof(serializedBody));
            }

            var secretBytes = Encoding.UTF8.GetBytes(signingSecret);

            using var hasher = new HMACSHA256(secretBytes);
            request.Content = new StringContent(serializedBody, Encoding.UTF8, mediaType);

            var data = Encoding.UTF8.GetBytes(serializedBody);
            var sha256 = hasher.ComputeHash(data);

            var headerValue = string.Format(CultureInfo.InvariantCulture, SignatureHeaderValueTemplate,
                BitConverter.ToString(sha256));

            request.Headers.Add(signatureHeaderName, headerValue);
        }
    }
}
