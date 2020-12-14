namespace NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Pipeline;

    abstract class IncomingContext : BehaviorContext, IIncomingContext
    {
        protected IncomingContext(string messageId, string replyToAddress, IReadOnlyDictionary<string, string> headers, IBehaviorContext parentContext)
            : base(parentContext)
        {
            MessageId = messageId;
            ReplyToAddress = replyToAddress;
            MessageHeaders = headers;
        }

        MessageOperations messageOperations => Extensions.Get<MessageOperations>();

        public string MessageId { get; }

        public string ReplyToAddress { get; }

        public IReadOnlyDictionary<string, string> MessageHeaders { get; }

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

        public Task Reply(object message, ReplyOptions options, CancellationToken token)
        {
            return messageOperations.Reply(this, message, options, token);
        }

        public Task Reply<T>(Action<T> messageConstructor, ReplyOptions options, CancellationToken token)
        {
            return messageOperations.Reply(this, messageConstructor, options, token);
        }

        public Task ForwardCurrentMessageTo(string destination)
        {
            return IncomingMessageOperations.ForwardCurrentMessageTo(this, destination);
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