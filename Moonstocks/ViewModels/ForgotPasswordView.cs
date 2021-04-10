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
    public class ForgotPasswordView : ViewModelBase
    {
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

        // Email
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        // Reset password relay
        public ICommand ResetPasswordCommand { get; private set; }

        // Navigate to sign in command
        public ICommand NavigateSignInCommand { get; }
        #endregion

        #region -- Constructor --
        public ForgotPasswordView(NavigationStore navigationStore, UserService userService)
        {
            // Busy
            IsBusy = true;

            // Get user service
            _userService = userService;

            // Get navigation store
            _navigationStore = navigationStore;

            // Declare reset password relay
            ResetPasswordCommand = new RelayCommand(SendResetEmail, CanSendResetEmail);

            // Declare navigate to sign in command
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService));

            // Not busy
            IsBusy = false;
        }
        #endregion

        #region -- Methods --
        private bool CanSendResetEmail()
        {
            // Return true to reset password relay
            return true;
        }

        private async Task SendResetEmail()
        {
            // Busy
            IsBusy = true;

            // Define auth provider
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                // Send password reset email
                await authProvider.SendPasswordResetEmailAsync(Email);
            }
            catch (Exception ex)
            {
                // Error message
                MessageBox.Show(ex.Message);
            }
            
            // Not busy
            IsBusy = false;
        }
        #endregion
    }
}
