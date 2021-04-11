using Moonstocks.Services;
using Moonstocks.ViewModels;

namespace Moonstocks.Commands
{
    public class SignInCommand : CommandBase
    {
        private readonly SignInViewModel _viewModel;
        private readonly UserService _userService;

        public SignInCommand(SignInViewModel viewModel, UserService userService)
        {
            _viewModel = viewModel;
            _userService = userService;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.CanSignIn))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return _viewModel.CanSignIn;
        }

        public async override void Execute(object parameter)
        {
            _viewModel.IsBusy = true;
            await _userService.SignInUser(_viewModel.Email, _viewModel.Password, _viewModel.RememberMe);
            _viewModel.IsBusy = false;
        }
    }
}
