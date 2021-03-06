using System.ComponentModel;

namespace Moonstocks.Models
{
    public class AuthUserModel : INotifyPropertyChanged
    {

        #region -- Properties --
        // Id token
        private string _localId;
        public string localId
        {
            get { return _localId; }
            set { _localId = value; OnPropertyChanged("localId"); }
        }

        // Display name
        private string _displayName;
        public string displayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged("displayName"); }
        }

        // Email
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("email"); }
        }

        // Email verified boolean
        private bool _emailVerified;
        public bool emailVerified
        {
            get { return _emailVerified; }
            set { _emailVerified = value; OnPropertyChanged("emailVerified"); }
        }

        #endregion

        #region -- Constructor --
        public AuthUserModel()
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
