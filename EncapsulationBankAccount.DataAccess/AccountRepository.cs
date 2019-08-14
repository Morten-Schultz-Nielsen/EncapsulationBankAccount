using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncapsulationBankAccount.Entities;
using System.Data.SqlClient;
using System.Data;

namespace EncapsulationBankAccount.DataAccess
{
    /// <summary>
    /// A repository used for getting accounts from the database
    /// </summary>
    public class AccountRepository : RepositoryBase
    {
        /// <summary>
        /// Inserts the new <see cref="Account"/> into the database and gives it an ID
        /// </summary>
        /// <param name="account">The <see cref="Account"/> to insert</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Insert(Account account)
        {
            if(account is null)
            {
                throw new ArgumentNullException(nameof(account) ,"Value must not be null");
            }
            if(account.Id != 0)
            {
                throw new ArgumentException(nameof(account), "Already existing account cannot be inserted");
            }

            SqlCommand insertCommand = new SqlCommand("INSERT INTO Accounts (Balance, Created) OUTPUT INSERTED.Id VALUES (@Balance, @Created)");
            insertCommand.Parameters.AddWithValue("@Id", account.Id);
            insertCommand.Parameters.AddWithValue("@Balance", account.Balance);
            insertCommand.Parameters.AddWithValue("@Created", account.Created);

            DataSet output = Execute(insertCommand);
            account.Id = output.Tables[0].Rows[0].Field<int>("Id");
        }

        /// <summary>
        /// Updates the given <see cref="Account"/> in the database
        /// </summary>
        /// <param name="account">The <see cref="Account"/> to update</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Update(Account account)
        {
            if(account is null)
            {
                throw new ArgumentNullException(nameof(account), "Value must not be null");
            }
            if(account.Id == 0)
            {
                throw new ArgumentException(nameof(account), "Accounts not added to the database yet cannot be updated");
            }

            SqlCommand updateCommand = new SqlCommand("UPDATE Accounts set Balance = @Balance, Created = @Created WHERE Id = @Id");
            updateCommand.Parameters.AddWithValue("@Id", account.Id);
            updateCommand.Parameters.AddWithValue("@Balance", account.Balance);
            updateCommand.Parameters.AddWithValue("@Created", account.Created);
            _ = Execute(updateCommand);
        }

        /// <summary>
        /// Gets the <see cref="Account"/> with the given ID from the database
        /// </summary>
        /// <param name="id">The ID of the <see cref="Account"/> to find</param>
        /// <returns>The <see cref="Account"/> from the database</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public Account Select(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException(nameof(id), "cannot select account with an Id under 1");
            }

            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Accounts WHERE Id = @Id");
            selectCommand.Parameters.AddWithValue("@Id", id);

            DataSet output = Execute(selectCommand);
            if(output.Tables[0].Rows.Count == 0)
            {
                throw new KeyNotFoundException("cannot find account with the given ID");
            }
            return AccountFromRow(output.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Gets all <see cref="Account"/>s from the database
        /// </summary>
        /// <returns>All <see cref="Account"/>s from the database</returns>
        public IEnumerable<Account> Select()
        {
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Accounts");
            DataSet output = Execute(selectCommand);
            foreach(DataRow row in output.Tables[0].Rows)
            {
                yield return AccountFromRow(row);
            }
        }

        /// <summary>
        /// Deletes the given <see cref="Account"/> from the database
        /// </summary>
        /// <param name="account">the <see cref="Account"/> to delete from the database</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Delete(Account account)
        {
            if(account is null)
            {
                throw new ArgumentNullException(nameof(account), "Value must not be null");
            }
            if(account.Id == 0)
            {
                throw new ArgumentException(nameof(account), "Already existing account cannot be inserted");
            }

            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Accounts WHERE Id = @Id");
            deleteCommand.Parameters.AddWithValue("@Id", account.Id);
            _ = Execute(deleteCommand);
        }

        /// <summary>
        /// Converts a <see cref="DataRow"/> into an <see cref="Account"/> object
        /// </summary>
        /// <param name="row">the <see cref="DataRow"/> to convert</param>
        /// <returns>the <see cref="Account"/> object from the <see cref="DataRow"/></returns>
        private static Account AccountFromRow(DataRow row)
        {
            int id = row.Field<int>("Id");
            decimal balance = row.Field<decimal>("Balance");
            DateTime created = row.Field<DateTime>("Created");
            return new Account(id, balance, created);
        }
    }
}
