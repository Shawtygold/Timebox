using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;

namespace Timebox.MVVM.ViewModel.FormViewModel
{
    class HourglassModificationViewModel : Core.ViewModel
    {
        //ADD
        public HourglassModificationViewModel(string action)
        {
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);

            Action = action;
        }

        //EDIT
        public HourglassModificationViewModel(string action, Hourglass hourglass)
        {
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);
            
            Action = action;

            Id = hourglass.Id;
            Description = hourglass.Description;
            Hours = hourglass.Interval / 3600;
            Minutes = (hourglass.Interval % 3600) / 60;
            Seconds = (hourglass.Interval % 3600) % 60; 
        }

        #region [Properties]

        public string Action { get; set; }

        public ulong Id { get; set; }

        private string _description = "";
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

        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand AcceptCommand { get; set; }

        private void Close(object obj)
        {
            if (obj is not Window form)
                return;

            form.Close();
        }
        private void Minimize(object obj)
        {
            if (obj is not Window form)
                return;

            form.WindowState = WindowState.Minimized;
        }
        private async void Accept(object obj)
        {
            if (obj is not Window form)
                return;

            string msg = Validator.ValidateHourglass(Hours, Minutes, Seconds);

            if(msg == "OK")
            {
                int intervalInSeconds = (Hours * 3600) + (Minutes * 60) + Seconds;

                if (Action == "ADD")
                {
                    if (!await HourglassDatabase.AddHourglass(new Hourglass(Description, intervalInSeconds)))
                        MessageBox.Show("Error! Hourglass has not been added to the database.");

                    form.Close();
                }
                else if(Action == "EDIT")
                {
                    if (!await HourglassDatabase.EditHourglass(new Hourglass(Id, Description, intervalInSeconds)))
                        MessageBox.Show("Error! The hourglass has not been edited in database.");

                    form.Close();
                }
            }
            else
            {
                MessageBox.Show(msg);
            }
        }

        #endregion
    }
}
