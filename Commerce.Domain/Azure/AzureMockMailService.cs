using System;
using Commerce.Application.Email;
using Commerce.Application.Email.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class AzureMockMailService : IEmailService
    {
        private readonly string _applicationName;
        private readonly CloudTable _mailDumpTable;
        private const string TableName = "MailDump";

        public AzureMockMailService(AzureConfiguration configuration, string applicationName)
        {
            _applicationName = applicationName;
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(configuration.StorageConnectionString);

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Get table reference 
            _mailDumpTable = tableClient.GetTableReference(TableName);

            //Create the table if it doesn't exist.
            _mailDumpTable.CreateIfNotExists();
        }

        public void Cleanup()
        {
            // Create the table query.
            var cutoffTime = DateTime.Now.AddDays(-2);
            var timestamp = MockEmailService.FormatTimeStamp(cutoffTime);

            var rangeQuery = new TableQuery<MailDumpRow>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _applicationName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, timestamp)));

            foreach (var row in _mailDumpTable.ExecuteQuery(rangeQuery))
            {
                var deleteOp = TableOperation.Delete(row);
                _mailDumpTable.Execute(deleteOp);
            }
        }

        public void Send(EmailMessage emailMessage)
        {
            Cleanup();

            var emailIdentifier = MockEmailService.CreateEmailIdentifier(emailMessage.To);
            var content = MockEmailService.CreateEmailContent(emailMessage);
            var row = new MailDumpRow()
            {
                Content = content,
                RowKey = emailIdentifier,
                PartitionKey = _applicationName
            };

            var insertOperation = TableOperation.Insert(row);
            _mailDumpTable.Execute(insertOperation);
        }
    }
}
