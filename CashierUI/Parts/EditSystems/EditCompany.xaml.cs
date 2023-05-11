using CashierUI.Dto;
using CashierUI.ViewModels;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for EditCompany.xaml
    /// </summary>
    public partial class EditCompany : UserControl
    {
        public EditCompany()
        {
            InitializeComponent();
        }
        EditCompanyViewModel _context;

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditCompanyViewModel;
            Button b = sender as Button;
            PriceModifiersName pm = b.CommandParameter as PriceModifiersName;
            string sureMessage = $"Are you sure to permanently remove selected Price Modifier named \"{pm.Name}\"? This action will also remove the item from the records";
            var DialogResult = MessageBox.Show(sureMessage, "Warning", MessageBoxButton.YesNo);
            if (DialogResult == MessageBoxResult.Yes) _context.Remove(pm);

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditCompanyViewModel;
            _context.Add();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditCompanyViewModel; 
            _context.Cancel();
        }

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png;) |*.jpg; *.jpeg; *.bmp; *.png;";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(openDialog.FileName);
                File.Copy(openDialog.FileName, @"Images/BusinessImage.png", true);                
            }

        }

        private void AddPModBtn_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditCompanyViewModel;
            _context.AddPM();
        }

        private void autoApplyCB_Click(object sender, RoutedEventArgs e)
        {
            _context = DataContext as EditCompanyViewModel;
            CheckBox b = sender as CheckBox;
            PriceModifiersName pm = b.CommandParameter as PriceModifiersName;
            _context.ActivateAutoApply(pm);
        }
    }
}
