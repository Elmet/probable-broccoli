using System;
using NUnit.Framework;
using HemtentaTdd2017;

namespace UnitTestProjectHemtenta
{
    [TestFixture]
    public class AccountTest
    {
        private Account bankAccount_A, bankAccount_B;
        private double balance_bankAccount_A;
        private double balance_bankAccount_B;


        [SetUp]
        public void Setup()
        {
            bankAccount_A = new Account();
            bankAccount_B = new Account();
            balance_bankAccount_A = 5000.00;
            balance_bankAccount_B = 3000.00;
        }

        [Test]
        [TestCase(-1000.00)]
        [TestCase(0.00)]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        public void Make_Deposit_Incorrect_Parameters_Throws(double amount)
        {
            Assert.That(() => bankAccount_A.Deposit(amount), Throws.TypeOf<IllegalAmountException>());
        }

        [Test] // After the deposit the account's amount must increase with the same amount as the deposit
               // Check the balance
        [TestCase(1000.00, 6000.00)]
        public void Make_Deposit_Success(double amount, double expected)
        {
            bankAccount_A.Deposit(amount);
            var newBalanceA_AfterDeposit = balance_bankAccount_A + amount;
            Assert.AreEqual(expected, newBalanceA_AfterDeposit);   
        }

        [Test]
        [TestCase(-1000.00)]
        [TestCase(0.00)]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        public void Make_Withdraw_Incorrect_Parameters_Throws(double amount)
        {
            Assert.That(() => bankAccount_A.Withdraw(amount), Throws.TypeOf<IllegalAmountException>());
        }

        [Test] // After the withdraw the amount on the account must decrease with the same amount as the withdraw
                // Check the balance and also check that the withdraw isn't larger then the amount on the account
        [TestCase(1000.00, 4000.00)]
        [TestCase(5000.00, 0.00)]
        public void Make_Withdraw_Success(double amount, double expected)
        {
            bankAccount_A.Deposit(amount);
            bankAccount_A.Withdraw(amount);
            var newBalanceA_AfterWithdraw = balance_bankAccount_A - amount;
            Assert.AreEqual(expected, newBalanceA_AfterWithdraw);
        }

        [Test] 
        [TestCase(6000.00)]
        public void Make_Withdraw_LargerThen_AmountOfAccount_Throws(double amount)
        { 
            if (amount > balance_bankAccount_A)
            {
                Assert.That(() => bankAccount_A.Withdraw(amount), Throws.TypeOf<InsufficientFundsException>());
            }
        }

        [Test] // Check transfer ok, that same amount as the withdraw is sent to the destination account
        [TestCase(2000.00)]
        [TestCase(5000.00)]
        public void Make_Transfer_Funds_Success(double amount)
        {
            bankAccount_A.Deposit(amount);
            bankAccount_A.TransferFunds(bankAccount_B, amount);
            var newBalanceA_AfterTransfer = balance_bankAccount_A - amount;
            var newBalanceB_AfterTransfer = balance_bankAccount_B + amount;

            Assert.That(newBalanceB_AfterTransfer - balance_bankAccount_B, Is.EqualTo(balance_bankAccount_A - newBalanceA_AfterTransfer));
        }

        [Test] // Destination account doesn't exist
        public void Make_Transfer_Funds_Fail_ThrowsEx()
        {
            Assert.That(() => bankAccount_A.TransferFunds(null, 500.00), Throws.TypeOf<OperationNotPermittedException>());
        }
    }
}
