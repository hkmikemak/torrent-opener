using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace TorrentOpener {

  public partial class App : Application, INotifyPropertyChanged {

    #region Private Fields

    /// <summary>
    /// FullPath need 2 way binding
    /// </summary>
    private string _fullPath;

    #endregion Private Fields

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Public Events

    #region Public Properties

    /// <summary>
    /// CanEditFullPath is set before any binding from UI and will not change
    /// </summary>
    public bool CanEditFullPath { get; set; } = true;

    public string FullPath { get => this._fullPath; set { this._fullPath = value; this.RaisePropertyChanged(nameof(this.FullPath)); } }

    #endregion Public Properties

    #region Protected Methods

    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      if (e.Args != null && e.Args.Any()) {
        this.FullPath = e.Args.LastOrDefault();
        this.CanEditFullPath = false;
      }
    }

    #endregion Protected Methods

    #region Private Methods

    private void RaisePropertyChanged(string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion Private Methods
  }
}
