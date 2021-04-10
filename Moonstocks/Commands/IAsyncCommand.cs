using System.Threading.Tasks;
using System.Windows.Input;

namespace Moonstocks.Commands
{
    public interface IAsyncCommand : ICommand
    {
        #region -- Methods --
        // Execute async
        Task ExecuteAsync();

        // Can execute
        bool CanExecute();
        #endregion
    }
}
