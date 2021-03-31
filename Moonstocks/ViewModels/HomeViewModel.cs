using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly NavigationStore _navigationStore;

        public string DisplayName { get; }

        public ICommand NavigateLogoutCommand { get; }
        public ICommand SignOutCommand { get; private set; }

        public HomeViewModel(NavigationStore navigationStore, UserService userService)
        {
            _userService = userService;
            DisplayName = userService.GetDisplayName();
            SignOutCommand = new RelayCommand(SignOutUser, CanSignOut);
            NavigateLogoutCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));

            _userService.CurrentUserSignedOut += _userService_CurrentUserSignedOut;
        }

        private void _userService_CurrentUserSignedOut()
        {
            NavigateLogoutCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async Task SignOutUser()
        {
            _userService.SignOutUser();
        }

        private bool CanSignOut()
        {
            return true;
        }
    }
}
