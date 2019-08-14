using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace EncapsulationBankAccount.DataAccess
{
    /// <summary>
    /// The base of a repository
    /// </summary>
    public class RepositoryBase
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new repository with the correct connection string
        /// </summary>
        public RepositoryBase()
        {
            connectionString = GetConnectionString();
        }

        /// <summary>
        /// executes the given sql command
        /// </summary>
        /// <param name="command">The sql command to execute</param>
        /// <returns>The output from the sql command</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal DataSet Execute(SqlCommand command)
        {
            if(command is null)
            {
                throw new ArgumentNullException(nameof(command), "command may not be null");
            }

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

        /// <summary>
        /// Gets the connection string used for connecting to the database
        /// </summary>
        /// <returns>The connection string for the database</returns>
        private static string GetConnectionString()
        {
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EncapsulationBankAccount;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
    }
}
