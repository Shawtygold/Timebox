using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.MVVM.Model.HourglassModel;

namespace Timebox.MVVM.ViewModel.FormViewModel
{
    class HourglassModificationViewModel : Core.ViewModel
    {
        //EDIT
        public HourglassModificationViewModel(string action, Hourglass hourglass)
        {
            AcceptCommand = new RelayCommand(Accept);
        }

        #region [Properties]

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        private int _hours;
        public int Hours
        {
            get { return _hours; }
            set { _hours = value; OnPropertyChanged(); }
        }

        private int _minutes;
        public int Minutes
        {
            get { return _minutes; }
            set { _minutes = value; OnPropertyChanged(); }
        }

        private int _seconds;
        public int Seconds
        {
            get { return _seconds; }
            set { _seconds = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]

        public ICommand AcceptCommand { get; set; }
        private void Accept(object obj)
        {
            if (obj is not Window form)
                return;

            string msg = Validator.ValidateHourglass(Hours, Minutes, Seconds);

            if(msg == "OK")
            {

            }
            else
            {
                MessageBox.Show(msg);
            }
        }

        #endregion
    }
}
