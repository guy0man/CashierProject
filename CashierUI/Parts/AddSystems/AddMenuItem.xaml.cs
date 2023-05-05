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

namespace CashierUI.Parts.AddSystems
{
    /// <summary>
    /// Interaction logic for AddMenuItem.xaml
    /// </summary>
    public partial class AddMenuItem : UserControl
    {
        public AddMenuItem()
        {
            InitializeComponent();
        }
        AddMenuItemViewModel _context;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddMenuItemViewModel;
            _context.Add();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddMenuItemViewModel;
            _context.Cancel();
        }

        private void addItemTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddMenuItemViewModel;
            _context.AddType();
        }

        private void RemoveTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddMenuItemViewModel;
            _context.RemoveType();
        }
    }
}
