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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }
        MainTabViewModel _context;

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.CloseSettings();
        }

        private void ClearTabsBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.ClearTabs();
        }

        private void ClearMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.ClearMenu();
        }

        private void ClearDbBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.ClearDatabase();
        }
    }
}
