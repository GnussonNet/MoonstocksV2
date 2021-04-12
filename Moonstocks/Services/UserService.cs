using Moonstocks.Models;
using System;
using Firebase.Auth;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Moonstocks.Secrets;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;

namespace Moonstocks.Services
{
    public class UserService
    {
        #region -- Global ish --
        // This is the location for the user data. This is required to be able to auto signin user
        string storedAuthFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks/authData.json");
        #endregion

        #region -- Events --
        // This event fires when any user chnages are made
        public event Action CurrentUserChanged;

        // This event fires when a user signs in. It is required to redirect user to home page when successfully signed in
        public event Action CurrentUserSignedIn;

        // Fires when a user signs out. Required to redirect user to sign in page after succssfully signed out
        public event Action CurrentUserSignedOut;

        // Fires when a new user is created. Required to redirect user to sign in page after successfully created a new account
        public event Action UserCreated;
        #endregion

        #region -- Properties --
        // This boolean is required to automatically redirect users between pages depending on sign in status
        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { _isSignedIn = value; OnCurrentUserSignedInChanged(); }
        }

        // This stores the users token etc. Via this property the other viewmodels can access user information (NOT PASSWORD)
        // The reason why this is not named UserModel is because this deserializes from FirebaseAuthLink, therefor authModel
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
            // This just defins the CurrentUser AuthModel. It is necessarily else will give an error "can't get .... of null"
            CurrentUser = new AuthModel();
            CurrentUser.User = new AuthUserModel();
        }
        #endregion

        #region -- Methods --
        /// <summary>
        /// Simply return the users displayname. Could be accessed by "UserService.CurrentUser.User.displayName" which is very long and un pleasant 
        /// </summary>
        /// <returns> Users displayName </returns>
        public string GetDisplayName()
        {
            return CurrentUser.User.displayName;
        }

        /// <summary>
        /// This returns the users authentication token. This is a shortcut to "UserService.CurrentUser.User.idToken"
        /// </summary>
        /// <returns> Users authentication token </returns>
        public string GetToken()
        {
            return CurrentUser.idToken;
        }

        /// <summary>
        /// Sign in user with email and password
        /// </summary>
        /// <param name="email"> Binded to email textbox via SignInViewModel </param>
        /// <param name="password"> Binded to password textbox via SignInViewModel </param>
        /// <param name="RememberMe"> Binded to rememberMe checkbox via SignInViewModel </param>
        public async Task SignInUser(string email, string password, bool RememberMe)
        {
            // New Firebase auth provider, using firebase secrets in ./secrets/Credentials.cs/FirebaseApiKey
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Store and sign in user with email and password
                FirebaseAuthLink userData = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Refresh user token (this is necessarily to always have a valid auth token)
                await userData.GetFreshAuthAsync();

                // Store user data ONLY if user checked the checkbox. (This does NOT store PASSWORD)
                if (RememberMe)
                    File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));

                // Serialize, Deserialize users data to the current user Auth Model propertie. This is required to be able to access user from all views and viewmodels
                CurrentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));

                // Set signed in state to true (Propertychanged will create an event which redirects the user to home page)
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // If by any chance the user is signed in this will make sure the user gets signed out
                IsSignedIn = false;

                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Sign in user with stored user data
        /// </summary>
        /// <param name="jsonUserData"> user data stored locally (json format) </param>
        public async Task SignInUser(string jsonUserData)
        {
            // New Firebase auth provider, using firebase secrets in ./secrets/Credentials.cs/FirebaseApiKey
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Store and sign in user with the stored user data
                FirebaseAuthLink userData = await authProvider.RefreshAuthAsync(JsonConvert.DeserializeObject<FirebaseAuth>(jsonUserData));

                // Store user data (NOT PASSWORD)
                File.WriteAllText(storedAuthFullPath, JsonConvert.SerializeObject(userData));

                // Serialize, Deserialize users data to the current user Auth Model propertie. This is required to be able to access user from all views and viewmodels
                CurrentUser = JsonConvert.DeserializeObject<AuthModel>(JsonConvert.SerializeObject(userData));

                // Set signed in state to true (Propertychanged will create an event which redirects the user to home page)
                IsSignedIn = true;
            }
            catch (Exception ex)
            {
                // If by any chance the user is signed in this will make sure the user gets signed out
                IsSignedIn = false;

                // Display error message
                MessageBox.Show(ex.Message);
            }
        }

        // Created account
        public async void CreateAccount(FirebaseAuthLink userData)
        {
            try
            {
                // Refresh user token (this is necessarily to always have a valid auth token)
                await userData.GetFreshAuthAsync();

                // define firebase and change settings
                var firebase = new FirebaseClient(
                  "https://moonstocksdata-default-rtdb.firebaseio.com/",
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(userData.FirebaseToken)
                  });

                FirebaseStockModel newUserStock = new FirebaseStockModel()
                {
                    Name = "Apple",
                    AvgPrice = "1",
                    Shares = "1",
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    Active = true
                };

                Dictionary<string, FirebaseStockModel> newUserStock1 = new Dictionary<string, FirebaseStockModel>();
                newUserStock1.Add("Apple", newUserStock);

                WatchlistModel newUserWatchlist = new WatchlistModel()
                {
                    Name = "My Watchlist",
                    Stocks = newUserStock1
                };

                // Define watchlists and stocks (all watchlist in users directory)
                await firebase.Child("users/"+userData.User.LocalId+"/My Watchlist").PutAsync(newUserWatchlist);

                // Fire user created event. Needed to navigate user to sign in page
                UserCreated?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task SignOutUser()
        {
            try
            {
                // THIS SHOULD MAKE THE AUTH TOKEN INVALID TO MAKE SURE NO HACKERS CAN ACCESS THE ACCOUNT
                // If userdata is stored, delete it. This is to make sure the auth token is gone.
                if (File.Exists(storedAuthFullPath))
                    File.Delete(storedAuthFullPath);

                // Clear current user
                CurrentUser = null;

                // Set signed in state to false (Propertychanged will create an event which redirects the user to sign in page)
                IsSignedIn = false;

            }
            catch (Exception ex)
            {
                // If the signout proccess is given any errors and the user still is signed in, this will keep the user signed in
                if (CurrentUser.idToken != null)
                    IsSignedIn = true;

                MessageBox.Show(ex.Message);
            }
        }

        private void OnCurrentUserSignedInChanged()
        {
            // If user is signed in, fire the signed in event to redirect user to home page
            if (IsSignedIn)
                CurrentUserSignedIn?.Invoke();

            // If user is signed out, fire the signed out event to redirect user to sign in page
            else
                CurrentUserSignedOut?.Invoke();
        }

        private void OnCurrentUserChanged()
        {
            // If any user data is changed ex. auth token etc. fire user changed event to update subscribed models
            CurrentUserChanged?.Invoke();
        }
        #endregion

        #region -- Overrides --
        public override string ToString()
        {
            // This returs the signed in users uid. This makes things easier when trying to access the users uid
            // Could be replaced by a method called "getUid" but this is more pleasant
            return _currentUser.User.localId;
        }
        #endregion
    }
}
