namespace NServiceBus.Transport
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensibility;

    /// <summary>
    /// Implemented by transports to provide pub/sub capabilities.
    /// </summary>
    public interface IManageSubscriptions
    {
        /// <summary>
        /// Subscribes to the given event.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="context">The current context.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while subscribing.</param>
        Task Subscribe(Type eventType, ContextBag context, CancellationToken token);

        /// <summary>
        /// Unsubscribes from the given event.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="context">The current context.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while unsubscribing.</param>
        Task Unsubscribe(Type eventType, ContextBag context, CancellationToken token);
    }
}