namespace NServiceBus
{
    using System.Threading;
    using System.Threading.Tasks;

    class InternallyManagedContainerHost : IStartableEndpoint
    {
        public InternallyManagedContainerHost(IStartableEndpoint startableEndpoint, HostingComponent hostingComponent)
        {
            this.startableEndpoint = startableEndpoint;
            this.hostingComponent = hostingComponent;
        }

        public Task<IEndpointInstance> Start(CancellationToken token)
        {
            return hostingComponent.Start(startableEndpoint, token);
        }

        readonly IStartableEndpoint startableEndpoint;
        readonly HostingComponent hostingComponent;
    }
}