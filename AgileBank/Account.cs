using System;
using System.Transactions;

namespace AgileBank
{
    public class Account : IAccount
    {
        private decimal balance;
        private const decimal MinimumBalance = -50;

        public Account(string name)
        {
            Name = name;
            balance = 0m;
        }

        public string Name { get; }

        public decimal GetBalance()
        {
            return balance;
        }

        public void SetDeposit(decimal amount, Transaction transaction = null)
        {
            balance += amount;

            if (transaction != null)
            {
                transaction.TransactionCompleted += RollBackDeposit(amount);
            }
        }

        public TransactionCompletedEventHandler RollBackDeposit(decimal amount)
        {
            return (sender, eventArgs) =>
            {
                var transaction = eventArgs.Transaction;
                if (transaction.TransactionInformation.Status == TransactionStatus.Aborted)
                {
                    balance -= amount;
                }
            };
        }

        public void Withdrawal(decimal amount, Transaction transaction = null)
        {
            var newBalance = balance - amount;
            if (newBalance < MinimumBalance)
            {
                var amountBelowMinimum = 0 - (newBalance - MinimumBalance);
                throw new InsufficientFundsException(amountBelowMinimum);
            }

            balance -= amount;
            if (transaction != null)
            {
                transaction.TransactionCompleted += RollBackWithdrawal(amount);
            }
        }

        public TransactionCompletedEventHandler RollBackWithdrawal(decimal amount)
        {
            return (sender, eventArgs) =>
            {
                var transaction = eventArgs.Transaction;
                if (transaction.TransactionInformation.Status == TransactionStatus.Aborted)
                {
                    balance += amount;
                }
            };
        }
    }
}
