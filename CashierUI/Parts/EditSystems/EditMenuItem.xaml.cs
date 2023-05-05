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
    /// Interaction logic for EditMenuItem.xaml
    /// </summary>
    public partial class EditMenuItem : UserControl
    {
        public EditMenuItem()
        {
            InitializeComponent();
        }
        EditMenuItemViewModel _context;
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditMenuItemViewModel;
            _context.Add();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditMenuItemViewModel;
            _context.Cancel();
        }
    }
}
