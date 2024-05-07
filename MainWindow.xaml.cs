using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Member

        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        private MarkType[] mResults;

        /// <summary>
        /// True if it is the player 1's turn (X) or player 2's turn (O)
        /// </summary>
        private bool mPlayer1Turn;

        /// <summary>
        /// True if the game has ended
        /// </summary>
        private bool mGameEnded;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }

        #endregion

        /// <summary>
        /// Starts a new game and clears all values back to the start
        /// </summary>
        private void NewGame()
        {
            // Create a new blank array of free cells
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            // Make sure Player 1 starts the game
            mPlayer1Turn = true;

            // Interate every button on the grid...
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                // Change background, foreground and content to default value
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            // Make sure the game hasn't finished
            mGameEnded = false;
        }

        /// <summary>
        /// Handles a button clicked event
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">The events of the clicked</param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            // Start a new game on the click after it finished
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // Cast the sender to a button
            var button = (Button)sender;

            // Find the buttons position in the array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + (row * 3);

            // Don't do anything if the cell already has a value in it
            if (mResults[index] != MarkType.Free) return;

            // Set the cell value based on which players turn it is
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            // Set the button text to the result
            button.Content = mPlayer1Turn ? "X" : "O";

            // Change Noughts to green
            if (!mPlayer1Turn) button.Foreground = Brushes.Red;

            // Toggle the player turns
            mPlayer1Turn ^= true; // 0^0 = 0, 1^1 = 0, 0^1 = 1

            // Check for a winner
            CheckForWinner();
        }

        /// <summary>
        /// Checks of there is a winner of a 3 line straight
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CheckForWinner()
        {
            // Define winning combinations (indices of cells forming winning lines)
            int[][] winningCombinations =
            {
                [0, 1, 2], // Horizontal top row
                [3, 4, 5 ], // Horizontal middle row
                [6, 7, 8 ], // Horizontal bottom row
                [0, 3, 6 ], // Vertical left column
                [1, 4, 7 ], // Vertical middle column
                [2, 5, 8 ], // Vertical bottom column
                [0, 4, 8 ], // Diagon from top-left to bottom-right
                [2, 4, 6 ] // Diagon from top-right to bottom-left
            };

            // Iterate through each winning combination
            foreach (var pattern in winningCombinations)
            {
                MarkType firstMark = mResults[pattern[0]];

                // Check if the first cell in the pattern is not empty and all cells in the pattern have the same mark
                if (firstMark != MarkType.Free && pattern.All(index => mResults[index] == firstMark))
                {
                    // Highlight winning cells

                    HighlightWinningCells(pattern);
                    MessageBox.Show($"Player {(firstMark == MarkType.Cross ? "X" : "O")} wins!", "Game Over", MessageBoxButton.OK);
                    GameOver();
                    return;
                }
            }

            // If there's no winner and all cells are filled, it's a draw
            if (!mResults.Contains(MarkType.Free))
            {
                MessageBox.Show("It's a draw!", "Game Over", MessageBoxButton.OK);
                GameOver();
            }
        }

        /// <summary>
        /// Handles game over logic
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void GameOver()
        {
            mGameEnded = true;
        }

        /// <summary>
        /// Highlights the winning cells
        /// </summary>
        /// <param name="cellIndices">Indices of the winning cells</param>
        private void HighlightWinningCells(int[] cellIndices)
        {
            foreach(var index in cellIndices)
            {
                var button = Container.Children.Cast<Button>().ElementAt(index);
                button.Background = Brushes.Yellow;
            }
        }
    }
}