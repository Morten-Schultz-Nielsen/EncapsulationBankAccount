using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncapsulationBankAccount.UI
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker: UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }

        public TimeSpan Time
        {
            get
            {
                return new TimeSpan(int.Parse(Hours.Text), int.Parse(Minutes.Text), int.Parse(Seconds.Text));
            }
            set
            {
                Hours.Text = value.Hours.ToString();
                Minutes.Text = value.Minutes.ToString();
                Seconds.Text = value.Seconds.ToString();
            }
        }

        private void Number_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox numberBox = sender as TextBox;
            string numberboxNumbers = string.Join("", numberBox.Text.Where(c => char.IsDigit(c)));
            numberboxNumbers = numberboxNumbers.Substring(0, Math.Min(2, numberboxNumbers.Length));
            int number = string.IsNullOrEmpty(numberboxNumbers) ? 0 : int.Parse(numberboxNumbers);

            if(numberBox.Name == "Hours")
            {
                number = Math.Min(24, Math.Max(number, 0));
            }
            else
            {
                number = Math.Min(60, Math.Max(number, 0));
            }

            numberBox.Text = number.ToString();
            numberBox.CaretIndex = numberBox.Text.Length;
        }
    }
}
