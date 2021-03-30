using Firebase.Auth;
using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Secrets;
using Moonstocks.Stores;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private AuthModel _authUser;
        private readonly NavigationStore _navigationStore;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set => _isBusy = value; 
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged("Email"); }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged("Password"); }
        }
        //private SecureString _password;
        //public SecureString Password
        //{
        //    private get => _password;
        //    set { _password = value; OnPropertyChanged("Password"); }
        //}

        public ICommand SignInCommand { get; private set; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateCreateAccountCommand { get; }

        public SignInViewModel(NavigationStore navigationStore, AuthModel authUser)
        {
            _authUser = authUser;
            _navigationStore = navigationStore;
            SignInCommand = new SignInCommand(SignInUser, CanSignIn);
            NavigateHomeCommand = new NavigateSignInCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore, _authUser));
            NavigateCreateAccountCommand = new NavigateCommand<CreateAccountViewModel>(navigationStore, () => new CreateAccountViewModel(navigationStore, _authUser));
        }

        private async Task SignInUser()
        {
            IsBusy = true;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                _authUser.idToken = auth.FirebaseToken;
                _authUser.refreshToken = auth.RefreshToken;
                _authUser.User.localId = auth.User.LocalId;
                _authUser.User.displayName = auth.User.DisplayName;
                _authUser.User.email = auth.User.Email;
                _authUser.User.emailVerified = auth.User.IsEmailVerified;

                await auth.GetFreshAuthAsync();
                NavigateHomeCommand.Execute(new NavigateCommand<CreateAccountViewModel>(_navigationStore, () => new CreateAccountViewModel(_navigationStore, _authUser)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsBusy = false;
        }

        private bool CanSignIn()
        {
            return true;
        }
    }
}
