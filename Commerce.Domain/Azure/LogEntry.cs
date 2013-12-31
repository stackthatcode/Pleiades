using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class LogEntry : TableEntity
    {
        // public string DeploymentId { get; set; } - THIS IS INTERESTING - ENHANCE LATER!!!        
        public string Level { get; set; }
        public string Message { get; set; }        
    }
}
