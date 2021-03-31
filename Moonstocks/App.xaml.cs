using Moonstocks.Stores;
using Moonstocks.ViewModels;
using Moonstocks.Views;
using Moonstocks.Models;
using System.Windows;
using Moonstocks.Services;

namespace Moonstocks
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            UserService userSerice = new UserService();
            navigationStore.CurrentViewModel = new SignInViewModel(navigationStore, userSerice);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore, userSerice)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
