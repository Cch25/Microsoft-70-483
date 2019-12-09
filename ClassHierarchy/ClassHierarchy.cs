using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Hierarchies
{
    public class ClassHierarchy
    {
        public void DepositInMyBank(decimal amount)
        {
            using IAccount account = new BabyAccount();
            account.Deposit(amount);
            account.Deposit(amount);
            Console.WriteLine($"You currently have: {account.GetBalance():C}");
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                account.Withdraw(rnd.Next(i % 15, 15));
            }
        }

        public void ComparePricesInMyBank()
        {
            List<IAccount> accounts = new List<IAccount>();
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                IAccount account = new BabyAccount();
                accounts.Add(account);
            }
            accounts.Sort();
            foreach (IAccount account in accounts)
            {
                Console.WriteLine($"Balance {account.GetBalance()}");
            }
        }


    }

    #region [ Interfaces and abstractions ]
    public interface IAccount : IComparable<IAccount>, IDisposable
    {
        void Deposit(decimal amount);
        decimal GetBalance();
        bool Withdraw(decimal amount);
    }
    public abstract class BankAccount : IAccount
    {
        private decimal _balance = 0;
        private object syncLock = new object();
        private static Random rnd = new Random();
        public BankAccount(decimal amount)
        {
            _balance = amount;
        }
        public BankAccount() : this(rnd.Next(1, 50))
        {

        }
        public int CompareTo([AllowNull] IAccount other)
        {
            if (other == null)
            {
                return 1;
            }

            return this._balance.CompareTo(other.GetBalance());
        }

        public void Deposit(decimal amount)
        {
            lock (syncLock)
            {
                _balance += amount;
            }
            Console.WriteLine($"Deposit {amount:C}");
        }

        public decimal GetBalance()
        {
            return _balance;
        }

        public virtual bool Withdraw(decimal amount)
        {
            lock (syncLock)
            {
                if (amount > _balance)
                {
                    Console.WriteLine($"Insufficient funds.");
                    return false;
                }
                _balance -= amount;
                Console.WriteLine($"Withdrew {amount:C}.Current balance {_balance:C}.");
                return true;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Object disposed. I'm leaving the bank :)");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    public class BabyAccount : BankAccount, IAccount
    {//Great way for sOlid principle :)
        public sealed override bool Withdraw(decimal amount)
        {
            if (amount > 10)
            {
                Console.WriteLine($"You can't draw more than {10:C}" +
                    $".Current balance: {GetBalance():C}");
                return false;
            }
            return base.Withdraw(amount);
        }
    }
    #endregion

}
