using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class CreateAccountViewModel : ViewModelBase
    {
        private readonly AuthModel _authUser;
        public ICommand NavigateSignInCommand { get; }

        public CreateAccountViewModel(NavigationStore navigationStore, AuthModel authUser)
        {
            _authUser = authUser;
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _authUser));
        }
    }
}
