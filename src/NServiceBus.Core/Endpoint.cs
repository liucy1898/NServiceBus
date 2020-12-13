namespace NServiceBus
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides factory methods for creating and starting endpoint instances.
    /// </summary>
    public static class Endpoint
    {
        /// <summary>
        /// Creates a new startable endpoint based on the provided configuration.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while creating the endpoint.</param>
        public static Task<IStartableEndpoint> Create(EndpointConfiguration configuration, CancellationToken token)
        {
            Guard.AgainstNull(nameof(configuration), configuration);

            return HostCreator.CreateWithInternallyManagedContainer(configuration, token);
        }

        /// <summary>
        /// Creates and starts a new endpoint based on the provided configuration.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while starting the endpoint.</param>
        public static async Task<IEndpointInstance> Start(EndpointConfiguration configuration, CancellationToken token)
        {
            Guard.AgainstNull(nameof(configuration), configuration);

            var startableEndpoint = await Create(configuration, token).ConfigureAwait(false);

            return await startableEndpoint.Start(token).ConfigureAwait(false);
        }
    }
}