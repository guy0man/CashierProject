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
    /// Interaction logic for AddPriceModifier.xaml
    /// </summary>
    public partial class AddPriceModifier : UserControl
    {
        public AddPriceModifier()
        {
            InitializeComponent();
        }
        AddPriceModifierViewModel _context;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddPriceModifierViewModel;
            _context.Add();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as AddPriceModifierViewModel;
            _context.Cancel();
        }
    }
}
