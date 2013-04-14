using System;

namespace Commerce.Persist.Model.Orders
{
    public class OrderNote
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        public OrderNote(string content)
        {
            this.Content = content;
            this.DateCreated = DateTime.Today;
        }
    }
}
