using System;

namespace Commerce.Application.Orders.Entities
{
    public class OrderNote
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        public OrderNote(string content)
        {
            this.Content = content;
            this.DateCreated = DateTime.UtcNow;
        }
    }
}
