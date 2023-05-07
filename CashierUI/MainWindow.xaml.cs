using CashierDB;
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

namespace CashierUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainTabViewModel mainTabViewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new CashierContext();
            mainTabViewModel = new MainTabViewModel(context);
            DataContext = mainTabViewModel;
            mainTabViewModel.LoadMenuTabs();
            mainTabViewModel.LoadOpenTabs();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            mainTabViewModel.OpenSettings();
        }
    }
}
