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
    public class AccountRepository
    {
        private readonly string connectionString;

        public AccountRepository()
        {
            connectionString = GetConnectionString();
        }

        private DataSet Execute(SqlCommand command)
        {
            DataSet outputSet = new DataSet();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;
                using(SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(outputSet);
                }
            }

            return outputSet;
        }

        public void Insert(Account account)
        {
            if(account is null)
            {
                throw new ArgumentNullException(nameof(account) ,"Value must not be null");
            }
            if(account.Id != 0)
            {
                throw new ArgumentException(nameof(account), "Already existing account cannot be insrted");
            }

            SqlCommand insertCommand = new SqlCommand("INSERT INTO Accounts (Balance, Created) OUTPUT INSERTED.Id VALUES (@Balance, @Created)");
            insertCommand.Parameters.AddWithValue("@Id", account.Id);
            insertCommand.Parameters.AddWithValue("@Balance", account.Balance);
            insertCommand.Parameters.AddWithValue("@Created", account.Created);

            DataSet output = Execute(insertCommand);
            account.Id = output.Tables[0].Rows[0].Field<int>("Id");
        }

        public void Update(Account account)
        {
            if(account is null)
            {
                throw new ArgumentNullException(nameof(account), "Value must not be null");
            }
            if(account.Id == 0)
            {
                throw new ArgumentException(nameof(account), "Already existing account cannot be insrted");
            }

            SqlCommand updateCommand = new SqlCommand("UPDATE Accounts set Balance = @Balance, Created = @Created WHERE Id = @Id");
            updateCommand.Parameters.AddWithValue("@Id", account.Id);
            updateCommand.Parameters.AddWithValue("@Balance", account.Balance);
            updateCommand.Parameters.AddWithValue("@Created", account.Created);
            _ = Execute(updateCommand);
        }

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

        public IEnumerable<Account> Select()
        {
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Accounts");
            DataSet output = Execute(selectCommand);
            foreach(DataRow row in output.Tables[0].Rows)
            {
                yield return AccountFromRow(row);
            }
        }

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

        private static string GetConnectionString()
        {
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EncapsulationBankAccount;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        private static Account AccountFromRow(DataRow row)
        {
            int id = row.Field<int>("Id");
            decimal balance = row.Field<decimal>("Balance");
            DateTime created = row.Field<DateTime>("Created");
            return new Account(id, balance, created);
        }
    }
}
