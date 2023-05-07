using CashierUI.ViewModels;
using CashierUI.Dto;
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
    /// Interaction logic for MenuItemsTab.xaml
    /// </summary>
    public partial class MenuItemsTab : UserControl
    {
        public MenuItemsTab()
        {
            InitializeComponent();
        }
        MainTabViewModel _context;

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MainTabViewModel;
            _context.AddMenu();
        }
    }
}
