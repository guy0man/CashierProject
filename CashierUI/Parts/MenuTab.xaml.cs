using CashierUI.Dto;
using CashierUI.ViewModels;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for MenuTab.xaml
    /// </summary>
    public partial class MenuTab : UserControl
    {
        public MenuTab()
        {
            InitializeComponent();
        }
        MenuTabViewModel _context;

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MenuTabViewModel;
            Button b = sender as Button;
            MenuItemName menuItem = b.CommandParameter as MenuItemName;
            string sureMessage = $"Are you sure to permanently remove selected menu item named \"{menuItem.Name}\"? This action will also remove the item from the records";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes) _context.RemoveMenuItem(menuItem);
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MenuTabViewModel;
            Button b = sender as Button;
            MenuItemName menuItem = b.CommandParameter as MenuItemName;            
            _context.EditMenuItem(menuItem);
        }
        private void AddMenuItemBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as MenuTabViewModel;
            _context.AddMenuItem();
        }
    }
}
