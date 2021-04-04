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

        private readonly NavigationStore _navigationStore;
        private readonly UserService _userService;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public ICommand ResetPasswordCommand { get; private set; }
        public ICommand NavigateSignInCommand { get; }


        public ForgotPasswordView(NavigationStore navigationStore, UserService userService)
        {
            IsBusy = true;
            _userService = userService;
            _navigationStore = navigationStore;
            ResetPasswordCommand = new RelayCommand(SendResetEmail, CanSendResetEmail);
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService));
            IsBusy = false;
        }

        private bool CanSendResetEmail()
        {
            return true;
        }

        private async Task SendResetEmail()
        {
            IsBusy = true;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                await authProvider.SendPasswordResetEmailAsync(Email);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsBusy = false;
        }
    }
}
