using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileBank.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void AccountShouldHaveAName()
        {
            var account = new Account("Checking");

            Assert.AreEqual("Checking", account.Name);
        }

        [TestMethod]
        public void AccountShouldStartWithZeroBalance()
        {
            var account = new Account("Checking");

            Assert.AreEqual(0m, account.GetBalance());
        }

        [TestMethod]
        public void AccountShouldAcceptDepositAndUpdateBalance()
        {
            var account = new Account("Checking");

            account.SetDeposit(10m);

            Assert.AreEqual(10m, account.GetBalance());
        }

        [TestMethod]
        public void AccountShouldAllowWithdrawalAndUpdateBalance()
        {
            var account = new Account("Checking");
            account.SetDeposit(100m);

            account.Withdrawal(45m);

            Assert.AreEqual(55m, account.GetBalance());
        }

        [TestMethod]
        public void AccountShouldAllowWithdrawalInsufficientFunds()
        {
            var account = new Account("Checking");

            account.Withdrawal(10m);

            Assert.AreEqual(-10m, account.GetBalance());
        }

        [TestMethod]
        public void AccountShouldNotAllowWithdrawalInsufficientFundsBelowMinimumBalance()
        {
            var account = new Account("Checking");

            try
            {
                account.Withdrawal(55m);
                Assert.Fail("InsufficientFundsException not thrown");
            }
            catch (InsufficientFundsException)
            {

            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: {0}", ex);
            }
            
            Assert.AreEqual(0m, account.GetBalance(), "Balance should not have changed");
        }

        [TestMethod]
        public void WithdrawalBelowMinimumBalanceShouldProvideDetail()
        {
            var account = new Account("Checking");

            try
            {
                account.Withdrawal(55m);
                Assert.Fail("InsufficientFundsException not thrown");
            }
            catch (InsufficientFundsException ex)
            {
                Assert.AreEqual("Withdrawal would exceed minimum balance by $5.00", ex.Message);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: {0}", ex);
            }
        }

        [TestMethod]
        public void LargeWithdrawalBelowMinimumBalanceShouldProvideDetail()
        {
            var account = new Account("Checking");

            try
            {
                account.Withdrawal(1050000m);
                Assert.Fail("InsufficientFundsException not thrown");
            }
            catch (InsufficientFundsException ex)
            {
                Assert.AreEqual("Withdrawal would exceed minimum balance by $1,049,950.00", ex.Message);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: {0}", ex);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToTransferFundsBetweenTwoAccounts()
        {
            var checking = new Account("Checking");
            var savings = new Account("Savings");

            savings.SetDeposit(1000m);

            var bank = new Bank();

            bank.Transfer(savings, checking, 370m);

            Assert.AreEqual(630m, savings.GetBalance(), "Savings balance did not match");
            Assert.AreEqual(370m, checking.GetBalance(), "Checking balance did not match");
        }

        [TestMethod]
        public void ShouldNotAllowTransferBelowMinimumBalance()
        {
            var checking = new Account("Checking");
            var savings = new Account("Savings");

            savings.SetDeposit(200m);

            var bank = new Bank();

            try
            {
                bank.Transfer(savings, checking, 370m);
                Assert.Fail("InsufficientFundsException not thrown");
            }
            catch (InsufficientFundsException ex)
            {
                Assert.AreEqual("Withdrawal would exceed minimum balance by $120.00", ex.Message,
                    "Exception message did not match");
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: {0}", ex);
            }

            Assert.AreEqual(200m, savings.GetBalance(), "Savings balance did not match");
            Assert.AreEqual(0m, checking.GetBalance(), "Checking balance did not match");
        }

        public class FakeAccount : IAccount
        {
            private readonly string exceptionMessage;

            public FakeAccount(string exceptionMessage)
            {
                this.exceptionMessage = exceptionMessage;
            }

            public string Name { get; }
            public decimal GetBalance()
            {
                return 0m;
            }

            public void SetDeposit(decimal amount, Transaction transaction = null)
            {
                throw new Exception(exceptionMessage);
            }

            public void Withdrawal(decimal amount, Transaction transaction = null)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void ShouldNotAllowTransferIfDepositFails()
        {
            var expectedException = "Fake Account always fails deposit";
            var checking = new FakeAccount(expectedException);
            var savings = new Account("Savings");

            savings.SetDeposit(1000m);

            var bank = new Bank();

            try
            {
                bank.Transfer(savings, checking, 370m);
                Assert.Fail("Fake Account Exception not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedException, ex.Message);
            }

            Assert.AreEqual(1000m, savings.GetBalance(), "Savings balance did not match");
        }
    }
}
