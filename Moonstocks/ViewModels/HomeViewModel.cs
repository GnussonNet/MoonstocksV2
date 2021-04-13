using Firebase.Database;
using Firebase.Database.Query;
using Moonstocks.Commands;
using Moonstocks.Models;
using Moonstocks.Services;
using Moonstocks.Stores;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region -- Properties --
        // User service (Authentication)
        private readonly UserService _userService;

        // Navigation store (Navigation)
        private readonly NavigationStore _navigationStore;

        // Is busy (Loadingscreen when executing)
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); }
        }

        // Selected listview index (-1 as default)
        private int _listViewSelectedIndex = -1;
        public int ListViewSelectedIndex
        {
            get { return _listViewSelectedIndex; }
            set { _listViewSelectedIndex = value; OnPropertyChanged(); GetSelectedWatchlistStocksAsync(); }
        }

        // Display name
        public string DisplayName { get; }

        // OS for watchlists (Listview)
        private ObservableCollection<WatchlistModel> _watchlists = new ObservableCollection<WatchlistModel>();
        public ObservableCollection<WatchlistModel> Watchlists
        {
            get { return _watchlists; }
            set { _watchlists = value; OnPropertyChanged(); }
        }

        // OS for selected watchlist (Listview) to display data in datagrid
        private ObservableCollection<WatchlistModel> _selectedWatchlist = new ObservableCollection<WatchlistModel>();
        public ObservableCollection<WatchlistModel> SelectedWatchlist
        {
            get { return _selectedWatchlist; }
            set { _selectedWatchlist = value; OnPropertyChanged(); }
        }

        // Navigate sign in
        public ICommand NavigateLogoutCommand { get; }

        // Sign out relay
        public ICommand SignOutCommand { get; }
        #endregion

        #region -- Constructor --
        public HomeViewModel(NavigationStore navigationStore, UserService userService)
        {
            // Busy
            IsBusy = true;

            // Get navigation store
            _navigationStore = navigationStore;

            // Get user service
            _userService = userService;

            // Get display name
            DisplayName = userService.GetDisplayName();

            // Define sign out relay
            SignOutCommand = new RelayCommand(SignOutUser, CanSignOut);

            // Define navigate sign in command
            NavigateLogoutCommand = new NavigateCommand<SignInViewModel>(navigationStore, () => new SignInViewModel(navigationStore, _userService));

            // Run GetWatchlists method (update watchlists to listview)
            GetWatchlists();
            
            // Subscribe to user service - user sign out event
            _userService.CurrentUserSignedOut += _userService_CurrentUserSignedOut;
        }
        #endregion

        #region -- Methods --
        private void _userService_CurrentUserSignedOut()
        {
            // Navigate to sign in if user signed out
            NavigateLogoutCommand.Execute(new NavigateCommand<SignInViewModel>(_navigationStore, () => new SignInViewModel(_navigationStore, _userService)));
        }

        private async Task GetSelectedWatchlistStocksAsync()
        {
            // Busy
            IsBusy = true;

            // Wait 0.25 secound after getting watchlist stocks    
            await Task.Delay(250).ConfigureAwait(GetSelectedWatchlistStocks());

            // Not busy
            IsBusy = false;
        }

        private bool GetSelectedWatchlistStocks()
        {
            // Clear selected watchlist OS
            SelectedWatchlist.Clear();

            // Add selected watchlist OS
            SelectedWatchlist.Add(Watchlists[ListViewSelectedIndex]);

            // Return true
            return true;
        }

        private async void GetWatchlists()
        {
            // Busy
            IsBusy = true;

            // define firebase and change settings
            var firebase = new FirebaseClient(
              "https://moonstocksdata-default-rtdb.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(_userService.GetToken())
              });

            #region -- No Auto Update --
            // Define watchlists and stocks (all watchlist in users directory)
            var WatchlistsandStocks = await firebase.Child("users/" + _userService.ToString()).OrderByKey().OnceAsync<WatchlistModel>();

            if (WatchlistsandStocks != null && WatchlistsandStocks.Count != 0)
                // Each watchlist
                foreach (var watchlists in WatchlistsandStocks)
                {
                    if (watchlists.Object.Stocks != null && watchlists.Object.Stocks.Count != 0)
                    {
                        // Each stock 
                        foreach (var objectItem in watchlists.Object.Stocks)
                        {
                            // Declare and and stock data to a new stockmodel
                            StockModel stockmodel = new StockModel()
                            {
                                Company = objectItem.Key,
                                AvgPrice = objectItem.Value.AvgPrice,
                                Shares = objectItem.Value.Shares,
                                Ticker = objectItem.Value.Ticker,

                                // Calculate days left (date purchased + 1 year - todays date) 
                                DaysLeft = (objectItem.Value.Date.AddYears(1) - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalDays,
                                Active = objectItem.Value.Active
                            };
                            // Add new stockmodel to watchlist StocksOS OS (used for datagrid)
                            watchlists.Object.StocksOS.Add(stockmodel);
                            watchlists.Object.Name = watchlists.Key;
                        }
                        // Add watchlist to Watchlists OS
                        Watchlists.Add(watchlists.Object);
                    }
                }
            #endregion

            #region -- With Auto Update --
            // LIVE UPDATE, LOT OF BUGGS
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

            // Not busy
            IsBusy = false;
        }

        private async Task SignOutUser()
        {
            // Sign out user
            await _userService.SignOutUser();
        }

        private bool CanSignOut()
        {
            // Return true to can sign out command
            return true;
        }
        #endregion
    }
}
