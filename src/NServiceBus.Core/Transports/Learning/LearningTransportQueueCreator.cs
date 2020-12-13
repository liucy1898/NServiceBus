namespace NServiceBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Transport;

    class LearningTransportQueueCreator : ICreateQueues
    {
        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity, CancellationToken token) => Task.CompletedTask;
    }
}