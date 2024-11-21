using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using TicTacToeWPF.Entities;

namespace TicTacToeWPF.UiComponents
{
    /// <summary>
    /// Interaction logic for NameDialogBox.xaml
    /// </summary>
    public partial class NameDialogBox : Window
    {
        private Player _player;

        public NameDialogBox(Player player)
        {
            _player = player;
            InitializeComponent();
        }

        private void RecieveUserInput(object sender, RoutedEventArgs e)
        {
            NamePlaceholder.Visibility = Visibility.Hidden;
            UserNameTextBox.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                _player.Name = UserNameTextBox.Text;
            }
            else
            {
                _player.Name = "Player One";
            }
            DialogResult = true;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _player.Name = "Player One";
            DialogResult = false;
        }
    }
}
