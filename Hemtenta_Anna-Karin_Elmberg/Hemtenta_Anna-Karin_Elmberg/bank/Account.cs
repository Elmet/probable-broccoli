using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    public class Account : IAccount
    {
        double balance;

        public double Amount
        {
            get
            {
                return balance;
            } 
        }

        public void Deposit(double amount)
        {
            if (amount <= 0 || double.IsNaN(amount) || double.IsNegativeInfinity(amount) || double.IsPositiveInfinity(amount) || amount.Equals(double.MinValue))
            { 
                throw new IllegalAmountException();
            }

            balance += amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            if (destination == null)
            {
                throw new OperationNotPermittedException();
            }

            if (amount <= 0 || double.IsNaN(amount) || double.IsNegativeInfinity(amount) || double.IsPositiveInfinity(amount) || amount.Equals(double.MinValue))
            {
                throw new IllegalAmountException();
            }

            if (amount > balance)
            {
                throw new InsufficientFundsException();
            }

            Withdraw(amount);
            destination.Deposit(amount);
        }

        public void Withdraw(double amount)
        {
            if (amount <= 0 || double.IsNaN(amount) || double.IsNegativeInfinity(amount) || double.IsPositiveInfinity(amount) || amount.Equals(double.MinValue))
            {
                throw new IllegalAmountException();
            }

            if (amount > balance)
            {
                throw new InsufficientFundsException();
            }

            balance -= amount;
        }
    }
}
