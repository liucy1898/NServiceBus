namespace NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Pipeline;

    abstract class OutgoingContext : BehaviorContext, IOutgoingContext
    {
        protected OutgoingContext(string messageId, Dictionary<string, string> headers, IBehaviorContext parentContext)
            : base(parentContext)
        {
            Headers = headers;
            MessageId = messageId;
        }

        MessageOperations messageOperations => Extensions.Get<MessageOperations>();

        public string MessageId { get; }

        public Dictionary<string, string> Headers { get; }

        public Task Send(object message, SendOptions options, CancellationToken token)
        {
            return messageOperations.Send(this, message, options, token);
        }

        public Task Send<T>(Action<T> messageConstructor, SendOptions options, CancellationToken token)
        {
            return messageOperations.Send(this, messageConstructor, options, token);
        }

        public Task Publish(object message, PublishOptions options, CancellationToken token)
        {
            return messageOperations.Publish(this, message, options, token);
        }

        public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions, CancellationToken token)
        {
            return messageOperations.Publish(this, messageConstructor, publishOptions, token);
        }

        public Task Subscribe(Type eventType, SubscribeOptions options, CancellationToken token)
        {
            return messageOperations.Subscribe(this, eventType, options, token);
        }

        public Task Unsubscribe(Type eventType, UnsubscribeOptions options, CancellationToken token)
        {
            return messageOperations.Unsubscribe(this, eventType, options, token);
        }
    }
}