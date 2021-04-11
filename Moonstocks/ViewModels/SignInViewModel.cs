using Moonstocks.Commands;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.IO;
using System.Threading.Tasks;
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
            IsBusy = true;

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
            else
                IsBusy = false;
        }
        #endregion

        #region -- Methods --
        private void CurrentUserSignedIn()
        {
            // Navigate create account command
            NavigateHomeCommand.Execute(new NavigateCommand<CreateAccountViewModel>(_navigationStore, () => new CreateAccountViewModel(_navigationStore, _userService)));
        }

        private bool CanSignIn()
        {
            return true;
        }
        #endregion
    }
}
