﻿using BankSystem.Models;

namespace BankSystem.Services.Account
{
    public interface IAccountService
    {
        decimal GetBalance(IUser user);

        IMoneyTransfer CreateMoneyTransfer(IUser sender, IUser recipient, decimal amount);

        bool ExecuteMoneyTransfer(IMoneyTransfer transfer);
    }
}
