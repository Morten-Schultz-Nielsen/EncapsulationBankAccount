using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncapsulationBankAccount.Entities;
using System.Data;
using System.Data.SqlClient;

namespace EncapsulationBankAccount.DataAccess
{
    /// <summary>
    /// A repository for getting/adding transaction logs
    /// </summary>
    public class TransactionRepository : RepositoryBase
    {
        /// <summary>
        /// Logs the given transaction into the database
        /// </summary>
        /// <param name="transaction"></param>
        public void LogTransaction(Transaction transaction)
        {
            if(transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction must not be null");
            }
            if(transaction.Id != 0)
            {
                throw new ArgumentException("Cannot log an already logged transaction", nameof(transaction));
            }

            SqlCommand logCommand = new SqlCommand("INSERT INTO Transactions (TransactionTime, Money, Withdrawn, AccountId) OUTPUT INSERTED.Id VALUES (@TransactionTime, @Money, @Withdrawn, @AccountId)");
            logCommand.Parameters.AddWithValue("@TransactionTime", transaction.TransactionTime);
            logCommand.Parameters.AddWithValue("@Money", transaction.Money);
            logCommand.Parameters.AddWithValue("@Withdrawn", transaction.Withdraw);
            logCommand.Parameters.AddWithValue("@AccountId", transaction.AccountId);

            DataSet output = Execute(logCommand);
            transaction.Id = output.Tables[0].Rows[0].Field<int>("Id");
        }

        /// <summary>
        /// Returns all transaction logs which happened between the 2 times
        /// </summary>
        /// <param name="from">the starting time</param>
        /// <param name="to">the ending time</param>
        /// <returns>A list of transactions</returns>
        public IEnumerable<Transaction> GetTransactionLogs(DateTime from, DateTime to)
        {
            if(from > to)
            {
                throw new ArgumentException("to cannot be before from");
            }

            SqlCommand getCommand = new SqlCommand("SELECT * FROM Transactions WHERE TransactionTime BETWEEN @From AND @To");
            getCommand.Parameters.AddWithValue("@From", from);
            getCommand.Parameters.AddWithValue("@To", to);

            DataSet output = Execute(getCommand);
            foreach(DataRow row in output.Tables[0].Rows)
            {
                yield return TransactionFromRow(row);
            }
        }

        /// <summary>
        /// Returns all transaction logs
        /// </summary>
        /// <returns>A list of transactions</returns>
        public IEnumerable<Transaction> GetTransactionLogs()
        {
            SqlCommand getCommand = new SqlCommand("SELECT * FROM Transactions");

            DataSet output = Execute(getCommand);
            foreach(DataRow row in output.Tables[0].Rows)
            {
                yield return TransactionFromRow(row);
            }
        }

        /// <summary>
        /// Converts the given <see cref="DataRow"/> into an <see cref="Transaction"/> object
        /// </summary>
        /// <param name="row">The <see cref="DataRow"/> to convert</param>
        /// <returns>the converted <see cref="Transaction"/></returns>
        private static Transaction TransactionFromRow(DataRow row)
        {
            int id = row.Field<int>("Id");
            decimal money = row.Field<decimal>("Money");
            DateTime transactionTime = row.Field<DateTime>("TransactionTime");
            bool withdrawn = row.Field<bool>("Withdrawn");
            int accountId = row.Field<int>("AccountId");

            return new Transaction(id, money, accountId, withdrawn, transactionTime);
        }
    }
}
