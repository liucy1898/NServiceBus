namespace NServiceBus.Transport
{
    using NServiceBus.Settings;

    /// <summary>
    /// Extension methods for access to various transport address helpers.
    /// </summary>
    public static class LogicalAddressExtensions
    {
        /// <summary>
        /// Gets the native transport address for the given logical address.
        /// </summary>
        /// <returns>The native transport address.</returns>
        public static string GetTransportAddress(this ReadOnlySettings settings, LogicalAddress logicalAddress)
        {
            Guard.AgainstNull(nameof(settings), settings);
            Guard.AgainstNull(nameof(logicalAddress), logicalAddress);
            return settings.Get<TransportDefinition>().ToTransportAddress(logicalAddress.ToEndpointAddress());
        }
    }
}