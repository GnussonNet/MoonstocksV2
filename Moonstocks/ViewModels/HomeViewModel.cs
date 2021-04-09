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
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        private int _listViewSelectedIndex = -1;
        public int ListViewSelectedIndex
        {
            get { return _listViewSelectedIndex; }
            set { _listViewSelectedIndex = value; OnPropertyChanged(); Message(); }
        }


        public string DisplayName { get; }

        private ObservableCollection<WatchlistModel> _watchlists;
        public ObservableCollection<WatchlistModel> Watchlists
        {
            get { return _watchlists; }
            set { _watchlists = value; OnPropertyChanged(); }
        }

        private ObservableCollection<WatchlistModel> _selectedWatchlist = new ObservableCollection<WatchlistModel>();
        public ObservableCollection<WatchlistModel> SelectedWatchlist
        {
            get { return _selectedWatchlist; }
            set { _selectedWatchlist = value; OnPropertyChanged(); }
        }

        public ICommand NavigateLogoutCommand { get; }
        public ICommand SignOutCommand { get; private set; }

        public HomeViewModel(NavigationStore navigationStore, UserService userService)
        {
            IsBusy = true;
            Watchlists = new ObservableCollection<WatchlistModel>();
            _navigationStore = navigationStore;
            _userService = userService;
            DisplayName = userService.GetDisplayName();
            SignOutCommand = new RelayCommand(SignOutUser, CanSignOut);
            NavigateLogoutCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));

            GetWatchlists();
            _userService.CurrentUserSignedOut += _userService_CurrentUserSignedOut;
        }

        private void _userService_CurrentUserSignedOut()
        {
            NavigateLogoutCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async Task Message()
        {
            IsBusy = true;
            await Task.Delay(250).ConfigureAwait(Message1());
            IsBusy = false;
        }

        private bool Message1()
        {
            SelectedWatchlist.Clear();
            SelectedWatchlist.Add(Watchlists[ListViewSelectedIndex]);
            return true;
        }

        private async void GetWatchlists()
        {
            IsBusy = true;
            var firebase = new FirebaseClient(
              "https://moonstocksdata-default-rtdb.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(_userService.GetToken())
              });

            #region -- No Auto Update --
            var WatchlistsandStocks = await firebase.Child("users/" + _userService.GetUid()).OnceAsync<WatchlistModel>();
            foreach (var item in WatchlistsandStocks)
            {
                foreach (var objectItem in item.Object.Stocks)
                {
                    StockModel stockmodel = new StockModel()
                    {
                        Name = objectItem.Key,
                        AvgPrice = objectItem.Value.AvgPrice,
                        Shares = objectItem.Value.Shares,
                        DaysLeft = (objectItem.Value.Date.AddYears(1) - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) ).TotalDays,
                        Active = objectItem.Value.Active
                    };
                    item.Object.StocksOS.Add(stockmodel);
                }
                Watchlists.Add(item.Object);
            }
            #endregion

            #region -- With Auto Update --
            //var observable = firebase.Child("users/" + _userService.GetUid()).AsObservable<WatchlistModel>().Subscribe(d =>
            //{
            //    foreach (var item in d.Object.Stocks)
            //    {
            //        // Original it was NewStockModel but this is an test
            //        StockModel newStockModel = new StockModel()
            //        {
            //            Name = item.Key,
            //            AvgPrice = item.Value.AvgPrice,
            //            Shares = item.Value.Shares
            //        };
            //        App.Current.Dispatcher.Invoke((Action)delegate
            //        {
            //            d.Object.StocksOS.Add(newStockModel);
            //        });
            //    }

            //    App.Current.Dispatcher.Invoke((Action)delegate 
            //    {
            //        //if (Watchlists.Contains(d.Object))
            //        //    MessageBox.Show("CONTAINS");
            //        //else
            //        //    MessageBox.Show("NOT CONTAINS");
            //        Watchlists.Add(d.Object);
            //    });
            //});
            #endregion

            IsBusy = false;
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
