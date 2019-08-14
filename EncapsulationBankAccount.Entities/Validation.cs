using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncapsulationBankAccount.Entities
{
    public static class Validation
    {
        /// <summary>
        /// Validates the given creation date
        /// </summary>
        /// <param name="created">The date to validate</param>
        /// <returns>A tuple with a bool which is true if valid and a string containing the error message if invalid</returns>
        public static (bool Valid, string ErrorMessage) ValidateCreated(DateTime created)
        {
            if(created.Ticks / 10000 > DateTime.Now.Ticks / 10000)
            {
                return (false, "Creation date cannot be in the future");
            }
            return (true, string.Empty);
        }

        /// <summary>
        /// Validates the given transaction amount
        /// </summary>
        /// <param name="amount">the transaction amount</param>
        /// <returns>A tuple with a bool which is true if valid and a string containing the error message if invalid</returns>
        public static (bool Valid, string ErrorMessage) ValidateTransaction(decimal amount)
        {
            if(amount > 25000)
            {
                return (false, "Transaktionen må ikke være større end 25000");
            }
            if(amount < 0)
            {
                return (false, "Transaktionen må ikke være mindre end 25000");
            }

            return (true, string.Empty);
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
    }
}
