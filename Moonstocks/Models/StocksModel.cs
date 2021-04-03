using System.ComponentModel;

namespace Moonstocks.Models
{
    public class StocksModel : INotifyPropertyChanged
    {
        #region -- Properties --
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _avgPrice;
        public string AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; OnPropertyChanged("AvgPrice"); }
        }

        private string _shares;
        public string Shares
        {
            get { return _shares; }
            set { _shares = value; OnPropertyChanged("Shares"); }
        }
        #endregion

        #region -- Constructor --
        public StocksModel()
        {

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
