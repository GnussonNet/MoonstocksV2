using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Stores;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly AuthModel _authUser;
        public string DisplayName { get; }
        public ICommand NavigateSignInCommand { get; }

        public HomeViewModel(NavigationStore navigationStore, AuthModel authUser)
        {
            DisplayName = authUser.User.displayName;
            _authUser = authUser;
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _authUser));
        }
    }
}
