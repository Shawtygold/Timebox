﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Timebox.Core;
using Timebox.MVVM.Model;
using Timebox.MVVM.View.Forms;
using Timebox.MVVM.ViewModel.FormViewModel;
using Timebox.Services;
using Windows.UI.WebUI;

namespace Timebox.MVVM.ViewModel
{
    internal class HourglassViewModel : Core.ViewModel
    {
        public HourglassViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            AddHourglassCommand = new RelayCommand(AddHourglass);
            EditHourglassCommand = new RelayCommand(EditHourglass);
            RemoveHourglassCommand = new RelayCommand(RemoveHourglass);

            Hourglasses = HourglassDatabase.GetHourglass();
            HourglassDatabase.DataChanged += HourglassDatabase_DataChanged;
        }

        private void HourglassDatabase_DataChanged(DatabaseChangedEventArgs e)
        {
            if (e.ChangedElement is not Hourglass hourglass || Hourglasses == null)
                return;

            if (e.Action == "ADD")
            {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => Hourglasses.Add(hourglass)));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else if (e.Action == "EDIT")
            {
                Hourglass? oldHourglass = Hourglasses.ToList().Find(h => h.Id == hourglass.Id);
                if (oldHourglass == null)
                    return;

                int index = Hourglasses.IndexOf(oldHourglass);

                try
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => Hourglasses.Remove(oldHourglass)));
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => Hourglasses.Insert(index, hourglass)));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else if (e.Action == "REMOVE")
            {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => Hourglasses.Remove(hourglass)));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        #region [Properties]

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private Hourglass? _selectedItem;
        public Hourglass? SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }


        private ObservableCollection<Hourglass>? _hourglasses;
        public ObservableCollection<Hourglass>? Hourglasses
        {
            get { return _hourglasses; }
            set { _hourglasses = value; OnPropertyChanged(); }
        }

        #endregion

        #region [Commands]

        public ICommand AddHourglassCommand { get; set; }
        public ICommand EditHourglassCommand { get; set; }
        public ICommand RemoveHourglassCommand { get; set; }

        private void AddHourglass(object obj)
        {
            HourglassModificationForm form = new();
            HourglassModificationViewModel vm = new("ADD");
            form.DataContext = vm;
            form.ShowDialog();
        }
        private void EditHourglass(object obj)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Choose an hourglass!");
                return;
            }

            HourglassModificationForm form = new();
            HourglassModificationViewModel vm = new("EDIT", SelectedItem);
            form.DataContext = vm;
            form.ShowDialog();
        }
        private async void RemoveHourglass(object obj)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Choose an hourglass!");
                return;
            }

            if(!await HourglassDatabase.RemoveHourglass(SelectedItem))
                MessageBox.Show("Error! Hourglass has not been removed from the database");
        }

        #endregion
    }
}
