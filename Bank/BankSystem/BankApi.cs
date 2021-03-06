﻿using System;
using BankSystem.DAL;
using BankSystem.Services.Account;
using BankSystem.Services.Authentication;

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
            
            try
            {
                var transfer = _accountService.CreateMoneyTransfer(sender, recipient, amount);
                return _accountService.ExecuteMoneyTransfer(transfer);
            }
            catch (AccountOperationException)
            {
                return false;
            }
        }

        public bool SignUp(string login, string password)
        {
            return _authenticationService.SignUp(login, password);
        }
    }
}
