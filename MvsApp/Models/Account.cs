using System.Collections.Generic;

namespace MvsApplication.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string BalanceName { get; set; }
        public string BalanceNumber { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public virtual ICollection<Transaction> Transactions { get; set; }

        public override string ToString()
        {
            return $"Balance Name - {BalanceName}, LastName - {User.Lastname}";
        }
    }
}