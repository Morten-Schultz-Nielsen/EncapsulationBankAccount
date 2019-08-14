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
using EncapsulationBankAccount.DataAccess;
using EncapsulationBankAccount.Entities;

namespace EncapsulationBankAccount.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccountRepository repository = new AccountRepository();
            TotalMoneyLabel.Content = "test";
            Account[] accounts = repository.Select().ToArray();
            decimal totalBalance = 0;
            foreach(Account account in accounts)
            {
                totalBalance += account.Balance;
            }
            TotalMoneyLabel.Content = "Penge i alt: " + Math.Round(totalBalance, 2).ToString("c");
            NumberOfBankAccountsLabel.Content = "Konti i alt: " + accounts.Length;
            AvaregeMoneyLabel.Content = "Penge per konti i gennemsnit: " + Math.Round(totalBalance / accounts.Length, 2).ToString("c");
        }
    }
}
