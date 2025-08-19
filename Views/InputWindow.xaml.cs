using JuanNotTheHuman.Spending.ViewModels;
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

namespace JuanNotTheHuman.Spending.Views
{
    /// <summary>
    /// Logika interakcji dla klasy InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindowViewModel ViewModel => (InputWindowViewModel)DataContext;
        public InputWindow(InputWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.CloseRequested += (success) =>
            {
                DialogResult = success;
                Close();
            };
        }
    }
}
