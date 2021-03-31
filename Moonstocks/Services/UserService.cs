using Moonstocks.Models;
using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Moonstocks.Services
{
    public class UserService
    {
        string storedAuthFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks/authData.json");

        public event Action CurrentUserChanged;
        public event Action CurrentUserSignedIn;
        public event Action CurrentUserSignedOut;
        public event Action UserCreated;

        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { _isSignedIn = value; OnCurrentUserSignedInChanged(); }
        }

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

        public UserService()
        {
            _currentUser = new AuthModel();
            _currentUser.User = new AuthUserModel();
        }

        private void OnCurrentUserSignedInChanged()
        {
            if (IsSignedIn)
                CurrentUserSignedIn?.Invoke();
            else
                CurrentUserSignedOut?.Invoke();
        }

        public string GetDisplayName()
        {
            return _currentUser.User.displayName;
        }

        public bool SignInUser(FirebaseAuthLink userData, bool saveData)
        {
            try
            {
                userData.GetFreshAuthAsync();
                _currentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));
                if (saveData)
                    File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));
                IsSignedIn = true;
            }
            catch (Exception)
            {
                IsSignedIn = false;
            }
            return IsSignedIn;
        }

        public bool CreateAccount(FirebaseAuthLink userData)
        {
            bool output = false;
            try
            {
                userData.GetFreshAuthAsync();
                UserCreated?.Invoke();
                output = true;
            }
            catch (Exception)
            {
                output = false;
            }
            return output;
        }

        public bool SignOutUser()
        {
            try
            {
                if (File.Exists(storedAuthFullPath))
                    File.Delete(storedAuthFullPath);
                _currentUser = null;
                IsSignedIn = false;

            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong when trying to create an account...");
                if (_currentUser.idToken != null)
                    IsSignedIn = true;
            }
            return IsSignedIn;

        }

        public override string ToString()
        {
            return _currentUser.User.localId;
        }

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }
    }
}
