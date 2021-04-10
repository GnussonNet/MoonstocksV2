using Moonstocks.Models;
using System;
using Firebase.Auth;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Moonstocks.Secrets;
using System.Threading.Tasks;

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

        /// <summary>
        /// Sign in user with email and password
        /// </summary>
        /// <param name="email"> Binded to email textbox via SignInViewModel </param>
        /// <param name="password"> Binded to password textbox via SignInViewModel </param>
        /// <param name="RememberMe"> Binded to rememberMe checkbox via SignInViewModel </param>
        public async Task SignInUser(string email, string password, bool RememberMe)
        {
            // Define authProvider and use Firebase API secret
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Store and sign in user with email and password
                FirebaseAuthLink userData = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Refresh user token
                await userData.GetFreshAuthAsync();

                // If rememeberMe checkbox is checked, store user data (NOT PASSWORD)
                if (RememberMe)
                    File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));

                // Add userData to CurrentUser
                CurrentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));

                // Set signed in state to true (Propertychanged will create an event which redirects the user to home page)
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // Set signed in state to false
                IsSignedIn = false;

                // Display error message
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Sign in user with stored user data
        /// </summary>
        /// <param name="jsonUserData"> user data stored locally (json format) </param>
        public async Task SignInUser(string jsonUserData)
        {
            // Define auth provider
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Sign in user
                FirebaseAuthLink userData = await authProvider.RefreshAuthAsync(JsonConvert.DeserializeObject<FirebaseAuth>(jsonUserData));

                // Store user data (NOT PASSWORD)
                File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));

                // Add userData to CurrentUser
                CurrentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));

                // Set signed in state to true (Propertychanged will create an event which redirects the user to home page)
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // Set signed in state to false
                IsSignedIn = false;

                // Display error message
                MessageBox.Show(ex.Message);
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
