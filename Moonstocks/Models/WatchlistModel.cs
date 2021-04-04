using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.Models
{
    public class WatchlistModel : INotifyPropertyChanged
    {

        #region -- Properties --
        private string _name;
        public string Name
        {
            get { return _name; }
            set { if(_name != value) { _name = value; OnPropertyChanged(); } }
        }

        private Dictionary<string, StockModel> _stocks;
        public Dictionary<string, StockModel> Stocks
        {
            get { return _stocks; }
            set { if (_stocks != value) { _stocks = value; OnPropertyChanged(); } }
        }

        #endregion

        #region -- Constructor --
        public WatchlistModel()
        {
        }
        #endregion

        #region -- Overrides --
        public override string ToString()
        {
            return Name;
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
