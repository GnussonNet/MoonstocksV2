using Moonstocks.Services;
using Moonstocks.Stores;

namespace Moonstocks.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region -- Properties
        // Navigation store (Navigation)
        private readonly NavigationStore _navigationStore;

        // User service (Authentication)
        private readonly UserService _userService;

        // Current viemodel
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        #endregion

        #region -- Constructor --
        public MainViewModel(NavigationStore navigationStore, UserService userService)
        {
            // Get user service
            _userService = userService;

            // Get navigation store
            _navigationStore = navigationStore;

            // Subscribe to navigation store - viewmodel changed event
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }
        #endregion

        #region -- Methods --
        private void OnCurrentViewModelChanged()
        {
            // Update currentviewmodel to navigationstore viewmodel
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        #endregion
    }
}
