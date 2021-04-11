using Moonstocks.Commands;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        #region -- Global Ish --
        // Stored user data location
        string storedAuthPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks");

        // Stored user data full location
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
            set { _isBusy = value; OnPropertyChanged(); }
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
            set { _email = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignIn)); }
        }

        // Password
        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignIn)); }
        }

        //public bool CanSignIn => !string.IsNullOrEmpty(Email) && Email.Length > 4;
        public bool CanSignIn => Email != null &&
            Password != null &&
            new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
                RegexOptions.IgnoreCase).
            IsMatch(Email) && Password.Length > 6;


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

            #region Commands
            // Declare sign in relay
            SignInCommand = new SignInCommand(this, _userService);

            // Declare navigate forgot password command
            ForgotPasswordCommand = new NavigateCommand<ForgotPasswordView>(_navigationStore, () => new ForgotPasswordView(_navigationStore, _userService));

            // Declare navigate home command
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(_navigationStore, () => new HomeViewModel(_navigationStore, _userService));

            // Declare navigate create account command
            NavigateCreateAccountCommand = new NavigateCommand<CreateAccountViewModel>(navigationStore, () => new CreateAccountViewModel(navigationStore, _userService));
            #endregion

            // Subscribe to Current user signed in to automatically navigate user when signed in
            _userService.CurrentUserSignedIn += UserSignedIn;

            // If directory where user data is stored exists, Sign in user with stored data
            Directory.CreateDirectory(storedAuthPath);
            if (File.Exists(storedAuthFullPath))
                _userService.SignInUser(File.ReadAllText(storedAuthFullPath));
            else
                IsBusy = false;
        }
        #endregion

        #region -- Methods --
        private void UserSignedIn()
        {
            // Navigate create account command
            NavigateHomeCommand.Execute(new NavigateCommand<HomeViewModel>(_navigationStore, () => new HomeViewModel(_navigationStore, _userService)));
        }
        #endregion
    }
}
