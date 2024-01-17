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

            NavigateToAlarmsCommand = new RelayCommand(NavigateToAlarm);   
            NavigateToHourglassCommand = new RelayCommand(NavigateToHourglass);
            NavigateToStopwatchCommand = new RelayCommand(NavigateToStopwatch);
            NavigateToClockCommand = new RelayCommand(NavigateToClock);

            Navigation.NavigateTo<AlarmsViewModel>();                    
        }

        #region [Properties]

        public string AppTitle { get; set; } = null!;

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
        public ICommand NavigateToAlarmsCommand { get; set; }
        public ICommand NavigateToHourglassCommand { get; set; }
        public ICommand NavigateToStopwatchCommand { get; set; }
        public ICommand NavigateToClockCommand { get; set; }

        private void Close(object obj) => Application.Current.Shutdown();
        private void Minimize(object obj) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        private void NavigateToAlarm(object obj) => Navigation.NavigateTo<AlarmsViewModel>();
        private void NavigateToClock(object obj) => Navigation.NavigateTo<ClockViewModel>();
        private void NavigateToStopwatch(object obj) => Navigation.NavigateTo<StopwatchViewModel>();
        private void NavigateToHourglass(object obj) => Navigation.NavigateTo<HourglassViewModel>();

        #endregion
    }
}
