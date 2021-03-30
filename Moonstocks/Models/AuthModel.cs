using System.ComponentModel;

namespace Moonstocks.Models
{
    public class AuthModel : INotifyPropertyChanged
    {

        #region -- Properties --
        private string _idToken;
        public string idToken
        {
            get { return _idToken; }
            set { _idToken = value; OnPropertyChanged("idToken"); }
        }

        private string _refreshToken;
        public string refreshToken
        {
            get { return _refreshToken; }
            set { _refreshToken = value; OnPropertyChanged("refreshToken"); }
        }

        private AuthUserModel _User;
        public AuthUserModel User
        {
            get { return _User; }
            set { _User = value; OnPropertyChanged("User"); }
        }

        #endregion

        #region -- Constructor --
        public AuthModel(string idToken, string refreshToken, AuthUserModel user)
        {
            this.idToken = idToken;
            this.refreshToken = refreshToken;
            this.User = user;
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
