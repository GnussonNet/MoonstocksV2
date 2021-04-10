using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.Commands
{
    public class RelayCommand : IAsyncCommand
    {
        #region -- Events --
        // Can execute event
        public event EventHandler CanExecuteChanged;
        #endregion

        #region -- Properties --
        // Executing boolean
        private bool _isExecuting;

        // Execute Func
        private readonly Func<Task> _execute;

        // Can execute Func boolean
        private readonly Func<bool> _canExecute;
        #endregion

        #region -- Constructor --
        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            // Get execute
            _execute = execute;

            // Get canExecute
            _canExecute = canExecute;
        }
        #endregion

        #region -- Methods --
        public bool CanExecute()
        {
            // If previous execute is done, return true
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            // If canExecute is true
            if (CanExecute())
            {
                try
                {
                    // Executing true
                    _isExecuting = true;

                    // Execute
                    await _execute();
                }
                finally
                {
                    // Executing false
                    _isExecuting = false;
                }
            }

            // Invoke execute event method
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            // Ivoke execute event
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region -- Explicit implementations --
        async void ICommand.Execute(object parameter)
        {
            // Wait on execute
            await ExecuteAsync();
        }

        bool ICommand.CanExecute(object parameter)
        {
            // Return value of canExecute
            return CanExecute();
        }
        #endregion
    }
}
