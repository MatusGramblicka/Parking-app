using System.Net;

namespace WebHooks.Contracts
{
    public class WebHookSendResponse
    {
        public bool IsSucceed { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseContent { get; set; }
    }
}
