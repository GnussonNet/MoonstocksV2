using Moonstocks.Models;
using System;

namespace Moonstocks.Stores
{
    public class UserStore
    {
        public event Action CurrentUserChanged;

        private AuthModel _currentUser;
        public AuthModel CurrentUser
        {
            get => _currentUser;

            set
            {
                _currentUser = value;
                OnCurrentUserChanged();
            }
        }

        public UserStore()
        {
            _currentUser = new AuthModel();
            _currentUser.User = new AuthUserModel();
        }

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }
    }
}
