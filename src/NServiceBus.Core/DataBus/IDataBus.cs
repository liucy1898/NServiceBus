namespace NServiceBus.DataBus
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The main interface for interactions with the databus.
    /// </summary>
    public interface IDataBus
    {
        /// <summary>
        /// Gets a data item from the bus.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while getting the data item.</param>
        /// <returns>The data <see cref="Stream" />.</returns>
        Task<Stream> Get(string key, CancellationToken token);

        /// <summary>
        /// Adds a data item to the bus and returns the assigned key.
        /// </summary>
        /// <param name="stream">A create containing the data to be sent on the databus.</param>
        /// <param name="timeToBeReceived">The time to be received specified on the message type. TimeSpan.MaxValue is the default.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to observe while adding the data item.</param>
        Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken token);

        /// <summary>
        /// Called when the bus starts up to allow the data bus to active background tasks.
        /// </summary>
        Task Start(CancellationToken token);
    }
}