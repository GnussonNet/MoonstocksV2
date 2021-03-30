using Moonstocks.Stores;
using Moonstocks.ViewModels;
using Moonstocks.Views;
using Moonstocks.Models;
using System.Windows;

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
            AuthModel authUser = new AuthModel("","", new AuthUserModel("","","",false));
            navigationStore.CurrentViewModel = new SignInViewModel(navigationStore, authUser);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore, authUser)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
