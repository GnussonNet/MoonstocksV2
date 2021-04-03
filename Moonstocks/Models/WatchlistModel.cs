using System.ComponentModel;

namespace Moonstocks.Models
{
    public class WatchlistModel : INotifyPropertyChanged
    {

        #region -- Properties --
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private StocksModel _stocks;
        public StocksModel Stocks
        {
            get { return _stocks; }
            set { _stocks = value; OnPropertyChanged("Stocks"); }
        }

        #endregion

        #region -- Constructor --
        public WatchlistModel(StocksModel stocks)
        {
            Stocks = stocks;
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
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
