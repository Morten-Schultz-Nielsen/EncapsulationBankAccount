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
            //Load account information
            AccountRepository accountRepository = new AccountRepository();
            TotalMoneyLabel.Content = "test";
            Account[] accounts = accountRepository.Select().ToArray();
            decimal totalBalance = 0;
            foreach(Account account in accounts)
            {
                totalBalance += account.Balance;
            }
            TotalMoneyLabel.Content = "Penge i alt: " + Math.Round(totalBalance, 2).ToString("c");
            NumberOfBankAccountsLabel.Content = "Konti i alt: " + accounts.Length;
            AvaregeMoneyLabel.Content = "Penge per konti i gennemsnit: " + Math.Round(totalBalance / accounts.Length, 2).ToString("c");

            //transactions
            TransactionsToTimeSelector.Time = new TimeSpan(23, 59, 59);
            LoadAllTransactions();
        }

        private void ShowTransactions(Transaction[] transactions)
        {
            TransactionsCountLabel.Content = "Transaktioner: " + transactions.Length;
            TransactionsDataGrid.ItemsSource = transactions;
        }

        private void FindTransactions_Click(object sender, RoutedEventArgs e)
        {
            if(TransactionsFromDatePicker.SelectedDate is null)
            {
                MessageBox.Show("Vælg venligst en starts dato");
                return;
            }
            if(TransactionsToDatePicker.SelectedDate is null)
            {
                TransactionsToDatePicker.SelectedDate = DateTime.Now;
            }

            DateTime fromDate = TransactionsFromDatePicker.SelectedDate.Value + TransactionsFromTimeSelector.Time;
            DateTime toDate = TransactionsToDatePicker.SelectedDate.Value + TransactionsToTimeSelector.Time;

            if(fromDate > toDate)
            {
                MessageBox.Show("Datoen \"fra\" skal være mindre end \"til\"");
                return;
            }

            TransactionRepository transactionRepository = new TransactionRepository();
            ShowTransactions(transactionRepository.GetTransactionLogs(fromDate, toDate).ToArray());
        }

        private void GetAllTransactions_Click(object sender, RoutedEventArgs e)
        {
            TransactionsFromDatePicker.SelectedDate = null;
            TransactionsToDatePicker.SelectedDate = null;
            TransactionsFromTimeSelector.Time = new TimeSpan();
            TransactionsToTimeSelector.Time = new TimeSpan(23,59,59);

            LoadAllTransactions();
        }

        private void LoadAllTransactions()
        {
            TransactionRepository transactionRepository = new TransactionRepository();
            ShowTransactions(transactionRepository.GetTransactionLogs().ToArray());
        }
    }
}
