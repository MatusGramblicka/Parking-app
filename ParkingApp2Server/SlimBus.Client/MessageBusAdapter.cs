using Common.Utils.Utils;
using SlimMessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace SlimBus.Client
{
    public class MessageBusAdapter : IMessagePublisher
    {
        private readonly IMessageBus _messageBus;

        public MessageBusAdapter(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Publish<T>(T message, string topic = null)
        {
            AsyncHelper.RunSync(() => _messageBus.Publish(message, topic));
        }

        public TResult Publish<T, TResult>(T message, string topic = null)
        {
            var result = AsyncHelper.RunSync(() => _messageBus.Send<TResult, T>(message, topic));

            return result;
        }

        public Task PublishAsync<T>(T message, string topic = null, CancellationToken cancellationToken = default)
        {
            return _messageBus.Publish(message, topic);
        }

        public async Task<TResult> PublishAsync<T, TResult>(T message, string topic = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _messageBus.Send<TResult, T>(message, topic, cancellationToken: cancellationToken).ConfigureAwait(false);

            return result;
        }
    }
}