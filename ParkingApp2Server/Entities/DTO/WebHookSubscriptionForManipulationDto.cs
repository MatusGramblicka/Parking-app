using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class WebHookSubscriptionForManipulationDto
    {
        [Required(ErrorMessage = "WebHookUri is a required field.")]
        [Url(ErrorMessage = "Please Enter Valid Uri")]
        [MaxLength(25, ErrorMessage = "Maximum length for the WebHookUri is 25 characters.")]
        public string WebHookUri { get; set; }
        [Required(ErrorMessage = "SigningSecret is a required field.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the SigningSecret is 15 characters.")]
        public string SigningSecret { get; set; }
        [Required(ErrorMessage = "SignatureHeaderName is a required field.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the SignatureHeaderName is 15 characters.")]
        public string SignatureHeaderName { get; set; }
        //public Dictionary<string, string> Headers { get; set; }
        public int MaxSendAttemptCount { get; set; }
        public bool IsActive { get; set; }
        public FailureHandlingStrategy FailureHandlingStrategyFlags { get; set; }
    }
}
