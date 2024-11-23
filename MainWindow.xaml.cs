using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Player _player = new();
        private CpuAI _cpuAi = new();
        private string _winner;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string playerResult;
        public string PlayerResult
        {
            get
            {
                return playerResult;
            }
            set
            {
                if (playerResult != value)
                {
                    playerResult = value;
                    OnPropertyChanged(nameof(PlayerResult));
                }
            }
        }

        private string cpuAiResult;
        public string CpuAiResult
        {
            get
            {
                return cpuAiResult;
            }
            set
            {
                if (cpuAiResult != value)
                {
                    cpuAiResult = value;
                    OnPropertyChanged(nameof(CpuAiResult));
                }
            }
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                DataContext = this;
                this.Show();
                AddPlayerName();
                NewGame();
            }
            catch(Exception ex)
            {
                HandleError("Sorry, an error have occured while loading the game :-\\ \n" +
                    "Try shutting down and starting over.", ex);
            }
        }


        private void NewGame()
        {
            try
            {
                _winner = "";
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
                UpdateNameAndResult();

                if (!_player.FirstTurn)
                {
                    CpuPlaysRandom();
                }
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error have occured while starting a new game :-\\ \n" +
                    "Try shutting down and starting over.", ex);
            }
        }


        private void NewGame(object sender, RoutedEventArgs e)
        {
            try
            {
                _winner = "";
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
                UpdateNameAndResult();

                if (!_player.FirstTurn)
                {
                    CpuPlaysRandom();
                }
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error have occured while starting a new game :-\\ \n" +
                    "Try shutting down and starting over.", ex);
            }
        }


        private void ChooseFirstTurn()
        {
            try
            {
                MessageBoxResult firstTurnResult = MessageBox.Show("Do you want to go first?", "First turn", MessageBoxButton.YesNo);

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
            catch (Exception ex)
            {
                HandleError("Sorry, an error have occured :-\\ \n", ex);
            }
        }


        private void AddPlayerName()
        {
            try
            {
                NameDialogBox nameDialogBox = new NameDialogBox(_player);
                nameDialogBox.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error have occured while saving player's name :-\\", ex);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                if (button == null || !_buttonIndex.TryGetValue(button.Name, out int index))
                {
                    throw new InvalidOperationException("Invalid button click detected.");
                }

                if (button.Content == null && _board[index] == MarkType.EMPTY && !_cpuAi.Turn)
                {
                    _board[index] = _player.Mark;
                    button.Content = _player.Mark;
                    _cpuAi.Turn = true;

                    if (GameOver())
                    {
                        PresentWinner();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error occurred during player's turn.", ex);
            }

            try
            {
                CpuPlaysRandom();

                if (GameOver())
                {
                    PresentWinner();
                    return;
                }
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error occurred during CpuAI's turn.", ex);
            }
        }


        //CpuPlaysSmart()
        // calls separate logic class 


        // Only called when Cpu plays first turn
        private void CpuPlaysRandom()
        {
            try
            {
                Random cpuRandom = new();
                int index;

                do
                {
                    index = cpuRandom.Next(_board.Length);
                } while (_board[index] != MarkType.EMPTY);

                _board[index] = _cpuAi.Mark;
                Button button = new();
                button = (Button)this.FindName(_buttonIndex.FirstOrDefault(b => b.Value == index).Key);
                button.Content = _cpuAi.Mark;
                _cpuAi.Turn = false;
            }
            catch (Exception ex)
            {
                HandleError("Sorry, an error occurred during CpuAI's turn.", ex);
            }
        }


        //Evaluates if anyone has won the game or if the board is full, called before each turn
        private bool GameOver()
        {
            try
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

                foreach (MarkType[] combination in _winningCombinations)
                {
                    if (combination[0] == _cpuAi.Mark &&
                        combination[1] == _cpuAi.Mark &&
                        combination[2] == _cpuAi.Mark)
                    {
                        _winner = _cpuAi.Name;
                        return true;
                    }
                    else if (combination[0] == _player.Mark &&
                        combination[1] == _player.Mark &&
                        combination[2] == _player.Mark)
                    {
                        _winner = _player.Name;
                        return true;
                    }
                }
                if (!_board.Contains(MarkType.EMPTY))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                HandleError("An error unfortunately occurred while checking the game status.", ex);
                return false;
            }
        }


        private void PresentWinner()
        {
            try
            {
                DisableBoard();

                if (_winner == _player.Name)
                {
                    MessageBox.Show($"The winner is {_winner}", "Congatulations!");
                    _player.Score++;
                }
                else if (_winner == _cpuAi.Name)
                {
                    MessageBox.Show($"The winner is {_winner}", "You lost!");
                    _cpuAi.Score++;
                }
                else
                {
                    MessageBox.Show("Game is a tie", "Try again!");
                }
                UpdateNameAndResult();
            }
            catch (Exception ex)
            {
                HandleError("An error occurred while presenting the winner.", ex);
            }
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


        private void UpdateNameAndResult()
        {
            playerResult = $"{_player.Name} ({_player.Mark}): {_player.Score}";
            OnPropertyChanged(nameof(PlayerResult));
            cpuAiResult = $"{_cpuAi.Name} ({_cpuAi.Mark}): {_cpuAi.Score}";
            OnPropertyChanged(nameof(CpuAiResult));
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


        private void HandleError(string message, Exception ex)
        {
            MessageBox.Show($"{message}\n\nError Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}