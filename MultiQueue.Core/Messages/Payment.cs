using System;

namespace MultiQueue.Core.Messages
{
    public class Payment
    {
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public Guid Id { get; set; }
    }
}
