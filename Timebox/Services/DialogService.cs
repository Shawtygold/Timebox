using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Timebox.Services
{
    interface IDialogService
    {
        string FilePath { get; set; }
        bool OpenFileDialog();
        void ShowMessage(string message);
    }

    internal class DialogService : IDialogService
    {
        public string FilePath { get; set; } = null!;

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new();

            openFileDialog.Filter = "sound files (*.wav)|*.wav|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
