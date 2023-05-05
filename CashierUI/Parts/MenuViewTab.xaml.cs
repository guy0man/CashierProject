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
    /// Interaction logic for MenuViewTab.xaml
    /// </summary>
    public partial class MenuViewTab : UserControl
    {
        public MenuViewTab()
        {
            InitializeComponent();
        }
        ViewMenuViewModel _context;

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as ViewMenuViewModel;
            Button b = sender as Button;
            MenuItemName menuItem = b.CommandParameter as MenuItemName;
            _context.AddOrder(menuItem);

        }
    }
}
