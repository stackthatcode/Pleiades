using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Commerce.Application.Azure
{
    public class LogEntry : TableEntity
    {
        // public string DeploymentId { get; set; } - THIS IS INTERESTING - ENHANCE LATER!!!        
        public string LoggerName { get; set; }        
        public DateTime DateTime { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }        
    }
}
