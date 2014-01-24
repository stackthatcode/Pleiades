using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class MailDumpRow : TableEntity
    {
        public string Content { get; set; }
    }
}
