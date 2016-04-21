using System.Transactions;

namespace AgileBank
{
    public interface IAccount
    {
        string Name { get; }
        decimal GetBalance();
        
        void SetDeposit(decimal amount, Transaction transaction = null);

        void Withdrawal(decimal amount, Transaction transaction = null);
    }
}