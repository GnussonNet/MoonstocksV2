using Firebase.Auth;
using Moonstocks.Commands;
using Moonstocks.Secrets;
using Moonstocks.Services;
using Moonstocks.Stores;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        #region -- Global Ish --
        // Stored user data location
        string storedAuthPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks");

        // Stored user data location full
        string storedAuthFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks/authData.json");
        #endregion

        #region -- Properties --
        // Navigation store (Navigation)
        private readonly NavigationStore _navigationStore;

        // User service (Authentication)
        private readonly UserService _userService;

        // Is busy (Loadingscreen when executing)
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        // Remember me boolean
        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set { _rememberMe = value; OnPropertyChanged(); }
        }

        // Email
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        // Password
        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        // Sign in relay
        public ICommand SignInCommand { get; private set; }

        // Navigate forgot password command
        public ICommand ForgotPasswordCommand { get; private set; }

        // Navigate home command
        public ICommand NavigateHomeCommand { get; }

        // Navigate create account command
        public ICommand NavigateCreateAccountCommand { get; }
        #endregion

        #region -- Constructor --
        public SignInViewModel(NavigationStore navigationStore, UserService userService)
        {
            // Get user service
            _userService = userService;

            // Get navigation store
            _navigationStore = navigationStore;

            // Declare sign in relay
            SignInCommand = new RelayCommand(async () => { IsBusy = true; await _userService.SignInUser(Email, Password, RememberMe); IsBusy = false; }, CanSignIn);

            // Declare navigate forgot password command
            ForgotPasswordCommand = new NavigateCommand<ForgotPasswordView>(_navigationStore, () => new ForgotPasswordView(_navigationStore, _userService));

            // Declare navigate home command
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(_navigationStore, () => new HomeViewModel(_navigationStore, _userService));

            // Declare navigate create account command
            NavigateCreateAccountCommand = new NavigateCommand<CreateAccountViewModel>(navigationStore, () => new CreateAccountViewModel(navigationStore, _userService));

            // Subscribe to user service - user signed in
            _userService.CurrentUserSignedIn += CurrentUserSignedIn;

            // If directory where user data is stored exists, Sign in user with stored data
            Directory.CreateDirectory(storedAuthPath);
            if (File.Exists(storedAuthFullPath))
                _userService.SignInUser(File.ReadAllText(storedAuthFullPath));
        }
        #endregion

        #region -- Methods --
        private void CurrentUserSignedIn()
        {
            // Navigate create account command
            NavigateHomeCommand.Execute(new NavigateCommand<CreateAccountViewModel>(_navigationStore, () => new CreateAccountViewModel(_navigationStore, _userService)));
        }

        /// <summary>
        /// Sign in user with Email and Password
        /// </summary>
        private async Task SignInUser()
        {
            //// Busy
            //IsBusy = true;

            ////// Define auth provider
            ////var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            ////try
            ////{
            ////    // Sign in user
            ////    _userService.SignInUser(await authProvider.SignInWithEmailAndPasswordAsync(Email, Password), RememberMe);
            ////}
            ////catch (Exception ex)
            ////{
            ////    // Display error message
            ////    MessageBox.Show(ex.Message);
            ////}
            ////finally
            ////{
            ////    // Not busy
            ////    IsBusy = false;
            ////}
            //await _userService.SignInUser(Email, Password, RememberMe);
        }

        /// <summary>
        /// Sign in user with stored data
        /// </summary>
        /// <param name="userData"> Stored user data in roaming </param>
        //private async void SignInUser(string userData)
        //{
        //    // Busy
        //    IsBusy = true;

        //    // Define auth provider
        //    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
        //    try
        //    {
        //        // Sign in user
        //        _userService.SignInUser(await authProvider.RefreshAuthAsync(JsonConvert.DeserializeObject<FirebaseAuth>(userData)), true);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Display error message
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        // Not busy
        //        IsBusy = false;
        //    }
        //}

        private bool CanSignIn()
        {
            // Return true to sign in relay
            return true;
        }
        #endregion
    }
}
