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
            balance_bankAccount_A = 500.15;
            balance_bankAccount_B = 300.20;

        }


        [Test]
        [TestCase(-1.00)]
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
        public void Make_Deposit_Success(double amount)
        {
            bankAccount_A.Deposit(amount);
            var newBalanceA_AfterDeposit = balance_bankAccount_A + amount;
            Assert.That(bankAccount_A.Amount, Is.EqualTo(newBalanceA_AfterDeposit));   
        }

        [Test]
        public void Make_Withdraw_Incorrect_Parameters_Throws(double amount)
        {

        }

        [Test] // After the withdraw the amount on the account must decrease with the same amount as the withdraw
                // Check the balance and also check that the withdraw isn't larger then the amount on the account
        public void Make_Withdraw_Success(double amount)
        {

        }

        //[Test]
        //public void 
    }
}
