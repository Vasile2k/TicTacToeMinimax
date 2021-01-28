using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public char[][] Game;
        public Label[][] Labels;

        public int MINIMAX_LEVELS
        {
            get
            {
                return (int)numericUpDownLevels.Value;
            }
        }

        public Form1()
        {
            Game = new char[][]
            {
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '}
            };
            InitializeComponent();
            Labels = new Label[][]
            {
                new Label[]{label00, label01, label02},
                new Label[]{label10, label11, label12},
                new Label[]{label20, label21, label22}
            };
            RedrawGame();
        }

        void PutX(int x, int y)
        {
            if(Game[x][y] == ' ')
            {
                Game[x][y] = 'X';
                RedrawGame();
            }
            else
            {
                throw new Exception("Invalid X at " + x + " " + y);
            }
        }

        void Put0(int x, int y)
        {
            if (Game[x][y] == ' ')
            {
                Game[x][y] = '0';
                RedrawGame();
            }
            else
            {
                throw new Exception("Invalid 0 at " + x + " " + y);
            }
        }

        public static char[][] CloneGame(char[][] game)
        {
            char[][] newGame = new char[][]
            {
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '}
            };
            for(int i = 0; i < 3; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    newGame[i][j] = game[i][j];
                }
            }
            return newGame;
        }

        public int Heuristic(char[][] game)
        {
            int heuristic = 0;
            for (int i = 0; i < 3; ++i)
            {
                // Horizontal lines
                List<char> horizontalHeuristic = new List<char>();
                horizontalHeuristic.Add(game[i][0]);
                horizontalHeuristic.Add(game[i][1]);
                horizontalHeuristic.Add(game[i][2]);
                // 3 X in a row
                if(horizontalHeuristic.Where(c => c == 'X').Count() == 3)
                {
                    heuristic += 100;
                }
                // 2 X and one space
                if (horizontalHeuristic.Where(c => c == 'X').Count() == 2 && horizontalHeuristic.Where(c => c == ' ').Count() == 1)
                {
                    heuristic += 10;
                }
                // 1 X and two spaces
                if (horizontalHeuristic.Where(c => c == 'X').Count() == 1 && horizontalHeuristic.Where(c => c == ' ').Count() == 2)
                {
                    heuristic += 1;
                }
                // 3 0 in a row
                if (horizontalHeuristic.Where(c => c == '0').Count() == 3)
                {
                    heuristic -= 100;
                }
                // 2 0 and one space
                if (horizontalHeuristic.Where(c => c == '0').Count() == 2 && horizontalHeuristic.Where(c => c == ' ').Count() == 1)
                {
                    heuristic -= 10;
                }
                // 1 0 and two spaces
                if (horizontalHeuristic.Where(c => c == '0').Count() == 1 && horizontalHeuristic.Where(c => c == ' ').Count() == 2)
                {
                    heuristic -= 1;
                }

                // Vertical lines
                List<char> verticalHeuristic = new List<char>();
                verticalHeuristic.Add(game[0][i]);
                verticalHeuristic.Add(game[1][i]);
                verticalHeuristic.Add(game[2][i]);
                // 3 X in a row
                if (verticalHeuristic.Where(c => c == 'X').Count() == 3)
                {
                    heuristic += 100;
                }
                // 2 X and one space
                if (verticalHeuristic.Where(c => c == 'X').Count() == 2 && verticalHeuristic.Where(c => c == ' ').Count() == 1)
                {
                    heuristic += 10;
                }
                // 1 X and two spaces
                if (verticalHeuristic.Where(c => c == 'X').Count() == 1 && verticalHeuristic.Where(c => c == ' ').Count() == 2)
                {
                    heuristic += 1;
                }
                // 3 0 in a row
                if (verticalHeuristic.Where(c => c == '0').Count() == 3)
                {
                    heuristic -= 100;
                }
                // 2 0 and one space
                if (verticalHeuristic.Where(c => c == '0').Count() == 2 && verticalHeuristic.Where(c => c == ' ').Count() == 1)
                {
                    heuristic -= 10;
                }
                // 1 0 and two spaces
                if (verticalHeuristic.Where(c => c == '0').Count() == 1 && verticalHeuristic.Where(c => c == ' ').Count() == 2)
                {
                    heuristic -= 1;
                }
            }

            // Diagonal 1
            List<char> diagonal1Heuristic = new List<char>();
            diagonal1Heuristic.Add(game[0][0]);
            diagonal1Heuristic.Add(game[1][1]);
            diagonal1Heuristic.Add(game[2][2]);
            // 3 X in a row
            if (diagonal1Heuristic.Where(c => c == 'X').Count() == 3)
            {
                heuristic += 100;
            }
            // 2 X and one space
            if (diagonal1Heuristic.Where(c => c == 'X').Count() == 2 && diagonal1Heuristic.Where(c => c == ' ').Count() == 1)
            {
                heuristic += 10;
            }
            // 1 X and two spaces
            if (diagonal1Heuristic.Where(c => c == 'X').Count() == 1 && diagonal1Heuristic.Where(c => c == ' ').Count() == 2)
            {
                heuristic += 1;
            }
            // 3 0 in a row
            if (diagonal1Heuristic.Where(c => c == '0').Count() == 3)
            {
                heuristic -= 100;
            }
            // 2 0 and one space
            if (diagonal1Heuristic.Where(c => c == '0').Count() == 2 && diagonal1Heuristic.Where(c => c == ' ').Count() == 1)
            {
                heuristic -= 10;
            }
            // 1 0 and two spaces
            if (diagonal1Heuristic.Where(c => c == '0').Count() == 1 && diagonal1Heuristic.Where(c => c == ' ').Count() == 2)
            {
                heuristic -= 1;
            }

            // Diagonal 2
            List<char> diagonal2Heuristic = new List<char>();
            diagonal2Heuristic.Add(game[0][2]);
            diagonal2Heuristic.Add(game[1][1]);
            diagonal2Heuristic.Add(game[2][0]);
            if (diagonal2Heuristic.Where(c => c == 'X').Count() == 3)
            {
                heuristic += 100;
            }
            // 2 X and one space
            if (diagonal2Heuristic.Where(c => c == 'X').Count() == 2 && diagonal2Heuristic.Where(c => c == ' ').Count() == 1)
            {
                heuristic += 10;
            }
            // 1 X and two spaces
            if (diagonal2Heuristic.Where(c => c == 'X').Count() == 1 && diagonal2Heuristic.Where(c => c == ' ').Count() == 2)
            {
                heuristic += 1;
            }
            // 3 0 in a row
            if (diagonal2Heuristic.Where(c => c == '0').Count() == 3)
            {
                heuristic -= 100;
            }
            // 2 0 and one space
            if (diagonal2Heuristic.Where(c => c == '0').Count() == 2 && diagonal2Heuristic.Where(c => c == ' ').Count() == 1)
            {
                heuristic -= 10;
            }
            // 1 0 and two spaces
            if (diagonal2Heuristic.Where(c => c == '0').Count() == 1 && diagonal2Heuristic.Where(c => c == ' ').Count() == 2)
            {
                heuristic -= 1;
            }

            return heuristic;
        }

        public (int eval, int x, int y) MinimaxAlphaBetaPruning(char[][] table, int depth, bool maximizingPlayer = true, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if(depth == 0 || IsGameDone(table))
            {
                return (Heuristic(table), -1, -1);
            }

            if (maximizingPlayer)
            {
                // X player
                int maxEval = int.MinValue;
                int x = -1;
                int y = -1;
                // For each child move
                for(int i = 0; i < 3; ++i)
                {
                    for(int j = 0; j < 3; ++j)
                    {
                        // Possible move, child
                        if(IsCellEmpty(table, i, j))
                        {
                            char[][] newGame = CloneGame(table);
                            newGame[i][j] = 'X';
                            var evalMM = MinimaxAlphaBetaPruning(newGame, depth - 1, false, alpha, beta);
                            int eval = evalMM.eval;
                            if(eval > maxEval)
                            {
                                maxEval = eval;
                                x = i;
                                y = j;
                            }
                            alpha = Math.Max(alpha, eval);
                            if(beta <= alpha)
                            {
                                goto break_for_max;
                            }
                        }
                    }
                }
                break_for_max:
                return (maxEval, x, y);
            }
            else
            {
                // 0 player
                int minEval = int.MaxValue;
                int x = -1;
                int y = -1;
                // For each child move
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        // Possible move, child
                        if (IsCellEmpty(table, i, j))
                        {
                            char[][] newGame = CloneGame(table);
                            newGame[i][j] = '0';
                            var evalMM = MinimaxAlphaBetaPruning(newGame, depth - 1, true, alpha, beta);
                            int eval = evalMM.eval;
                            if(eval < minEval)
                            {
                                minEval = eval;
                                x = i;
                                y = j;
                            }
                            beta = Math.Min(beta, eval);
                            if (beta <= alpha)
                            {
                                goto break_for_min;
                            }
                        }
                    }
                }
                break_for_min:
                return (minEval, x, y);
            }
        }

        public (int, int) AiGetNextMove()
        {
            var result = MinimaxAlphaBetaPruning(Game, MINIMAX_LEVELS);
            return (result.x, result.y);
        }

        public bool IsCellEmpty(char[][] game, int x, int y)
        {
            return game[x][y] == ' ';
        }

        public bool IsGameFull(char[][] game)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (IsCellEmpty(game, i, j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool HasWinner(char[][] game)
        {
            // Conditions in separate ifs so both are checked
            // With OR between them, the second one is checked only if it makes sense for the result
            if (IsWinner(game, 'X'))
            {
                return true;
            }
            if (IsWinner(game, '0'))
            {
                return true;
            }
            return false;
        }

        public bool IsWinner(char[][] game, char player)
        {
            for (int i = 0; i < 3; ++i)
            {
                // Horizontal lines
                if (game[i][0] == player && game[i][1] == player && game[i][2] == player)
                {
                    if (game == Game)
                    {
                        Labels[i][0].ForeColor = Color.Red;
                        Labels[i][1].ForeColor = Color.Red;
                        Labels[i][2].ForeColor = Color.Red;
                    }
                    return true;
                }

                // Vertical lines
                if (game[0][i] == player && game[1][i] == player && game[2][i] == player)
                {
                    if (game == Game)
                    {
                        Labels[0][i].ForeColor = Color.Red;
                        Labels[1][i].ForeColor = Color.Red;
                        Labels[2][i].ForeColor = Color.Red;
                    }
                    return true;
                }
            }

            // Diagonal 1
            if (game[0][0] == player && game[1][1] == player && game[2][2] == player)
            {
                if (game == Game)
                {
                    Labels[0][0].ForeColor = Color.Red;
                    Labels[1][1].ForeColor = Color.Red;
                    Labels[2][2].ForeColor = Color.Red;
                }
                return true;
            }

            // Diagonal 2
            if (game[0][2] == player && game[1][1] == player && game[2][0] == player)
            {
                if (game == Game)
                {
                    Labels[0][2].ForeColor = Color.Red;
                    Labels[1][1].ForeColor = Color.Red;
                    Labels[2][0].ForeColor = Color.Red;
                }
                return true;
            }

            return false;
        }

        public bool IsGameDone(char[][] game)
        {
            return HasWinner(game) || IsGameFull(game);
        }

        public void AiDoMove(int x, int y)
        {
            try
            {
                PutX(x, y);
                IsGameDone(Game);
            }
            catch (Exception e)
            {
                // Invalid move
                Debug.WriteLine("AI invalid move!");
            }
        }

        public void UserDoMove(int x, int y)
        {
            if (!IsGameDone(Game))
            {
                try
                {
                    Put0(x, y);

                    if (!IsGameDone(Game))
                    {
                        var move = AiGetNextMove();
                        AiDoMove(move.Item1, move.Item2);
                    }
                }
                catch(Exception e)
                {
                    // Invalid move
                    Debug.WriteLine("User invalid move!");
                }
            }
        }

        private void RedrawGame()
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    Labels[i][j].Text = "" + Game[i][j];
                }
            }
        }

        private void label00_Click(object sender, EventArgs e)
        {
            UserDoMove(0, 0);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            UserDoMove(1, 0);
        }

        private void label20_Click(object sender, EventArgs e)
        {
            UserDoMove(2, 0);
        }

        private void label01_Click(object sender, EventArgs e)
        {
            UserDoMove(0, 1);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            UserDoMove(1, 1);
        }

        private void label21_Click(object sender, EventArgs e)
        {
            UserDoMove(2, 1);
        }

        private void label02_Click(object sender, EventArgs e)
        {
            UserDoMove(0, 2);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            UserDoMove(1, 2);
        }

        private void label22_Click(object sender, EventArgs e)
        {
            UserDoMove(2, 2);
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            Game = new char[][]
            {
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '},
                new char[]{' ', ' ', ' '}
            };
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    Labels[i][j].ForeColor = Color.Black;
                }
            }
            RedrawGame();
        }
    }
}
