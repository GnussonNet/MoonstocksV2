﻿using Firebase.Auth;
using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Secrets;
using Moonstocks.Services;
using Moonstocks.Stores;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        string storedAuthPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks");
        string storedAuthFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Moonstocks/authData.json");

        private readonly NavigationStore _navigationStore;
        private readonly UserService _userService;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set { _rememberMe = value; OnPropertyChanged("RememberMe"); }
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

        public ICommand SignInCommand { get; private set; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateCreateAccountCommand { get; }

        public SignInViewModel(NavigationStore navigationStore, UserService userService)
        {
            _userService = userService;
            _navigationStore = navigationStore;
            SignInCommand = new RelayCommand(SignInUser, CanSignIn);
            NavigateHomeCommand = new NavigateSignInCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore, _userService));
            NavigateCreateAccountCommand = new NavigateCommand<CreateAccountViewModel>(navigationStore, () => new CreateAccountViewModel(navigationStore, _userService));

            _userService.CurrentUserSignedIn += CurrentUserSignedIn;

            Directory.CreateDirectory(storedAuthPath);
            if (File.Exists(storedAuthFullPath))
                SignInUser(File.ReadAllText(storedAuthFullPath));
        }

        private void CurrentUserSignedIn()
        {
            NavigateHomeCommand.Execute(new NavigateCommand<CreateAccountViewModel>(_navigationStore, () => new CreateAccountViewModel(_navigationStore, _userService)));
        }

        private async Task SignInUser()
        {
            IsBusy = true;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                _userService.SignInUser(await authProvider.SignInWithEmailAndPasswordAsync(Email, Password), RememberMe);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsBusy = false;
        }

        private async void SignInUser(string userData)
        {
            IsBusy = true;
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));
            try
            {
                _userService.SignInUser(await authProvider.RefreshAuthAsync(JsonConvert.DeserializeObject<FirebaseAuth>(userData)), true);

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
