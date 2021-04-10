using Moonstocks.ViewModels;
using System;

namespace Moonstocks.Stores
{
    public class NavigationStore
    {
        #region -- Events --
        // Viewmodel changed event
        public event Action CurrentViewModelChanged;
        #endregion

        #region -- Properties --
        // Viewmodel
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;

            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        #endregion

        #region -- Methods --
        private void OnCurrentViewModelChanged()
        {
            // Invoke viewmodel changed event
            CurrentViewModelChanged?.Invoke();
        }
        #endregion

    }
}
