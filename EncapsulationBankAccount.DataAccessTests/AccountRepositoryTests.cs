using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncapsulationBankAccount.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.DataAccess.Tests
{
    [TestClass()]
    public class AccountRepositoryTests
    {
        [TestMethod()]
        public void AccountRepositoryTest()
        {
            new AccountRepository();
        }

        [TestMethod()]
        public void InsertTest()
        {
            //test account gets an id after insert
            AccountRepository repository = new AccountRepository();
            Account testAccount = new Account(10.50m);
            repository.Insert(testAccount);
            Assert.AreNotEqual(0, testAccount.Id);

            //make sure account was inserted correctly
            Account savedAccount = repository.Select(testAccount.Id);
            Assert.AreEqual(testAccount.Created.Ticks, savedAccount.Created.Ticks, 10000);
            Assert.AreEqual(testAccount.Balance, savedAccount.Balance);

            //make sure you cant insert existing account or null
            Assert.ThrowsException<ArgumentException>(() => { repository.Insert(new Account(1, 10, DateTime.Now)); });
            Assert.ThrowsException<ArgumentNullException>(() => { repository.Insert(null); });
        }

        [TestMethod()]
        public void UpdateTest()
        {
            //insert account for later updating
            AccountRepository repository = new AccountRepository();
            Account testAccount = new Account(10);
            repository.Insert(testAccount);

            //update the account again for the next test
            testAccount.Balance = 55;
            testAccount.Created = new DateTime(2010, 10, 5, 10, 12, 3);
            repository.Update(testAccount);

            Account updatedAccount = repository.Select(testAccount.Id);
            Assert.AreEqual(testAccount.Created.Ticks, updatedAccount.Created.Ticks, 10000);
            Assert.AreEqual(testAccount.Balance, updatedAccount.Balance);

            //make sure you cant update new accounts or null
            Assert.ThrowsException<ArgumentException>(() => { repository.Update(new Account(10)); });
            Assert.ThrowsException<ArgumentNullException>(() => { repository.Update(null); });
        }

        [TestMethod()]
        public void SelectOneTest()
        {
            //select single is already tested inside of update and insert
            //this only test invalid selection

            AccountRepository repository = new AccountRepository();
            Assert.ThrowsException<ArgumentException>(() => { repository.Select(0); });
            Assert.ThrowsException<KeyNotFoundException>(() => { repository.Select(int.MaxValue); });
        }

        [TestMethod()]
        public void SelectAllTest()
        {
            //test if select all works
            AccountRepository repository = new AccountRepository();
            Account[] accounts = repository.Select().ToArray();

            //test if select all selects the correct amount of accounts (by adding one and checking if there is 1 more account)
            Account testAccount = new Account(0);
            repository.Insert(testAccount);
            Account[] newAccountsList = repository.Select().ToArray();
            Assert.IsTrue(newAccountsList.Length - 1 == accounts.Length);

            //make sure select all outputs accounts correctly
            Account insertedAccount = newAccountsList.Single(a => a.Id == testAccount.Id);
            Assert.AreEqual(testAccount.Created.Ticks, insertedAccount.Created.Ticks, 10000);
            Assert.AreEqual(testAccount.Balance, insertedAccount.Balance);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            AccountRepository repository = new AccountRepository();
            Account testAccount = new Account(10);

            //make sure account is being deleted
            repository.Insert(testAccount);
            repository.Select(testAccount.Id);
            repository.Delete(testAccount);
            Assert.ThrowsException<KeyNotFoundException>(() => { repository.Select(testAccount.Id); });

            //make sure you can't delete new accounts or null
            Assert.ThrowsException<ArgumentException>(() => { repository.Delete(new Account(10)); });
            Assert.ThrowsException<ArgumentNullException>(() => { repository.Delete(null); });
        }
    }
}