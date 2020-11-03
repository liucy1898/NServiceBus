namespace NServiceBus.Transport
{
    using System;

    /// <summary>
    /// Represents the result for configuring the transport for subscribing.
    /// </summary>
    public class TransportSubscriptionInfrastructure
    {
        /// <summary>
        /// Creates new result object.
        /// </summary>
        public TransportSubscriptionInfrastructure(Func<ISubscriptionManager> subscriptionManagerFactory)
        {
            Guard.AgainstNull(nameof(subscriptionManagerFactory), subscriptionManagerFactory);
            SubscriptionManagerFactory = subscriptionManagerFactory;
        }

        internal Func<ISubscriptionManager> SubscriptionManagerFactory { get; }
    }
}