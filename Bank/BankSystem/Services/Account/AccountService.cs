﻿using BankSystem.Models;

namespace BankSystem.Services.Account
{
    public class AccountService : IAccountService
    {
        public decimal GetBalance(IUser user)
        {
            return user.Balance;
        }

        public IMoneyTransfer CreateMoneyTransfer(IUser sender, IUser recipient, decimal amount)
        {
            if (amount <= 0.0M)
            {
                throw new AccountOperationException("Invalid amount.");
            }

            var senderBalance = GetBalance(sender);
            if (senderBalance < amount)
            {
                throw new AccountOperationException("Specified amount not available.");
            }

            sender.Balance -= amount;
            var transfer = new MoneyTransfer(sender, recipient, amount);
            sender.PendingTransfers.Add(transfer);
            return transfer;
        }


        public bool ExecuteMoneyTransfer(IMoneyTransfer transfer)
        {
            if (transfer.Completed)
            {
                return false;
            }

            transfer.Recipient.Balance += transfer.Amount;
            transfer.Sender.PendingTransfers.Remove(transfer);
            transfer.Sender.CompletedTransfers.Add(transfer);
            transfer.Recipient.CompletedTransfers.Add(transfer);
            transfer.SetCompleted();

            return true;
        }
    }
}
