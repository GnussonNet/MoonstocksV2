using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.Models
{
    public class FirebaseStockModel : INotifyPropertyChanged
    {
        #region -- Properties --
        // Ticker
        private string _ticker;
        public string Ticker
        {
            get { return _ticker; }
            set { if (_ticker != value) { _ticker = value; OnPropertyChanged("Ticker"); } }
        }

        // Company
        private string _company;
        public string Company
        {
            get { return _company; }
            set { if (_company != value) { _company = value; OnPropertyChanged("Company"); } }
        }

        // Average price
        private string _avgPrice;
        public string AvgPrice
        {
            get { return _avgPrice; }
            set { if (_avgPrice != value) { _avgPrice = value; OnPropertyChanged("AvgPrice"); } }
        }

        // Shares
        private string _shares;
        public string Shares
        {
            get { return _shares; }
            set { if (_shares != value) { _shares = value; OnPropertyChanged("Shares"); } }
        }

        // Date
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { if (_date != value) { _date = value; OnPropertyChanged("Date"); } }
        }

        // Active boolean
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { if (_active != value) { _active = value; OnPropertyChanged("Active"); } }
        }
        #endregion

        #region -- Constructor --
        public FirebaseStockModel()
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
