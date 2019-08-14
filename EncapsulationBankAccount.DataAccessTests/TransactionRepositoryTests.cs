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
    public class TransactionRepositoryTests
    {
        /*
         * Tests were overlapping too much so i concluded it would be best to have them in the same method
         */

        [TestMethod()]
        public void TransactionTest()
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            AccountRepository accountRepository = new AccountRepository();

            //test if get all transactions works
            int transactionsAmount = transactionRepository.GetTransactionLogs().ToArray().Length;

            //Make transaction
            Account testAccount = new Account(10);
            accountRepository.Insert(testAccount);
            Transaction testTransaction = new Transaction(10.1m, testAccount.Id, true);
            transactionRepository.LogTransaction(testTransaction);

            //Make sure transaction was logged correctly
            Transaction[] transactions = transactionRepository.GetTransactionLogs().ToArray();
            Assert.IsTrue(transactions.Length - 1 == transactionsAmount);
            Transaction savedTransaction = transactions[transactions.Length - 1];

            Assert.AreEqual(testTransaction.Id, savedTransaction.Id);
            Assert.AreEqual(testTransaction.Money, savedTransaction.Money);
            Assert.AreEqual(testTransaction.TransactionTime.Ticks, savedTransaction.TransactionTime.Ticks, 10000);
            Assert.AreEqual(testTransaction.Withdraw, savedTransaction.Withdraw);
            Assert.AreEqual(testTransaction.AccountId, savedTransaction.AccountId);

            //test get transaction from timeframe
            transactionsAmount = transactionRepository.GetTransactionLogs(DateTime.Now - new TimeSpan(0,0,10), DateTime.Now).ToArray().Length;
            Assert.AreEqual(1, transactionsAmount);

            savedTransaction = transactions[transactions.Length - 1];

            Assert.AreEqual(testTransaction.Id, savedTransaction.Id);
            Assert.AreEqual(testTransaction.Money, savedTransaction.Money);
            Assert.AreEqual(testTransaction.TransactionTime.Ticks, savedTransaction.TransactionTime.Ticks, 10000);
            Assert.AreEqual(testTransaction.Withdraw, savedTransaction.Withdraw);
            Assert.AreEqual(testTransaction.AccountId, savedTransaction.AccountId);
        }

        [TestMethod]
        public void TestExceptions()
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            Transaction testTransaction = new Transaction(10, 1000, 10, false, DateTime.Now);

            //adding transaction
            Assert.ThrowsException<ArgumentException>(() => { transactionRepository.LogTransaction(testTransaction); });
            Assert.ThrowsException<ArgumentNullException>(() => { transactionRepository.LogTransaction(null); });

            //getting transactions
            Assert.ThrowsException<ArgumentException>(() => { _ = transactionRepository.GetTransactionLogs(DateTime.Now, new DateTime(2018,10,10)).ToArray(); });
        }
    }
}