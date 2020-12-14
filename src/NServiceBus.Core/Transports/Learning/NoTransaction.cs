namespace NServiceBus
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    class NoTransaction : ILearningTransportTransaction
    {
        public NoTransaction(string basePath, string pendingDirName)
        {
            processingDirectory = Path.Combine(basePath, pendingDirName, Guid.NewGuid().ToString());
        }

        public string FileToProcess { get; private set; }

        public Task<bool> BeginTransaction(string incomingFilePath)
        {
            Directory.CreateDirectory(processingDirectory);
            FileToProcess = Path.Combine(processingDirectory, Path.GetFileName(incomingFilePath));

            return AsyncFile.Move(incomingFilePath, FileToProcess);
        }

        public Task Enlist(string messagePath, string messageContents, CancellationToken token) => AsyncFile.WriteText(messagePath, messageContents, token);

        public Task Commit() => Task.CompletedTask;

        public void Rollback() { }

        public void ClearPendingOutgoingOperations() { }

        public bool Complete()
        {
            Directory.Delete(processingDirectory, true);

            return true;
        }

        string processingDirectory;
    }
}