using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncapsulationBankAccount.Entities
{
    /// <summary>
    /// An object used to describe a bank account
    /// </summary>
    public class Account
    {
        private int id;
        private decimal balance;
        private DateTime created;

        /// <summary>
        /// Initializes a new <see cref="Account"/> object with the given starting balance
        /// </summary>
        /// <param name="initalBalance">The balance the account has from the start</param>
        public Account(decimal initalBalance)
        {
            Balance = initalBalance;
            id = 0;
            Created = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new <see cref="Account"/> object with data from an existing account
        /// </summary>
        /// <param name="id">The ID of this account</param>
        /// <param name="balance">The balance this account has</param>
        /// <param name="created">The time this account was created</param>
        public Account(int id, decimal balance, DateTime created)
        {
            Balance = balance;
            Id = id;
            Created = created;
        }

        /// <summary>
        /// The ID of this account
        /// </summary>
        /// <remarks>
        /// Is 0 if the account hasn't gotten an ID yet.
        /// </remarks>
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

        /// <summary>
        /// The balance of this account
        /// </summary>
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

        /// <summary>
        /// The time this account was created
        /// </summary>
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

        /// <summary>
        /// Withdraws the given amount of money from this account
        /// </summary>
        /// <param name="amount">The account of money to withdraw</param>
        /// <exception cref="ArgumentException"></exception>
        public void Withdraw(decimal amount)
        {
            if(amount < 0 || amount > 25000)
            {
                throw new ArgumentException("Amount has to be between 0 and 25000.", nameof(amount));
            }

            Balance -= amount;
        }

        /// <summary>
        /// Deposits the given amount of money into this account
        /// </summary>
        /// <param name="amount">The amount of money to deposit</param>
        /// <exception cref="ArgumentException"></exception>
        public void Deposit(decimal amount)
        {
            if(amount < 0 || amount > 25000)
            {
                throw new ArgumentException("Amount has to be between 0 and 25000.", nameof(amount));
            }

            Balance += amount;
        }

        /// <summary>
        /// Returns the amount of days since this account was created
        /// </summary>
        /// <returns>The amount of days since this account was created</returns>
        public int GetDaysSinceCreation()
        {
            DateTime exactCreationDay = new DateTime(Created.Year, Created.Month, Created.Day);
            return (DateTime.Now - exactCreationDay).Days;
        }

        /// <summary>
        /// Validates the given ID.
        /// </summary>
        /// <param name="id">the ID to validate</param>
        /// <returns>A tuple with a bool which is true if valid and a string containing the error message if invalid</returns>
        public static (bool Valid, string ErrorMessage) ValidateId(int id)
        {
            if(id <= 0)
            {
                return (false, "ID cannot be less than 0");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates the given balance amount
        /// </summary>
        /// <param name="balance">The balance to validate</param>
        /// <returns>A tuple with a bool which is true if valid and a string containing the error message if invalid</returns>
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

        /// <summary>
        /// Validates the given creation date
        /// </summary>
        /// <param name="created">The date to validate</param>
        /// <returns>A tuple with a bool which is true if valid and a string containing the error message if invalid</returns>
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
