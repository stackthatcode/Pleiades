using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class AzureLogEntryRepository
    {
        private readonly CloudTable _logEntryTable;

        public AzureLogEntryRepository(AzureConfiguration configuration)
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(configuration.StorageConnectionString);

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Get table reference 
            _logEntryTable = tableClient.GetTableReference(configuration.LogEntryTable);

            //Create the table if it doesn't exist.
            _logEntryTable.CreateIfNotExists();
        }

        public void AddEntry(string loggerName, string level, string message)
        {
            var logTimeStamp = DateTime.Now;
            var entry = new LogEntry()
            {
                LoggerName = loggerName,
                Level = level,
                Message = message,
                DateTime = logTimeStamp,
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = string.Format("{0}-{1:yyyy-MM-dd}", loggerName, logTimeStamp)
            };

            // Create the TableOperation that inserts the customer entity.
            var insertOperation = TableOperation.Insert(entry);

            // Execute the insert operation.
            _logEntryTable.Execute(insertOperation);
        }
    }
}
