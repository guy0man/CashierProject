﻿using CashierUI.ViewModels;
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

        }

        private void AddPModBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
