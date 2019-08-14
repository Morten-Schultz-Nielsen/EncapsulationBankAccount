using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.EntitiesTests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TestTransaction()
        {
            //Test creation of now transaction
            Transaction testTransaction = new Transaction(10.5m, 50, true);
            Assert.AreEqual(10.5m, testTransaction.Money);
            Assert.AreEqual(0, testTransaction.Id);
            Assert.AreEqual(50, testTransaction.AccountId);
            Assert.IsTrue(DateTime.Now.Ticks - testTransaction.TransactionTime.Ticks > -10000);
            Assert.IsTrue(testTransaction.Withdraw);

            //test creation of existing transaction
            testTransaction = new Transaction(10, 10.5m, 50, true, new DateTime(2018, 10, 10));
            Assert.AreEqual(10.5m, testTransaction.Money);
            Assert.AreEqual(10, testTransaction.Id);
            Assert.AreEqual(50, testTransaction.AccountId);
            Assert.AreEqual(new DateTime(2018, 10, 10), testTransaction.TransactionTime);
            Assert.IsTrue(testTransaction.Withdraw);
            //test exception transaction time exception
            Assert.ThrowsException<ArgumentException>(() => { new Transaction(10, 10.5m, 50, true, DateTime.Now + new TimeSpan(0, 0, 1)); });
        }

        [TestMethod]
        public void TestMoney()
        {
            Transaction testTransaction = new Transaction(10.5m, 50, true);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testTransaction.Money = 25001; });
        }

        [TestMethod]
        public void TestId()
        {
            Transaction testTransaction = new Transaction(10.5m, 50, true);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testTransaction.Id = 0; });
        }

        [TestMethod]
        public void TestAccountId()
        {
            Transaction testTransaction = new Transaction(10.5m, 50, true);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testTransaction.AccountId = 0; });
        }
    }
}
