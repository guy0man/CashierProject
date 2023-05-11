using CashierDB.Tables;
using CashierUI.Dto;
using CashierUI.Helper;
using CashierUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace CashierUI.Parts
{
    /// <summary>
    /// Interaction logic for ReceiptRecordsView.xaml
    /// </summary>
    public partial class ReceiptRecordsView : UserControl
    {
        public ReceiptRecordsView()
        {
            InitializeComponent();
        }
        ReceiptRecordsViewModel _context;
        Pagination _pagination;
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TabDetails tab = b.CommandParameter as TabDetails;
            string sureMessage = $"Are you sure to permanently remove selected Tab named \"{tab.Name}\" from the records?";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes) _context.Remove(tab);

        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TabDetails tab = b.CommandParameter as TabDetails;
            _context.OpenTab(tab);
        }

        private void searchInput_GotFocus(object sender, RoutedEventArgs e)
        {
            searchLabel.Visibility = Visibility.Hidden;

        }

        private void searchInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchInput.Text == string.Empty) searchLabel.Visibility = Visibility.Visible;
            else searchLabel.Visibility = Visibility.Hidden;

        }

        private void ViewRecords_Loaded(object sender, RoutedEventArgs e)
        {
            _context = DataContext as ReceiptRecordsViewModel;
            _pagination = _context.PageDetails;
            _context.PropertyChanged += ContextOnPropertyChanged;
            DetailsGrid.Visibility = Visibility.Collapsed;

        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _pagination.NextPage();

        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _pagination.LastPage();

        }

        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _pagination.PrevPage();
        }

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _pagination.FirstPage();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.Back();

        }
        private void ContextOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_context.SelectedTab))
            {
                if (_context.SelectedTab == null)
                    DetailsGrid.Visibility = Visibility.Collapsed;
                else
                    DetailsGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
