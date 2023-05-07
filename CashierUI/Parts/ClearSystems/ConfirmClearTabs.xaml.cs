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
using System.Windows.Shapes;

namespace CashierUI.Parts.ClearSystems
{
    /// <summary>
    /// Interaction logic for ConfirmClearTabs.xaml
    /// </summary>
    public partial class ConfirmClearTabs : Window
    {
        public ConfirmClearTabs()
        {
            InitializeComponent();
        }
        ClearAllTabsViewModel _context;
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as ClearAllTabsViewModel;
            _context.Confirm();
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
