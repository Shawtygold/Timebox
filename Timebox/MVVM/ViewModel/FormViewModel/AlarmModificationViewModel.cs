using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;

namespace Timebox.MVVM.ViewModel.FormViewModel
{
    class AlarmModificationViewModel : Core.ViewModel
    {
        //ADD
        public AlarmModificationViewModel(string _action)
        {
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);

            Sounds = new() { "Alarm", "Alarm2", "Alarm3", "Alarm4", "Alarm5", "Alarm6", "Alarm7", "Alarm8", "Alarm9", "Alarm10" };

            ACTION = _action;

            Hours = DateTime.Now.Hour;
            Minutes = DateTime.Now.Minute;
        }

        //EDIT
        public AlarmModificationViewModel(string _action, Alarm alarm)
        {
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);

            ACTION = _action;

            Sounds = new() { "Alarm", "Alarm2", "Alarm3", "Alarm4", "Alarm5", "Alarm6", "Alarm7", "Alarm8", "Alarm9", "Alarm10" };

            Id = alarm.Id;
            DateTime dateTime = DateTime.Parse(alarm.TriggerTime);
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            IsRepeat = alarm.IsRepeat;
            Description = alarm.Description;
            IsEnabled = alarm.IsEnabled;
            RemoveAfterTriggering = alarm.RemoveAfterTriggering;

            int indexSelectedItem = 0;
            for(int i = 0; i < Sounds.Count; i++)
            {
                if (alarm.SoundSource.EndsWith(Sounds[i]))
                {
                    indexSelectedItem = i;
                    break;
                }
            }

            SoundSource = Sounds[indexSelectedItem];
        }

        #region [Properties]

        private static string ACTION = null!;
        private ulong Id { get; set; }
        private bool IsEnabled { get; set; }

        private bool _removeAfterTriggering;
        public bool RemoveAfterTriggering
        {
            get { return _removeAfterTriggering; }
            set { _removeAfterTriggering = value; OnPropertyChanged(); }
        }

        private int _hours = 0;
        public int Hours
        {
            get { return _hours; }
            set { _hours = value; OnPropertyChanged(); }
        }

        private int _minutes = 0;
        public int Minutes
        {
            get { return _minutes; }
            set { _minutes = value; OnPropertyChanged(); }
        }

        private string _description = "";
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        private string _soundSource = "";
        public string SoundSource
        {
            get { return _soundSource; }
            set { _soundSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string>? _sounds;
        public ObservableCollection<string>? Sounds
        {
            get { return _sounds; }
            set { _sounds = value; OnPropertyChanged(); }
        }

        private bool _isRepeat;
        public bool IsRepeat
        {
            get { return _isRepeat; }
            set { _isRepeat = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand AcceptCommand { get; set; }

        private void Close(object obj)
        {
            if(obj is Window form)
                form.Close();
        }
        private void Minimize(object obj)
        {
            if (obj is Window form)
                form.WindowState = WindowState.Minimized;
        }
        private async void Accept(object obj)
        {
            if(obj is Window form)
            {
                string msg = Validator.ValidateAlarm(Hours, Minutes);
                if(msg == "OK")
                {
                    string minutes = Minutes.ToString();
                    if (Minutes < 10) minutes = "0" + minutes;

                    if (ACTION == "ADD")
                    {
                        if (!await AlarmDatabase.AddAlarm(new Alarm(Description, $"{Hours}:{minutes}", $"ms-winsoundevent:Notification.Looping.{SoundSource}", IsRepeat, true, RemoveAfterTriggering)))
                            MessageBox.Show("Error! The alarm has not been added to the database.");

                        form.Close();
                    }
                    else if(ACTION == "EDIT")
                    {
                        if (!await AlarmDatabase.EditAlarm(new Alarm(Id, Description, $"{Hours}:{minutes}", $"ms-winsoundevent:Notification.Looping.{SoundSource}", IsRepeat, IsEnabled, RemoveAfterTriggering)))
                            MessageBox.Show("Error! The alarm has not been edited in the database.");

                        form.Close();
                    }
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }

        #endregion
    }
}
