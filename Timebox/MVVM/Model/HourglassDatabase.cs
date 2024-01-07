using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Timebox.MVVM.Model.HourglassModel;

namespace Timebox.MVVM.Model
{
    internal class HourglassDatabase
    {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }

            return result;
        }

        public static Hourglass? GetHourglass()
        { 
            Hourglass? hourglass;
            try
            {
                using ApplicationContext db = new();
                hourglass = db.Hourglasses.ToList().First();
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
