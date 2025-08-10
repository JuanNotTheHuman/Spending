using JuanNotTheHuman.Spending.Services;
using JuanNotTheHuman.Spending.Views;
using System.Windows;

namespace JuanNotTheHuman.Spending.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationService.Navigate(new RecordsView());
        }
    }
}
