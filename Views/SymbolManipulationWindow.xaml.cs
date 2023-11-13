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
using TeleTraderAssignment.Models;
using TeleTraderAssignment.ViewModel;

namespace TeleTraderAssignment.Views
{
    /// <summary>
    /// Interaction logic for SymbolManipulationWindow.xaml
    /// </summary>
    public partial class SymbolManipulationWindow : Window
    {
        public SymbolManipulationWindow(SymbolManipulationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
