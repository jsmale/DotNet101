using System;
using System.Transactions;

namespace AgileBank
{
    public class Bank
    {
        public void Transfer(IAccount fromAccount, IAccount toAccount, decimal amount)
        {
            using (var transactionScope = new TransactionScope())
            {
                fromAccount.Withdrawal(amount, Transaction.Current);
                toAccount.SetDeposit(amount, Transaction.Current);
                transactionScope.Complete();
            }
        }
    }
}