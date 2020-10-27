using System.Collections.Generic;

namespace MvsApplication.ViewModels
{
    public class Transactions
    { 
        public string SenderAccountId { get; set; }
        
        public string BalanceName { get; set; }
        public string BalanceNumber { get; set; }
        public string ReceiverAccountId { get; set; }
        public decimal Sum { get; set; }
        
        public override string ToString()
        {
            return $"Sender Account -> {SenderAccountId}, Receiver Account -> {ReceiverAccountId}, Sum -> {Sum} ";
        }
    }
}