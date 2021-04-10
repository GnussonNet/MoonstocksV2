using System;
using System.Windows.Input;

namespace Moonstocks.Commands
{
    public abstract class CommandBase : ICommand
    {
        #region -- Events --
        // Can execute event
        public event EventHandler CanExecuteChanged;
        #endregion

        #region -- Execute Methods --
        // Can execute boolean
        public virtual bool CanExecute(object parameter) => true;

        // Execute Method
        public abstract void Execute(object parameter);
        #endregion

        #region -- Invoke Events --
        protected void OnCanExecuteChanged()
        {
            // Invoke CanExecute event
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
