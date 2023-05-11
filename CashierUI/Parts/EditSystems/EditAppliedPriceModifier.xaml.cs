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
    /// Interaction logic for EditAppliedPriceModifier.xaml
    /// </summary>
    public partial class EditAppliedPriceModifier : UserControl
    {
        public EditAppliedPriceModifier()
        {
            InitializeComponent();
        }
        EditAppliedPriceModifiersViewModel _context;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditAppliedPriceModifiersViewModel;
            _context.Save();

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditAppliedPriceModifiersViewModel;
            _context.Cancel();
        }
    }
}
