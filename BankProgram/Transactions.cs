using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProgram
{
    public class Transaction
    {
        private TransactionType type;
        private double amount;
        private String transactionDate;
        
        public Transaction(TransactionType type, double amount)
        {
            this.transactionDate = DateTime.Now.ToString();
            this.type = type;
            this.amount = amount;
        }

        public TransactionType GetTransactionType() { return type; }
        public double GetTransactionAmount() { return amount; }
        public String GetTransactionDate() { return transactionDate; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
}
