using System.Collections.ObjectModel;
using Timebox.Services;

namespace Timebox.MVVM.ViewModel
{
    internal class ClockViewModel : Core.ViewModel
    {
        public ClockViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            Model.Clock clock = new();
            Clock = new() { clock};
        }

        #region [Properties]

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Model.Clock>? _clock;
        public ObservableCollection<Model.Clock>? Clock
        {
            get { return _clock; }
            set { _clock = value; OnPropertyChanged(); }
        }

        #endregion
    }
}
