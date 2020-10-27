using System.Collections.Generic;

namespace MvsApplication.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string SenderAccountId { get; set; }
        public string ReceiverAccountId { get; set; }
        public decimal Sum { get; set; }
        public virtual Account Accounts { get; set; }
        public override string ToString()
        {
            return $"Sender Account -> {SenderAccountId}, Receiver Account -> {ReceiverAccountId}, Sum -> {Sum} ";
        }
    }
}