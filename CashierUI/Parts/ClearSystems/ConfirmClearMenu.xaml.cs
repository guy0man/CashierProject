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
    /// Interaction logic for ConfirmClearMenu.xaml
    /// </summary>
    public partial class ConfirmClearMenu : Window
    {
        public ConfirmClearMenu()
        {
            InitializeComponent();
        }
        ClearMenuViewModel _context;
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as ClearMenuViewModel;
            _context.Confirm();
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
