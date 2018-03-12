﻿using BankSystem.DAL;
using BankSystem.Models;

namespace BankSystem.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserStore _userStore;
        private readonly IUserFactory _userFactory;

        public IUser SignedUser { get; private set; }

        public AuthenticationService(IUserStore userStore, IUserFactory userFactory)
        {
            _userStore = userStore;
            _userFactory = userFactory;
        }

        public bool IsAuthenticated()
        {
            return SignedUser != null;
        }

        public bool Authenticate(string login, string password)
        {
            var user = _userStore.GetUserByLogin(login);
            if (user == null)
            {
                return false;
            }

            if (user.Password != password)
            {
                return false;
            }

            SignedUser = user;
            return true;
        }

        public bool Deauthenticate()
        {
            if (SignedUser == null)
            {
                return false;
            }

            SignedUser = null;
            return true;
        }
    }
}