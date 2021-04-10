using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region -- Events --
        // Property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region -- Methods --
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Invoke propertychanged event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
