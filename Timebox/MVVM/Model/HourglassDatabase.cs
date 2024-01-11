using System.Collections.ObjectModel;
using System.Windows;

namespace Timebox.MVVM.Model
{
    internal class HourglassDatabase
    {
        public delegate void DataChangedHandler(DatabaseChangedEventArgs e);
        public static event DataChangedHandler? DataChanged;

        private static void OnDataChanged(DatabaseChangedEventArgs e) => DataChanged?.Invoke(e);

        public static async Task<bool> AddHourglass(Hourglass hourglass)
        {
            if (hourglass == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                await db.Hourglasses.AddAsync(hourglass);
                await db.SaveChangesAsync();
                result = true;

                // Get the added item from the database so that the item has Id 
                Hourglass hourglassWithId = db.Hourglasses.ToList().Last();
                OnDataChanged(new DatabaseChangedEventArgs("ADD", hourglassWithId));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }

            return result;
        }
        public static async Task<bool> EditHourglass(Hourglass hourglass)
        {
            if (hourglass == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                db.Hourglasses.Update(hourglass);
                await db.SaveChangesAsync();
                result = true;

                OnDataChanged(new DatabaseChangedEventArgs("EDIT", hourglass));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }

            return result;
        }
        public static async Task<bool> RemoveHourglass(Hourglass hourglass)
        {
            if (hourglass == null)
                return false;

            bool result = false;

            try
            {
                using ApplicationContext db = new();
                await Task.Run(() => db.Hourglasses.Remove(hourglass));
                await db.SaveChangesAsync();
                result = true;

                OnDataChanged(new DatabaseChangedEventArgs("REMOVE", hourglass));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }

            return result;
        }
        public static ObservableCollection<Hourglass>? GetHourglass()
        {
            ObservableCollection<Hourglass>? hourglass;
            try
            {
                using ApplicationContext db = new();
                hourglass = new(db.Hourglasses.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                hourglass = null;
            }

            return hourglass;
        }
    }
}
