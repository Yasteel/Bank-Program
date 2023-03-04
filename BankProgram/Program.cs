using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BankProgram
{
    public class Bank
    {
        private List<BankAccount> accounts = new List<BankAccount>();

        public void addAccount(String accountNumber, double amount)
        {
            var existingAccount = accounts.FirstOrDefault(_ => _.getAccountNumber() == accountNumber);

            if (existingAccount is null)
            {
                accounts.Add(new BankAccount(accountNumber, amount));
            }
            else
            {
                throw new Exception("Account Number Already Exists");
            }
            
        }
        public int getListIndex(String accountNumber)
        {
            return accounts.FindIndex(match => match.getAccountNumber().Equals(accountNumber));//*//
        }

        public bool inList(String accountNumber) //*//
        {
            if(this.accounts.Count == 0) return false;

            return getListIndex(accountNumber) < 0 ? false : true;
        }

        public void viewAccount(String accountNumber)
        {
            if (!inList(accountNumber))
            {
                throw new Exception($"Account Number: {accountNumber} Not Found");
            }
            else
            {
                int index = getListIndex(accountNumber);

                Console.WriteLine("{0,-20}{1}", "Account Number:", accounts[index].getAccountNumber());
                Console.WriteLine("{0,-20}{1}", "Balance:", accounts[index].getBalance());

                if (accounts[index].GetTransactions().Count < 1)
                {
                    Console.WriteLine("\nNo Transactions Processed");
                }
                else
                {
                    Console.WriteLine("===== Transactions =====");
                    Console.WriteLine("{0,-30}{1,-20}{2,-15}", "Date", "Transaction Type", "Amount");
                    accounts[index].GetTransactions().ForEach(transaction =>
                    {
                        Console.WriteLine("{0,-30}{1,-20}{2,-15}", transaction.GetTransactionDate(), transaction.GetTransactionType(), transaction.GetTransactionAmount());
                    });
                }
                Console.WriteLine("\n\n");
                showMainMenu();
            }
        }

        public void processTransaction(String accountNumber, int transaction_type, double amount)
        {
            //Transaction Type 1 = Deposit
            //Transaction Type 2 = Withdrawal

            if (inList(accountNumber))
            {
                int index = getListIndex(accountNumber);
                int multiplyFactor = 1;
                TransactionType type;

                switch (transaction_type)
                {
                    case 1:
                        type = TransactionType.Deposit;
                        break;

                    case 2:
                        type = TransactionType.Withdrawal;
                        break;

                    default:
                        throw new IOException("No such option Exists");
                }

                multiplyFactor = type == TransactionType.Deposit ? 1 : -1;

                Transaction newTransaction = new Transaction(type, amount);
                List<Transaction> oldTransactions = accounts[index].GetTransactions();
                oldTransactions.Add(newTransaction);

                double newBalance = accounts[index].getBalance() + (amount * multiplyFactor);
                accounts[index].setBalance(newBalance);
                accounts[index].setTransactions(oldTransactions);

                Console.Clear();
                displaySuccessMessage("Transaction Processed");
                showMainMenu();
            }
            else
            {
                throw new Exception("Account Not Found");
            }
        }

        public void accountsListing()
        {
            if(accounts.Count < 1)
            {
                throw new Exception("Account Not Found");
            }
            else
            {
                accounts.ForEach(account =>
                {
                    Console.WriteLine("{0,-20}{1}", "Account Number:", account.getAccountNumber());
                    Console.WriteLine("{0,-20}{1}", "Balance:", account.getBalance());

                    if (account.GetTransactions().Count < 1)
                    {
                        Console.WriteLine("No Transactions Processed");
                    }
                    else
                    {
                        Console.WriteLine("===== Transactions =====");
                        Console.WriteLine("{0,-30}{1,-20}{2,-15}", "Date", "Transaction Type", "Amount");
                        account.GetTransactions().ForEach(transaction =>
                        {
                            Console.WriteLine("{0,-30}{1,-20}{2,-15}", transaction.GetTransactionDate(), transaction.GetTransactionType(), transaction.GetTransactionAmount());
                        });
                    }
                    Console.WriteLine("\n");

                });
            }
        }

        public void displayErrorMessage(String errorMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void displaySuccessMessage(String successMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(successMessage+ "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void newAccountMenu()
        {
            Console.WriteLine("Add New Account");
            Console.Write("Enter Account Number\n>");

            String accountNumber = Console.ReadLine();

            if(!Regex.IsMatch(accountNumber, @"^[0-9]+$"))
            {
                displayErrorMessage("Account Numbers can ONLY contain Numeric characters");
                newAccountMenu();
            }
            else
            {
                Console.Write("\nEnter a Balance for this Account\n>");
                String balanceInput = Console.ReadLine();
                bool correctInput = Double.TryParse(balanceInput, out double test);

                if (!correctInput || correctInput == null)
                {
                    displayErrorMessage("Balance Cannot Contain Alphabetic or Special Characters Other Than '.'");
                }
                else
                {
                    addAccount(accountNumber, Double.Parse(balanceInput));
                    displaySuccessMessage($"Account (#{accountNumber}) Added Successfully");
                    showMainMenu();
                }
            }


        }

        public void viewAccountMenu()
        {
            Console.WriteLine("View Account");
            Console.Write("Enter Account Number\n>");

            String accountNumber = Console.ReadLine();

            if (!Regex.IsMatch(accountNumber, @"^[0-9]+$"))
            {
                displayErrorMessage("Account Numbers can ONLY contain Numeric characters");
                viewAccountMenu();
            }
            else
            {
                viewAccount(accountNumber);
            }
        }

        public void transactionMenu()
        {
            Console.Write("Enter Account Number\n>");
            String accountNumber = Console.ReadLine();

            if (!Regex.IsMatch(accountNumber, @"^[0-9]+$"))
            {
                Console.Clear();
                displayErrorMessage("Account Numbers can ONLY contain Numeric characters");
                transactionMenu();
            }

            String menu =   "\n=====Transaction Menu=====\n" +
                            "1. Deposit Money\n" +
                            "2. Withdraw Money\n" +
                            "3. Back";

            Console.Write(menu + "\n>");

            String userInput = Console.ReadLine();
            bool correctInput = Int32.TryParse(userInput, out int result);

            if (!correctInput || correctInput && Int32.Parse(userInput) < 1 || correctInput && Int32.Parse(userInput) > 3)
            {
                Console.Clear();
                displayErrorMessage($"'{userInput}' IS NOT A VALID INPUT");
                transactionMenu();
            }
            else
            {
                int correctOption = Int32.Parse(userInput);
                int transactionType = 0;
                
                if(correctOption == 1 || correctOption == 2)
                {
                    transactionType = correctOption;
                }
                else if(correctOption == 3)
                {
                    Console.Clear();
                    showMainMenu();
                }

                Console.Write("\nEnter Amount:\n>");
                String amountInput = Console.ReadLine();

                if(Double.TryParse(amountInput, out double test))
                {
                    double amount = Double.Parse(amountInput);
                    processTransaction(accountNumber, transactionType, amount);
                }
            }
        }

        public void showMainMenu()
        {
            String menu =   "=====Menu=====\n" +
                            "Please Select An Option From Below\n\n" +
                            "1. Add An Account\n" +
                            "2. View Account\n" +
                            "3. Perform Transaction\n" +
                            "4. View All Accounts & Details\n" +
                            "5. Exit";

            Console.Write(menu + "\n>");

            String userInput = Console.ReadLine();
            bool correctInput = Int32.TryParse(userInput, out int result);

            if (!correctInput || correctInput && Int32.Parse(userInput) < 1 || correctInput && Int32.Parse(userInput) > 5)
            {
                displayErrorMessage($"'{userInput}' IS NOT A VALID INPUT");
                showMainMenu();
            }
            else
            {
                int correctOption = Int32.Parse(userInput);

                switch(correctOption) 
                {
                    case 1:
                        Console.Clear();
                        newAccountMenu();
                        break;
                    case 2:
                        Console.Clear();
                        viewAccountMenu();
                        break;
                    case 3:
                        Console.Clear();
                        transactionMenu();
                        break;
                    case 4:
                        Console.Clear();
                        accountsListing();
                        showMainMenu();
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Thank you for using My Program");
                        break;
                    default:
                        Console.WriteLine("Unrecognised input");
                        break;
                }
            }





        }


        public static void Main()
        {
            
            Bank absa = new Bank();

            try
            {
                absa.showMainMenu();
            }
            catch (Exception e)
            {
                absa.displayErrorMessage(e.Message);
                absa.showMainMenu();
            }
            
            Console.ReadLine();
        }
    }
}

