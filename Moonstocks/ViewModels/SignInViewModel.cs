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
    public class SignInViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }

        public SignInViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
        }
    }
}
