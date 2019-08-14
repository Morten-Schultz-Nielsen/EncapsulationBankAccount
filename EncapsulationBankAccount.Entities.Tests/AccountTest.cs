using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.EntitiesTests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void TestAccount()
        {
            //Test creation of a new account (and if newly created accounts can have id 0)
            Account testAccount = new Account(100);
            Assert.AreEqual(0, testAccount.Id);
            Assert.IsTrue(testAccount.Created > DateTime.Now - new TimeSpan(0,0,60));
            Assert.AreEqual(100, testAccount.Balance);

            //Test creation of an existing account
            testAccount = new Account(1,50.10m, new DateTime(2019, 4, 4));
            Assert.AreEqual(1, testAccount.Id);
            Assert.AreEqual(new DateTime(2019, 4, 4), testAccount.Created);
            Assert.AreEqual(50.10m, testAccount.Balance);
        }

        [TestMethod]
        public void TestId()
        {
            //Tests if id isn't allowed to be 0 or smaller
            Assert.IsTrue(Account.ValidateId(1).Valid);
            Assert.IsFalse(Account.ValidateId(0).Valid);
        }

        [TestMethod]
        public void TestBalance()
        {
            //Tests if balance only can be in the interval [-999999999,99;999999999,99]
            Assert.IsTrue(Account.ValidateBalance(999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(999999999.999m).Valid);
            Assert.IsTrue(Account.ValidateBalance(-999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(-999999999.999m).Valid);
        }

        [TestMethod]
        public void TestCreated()
        {
            //Tests if creation date cannot be in the future
            Assert.IsTrue(Account.ValidateCreated(new DateTime(2019,8,12)).Valid);
            Assert.IsFalse(Account.ValidateCreated(DateTime.Now + new TimeSpan(0,0,60)).Valid);
        }

        [TestMethod]
        public void TestWithdraw()
        {
            Account testAccount = new Account(0);

            //Test withdraw higher bound
            testAccount.Withdraw(25000);
            Assert.AreEqual(-25000, testAccount.Balance);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Withdraw(25000.01m); });

            //Test withdrawing twice
            testAccount.Withdraw(1);
            Assert.AreEqual(-25001, testAccount.Balance);

            //Test withdraw lower bound
            testAccount.Withdraw(0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Withdraw(-0.01m); });
        }

        [TestMethod]
        public void TestDeposit()
        {
            Account testAccount = new Account(0);

            //Test deposit higher bound
            testAccount.Deposit(25000);
            Assert.AreEqual(25000, testAccount.Balance);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Deposit(25000.01m); });

            //test deposit twice
            testAccount.Deposit(1);
            Assert.AreEqual(25001, testAccount.Balance);

            //Test deposit lower bound
            testAccount.Deposit(0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Deposit(-0.01m); });
        }

        [TestMethod]
        public void TestGetDaysSinceCreation()
        {
            //Test if a newly created account only has existed for 0 days
            Account testAccount = new Account(1, 100, DateTime.Now);
            Assert.AreEqual(0, testAccount.GetDaysSinceCreation());

            //Test if an account created 5 days ago outputs correct amount
            DateTime daysLater = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 5);
            testAccount.Created = daysLater;
            Assert.AreEqual(5, testAccount.GetDaysSinceCreation());
        }
    }
}
