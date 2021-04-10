using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.Models
{
    public class WatchlistModel : INotifyPropertyChanged
    {

        #region -- Properties --
        // Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set { if(_name != value) { _name = value; OnPropertyChanged("Name"); } }
        }

        // Dictionary of all stocks and corsponding data (This is for deserializing firebase)
        private Dictionary<string, FirebaseStockModel> _stocks;
        public Dictionary<string, FirebaseStockModel> Stocks
        {
            get { return _stocks; }
            set { if (_stocks != value) { _stocks = value; OnPropertyChanged("Stocks"); } }
        }

        // OS of all stocks and corsponding data (This is for datagrid)
        private ObservableCollection<StockModel> _stocksOS = new ObservableCollection<StockModel>();
        public ObservableCollection<StockModel> StocksOS
        {
            get { return _stocksOS; }
            set { if (_stocksOS != value) { _stocksOS = value; OnPropertyChanged("StocksOS"); } }
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
            // Return name instead of oject
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
