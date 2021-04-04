using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.Models
{
    public class StockModel : INotifyPropertyChanged
    {
        #region -- Properties --
        private string _avgPrice;
        public string AvgPrice
        {
            get { return _avgPrice; }
            set { if (_avgPrice != value) { _avgPrice = value; OnPropertyChanged(); } }
        }

        private string _shares;
        public string Shares
        {
            get { return _shares; }
            set { if (_shares != value) { _shares = value; OnPropertyChanged(); } }
        }
        #endregion

        #region -- Constructor --
        public StockModel()
        {

        }
        #endregion

        #region -- INotifyPropertyChnaged --
        // Event
        public event PropertyChangedEventHandler PropertyChanged;

        // Method
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
