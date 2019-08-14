using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncapsulationBankAccount.Entities
{
    /// <summary>
    /// An object for holding information about a transaction
    /// </summary>
    public class Transaction
    {
        private decimal money;
        private int accountId;
        private bool withdraw;
        private DateTime transactionTime;
        private int id;

        /// <summary>
        /// Intializes a new <see cref="Transaction"/> object with the given information
        /// </summary>
        /// <param name="money">The amount of money in the transaction</param>
        /// <param name="accountId">The ID of the account who made the transaction</param>
        /// <param name="Withdraw">True if the transaction is a withdraw, false if it's a deposite</param>
        public Transaction(decimal money, int accountId, bool withdraw)
        {
            Money = money;
            AccountId = accountId;
            Withdraw = withdraw;
            TransactionTime = DateTime.Now;
            id = 0;
        }

        /// <summary>
        /// Intializes a new <see cref="Transaction"/> object with data from the database
        /// </summary>
        /// <param name="money">The amount of money in the transaction</param>
        /// <param name="accountId">The ID of the account who made the transaction</param>
        /// <param name="Withdraw">True if the transaction is a withdraw, false if it's a deposite</param>
        /// <param name="transactionTime">The point in time the transaction was made</param>
        public Transaction(int id, decimal money, int accountId, bool withdraw, DateTime transactionTime)
        {
            Money = money;
            AccountId = accountId;
            Withdraw = withdraw;
            TransactionTime = transactionTime;
            Id = id;
        }

        /// <summary>
        /// The amount of money in the transaction
        /// </summary>
        public decimal Money
        {
            get
            {
                return money;
            }

            set
            {
                if(!Validation.ValidateTransaction(value).Valid)
                {
                    throw new ArgumentOutOfRangeException(nameof(Money), "the transaction amount is not inside the correct range.");
                }
                money = value;
            }
        }

        /// <summary>
        /// The id of the account who made the transaction
        /// </summary>
        public int AccountId
        {
            get
            {
                return accountId;
            }

            set
            {
                if(!Validation.ValidateId(value).Valid)
                {
                    throw new ArgumentOutOfRangeException(nameof(AccountId), "The account id is not inside the correct range");
                }
                accountId = value;
            }
        }

        /// <summary>
        /// If the transaction was a withdraw or a deposite
        /// </summary>
        public bool Withdraw
        {
            get
            {
                return withdraw;
            }

            set
            {
                withdraw = value;
            }
        }

        /// <summary>
        /// The time this transaction was made
        /// </summary>
        public DateTime TransactionTime
        {
            get
            {
                return transactionTime;
            }
            private set
            {
                if(!Validation.ValidateCreated(value).Valid)
                {
                    throw new ArgumentException("Transaction time cannot be in the future", nameof(TransactionTime));
                }

                transactionTime = value;
            }
        }

        /// <summary>
        /// The id of this transaction
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if(!Validation.ValidateId(value).Valid)
                {
                    throw new ArgumentOutOfRangeException(nameof(Id), "The id is not inside the correct range");
                }
                id = value;
            }
        }
    }
}
