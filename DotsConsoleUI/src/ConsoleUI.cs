using Dots.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotsCore.Entity;
using DotsCore.Service;
using Action = Dots.Core.Action; //to avoid conflict with System.Action

namespace Dots.ConsoleUI
{
    public class ConsoleUI
    {
        private string _nickname;
        private Field _field;
        private Regex _regex; //for checking input of initial coordinates
        private ScoreServiceFile _scoreServiceFile = new ScoreServiceFile();
        private RatingServiceFile _ratingServiceFile = new RatingServiceFile();
        private CommentServiceFile _commentServiceFile = new CommentServiceFile();

        private ScoreServiceEF _scoreServiceEF = new ScoreServiceEF();
        private RatingServiceEF _ratingServiceEF = new RatingServiceEF();
        private CommentServiceEF _commentServiceEF = new CommentServiceEF();

        public ConsoleUI(Field field)
        {
            _field = field;
            _regex = new Regex("^[0-9] [0-9]$");
        }

        public void Play()
        {
            Console.Clear();
            Console.WriteLine("Enter your nickname: ");
            _nickname = Console.ReadLine();
            Console.Clear();
            PrintField();
            while (_field.GameRunning)
            {
                // _field.Timer();
                HandleInput();
                PrintField();
                _field.Timer();
            }
            
            PostGame();
        }

