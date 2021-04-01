using Firebase.Auth;
using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Secrets;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class CreateAccountViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly NavigationStore _navigationStore;


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged("Email"); }
        }
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged("DisplayName"); }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged("Password"); }
        }


        public ICommand NavigateSignInCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand CreateAccountCommand { get; }

        public CreateAccountViewModel(NavigationStore navigationStore, UserService userService)
        {
            _userService = userService;
            _navigationStore = navigationStore;
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));
            CreateAccountCommand = new RelayCommand(CreateAccount, CanCreateAccount);

            _userService.UserCreated += _userService_UserCreated;
        }

        private void _userService_UserCreated()
        {
            NavigateSignInCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async Task CreateAccount()
        {
            IsBusy = true;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                _userService.CreateAccount(await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password, DisplayName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanCreateAccount()
        {
            return true;
        }

    }
}
