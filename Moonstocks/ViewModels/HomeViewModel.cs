using Moonstocks.Commands;
using Moonstocks.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand NavigateSignInCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateSignInCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore));
        }
    }
}
