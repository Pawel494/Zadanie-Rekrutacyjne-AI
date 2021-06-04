using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zadanie_rekrutacyjne
{

    class Program
    {
        static int countMinimax = 0;

        static int n = 4, k = 4;
        static int MAX = 10000, MIN = -10000;
        char player, opponent;

        static ulong [,,]zobristTable = new ulong[n, n, 2];
        Dictionary<ulong, int> computedBoards = new Dictionary<ulong, int>();

        public Program(bool is_x)
        {
            if(is_x)
            {
                player = 'x';
                opponent = 'o';
            } else
            {
                player = 'o';
                opponent = 'x';
            }
        }

        static int indexOf(char who)
        {
            if(who == 'x')
                return 0;
            if(who == 'o')
                return 1;
            else
                return -1;
        }

        static ulong computeHash(char [,]board)
        {
            ulong h = 0;
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(board[i, j] != '_')
                    {
                        int who = indexOf(board[i, j]);
                        h ^= zobristTable[i, j, who];
                    }
                }
            }
            return h;
        }

        class Move
        {
            public int row, col;
        }

        // checks if there are any moves left on the board
        static bool isMoveLeft(char [,]board) 
        {
            for(int i = 0; i < n; i++) 
            {
                for(int j = 0; j < n; j++) 
                {
                    if(board[i, j] == '_')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static void printBoard(char [,]board)
        {
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.Write("\n");
            }
        }

        int evaluate(char [,]board)
        {
            //checking rows for 'x' or 'o' victory
            for(int row = 0; row < n; row++) 
            {
                int count = 1;
                for(int j = 1; j < n; j++) 
                {
                    if(board[row, j-1] == board[row, j] && (board[row, j] == player || board[row, j] == opponent))
                    {
                        count++;
                        if(count == k)
                        {
                            if(board[row, j] == player)
                                return 100;
                            else
                                return -100;
                        }
                    } else
                    {
                        count = 1;
                    }
                }
            }

            //checking columns for 'x' or 'o' victory
            for(int col = 0; col < n; col++) 
            {
                int count = 1;
                for(int j = 1; j < n; j++)
                {
                    if(board[j-1, col] == board[j, col] && (board[j, col] == player || board[j, col] == opponent))
                    {
                        count++;
                        if(count == k)
                        {
                            if(board[j, col] == player)
                                return 100;
                            else
                                return -100;
                        }
                    } else
                    {
                        count = 1;
                    }
                }
            }

            //checking diagonals for 'x' or 'o' victory

            //diagonals from top left to bottom right
            if(n == 5)
            {
                if(k == 3)
                {
                    for(int i = 0; i <= 2; i++)
                    {
                        for(int j = 0 ; j <= 2; j++)
                        {
                            if(board[i, j] == board[i+1, j+1] && board[i+1, j+1] == board[i+2, j+2])
                            {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                            }
                        }
                    }
                }
                if(k == 4)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        for(int j = 0 ; j <= 1; j++)
                        {
                            if(board[i, j] == board[i+1, j+1] && board[i+1, j+1] == board[i+2, j+2]
                             && board[i+2, j+2] == board[i+3, j+3])
                            {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                            }
                        }
                    }
                }
                if(k == 5)
                {
                    if(board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]
                        && board[2, 2] == board[3, 3] && board[3, 3] == board[4, 4])
                    {
                        if(board[0, 0] == player)
                            return 100;
                        if(board[0, 0] == opponent)
                            return -100;
                    }
                }
            }
            if(n == 4)
            {
                if(k == 3)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        for(int j = 0; j <= 1; j++)
                        {
                            if(board[i, j] == board[i+1, j+1] && board[i+1, j+1] == board[i+2, j+2])
                            {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                            }
                        }
                    }
                }
                if(k == 4)
                {
                    if(board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]
                        && board[2, 2] == board[3, 3])
                    {
                        if(board[0, 0] == player)
                            return 100;
                        if(board[0, 0] == opponent)
                            return -100;
                    }
                }
            }
            if(n == 3)
            {
                if(board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                {
                    if(board[0, 0] == player)
                        return 100;
                    if(board[0, 0] == opponent)
                        return -100;
                }
            }


            //diagonals from top right to bottom left
            if(n == 5)
            {
                if(k == 5)
                {
                    if(board[0, 4] == board[1, 3] && board[1, 3] == board[2, 2]
                     && board[2, 2] == board[3, 1] && board[3, 1] == board[4, 0])
                    {
                        if(board[0, 4] == player)
                            return 100;
                        if(board[0, 4] == opponent) 
                            return -100;
                    }
                }
                if(k == 4)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        for(int j = 3; j <= 4; j++)
                        {
                            if(board[i, j] == board[i+1, j-1] && board[i, j] == board[i+2, j-2]
                             && board[i, j] == board[i+3, j-3])
                             {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                             }
                        }
                    }
                }
                if(k == 3)
                {
                    for(int i = 0; i <= 2; i++)
                    {
                        for(int j = 2; j <= 4; j++)
                        {
                            if(board[i, j] == board[i+1, j-1] && board[i, j] == board[i+2, j-2])
                             {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                             }
                        }
                    }
                }
            }
            if(n == 4)
            {
                if(k == 4)
                {
                    if(board[0, 3] == board[1, 2] && board[1, 2] == board[2, 1] && board[2, 1] == board[3, 0])
                    {
                        if(board[0, 3] == player)
                            return 100;
                        if(board[0, 3] == opponent) 
                            return -100;
                    }
                }
                if(k == 3)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        for(int j = 2; j <= 2; j++)
                        {
                            if(board[i, j] == board[i+1, j-1] && board[i, j] == board[i+2, j-2])
                             {
                                if(board[i, j] == player)
                                    return 100;
                                if(board[i, j] == opponent)
                                    return -100;
                             }
                        }
                    }
                }
            }
            if(n == 3)
            {
                if(board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                {
                    if(board[1, 1] == player)
                        return 100;
                    if(board[1, 1] == opponent) 
                        return -100;
                }
            }

            return 0;
        }

        int minimax(char [,]board, int depth, bool isMaximizingMove, int alpha, int beta, ulong hash)
        {
            countMinimax++;

            int score = evaluate(board);

            // If somebody won the game return score
            if(score < 0)
                return (score + depth);
            if(score > 0)
                return (score - depth);
            
            
            // If nobody won and there are no moves left it is a tie
            if(!isMoveLeft(board))
                return 0;
            
            // If score for similar board has already been computed
            if(computedBoards.ContainsKey(hash))
            {
                return computedBoards[hash];
            }

            if(isMaximizingMove) 
            {
                int best = MIN;

                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < n; j++)
                    {
                        if(board[i, j] == '_')
                        {
                            // make a move
                            board[i, j] = player;
                            hash ^= zobristTable[i, j, indexOf(player)];

                            // compute a value for that move
                            best = Math.Max(best, minimax(board, depth + 1, !isMaximizingMove, alpha, beta, hash));

                            // take move back 
                            board[i, j] = '_';
                            hash ^= zobristTable[i, j, indexOf(player)];

                            alpha = Math.Max(alpha, best);
                            if(beta <= alpha)
                            {
                                // if found a winning path, save it in dictionary
                                if(best > 0)
                                    computedBoards.Add(hash, best);
                                return best;
                            }
                        }
                    }
                }
                computedBoards.Add(hash, best);
                return best;
            } 
            else 
            {
                int best = MAX;

                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < n; j++)
                    {
                        if(board[i, j] == '_')
                        {
                            // make a move
                            board[i, j] = opponent;
                            hash ^= zobristTable[i, j, indexOf(opponent)];

                            // compute a value for that move
                            best = Math.Min(best, minimax(board, depth + 1, !isMaximizingMove, alpha, beta, hash));

                            // take move back
                            board[i, j] = '_';
                            hash ^= zobristTable[i, j, indexOf(opponent)];

                            beta = Math.Min(beta, best);
                            if(beta <= alpha)
                            {
                                // if found a losing path, save it in dictionary
                                if(best < 0)
                                    computedBoards.Add(hash, best);
                                return best;
                            }
                        }
                    }
                }
                computedBoards.Add(hash, best);
                return best;
            }
        }

        Move findBestMove(char [,]board)
        {
            int bestValue = MIN;
            Move bestMove = new Move();
            bestMove.row = -1;
            bestMove.col = -1;

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(board[i, j] == '_')
                    {
                        // make a move
                        board[i, j] = player;

                        // compute a value for that move
                        int moveValue = minimax(board, 0, false, MIN, MAX, computeHash(board));

                        // take that move back to try another
                        board[i, j] = '_';

                        // save move with best value
                        if(moveValue > bestValue)
                        {
                            bestValue = moveValue;
                            bestMove.row = i;
                            bestMove.col = j;
                        }
                    }
                }
            }

            return bestMove;
        }

        void makeMove(char [,]board, Move move)
        {
            board[move.row, move.col] = player;
        }

        static void playAsPlayer1(char [,]board, Program player1, Program player2)
        {
            int nr = 0;

            while(player1.evaluate(board) == 0 && isMoveLeft(board))
            {
                printBoard(board);
                Console.WriteLine();

                if(nr % 2 == 0)
                {
                    Console.WriteLine("Your move...");
                    string row = Console.ReadLine();
                    string col = Console.ReadLine();

                    Move nextMove = new Move();
                    nextMove.row = Convert.ToInt32(row);
                    nextMove.col = Convert.ToInt32(col);

                    board[nextMove.row, nextMove.col] = 'x';
                } else
                {
                    Move nextMove = player2.findBestMove(board);
                    player2.makeMove(board, nextMove);
                }
                nr++;
            }
        }

        static void playAsPlayer2(char [,]board, Program player1, Program player2)
        {
            int nr = 0;

            while(player1.evaluate(board) == 0 && isMoveLeft(board))
            {
                printBoard(board);
                Console.WriteLine();

                if(nr % 2 == 0)
                {
                    Move nextMove = player1.findBestMove(board);
                    player1.makeMove(board, nextMove);
                } else
                {
                    Console.WriteLine("Your move...");
                    string row = Console.ReadLine();
                    string col = Console.ReadLine();

                    Move nextMove = new Move();
                    nextMove.row = Convert.ToInt32(row);
                    nextMove.col = Convert.ToInt32(col);

                    board[nextMove.row, nextMove.col] = 'o';
                }
                nr++;
            }
        }

        static void watchAIGame(char [,]board, Program player1, Program player2)
        {
            int nr = 0;

            while(player1.evaluate(board) == 0 && isMoveLeft(board))
            {
                printBoard(board);
                Console.WriteLine();
                if(nr % 2 == 0)
                {
                    Move nextMove = player1.findBestMove(board);
                    player1.makeMove(board, nextMove);
                } else
                {
                    Move nextMove = player2.findBestMove(board);
                    player2.makeMove(board, nextMove);
                }
                nr++;
            }
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            char [,]board = new char[n, n];

            // initialize board
            for(int i = 0 ; i < n; i++)
                for(int j = 0; j < n; j++)
                    board[i, j] = '_';


            // initialize Zobrist Table
            Random rnd = new Random();

            for(int i = 0; i < n; i ++)
            {
                for(int j = 0; j < n; j++)
                {
                    for(int k = 0; k < 2; k++)
                    {
                        zobristTable[i, j, k] = (ulong)(rnd.Next(0, 2147483646) * rnd.Next(0, 2147483646));
                    }
                }
            }

            Program player1 = new Program(true);
            Program player2 = new Program(false);
            

            Console.WriteLine("If you want to play as player 1, type '1'");
            Console.WriteLine("if you want to play as player 2, type '2'");
            Console.WriteLine("If you want to watch AI play, type '3'");

            string x = Console.ReadLine();

            sw.Start();

            if(x == "1")
                playAsPlayer1(board, player1, player2);
            else if(x == "2")
                playAsPlayer2(board, player1, player2);
            else
                watchAIGame(board, player1, player2);

            sw.Stop();
            

            int result = player1.evaluate(board);
            if(result == 0)
                Console.WriteLine("Game finished in a tie");
            else if(result > 0)
                Console.WriteLine("Player 1 won");
            else if(result < 0)
                Console.WriteLine("Player 2 won");

            printBoard(board);

            Console.WriteLine("Number of minimaxes = " + countMinimax);
            Console.WriteLine("Elapsed={0}",sw.Elapsed);

        }
    }
}
