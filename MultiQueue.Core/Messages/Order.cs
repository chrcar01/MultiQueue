using System;

namespace MultiQueue.Core.Messages
{
    public class Order
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public int SalesRepId { get; set; }
    }
}
