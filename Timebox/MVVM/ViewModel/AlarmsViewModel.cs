using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.MVVM.View.Forms;
using Timebox.MVVM.ViewModel.FormViewModel;
using Timebox.Services;

namespace Timebox.MVVM.ViewModel
{
    internal class AlarmsViewModel : Core.ViewModel
    {
        public AlarmsViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            AddAlarmCommand = new RelayCommand(AddAlarm);
            EditAlarmCommand = new RelayCommand(EditAlarm);
            DeleteAlarmCommand = new RelayCommand(DeleteAlarm);
            AlarmDatabase.DataChanged += Database_DataChanged;

            Alarms = AlarmDatabase.GetAlarms();

            // Starting alarms that have the Enabled status
            AlarmHelper.AlarmStart(Alarms);
        }

        private void Database_DataChanged(DatabaseChangedEventArgs e)
        {
            if (e.ChangedElement is not Alarm alarm || Alarms == null) 
                return;

            if (e.Action == "ADD")
            {
                Application.Current.Dispatcher.Invoke(new Action(() => Alarms.Add(alarm)));

                if (alarm.IsEnabled)
                    alarm.StartTimer();

                return;
            }
            else if (e.Action == "EDIT")
            {
                // I get an old element with the same id
                var oldAlarm = Alarms.First(a => a.Id == alarm.Id);
                int index = Alarms.IndexOf(oldAlarm);

                oldAlarm.StopTimer();
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => Alarms.Remove(oldAlarm)));
                    Application.Current.Dispatcher.Invoke(new Action(() => Alarms.Insert(index, alarm)));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                if (alarm.IsEnabled)
                    alarm.StartTimer();

                return;
            }
            else if (e.Action == "REMOVE")
            {
                Application.Current.Dispatcher.Invoke(new Action(() => Alarms.Remove(alarm)));

                if (alarm.IsEnabled)
                    alarm.StopTimer();

            }
        }

        #region [Properties]

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Alarm>? _alarms;
        public ObservableCollection<Alarm>? Alarms
        {
            get { return _alarms; }
            set { _alarms = value; OnPropertyChanged(); }
        }

        private Alarm _selectedItem;
        public Alarm SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]

        public ICommand AddAlarmCommand { get; set; }
        public ICommand EditAlarmCommand { get; set; }
        public ICommand DeleteAlarmCommand { get; set; }

        private void AddAlarm(object obj)
        {
            AlarmModificationForm form = new();
            AlarmModificationViewModel vm = new("ADD");
            form.DataContext = vm;
            form.ShowDialog();
        }
        private void EditAlarm(object obj)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Select an alarm!");
                return;
            }

            AlarmModificationForm form = new();
            AlarmModificationViewModel vm = new("EDIT", SelectedItem);
            form.DataContext = vm;
            form.ShowDialog();
        }
        private async void DeleteAlarm(object obj)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Select an alarm!");
                return;
            }

            if (!await AlarmDatabase.RemoveAlarm(SelectedItem))
                MessageBox.Show("Failed to delete the alarm clock from the database!");
        }

        #endregion

        #region Methods

        //private void UpdateAlarms()
        //{
        //    // Останавливаю таймеры на текущих объектах будильников, чей статус IsEnabled = true (чтобы не сработали когда должны сработать таймеры на новых объектах будильников)
        //    AlarmHelper.AlarmStop(Alarms);

        //    // Получаю список таймеров из базы данных (Alarms теперь содержит новые объекты будильников (new Alarm))
        //    Alarms = Database.GetAlarms();

        //    // Запускаю таймеры на будильниках, у которых IsEnabled = true
        //    AlarmHelper.AlarmStart(Alarms);
        //} 

        #endregion
    }
}
