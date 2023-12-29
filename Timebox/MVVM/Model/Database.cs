﻿using System.Collections.ObjectModel;
using System.Windows;

namespace Timebox.MVVM.Model
{
    internal class Database
    {
        public static event Action DataChanged;
        private static void OnDataChanged() => DataChanged?.Invoke();

        public static async Task<bool> AddAlarm(Alarm alarm)
        {
            if (alarm == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                await db.Alarms.AddAsync(alarm);
                await db.SaveChangesAsync();
                result = true;
                OnDataChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return result;

        }
        public static async Task<bool> EditAlarm(Alarm alarm)
        {
            if(alarm == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                db.Alarms.Update(alarm);
                await db.SaveChangesAsync();
                result = true;
                OnDataChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return result;
        }
        public static async Task<bool> RemoveAlarm(Alarm alarm)
        {
            if(alarm == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                await Task.Run(() => db.Alarms.Remove(alarm));
                await db.SaveChangesAsync();
                result = true;
                OnDataChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return result;
        }
        public static ObservableCollection<Alarm>? GetAlarms()
        {
            ObservableCollection<Alarm> result = new();

            try
            {
                using ApplicationContext db = new();
                result = new(db.Alarms.ToList());              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return result;
        }
    }
}
