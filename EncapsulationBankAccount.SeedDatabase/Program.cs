using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncapsulationBankAccount.DataAccess;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.SeedDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();

            Random random = new Random();
            AccountRepository repository = new AccountRepository();
            Account[] accounts = repository.Select().ToArray();

            Console.WriteLine("Deleting the " + accounts.Length + " old accounts...");
            foreach(Account account in accounts)
            {
                repository.Delete(account);
            }

            Console.WriteLine("Adding the first 13000 accounts...");
            for(int i = 0; i < 13000; i++)
            {
                repository.Insert(new Account((decimal)random.NextDouble() * -150000));
                WriteLength(i);
            }

            Console.WriteLine("Adding the next 130000 accounts...");
            for(int i = 0; i < 130000; i++)
            {
                repository.Insert(new Account((decimal)random.NextDouble() * 1000000));
                WriteLength(i);
            }

            Console.WriteLine("Adding the last 13611 accounts...");
            for(int i = 0; i < 13611; i++)
            {
                repository.Insert(new Account((decimal)random.NextDouble() * 4000000 + 1000000));
                WriteLength(i);
            }
        }

        private static void WriteLength(int length)
        {
            if(length % 500 == 499)
            {
                Console.WriteLine("+500 ("+ length +")");
            }
        }
    }
}
