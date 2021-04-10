using Firebase.Auth;
using Moonstocks.Commands;
using Moonstocks.Secrets;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class CreateAccountViewModel : ViewModelBase
    {
        #region -- Properties --
        // User service (Handels authentication)
        private readonly UserService _userService;

        // Navigation store (Handels navigation)
        private readonly NavigationStore _navigationStore;

        // Is busy boolean (loadingscreen when executing methods)
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        // Email
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        // Display name
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged(); }
        }

        // Password
        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        // Navigate to sign in command
        public ICommand NavigateSignInCommand { get; }

        // Create account relay
        public ICommand CreateAccountCommand { get; }
        #endregion

        #region -- Constructor --
        public CreateAccountViewModel(NavigationStore navigationStore, UserService userService)
        {
            // Get user service
            _userService = userService;

            // Get navigation store
            _navigationStore = navigationStore;

            // Define navigate to sign in command
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));

            // Define create account relay
            CreateAccountCommand = new RelayCommand(CreateAccount, CanCreateAccount);

            // Subscribe to user service - create account event
            _userService.UserCreated += _userService_UserCreated;
        }
        #endregion

        #region -- Methods --
        private void _userService_UserCreated()
        {
            // Navigate to sign in when user created 
            NavigateSignInCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async Task CreateAccount()
        {
            // Set page to busy
            IsBusy = true;

            // Declare auth provider
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Create account
                _userService.CreateAccount(await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password, DisplayName));
            }
            catch (Exception ex)
            {
                // Show error message
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Set page to not busy
                IsBusy = false;
            }
        }

        private bool CanCreateAccount()
        {
            // Return true to create account relay
            return true;
        }
        #endregion
    }
}
