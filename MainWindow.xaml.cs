using System.Text;
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
using TicTacToeWPF.UiComponents;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player _player = new();
        private MarkType[] _board;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();
            NewGame();
            AddPlayerName();
        }





        private void NewGame()
        {
            _board = new MarkType[9];
            for(int i = 0; i < _board.Length; i++)
            {
                _board[i] = MarkType.EMPTY;
            }

            List<Button> allButtons = Board.Children.OfType<Button>().ToList();

            foreach(var button in allButtons)
            {
                button.ClearValue(ContentProperty);
                button.IsEnabled = true;
            }

            ChooseFirstTurn();
        }

        private void ChooseFirstTurn()
        {
            MessageBoxResult firstTurnResult = MessageBox.Show("Du you want to go first?", "First turn", MessageBoxButton.YesNo);

            if (firstTurnResult == MessageBoxResult.Yes)
            {
                _player.Mark = MarkType.X.ToString();
                _player.FirstTurn = true;
            }
            else
            {
                _player.Mark = MarkType.O.ToString();
                _player.FirstTurn = false;
            }
        }


        private void AddPlayerName()
        {
            NameDialogBox nameDialogBox = new NameDialogBox(_player);
            nameDialogBox.ShowDialog();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if(string.IsNullOrEmpty(button.Content.ToString()))
            {
                button.Content = _player.Mark.ToString();
            }

            // If board is not full && game not over - pause a moment (and maybe display some loading text) then let cpu ai play a random or not so random turn
        }


        //private bool GameOver()
        //{
        //Evaluates if anyone has won the game or if the board is full, before each turn
        //}


        // Entities: Player, CPU-_player, signs(X/O), board, buttons, current score


        // Start new game (immediatly?)
        // Choose if you want to start (play X) or if AI should start
        // When button is klicked with mouse it is filled with X or O
        // When three of the same sign are in a row, the game is stopped and winner is announced
        // Simple score keeping?


    }
}