using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimBus.Client
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message, string topic = null);
        TResult Publish<T, TResult>(T message, string topic = null);
        Task PublishAsync<T>(T message, string topic = null, CancellationToken cancellationToken = default);
        Task<TResult> PublishAsync<T, TResult>(T message, string topic = null,
            CancellationToken cancellationToken = default);
    }
}
