using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.Services;

namespace Timebox.MVVM.ViewModel
{
    internal class MainViewModel : Core.ViewModel
    {
        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            AppTitle = "Timebox";

            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);

            Navigation.NavigateTo<LoadingScreenViewModel>();
        }

        #region [Properties]
        public string AppTitle { get; set; }


        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]

        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }

        private void Close(object obj) => Application.Current.Shutdown();
        private void Minimize(object obj) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        private void Maximize(object obj)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        #endregion
    }
}
