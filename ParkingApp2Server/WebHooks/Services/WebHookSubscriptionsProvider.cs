using AutoMapper;
using Entities.DTO;
using Repository.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.Contracts;

namespace WebHooks.Services
{
    public class WebHookSubscriptionsProvider : IWebHookSubscriptionsProvider
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public WebHookSubscriptionsProvider(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<WebHookSubscriptionDto>> GetSubscriptionsAsync(CancellationToken cancellationToken, bool activeOnly = true)
        {
            var webHookSubscriptionsFromDB = await _repository.WebHook.GetAllWebHookSubscriptionsAsync(false);

            var webHookSubscriptions = _mapper.Map<IEnumerable<WebHookSubscriptionDto>>(webHookSubscriptionsFromDB);

            return webHookSubscriptions.Where(s => s.IsActive == activeOnly).ToList();
        }

        //public List<WebHookSubscriptionDto> GetSubscriptions(bool activeOnly = true)
        //{
        //    return AsyncHelper.RunSync(() => GetSubscriptionsAsync(new CancellationToken(), activeOnly));
        //}
    }
}
