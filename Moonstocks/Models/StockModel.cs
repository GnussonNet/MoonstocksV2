﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Moonstocks.Models
{
    public class StockModel : INotifyPropertyChanged
    {
        #region -- Properties --
        private string _name;
        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; OnPropertyChanged("Name"); } }
        }

        private string _avgPrice;
        public string AvgPrice
        {
            get { return _avgPrice; }
            set { if (_avgPrice != value) { _avgPrice = value; OnPropertyChanged("AvgPrice"); } }
        }

        private string _shares;
        public string Shares
        {
            get { return _shares; }
            set { if (_shares != value) { _shares = value; OnPropertyChanged("Shares"); } }
        }

        private double _daysLeft;
        public double DaysLeft
        {
            get { return _daysLeft; }
            set { if (_daysLeft != value) { _daysLeft = value; OnPropertyChanged("DaysLeft"); } }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { if (_active != value) { _active = value; OnPropertyChanged("Active"); } }
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
