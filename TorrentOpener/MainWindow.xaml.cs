using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TorrentOpener.Commands;
using TorrentOpener.Helpers;

namespace TorrentOpener {

  public partial class MainWindow : Window, INotifyPropertyChanged {

    #region Private Fields

    /// <summary>
    /// Need 2 way binding
    /// </summary>
    private string _category;

    #endregion Private Fields

    #region Public Constructors

    public MainWindow() {
      List<string> categoryList = ConfigurationManager.AppSettings["CategoryList"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
      categoryList.ForEach(this.CategoryList.Add);

      this.Command_SetCategory = new CustomCommand {
        CustomCanExecuteFunc = (obj) => true,
        CustomExecutAction = (obj) => { this.Category = obj.ToString(); },
      };

      this.Command_AddTorrent = new CustomCommand {
        CustomCanExecuteFunc = (obj) => !string.IsNullOrEmpty(this.Category) && !string.IsNullOrEmpty(((App)Application.Current).FullPath),
        CustomExecutAction = async (obj) => {
          await QBittorrentHelper.AddTorrent(((App)Application.Current).FullPath, this.Category);
          Application.Current.Shutdown();
        },
      };

      this.InitializeComponent();
    }

    #endregion Public Constructors

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Public Events

    #region Public Properties

    public string Category { get => this._category; set { this._category = value; this.RaisePropertyChanged(nameof(this.Category)); } }

    /// <summary>
    /// No need obserable
    /// </summary>
    public List<string> CategoryList { get; set; } = new List<string>();

    public ICommand Command_AddTorrent { get; set; }

    public ICommand Command_SetCategory { get; set; }

    #endregion Public Properties

    #region Private Methods

    private void RaisePropertyChanged(string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion Private Methods
  }
}
