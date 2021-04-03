using Firebase.Database;
using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Services;
using Moonstocks.Stores;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly NavigationStore _navigationStore;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        public string DisplayName { get; }

        private ObservableCollection<WatchlistModel> _watchlists;
        public ObservableCollection<WatchlistModel> Watchlists
        {
            get { return _watchlists; }
            set { _watchlists = value; }
        }


        public ICommand NavigateLogoutCommand { get; }
        public ICommand SignOutCommand { get; private set; }

        public HomeViewModel(NavigationStore navigationStore, UserService userService)
        {
            IsBusy = true;
            Watchlists = new ObservableCollection<WatchlistModel>();
            _userService = userService;
            DisplayName = userService.GetDisplayName();
            SignOutCommand = new RelayCommand(SignOutUser, CanSignOut);
            NavigateLogoutCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));

            GetWatchlists();
            _userService.CurrentUserSignedOut += _userService_CurrentUserSignedOut;
            IsBusy = false;
        }

        private void _userService_CurrentUserSignedOut()
        {
            NavigateLogoutCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async void GetWatchlists()
        {
            var firebase = new FirebaseClient(
              "https://moonstocksdata-default-rtdb.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(_userService.GetToken())
              });

            var observable = firebase.Child("users/" + _userService.GetUid()).AsObservable<dynamic>().Subscribe(d =>
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    string json = ((Newtonsoft.Json.Linq.JContainer)d.Object).Last.Root.Root.ToString();
                    Watchlists.Add(JsonConvert.DeserializeObject<WatchlistModel>(json));
                    //MessageBox.Show(json);

                    //if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                    //    Watchlists.Add(d.Object);
                    //else if (d.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
                    //    Watchlists.Remove(d.Object);
                    //else
                    //    Watchlists.Remove(d.Object);
                });
            });

        }

        private async Task SignOutUser()
        {
            _userService.SignOutUser();
        }

        private bool CanSignOut()
        {
            return true;
        }
    }
}
