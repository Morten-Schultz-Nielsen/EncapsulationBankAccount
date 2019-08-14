using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.EntitiesTests
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void TestId()
        {
            //Tests if id isn't allowed to be 0 or smaller
            Assert.IsTrue(Validation.ValidateId(1).Valid);
            Assert.IsFalse(Validation.ValidateId(0).Valid);
        }

        [TestMethod]
        public void TestValidateTransaction()
        {
            //Make sure transaction only can be between 0 and 25000
            Assert.IsTrue(Validation.ValidateTransaction(0).Valid);
            Assert.IsTrue(Validation.ValidateTransaction(25000).Valid);
            Assert.IsFalse(Validation.ValidateTransaction(-0.01m).Valid);
            Assert.IsFalse(Validation.ValidateTransaction(25000.01m).Valid);
        }

        [TestMethod]
        public void TestCreated()
        {
            //Tests if creation date cannot be in the future
            Assert.IsTrue(Validation.ValidateCreated(new DateTime(2019, 8, 12)).Valid);
            Assert.IsFalse(Validation.ValidateCreated(DateTime.Now + new TimeSpan(0, 0, 60)).Valid);
        }
    }
}
