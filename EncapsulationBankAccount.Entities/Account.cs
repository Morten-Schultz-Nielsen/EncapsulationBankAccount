using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncapsulationBankAccount.Entities
{
    public class Account
    {
        private int id;
        private decimal balance;
        private DateTime created;

        public Account(decimal initalBalance) : this(0, initalBalance, DateTime.Now)
        {
            
        }

        public Account(int id, decimal balance, DateTime created)
        {
            Balance = balance;
            Id = id;
            Created = created;
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public decimal Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
            }
        }

        public DateTime Created
        {
            get
            {
                return created;
            }

            set
            {
                created = value;
            }
        }

        public void Withdraw(decimal amount)
        {

        }

        public void Deposit(decimal balance)
        {

        }

        public int GetDaysSinceCreation()
        {
            return -1;
        }

        public static (bool Valid, string ErrorMessage) ValidateId(int id)
        {
            return (true, string.Empty);
        }

        public static (bool Valid, string ErrorMessage) ValidateBalance(decimal balance)
        {
            return (true, string.Empty);
        }

        public static (bool Valid, string ErrorMessage) ValidateCreated(DateTime created)
        {
            return (true, string.Empty);
        }
    }
}
