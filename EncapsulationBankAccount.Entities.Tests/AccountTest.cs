using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.Entities.Tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void TestAccount()
        {
            Account testAccount = new Account(100);
            Assert.AreEqual(0, testAccount.Id);
            Assert.IsTrue(testAccount.Created > DateTime.Now - new TimeSpan(0,0,60));
            Assert.AreEqual(100, testAccount.Balance);

            testAccount = new Account(1,50.10m, new DateTime(2019, 4, 4));
            Assert.AreEqual(1, testAccount.Id);
            Assert.IsTrue(testAccount.Created == new DateTime(2019, 4, 4));
            Assert.AreEqual(50.10m, testAccount.Balance);
        }

        [TestMethod]
        public void TestId()
        {
            Assert.IsTrue(Account.ValidateId(1).Valid);
            Assert.IsFalse(Account.ValidateId(0).Valid);
        }

        [TestMethod]
        public void TestBalance()
        {
            Assert.IsTrue(Account.ValidateBalance(999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(999999999.999m).Valid);
            Assert.IsTrue(Account.ValidateBalance(-999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(-999999999.999m).Valid);
        }

        [TestMethod]
        public void TestCreated()
        {
            Assert.IsTrue(Account.ValidateCreated(new DateTime(2019,8,12)).Valid);
            Assert.IsFalse(Account.ValidateCreated(DateTime.Now + new TimeSpan(0,0,60)).Valid);
        }

        [TestMethod]
        public void TestWithdraw()
        {
            Account testAccount = new Account(0);

            testAccount.Withdraw(25000);
            Assert.AreEqual(-25000, testAccount.Balance);
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Withdraw(25000.01m); });

            testAccount.Withdraw(1);
            Assert.AreEqual(-25001, testAccount.Balance);
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Withdraw(-0.01m); });
        }

        [TestMethod]
        public void TestDeposit()
        {
            Account testAccount = new Account(0);

            testAccount.Deposit(25000);
            Assert.AreEqual(25000, testAccount.Balance);
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Deposit(25000.01m); });

            testAccount.Deposit(1);
            Assert.AreEqual(25001, testAccount.Balance);
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Deposit(-0.01m); });
        }

        [TestMethod]
        public void TestGetDaysSinceCreation()
        {
            Account testAccount = new Account(1, 100, DateTime.Now);
            Assert.AreEqual(0, testAccount.GetDaysSinceCreation());

            testAccount.Created -= new TimeSpan(20, 0, 0, 0);
            Assert.AreEqual(20, testAccount.GetDaysSinceCreation());

            DateTime daysLater = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 5);
            testAccount.Created = daysLater;
            Assert.AreEqual(5, testAccount.GetDaysSinceCreation());
        }
    }
}
