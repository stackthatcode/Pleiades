using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class LogEntry : TableEntity
    {
        public string Level { get; set; }
        public string Message { get; set; }        
    }
}
