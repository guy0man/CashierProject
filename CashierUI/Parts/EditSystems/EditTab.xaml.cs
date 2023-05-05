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

namespace CashierUI.Parts.EditSystems
{
    /// <summary>
    /// Interaction logic for EditTab.xaml
    /// </summary>
    public partial class EditTab : UserControl
    {
        public EditTab()
        {
            InitializeComponent();
        }
        EditTabViewModel _context;

        private void MinusQuantBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            Button b = sender as Button;
            PartialOrderItem order = b.CommandParameter as PartialOrderItem;
            _context.MinusQuant(order);

        }

        private void AddQuantBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            Button b = sender as Button;
            PartialOrderItem order = b.CommandParameter as PartialOrderItem;
            _context.AddQuant(order);
        }

        private void RemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            Button b = sender as Button;
            PartialOrderItem order = b.CommandParameter as PartialOrderItem;
            _context.RemoveOrder(order);

        }

        private void SaveTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            _context.Add();
        }

        private void CancelTabBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            _context.Cancel();
        }

        private void cancelCB_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditTabViewModel;
            _context.LoadTotal();
        }
    }
}
