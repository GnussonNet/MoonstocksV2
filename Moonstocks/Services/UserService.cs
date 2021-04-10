using Moonstocks.Models;
using System;
using Firebase.Auth;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Moonstocks.Services
{
    public class UserService
    {
        #region -- Global ish --
        // Auth Data json file location path
        string storedAuthFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks/authData.json");
        #endregion

        #region -- Events --
        // Current user changed
        public event Action CurrentUserChanged;

        // User signed in
        public event Action CurrentUserSignedIn;

        // User signed out
        public event Action CurrentUserSignedOut;

        // User created
        public event Action UserCreated;
        #endregion

        #region -- Properties --
        // Is signed in boolean
        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { _isSignedIn = value; OnCurrentUserSignedInChanged(); }
        }

        // Current user data
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
        #endregion

        #region -- Constructor --
        public UserService()
        {
            // Create new user with authModel and authUserModel
            CurrentUser = new AuthModel();
            CurrentUser.User = new AuthUserModel();
        }
        #endregion

        #region -- Methods --
        // Return user displayname
        public string GetDisplayName()
        {
            return CurrentUser.User.displayName;
        }

        // Return user Id token
        public string GetToken()
        {
            return CurrentUser.idToken;
        }

        // Return user Id
        public string GetUid()
        {
            return CurrentUser.User.localId;
        }

        // Store signed in user data
        public void SignInUser(FirebaseAuthLink userData, bool saveData)
        {
            try
            {
                // Update id token
                userData.GetFreshAuthAsync();

                // Deserialize userData
                CurrentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));

                // If remember me checkbox is true, store user data
                if (saveData)
                    File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));

                // If no errors, isSignedIn = true
                IsSignedIn = true;
            }
            catch (Exception)
            {
                // If errors, isSignedIn = false
                IsSignedIn = false;
            }
        }

        // Created account
        public void CreateAccount(FirebaseAuthLink userData)
        {
            try
            {
                // Update user data
                userData.GetFreshAuthAsync();

                // Invoke user created event
                UserCreated?.Invoke();
            }
            catch (Exception)
            {
            }
        }

        public void SignOutUser()
        {
            try
            {
                // If userdata is stored, delete it
                if (File.Exists(storedAuthFullPath))
                    File.Delete(storedAuthFullPath);

                // Clear current user
                CurrentUser = null;

                // IsSignedIn = false
                IsSignedIn = false;

            }
            catch (Exception ex)
            {
                // If any errors, Show messagebox with error message
                MessageBox.Show("Something went wrong when trying to signout\n" +ex.Message);

                // If id token still exists, isSignedIn true 
                if (CurrentUser.idToken != null)
                    IsSignedIn = true;
            }
        }

        private void OnCurrentUserSignedInChanged()
        {
            // If signed in, invoke signed in event
            if (IsSignedIn)
                CurrentUserSignedIn?.Invoke();

            // If signed out, invoke signed out event 
            else
                CurrentUserSignedOut?.Invoke();
        }

        private void OnCurrentUserChanged()
        {
            // Invoke userchanged event
            CurrentUserChanged?.Invoke();
        }
        #endregion

        #region -- Overrides --
        public override string ToString()
        {
            // Return user uid instead of object
            return _currentUser.User.localId;
        }
        #endregion
    }
}
