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
            // Declare navigation store (Navigation)
            NavigationStore navigationStore = new NavigationStore();

            // Declare user service (Authentication)
            UserService userSerice = new UserService();

            // Startup viemodel
            navigationStore.CurrentViewModel = new SignInViewModel(navigationStore, userSerice);

            // Declare mainwindow
            MainWindow = new MainWindow()
            {
                // Set datacontext to mainviewmodel
                DataContext = new MainViewModel(navigationStore, userSerice)
            };

            // Show mainwindow 
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
