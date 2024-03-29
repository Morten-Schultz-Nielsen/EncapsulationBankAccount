﻿using System;
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
        public void TestBalance()
        {
            //Tests if balance only can be in the interval [-999999999,99;999999999,99]
            //test validation
            Assert.IsTrue(Account.ValidateBalance(999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(999999999.999m).Valid);
            Assert.IsTrue(Account.ValidateBalance(-999999999.99m).Valid);
            Assert.IsFalse(Account.ValidateBalance(-999999999.999m).Valid);

            //test property
            Account testAccount = new Account(1, 50.10m, new DateTime(2019, 4, 4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Balance = -999999999.999m; });
        }

        [TestMethod]
        public void TestWithdraw()
        {
            Account testAccount = new Account(0);
            testAccount.Id = 1;

            //Test withdraw higher bound
            Transaction transaction = testAccount.Withdraw(25000);
            Assert.AreEqual(-25000, testAccount.Balance);

            //Test withdrawing twice
            testAccount.Withdraw(10.10m);
            Assert.AreEqual(-25010.10m, testAccount.Balance);

            //test exceptions
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Withdraw(-1); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Withdraw(25001); });

            //test if transaction output is correct
            Assert.AreEqual(25000, transaction.Money);
            Assert.AreEqual(1, transaction.AccountId);
            Assert.AreEqual(0, transaction.Id);
            Assert.IsTrue(transaction.Withdraw);
        }

        [TestMethod]
        public void TestDeposit()
        {
            Account testAccount = new Account(0);
            testAccount.Id = 1;

            //Test deposit higher bound
            Transaction transaction = testAccount.Deposit(25000);
            Assert.AreEqual(25000, testAccount.Balance);

            //test deposit twice
            testAccount.Deposit(10.10m);
            Assert.AreEqual(25010.10m, testAccount.Balance);

            //test exceptions
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Deposit(-1); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Deposit(25001); });

            //test if transaction output is correct
            Assert.AreEqual(25000, transaction.Money);
            Assert.AreEqual(1, transaction.AccountId);
            Assert.AreEqual(0, transaction.Id);
            Assert.IsFalse(transaction.Withdraw);
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

            //test exception
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Created = DateTime.Now + new TimeSpan(0, 0, 1); });
        }

        [TestMethod]
        public void TestCreated()
        {
            Account testAccount = new Account(1, 100, DateTime.Now);
            Assert.ThrowsException<ArgumentException>(() => { testAccount.Created = DateTime.Now + new TimeSpan(0, 0, 1); });
        }

        [TestMethod]
        public void TestId()
        {
            Account testAccount = new Account(1, 100, DateTime.Now);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { testAccount.Id = 0; });
        }
    }
}
