using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private CpuAI _cpuAi = new();
        private string winner;
        private MarkType[] _board = new MarkType[9];
        private Dictionary<string, int> _buttonIndex = new()
        {
            {"Button0", 0},
            {"Button1", 1},
            {"Button2", 2},
            {"Button3", 3},
            {"Button4", 4},
            {"Button5", 5},
            {"Button6", 6},
            {"Button7", 7},
            {"Button8", 8}
        };

        public MainWindow()
        {
            InitializeComponent();
            this.Show();
            AddPlayerName();
            NewGame();
        }


        private void NewGame()
        {
            winner = null;
            for(int i = 0; i < _board.Length; i++)
            {
                _board[i] = MarkType.EMPTY;
            }

            List<Button> allButtons = Board.Children.OfType<Button>().ToList();

            foreach (var button in allButtons)
            {
                button.ClearValue(ContentProperty);
            }

            EnableBoard();
            ChooseFirstTurn();

            if (!_player.FirstTurn)
            {
                CpuPlaysRandom();
            }
        }


        private void NewGame(object sender, RoutedEventArgs e)
        {
            winner = null;
            for (int i = 0; i < _board.Length; i++)
            {
                _board[i] = MarkType.EMPTY;
            }

            List<Button> allButtons = Board.Children.OfType<Button>().ToList();

            foreach (var button in allButtons)
            {
                button.ClearValue(ContentProperty);
            }

            EnableBoard();
            ChooseFirstTurn();

            if (!_player.FirstTurn)
            {
                CpuPlaysRandom();
            }
        }


        private void ChooseFirstTurn()
        {
            MessageBoxResult firstTurnResult = MessageBox.Show("Du you want to go first?", "First turn", MessageBoxButton.YesNo);

            if (firstTurnResult == MessageBoxResult.Yes)
            {
                _player.FirstTurn = true;
                _player.Mark = MarkType.X;
                _cpuAi.Mark = MarkType.O;
            }
            else
            {
                _player.FirstTurn = false;
                _player.Mark = MarkType.O;
                _cpuAi.Mark = MarkType.X;
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

            if(!button.IsEnabled)
            {
                NewGame();
            }

            int index = _buttonIndex[button.Name.ToString()];

            if(button.Content == null)
            {
                _board[index] = _player.Mark;
                button.Content = _player.Mark;
            }

            // TODO: Ska inte gå att klicka på samma ruta som Cpu

            if(GameOver())
            {
                PresentWinner();
                return;
            }

            // If board is not full && game not over - pause a moment (and maybe display some loading text) then let cpu ai play a random or not so random turn (Progressbar for effect? ;-)
            CpuPlaysRandom();

            if (GameOver())
            {
                PresentWinner();
                return;
            }
        }


        //CpuPlaysSmart()
        // disable buttons for a moment
        // calls separate logic class 


        // Only called when Cpu plays first turn
        private void CpuPlaysRandom()
        {
            Random cpuRandom = new();
            int index = -1;
            while (index == -1 || _board[index] != MarkType.EMPTY)
            {
                index = cpuRandom.Next(_board.Length);
            }
            _board[index] = _cpuAi.Mark;
            Button button = new();
            button = (Button)this.FindName(_buttonIndex.FirstOrDefault(b => b.Value == index).Key);
            button.Content = _cpuAi.Mark;
        }


        //Evaluates if anyone has won the game or if the board is full, called before each turn
        private bool GameOver()
        {
            MarkType[][] _winningCombinations =
            {
                new MarkType[] {_board[0], _board[1], _board[2] }, // Top row
                new MarkType[] {_board[3], _board[4], _board[5] }, // Middle row
                new MarkType[] {_board[6], _board[7], _board[8] }, // Bottom row
                new MarkType[] {_board[0], _board[3], _board[6] }, // Left column
                new MarkType[] {_board[1], _board[4], _board[7] }, // Middle column
                new MarkType[] {_board[2], _board[5], _board[8] }, // Right column
                new MarkType[] {_board[0], _board[4], _board[8] }, // Diagonal from top left
                new MarkType[] {_board[2], _board[4], _board[6] }, // Diagonal from top right
            };

            foreach(MarkType[] mark in _winningCombinations)
            {
                if (mark[0] == _cpuAi.Mark &&
                    mark[1] == _cpuAi.Mark &&
                    mark[2] == _cpuAi.Mark)
                {
                    winner = _cpuAi.Name;
                    return true;
                }
                else if (mark[0] == _player.Mark &&
                    mark[1] == _player.Mark &&
                    mark[2] == _player.Mark)
                {
                    winner = _player.Name;
                    return true;
                }
                else if (!_board.Contains(MarkType.EMPTY) && !string.IsNullOrEmpty(winner))
                {
                    return true;
                }
            }
            return false;
        }


        private void PresentWinner()
        {
            DisableBoard();

            if (winner == _player.Name)
            {
                MessageBox.Show($"The winner is {winner}", "Congatulations!");
                _player.Score++;
            }
            else if(winner == _cpuAi.Name)
            {
                MessageBox.Show($"The winner is {winner}", "You lost!");
                _cpuAi.Score++;
            }
            else
            {
                MessageBox.Show("Game is a tie", "Try again!");
            }
            // Show score in UI
        }


        private void DisableBoard()
        {
            List<Button> allButtons = Board.Children.OfType<Button>().ToList();

            foreach (Button b in allButtons)
            {
                b.IsEnabled = false;
            }
        }

        private void EnableBoard()
        {
            List<Button> allButtons = Board.Children.OfType<Button>().ToList();

            foreach (Button b in allButtons)
            {
                b.IsEnabled = true;
            }
        }


        private void ExitGame(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sam's Ultra AI-Powered T.T.T Opponent \n" +
                "Made with <3 november 2024", "About this game");
        }

        // Entities: Player, CPU-_player, signs(X/O), board, buttons, current score
        // Start new game (immediatly?)
        // Choose if you want to start (play X) or if AI should start
        // When button is klicked with mouse it is filled with X or O
        // When three of the same sign are in a row, the game is stopped and winner is announced
        // Simple score keeping?
    }
}