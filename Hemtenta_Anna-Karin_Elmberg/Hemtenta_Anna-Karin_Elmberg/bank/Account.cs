using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    public class Account : IAccount
    {
        public double Amount { get; set; }

        public void Deposit(double amount)
        {
            if (amount <= 0 || double.IsNaN(amount) || double.IsNegativeInfinity(amount) || double.IsPositiveInfinity(amount) || amount.Equals(double.MinValue))
                throw new IllegalAmountException();
            Amount += amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
