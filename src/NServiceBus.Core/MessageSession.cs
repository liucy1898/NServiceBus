namespace NServiceBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class MessageSession : IMessageSession
    {
        public MessageSession(RootContext context)
        {
            this.context = context;
            messageOperations = context.Get<MessageOperations>();
        }

        public Task Send(object message, SendOptions sendOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(message), message);
            Guard.AgainstNull(nameof(sendOptions), sendOptions);
            return messageOperations.Send(context, message, sendOptions, token);
        }

        public Task Send<T>(Action<T> messageConstructor, SendOptions sendOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(messageConstructor), messageConstructor);
            Guard.AgainstNull(nameof(sendOptions), sendOptions);
            return messageOperations.Send(context, messageConstructor, sendOptions, token);
        }

        public Task Publish(object message, PublishOptions publishOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(message), message);
            Guard.AgainstNull(nameof(publishOptions), publishOptions);
            return messageOperations.Publish(context, message, publishOptions, token);
        }

        public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(messageConstructor), messageConstructor);
            Guard.AgainstNull(nameof(publishOptions), publishOptions);
            return messageOperations.Publish(context, messageConstructor, publishOptions, token);
        }

        public Task Subscribe(Type eventType, SubscribeOptions subscribeOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(eventType), eventType);
            Guard.AgainstNull(nameof(subscribeOptions), subscribeOptions);
            return messageOperations.Subscribe(context, eventType, subscribeOptions, token);
        }

        public Task Unsubscribe(Type eventType, UnsubscribeOptions unsubscribeOptions, CancellationToken token)
        {
            Guard.AgainstNull(nameof(eventType), eventType);
            Guard.AgainstNull(nameof(unsubscribeOptions), unsubscribeOptions);
            return messageOperations.Unsubscribe(context, eventType, unsubscribeOptions, token);
        }

        RootContext context;
        MessageOperations messageOperations;
    }
}