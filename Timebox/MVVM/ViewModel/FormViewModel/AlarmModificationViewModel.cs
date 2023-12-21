using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.Services;

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
            SetSoundCommand = new RelayCommand(SetSound);

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
            SetSoundCommand = new RelayCommand(SetSound);

            ACTION = _action;

            Id = alarm.Id;
            DateTime dateTime = DateTime.Parse(alarm.TriggerTime);
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            IsRepeat = alarm.IsRepeat;
            Description = alarm.Description;
            SoundSource = alarm.SoundSource;
            IsEnabled = alarm.IsEnabled;
        }

        #region [Properties]

        private ulong Id { get; set; }
        private bool IsEnabled { get; set; }

        private static string ACTION = "";

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

        private IDialogService dialogService;


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
        public ICommand SetSoundCommand { get; set; }

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
                string msg = Validator.Validate(Hours, Minutes, SoundSource);
                if(msg == "OK")
                {
                    string minutes = Minutes.ToString();
                    if (Minutes < 10) minutes = "0" + minutes;

                    if (ACTION == "ADD")
                    {
                        if (!await Database.AddAlarm(new Alarm(Description, $"{Hours}:{minutes}", SoundSource, IsRepeat, true)))
                            MessageBox.Show("Error! Alarm clock was not added to the database.");

                        form.Close();
                    }
                    else if(ACTION == "EDIT")
                    {
                        if (!await Database.EditAlarm(new Alarm(Id, Description, $"{Hours}:{minutes}", SoundSource, IsRepeat, IsEnabled)))
                            MessageBox.Show("Error! Alarm clock has not been modified in the database.");

                        form.Close();
                    }
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }
        private void SetSound(object obj)
        {
            dialogService = new DialogService();
            if(dialogService.OpenFileDialog() == true)
            {
                SoundSource = dialogService.FilePath;
            }
        }

        #endregion
    }
}
