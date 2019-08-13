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

        public Account(decimal initalBalance)
        {
            Balance = initalBalance;
            id = 0;
            Created = DateTime.Now;
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
                (bool Valid, string ErrorMessage) = ValidateId(value);
                if (!Valid)
                {
                    throw new ArgumentException(ErrorMessage, nameof(Id));
                }

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
                (bool Valid, string ErrorMessage) = ValidateBalance(value);
                if(!Valid)
                {
                    throw new ArgumentException(ErrorMessage, nameof(Balance));
                }

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
                (bool Valid, string ErrorMessage) = ValidateCreated(value);
                if(!Valid)
                {
                    throw new ArgumentException(ErrorMessage, nameof(Created));
                }

                created = value;
            }
        }

        public void Withdraw(decimal amount)
        {
            if(amount < 0 || amount > 25000)
            {
                throw new ArgumentException("Amount has to be between 0 and 25000.", nameof(amount));
            }

            Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if(amount < 0 || amount > 25000)
            {
                throw new ArgumentException("Amount has to be between 0 and 25000.", nameof(amount));
            }

            Balance += amount;
        }

        public int GetDaysSinceCreation()
        {
            DateTime exactCreationDay = new DateTime(Created.Year, Created.Month, Created.Day);
            return (DateTime.Now - exactCreationDay).Days;
        }

        public static (bool Valid, string ErrorMessage) ValidateId(int id)
        {
            if(id <= 0)
            {
                return (false, "ID cannot be less than 0");
            }

            return (true, string.Empty);
        }

        public static (bool Valid, string ErrorMessage) ValidateBalance(decimal balance)
        {
            if(balance > 999999999.99m)
            {
                return (false, "Balance is too big");
            }
            if(balance < -999999999.99m)
            {
                return (false, "Balance is too small");
            }

            return (true, string.Empty);
        }

        public static (bool Valid, string ErrorMessage) ValidateCreated(DateTime created)
        {
            if(created > DateTime.Now)
            {
                return (false, "Creation date cannot be in the future");
            }
            return (true, string.Empty);
        }
    }
}
