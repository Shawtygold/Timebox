using System.Windows;
using System.Windows.Controls;

namespace Timebox.Utilities
{
    internal class Btn : RadioButton
    {
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Btn), new FrameworkPropertyMetadata(typeof(Btn)));
        }
    }
}
