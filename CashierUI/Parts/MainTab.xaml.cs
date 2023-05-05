using CashierDB.Tables;
using CashierUI.Dto;
using CashierUI.ViewModels;
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

namespace CashierUI.Parts
{
    /// <summary>
    /// Interaction logic for MainTab.xaml
    /// </summary>
    public partial class MainTab : UserControl
    {
        public MainTab()
        {
            InitializeComponent();
        }
        MainTabViewModel _context;

        private void AddTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.AddTab();
        }

        private void EditTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            Button b = sender as Button;
            OpenTabDetails tab = b.CommandParameter as OpenTabDetails;
            if (tab.PaymentStatus != "Paid") _context.EditTab(tab);
            else MessageBox.Show("You cannot edit a paid tab", "Error");
        }

        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            Button b = sender as Button;
            OpenTabDetails tab = b.CommandParameter as OpenTabDetails;
            _context.PayTab(tab);
        }

        private void RemoveTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            Button b = sender as Button;
            OpenTabDetails tab = b.CommandParameter as OpenTabDetails;
            string sureMessage = $"Are you sure to permanently remove selected tab named \"{tab.Name}\"?";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes) _context.RemoveTab(tab);
        }

        private void CloseTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            Button b = sender as Button;
            OpenTabDetails tab = b.CommandParameter as OpenTabDetails;
            string sureMessage = $"Are you sure to permanently close selected tab named \"{tab.Name}\"?";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes) _context.CloseTab(tab);
        }

        private void ServedCB_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            CheckBox b = sender as CheckBox;
            ShortOrderItem tab = b.CommandParameter as ShortOrderItem;
            _context.Serve(tab);
        }      

        private void RecordsTabs_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.ViewTabs();
        }
    }
}
