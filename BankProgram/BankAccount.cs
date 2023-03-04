using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProgram
{
    public class BankAccount
    {
        private String AccountNumber;
        private double Balance;
        private List<Transaction> Transactions = new List<Transaction>();

        public BankAccount(String accountNumber, double balance)
        {
            this.AccountNumber = accountNumber;
            this.Balance = balance;
        }

        public String getAccountNumber()
        {
            return AccountNumber;
        }

        public double getBalance()
        {
            return Balance;
        }
        public void setBalance(double balance)
        {
            this.Balance = balance;
        }

        public List<Transaction> GetTransactions()
        {
            return Transactions;
        }

        public void setTransactions(List<Transaction> updatedTransactions)
        {
            this.Transactions = updatedTransactions;
        }

    }
}
