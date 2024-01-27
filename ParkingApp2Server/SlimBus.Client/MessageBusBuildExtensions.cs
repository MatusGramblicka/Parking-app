using SlimMessageBus;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

namespace SlimBus.Client;

public static class MessageBusBuildExtensions
{
    public static MessageBusBuilder CreateMemoryMessageBus()
    {
        return MessageBusBuilder
            .Create()
            .WithProviderMemory(cfg=>cfg.
                // Do not serialize the domain events and rather pass the same instance across handlers (faster) 
                EnableMessageSerialization = false
            );
        //.WithSerializer(new JsonMessageSerializer()); // no serializer  needed
    }

    public static MessageBusBuilder AddProducer<TMessage>(this MessageBusBuilder builder,
        string defaultProduceTopic)
    {
        builder.Produce<TMessage>(x => { x.DefaultTopic(defaultProduceTopic); });

        return builder;
    }

    public static MessageBusBuilder AddConsumer<TMessage, TConsumer>(this MessageBusBuilder builder,
        string messageSourceTopic) where TConsumer : class, IConsumer<TMessage>
    {
        builder.Consume<TMessage>(x => x.Topic(messageSourceTopic).WithConsumer<TConsumer>());

        return builder;
    }
}