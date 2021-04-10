using Moonstocks.Stores;
using Moonstocks.ViewModels;
using System;

namespace Moonstocks.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        #region -- Properties --
        // Navigation store
        private readonly NavigationStore _navigationStore;

        // Create viewmodel func
        private readonly Func<TViewModel> _createViewModel;
        #endregion

        #region -- Constructur --
        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            // Get navigation store
            _navigationStore = navigationStore;

            // Get Createviewmodel
            _createViewModel = createViewModel;
        }
        #endregion

        #region -- Methods --
        public override void Execute(object parameter)
        {
            // Update Viewmodel on execute
            _navigationStore.CurrentViewModel = _createViewModel();
        }
        #endregion
    }
}
