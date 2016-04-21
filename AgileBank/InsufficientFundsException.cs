using System;

namespace AgileBank
{
    public class InsufficientFundsException : Exception
    {
        private readonly decimal amountBelowMinimum;

        public InsufficientFundsException(decimal amountBelowMinimum)
        {
            this.amountBelowMinimum = amountBelowMinimum;
        }

        public override string Message => "Withdrawal would exceed minimum balance by $" + amountBelowMinimum.ToString("#,##0.00");
    }
}