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
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HandleError("Couldn't load name window.", ex);
            }
        }

        private void RecieveUserInput(object sender, RoutedEventArgs e)
        {
            try
            {
                NamePlaceholder.Visibility = Visibility.Hidden;
                UserNameTextBox.Focus();
            }
            catch (Exception ex)
            {
                HandleError("Something went wrong.", ex);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                HandleError("Something went wrong.", ex);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _player.Name = "Player One";
                DialogResult = false;
            }
            catch
            {
                HandleError("Something went wrong.", ex);
            }
        }

        private void HandleError(string message, Exception ex)
        {
            MessageBox.Show($"{message}\n\nError Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