        private void PrintField()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  ");
            for (int i = 0; i < _field.Cols; i++)
            {
                Console.Write(i + " ");
            } 
            Console.WriteLine();
            for (int r = 0; r < _field.Rows; r++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(r + " ");
                for (int c = 0; c < _field.Cols; c++)
                {
                    if (_field.GetDot(r, c) == null) //if there is no dots on current position, print blank space (shouldn't happen but who knows)
                    {
                        Console.Write("  ");
                        continue;
                    }
                    switch (_field.GetDot(r, c).Color) //choose the right color for dot
                    {
                            case Color.Blue:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case Color.Green:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case Color.Purple:
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            case Color.Red:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case Color.Yellow:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case Color.None:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            default:
                                continue;
                    }
                       /*out of bounds control*/                            /*----------check if 2 adjacent dots are both in the line----------*/                                 /*-----avoid unnecessary visual connections when 2 dots are adjacent, however they were not selected successively-----*/                                                                                                   //don't ask
                    if (c + 1 <= _field.Cols - 1 && _field.GetLine().Contains(_field.GetDot(r, c)) && _field.GetLine().Contains(_field.GetDot(r, c + 1)) && (_field.GetLine().IndexOf(_field.GetDot(r, c + 1)) - 1 == _field.GetLine().IndexOf(_field.GetDot(r, c)) || _field.GetLine().IndexOf(_field.GetDot(r, c + 1)) + 1 == _field.GetLine().IndexOf(_field.GetDot(r, c))))
                    {
                        Console.Write("*" + "-");
                    }
                    else
                    {
                        Console.Write("*" + " "); //white spaces will be replaced with "-" when colors are connected
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("\n"); //white spaces in blank rows will be replaced with "|" when colors are connected
            }

            for (int r = 0; r < _field.Rows; r++) //this cycle is made for writing | connections
            {
                for (int c = 0; c < _field.Cols; c++)
                {
                       /*out of bounds control*/                           /*----------check if 2 adjacent dots are both in the line----------*/                                  /*-----avoid unnecessary visual connections when 2 dots are adjacent, however they were not selected successively-----*/                                                                                                   //don't ask
                    if (r + 1 <= _field.Rows - 1 && _field.GetLine().Contains(_field.GetDot(r, c)) && _field.GetLine().Contains(_field.GetDot(r + 1, c)) && (_field.GetLine().IndexOf(_field.GetDot(r + 1, c)) - 1 == _field.GetLine().IndexOf(_field.GetDot(r, c)) || _field.GetLine().IndexOf(_field.GetDot(r + 1, c)) + 1 == _field.GetLine().IndexOf(_field.GetDot(r, c))))
                    {
                        Console.SetCursorPosition((c * 2) + 2, (r * 2) + 2);
                        switch (_field.GetDot(r, c).Color) //choose the right color for pipe
                        {
                            case Color.Blue:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case Color.Green:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case Color.Purple:
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            case Color.Red:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case Color.Yellow:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case Color.None:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            default:
                                continue;
                        }
                        Console.Write("|");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, (_field.Rows * 2) + 4); 
            Console.Write("Score: " + _field.Score);
            Console.SetCursorPosition(0, (_field.Rows * 2) + 5); 
            Console.Write("Time left: " + _field.SecondsLeft);
        }

        private void QuitPrompt()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write("Are you sure want to quit? [y/n]");
            string answer = null;
            while (answer == null)
            {
                answer = Console.ReadLine();
            }
            if (answer.Equals("y") || answer.Equals("yes"))
            {
                _field.Exit();
            }
            else if (answer.Equals("n") || answer.Equals("no"))
            {
                Console.Clear();
            }
        }

        private void HandleInput()
        {
            if (!this._field.GetLine().Any()) //if the line is empty, it would be good to start from something
            {
                Console.SetCursorPosition(0, (_field.Rows * 2) + 1);
                string inputStr = "";
                Console.Write("Enter the initial coordinates [row col]: ");
                while (!_regex.IsMatch(inputStr)) //handling input of initial coordinates
                {
                    Console.SetCursorPosition(41, (_field.Rows * 2) + 1);
                    Console.Write("                "); //just clearing the line for better look
                    Console.SetCursorPosition(41, (_field.Rows * 2) + 1);

                    inputStr = Console.ReadLine();
                }

                string[] inputNums = inputStr.Split(); //split the input into 2 numbers
                Coordinate startingCoordinates = new Coordinate(int.Parse(inputNums[0]), int.Parse(inputNums[1])); //convert number strings into respective int values and create Coordinate structure using them
                _field.Start(startingCoordinates); 
            }
            Console.SetCursorPosition(0, (_field.Rows * 2) + 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose direction: ");
            ConsoleKey key = Console.ReadKey().Key; //choosing direction to proceed with connecting
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _field.Connect(Action.Up);
                    break;
                case ConsoleKey.LeftArrow:
                    _field.Connect(Action.Left);
                    break;
                case ConsoleKey.DownArrow:
                    _field.Connect(Action.Down);
                    break;
                case ConsoleKey.RightArrow:
                    _field.Connect(Action.Right);
                    break;
                case ConsoleKey.Enter:
                    _field.Connect(Action.End);
                    Console.Clear();
                    break;
                case ConsoleKey.Q:
                    _field.Connect(Action.Cancel);
                    break;
                case ConsoleKey.Escape: //this will be changed slightly in future
                    QuitPrompt();
                    break;
            }
        }

        private void PostGame()
        {
            PostGameScore();
            PostGameRating();
            PostGameComment();
            while (true)
            {
                if (PostPostGame())
                {
                    return;
                }
            }
        }

        private void PostGameScore()
        {
            Console.Clear();
            Console.Write("Time's up! Your score is " + _field.Score + " points. Do you want to add it to the leaderboard? [y/n]: ");
            string answer = null;
            while (answer == null)
            {
                answer = Console.ReadLine();
                if (answer.Equals("y") || answer.Equals("yes"))
                {
                    _scoreServiceEF.AddScore(new Score{PlayerName = _nickname, Points = _field.Score});
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (answer.Equals("n") || answer.Equals("no"))
                {
                    break;
                }
                else
                {
                    answer = null;
                }
            }
        }

        private void PostGameRating()
        {
            Console.Clear();
            Console.Write("Do you wish to rate this game? [y/n]: ");
            string answer = null;
            while (answer == null)
            {
                answer = Console.ReadLine();
                if (answer.Equals("y") || answer.Equals("yes"))
                {
                    Console.Write("Please, rate the game from 0 to 100: ");
                    string rateStr = null;
                    int rate = 0;
                    while (rateStr == null)
                    {
                        rateStr = Console.ReadLine();
                        if (int.TryParse(rateStr, out rate))
                        {
                            rate = int.Parse(rateStr);
                        }
                        else
                        {
                            rateStr = null;
                        }
                    }
                    _ratingServiceEF.AddRating(new Rating(){PlayerName = _nickname, Rate = rate});
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (answer.Equals("n") || answer.Equals("no"))
                {
                    break;
                }
                else
                {
                    answer = null;
                }
            }
        }

        private void PostGameComment()
        {
            Console.Clear();
            Console.Write("Do you wish to leave a comment about your experience? [y/n]: ");
            string answer = null;
            while (answer == null)
            {
                answer = Console.ReadLine();
                if (answer.Equals("y") || answer.Equals("yes"))
                {
                    Console.WriteLine("Please, write a comment below. Feel free to tell whatever you want as long as the game is concerned.");
                    string comment = null;
                    while (comment == null)
                    {
                        comment = Console.ReadLine();
                    }
                    _commentServiceEF.AddComment(new Comment(){PlayerName = _nickname, Text = comment});
                    Console.WriteLine("Thank you for your feedback!");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (answer.Equals("n") || answer.Equals("no"))
                {
                    break;
                }
                else
                {
                    answer = null;
                }
            }
        }

        private bool PostPostGame()
        {
            Console.Clear();
            Console.WriteLine("That's all for now, thank you very much for your time. I hope the recent experience was pleasant for you.");
            Console.WriteLine("Press T to see top scores.");
            Console.WriteLine("Press R to see other ratings.");
            Console.WriteLine("Press C to see other comments.");
            Console.WriteLine("Press F to pay respects.");
            Console.WriteLine("Press ESCAPE to quit the game.");
            
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.T:
                    PrintScores();
                    break;
                case ConsoleKey.R:
                    PrintRatings();
                    break;
                case ConsoleKey.C:
                    PrintComments();
                    break;
                case ConsoleKey.F:
                    Console.Clear();
                    Console.WriteLine("Jokes on you XD. Funny, isn't it?");
                    System.Threading.Thread.Sleep(300);
                    return true;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.WriteLine("Have a nice day, see ya!");
                    System.Threading.Thread.Sleep(300);
                    return true;
            }

            return false;
        }

        private void PrintScores()
        {
            Console.Clear();
            IList<Score> scores = _scoreServiceEF.GetTopScores();
            foreach (var entry in scores)
            {
                Console.WriteLine(entry.PlayerName + ": " + entry.Points);
            }

            Console.WriteLine();

            Console.WriteLine("Press ESCAPE to go back.");
            while (true) 
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }
        
        private void PrintRatings()
        {
            Console.Clear();
            IList<Rating> ratings = _ratingServiceEF.GetRatings();
            foreach (var entry in ratings)
            {
                Console.WriteLine(entry.PlayerName + ": " + entry.Rate);
            }

            Console.WriteLine();

            Console.WriteLine("Press ESCAPE to go back.");
            while (true) 
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }
        
        private void PrintComments()
        {
            Console.Clear();
            IList<Comment> comments = _commentServiceEF.GetComments();
            foreach (var entry in comments)
            {
                Console.WriteLine(entry.PlayerName + ": " + entry.Text);
            }

            Console.WriteLine();

            Console.WriteLine("Press ESCAPE to go back.");
            while (true) 
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }
    }
}