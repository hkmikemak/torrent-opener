using System;
using System.Windows.Input;

namespace TorrentOpener.Commands {

  public class CustomCommand : ICommand {

    #region Public Events

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    #endregion Public Events

    #region Public Properties

    public Func<object, bool> CustomCanExecuteFunc { get; set; }
    public Action<object> CustomExecutAction { get; set; }

    #endregion Public Properties

    #region Public Methods

    public bool CanExecute(object parameter) => this.CustomCanExecuteFunc == null ? true : this.CustomCanExecuteFunc(parameter);

    public void Execute(object parameter) => this.CustomExecutAction?.Invoke(parameter);

    #endregion Public Methods
  }
}
