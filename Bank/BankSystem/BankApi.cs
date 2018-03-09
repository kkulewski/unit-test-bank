﻿using System;
using BankSystem.Account;
using BankSystem.Authentication;
using BankSystem.User;

namespace BankSystem
{
    public class BankApi
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly IUserStore _userStore;

        public BankApi(IAuthenticationService authenticationService, IAccountService accountService, IUserStore userStore)
        {
            _authenticationService = authenticationService;
            _accountService = accountService;
            _userStore = userStore;
        }

        public bool SignIn(string login, string password)
        {
            return _authenticationService.Authenticate(login, password);
        }

        public bool SignOut()
        {
            return _authenticationService.Deauthenticate();
        }

        public decimal GetMyAccountBalance()
        {
            if (!_authenticationService.IsAuthenticated())
            {
                throw new Exception("You are not signed in.");
            }

            var user = _authenticationService.SignedUser;
            return _accountService.GetBalance(user);
        }

        public bool SendMoneyTransfer(string recipientLogin, decimal amount)
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return false;
            }

            var sender = _authenticationService.SignedUser;
            if (sender == null)
            {
                return false;
            }

            var recipient = _userStore.GetUserByLogin(recipientLogin);
            if (recipient == null)
            {
                return false;
            }

            var transfer = _accountService.CreateMoneyTransfer(sender, recipient, amount);
            if (transfer == null)
            {
                return false;
            }

            var result = _accountService.ExecuteMoneyTransfer(transfer);
            return result;
        }
    }
}
