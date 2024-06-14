using System;
using System.Collections.Generic;

namespace Dots.Core
{
    [Serializable]
    public class Field
    {
        public int Rows { get; }
        public int Cols { get; }

        private DateTime _startTime;

        private int _time;
        public int SecondsLeft { get; private set; }
        
        public int Score { get; private set; }
        private Coordinate _currentCoordinates; //to save current position between Start() and Connect() methods

        public bool GameRunning { get; private set; }
        
        private List<Dot> _line = new List<Dot>(); //to save the line of connected dots in order to remove them in future

        private Dot[,] _dots; //to store the field layout

        public Field(int rows, int cols, int time)
        {
            Rows = rows;
            Cols = cols;
            _time = time;
            Score = 0;
            _startTime = DateTime.Now;
            _dots = new Dot[rows, cols];
            Generate();
            GameRunning = true;
        }

        public Dot GetDot(int row, int col) //returns dot on a certain position
        {
            return this._dots[row, col];
        }

        private void Generate() //generates initial field
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    var colorValues = Enum.GetValues (typeof (Color)); //gets array of Color Enum values, excepts None value
                    _dots[r, c] = new Dot((Color)colorValues.GetValue(new Random().Next(colorValues.Length - 1))); //creates a dot with a random color on current position
                }
            }
        }

        private void RemoveConnectedDots()
        {
            //cycle through the whole field
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    foreach (Dot dot in _line) //if field contains a dot, which is a part of line list, it must be removed from the field
                    {
                        if (_dots[r, c] == dot)
                        {
                            _dots[r, c] = null;
                        }
                    }
                }
            }
        }


        public bool Start(Coordinate coordinates) //to begin dots connecting
        {
            if (coordinates.Row > this.Rows - 1 || coordinates.Col > this.Cols - 1)
            {
                return false; //out of bounds handling
            }

            _currentCoordinates = coordinates; //save current coordinates 
            _line.Add(_dots[coordinates.Row, coordinates.Col]); //add the first dot to list of dots that are going to be connected
            
            return true;
        }
        
        public void Connect(Action action)
        {
            // Coordinate coordinateDiff = new Coordinate(); //to convert the action into coordinate shift form
            // switch (action) //converting
            // {
            //     case Action.None:
            //         return; //just in case
            //     case Action.Up:
            //         coordinateDiff.Row = -1;
            //         break;
            //     case Action.Left:
            //         coordinateDiff.Col = -1;
            //         break;
            //     case Action.Down:
            //         coordinateDiff.Row = 1;
            //         break;
            //     case Action.Right:
            //         coordinateDiff.Col = 1;
            //         break;
            //     case Action.Cancel:
            //         _line.Clear();
            //         return;
            //     case Action.End: //this case is made to finish connecting
            //         if (_line.Count <= 1) //do nothing if there is only 1 dot in connection line
            //         {
            //             return;
            //         }
            //         RemoveConnectedDots();
            //         Score += _line.Count;
            //         _line.Clear(); //make line list empty for future usage
            //         while (true) //make it rain
            //         {
            //             if (!this.Gravity())
            //             {
            //                 break;
            //             }
            //         }
            //         this.AddNewDots(); //for next game cycle
            //         return;
            // }

            //getting new coordinates
            // Coordinate newCoordinates = new Coordinate();
            // newCoordinates.Row = _currentCoordinates.Row + coordinateDiff.Row;
            // newCoordinates.Col = _currentCoordinates.Col + coordinateDiff.Col;
            // if (newCoordinates.Row < 0 || newCoordinates.Row > this.Rows - 1 || newCoordinates.Col < 0 ||
            //     newCoordinates.Col > this.Cols - 1)
            // {
            //     return; //out of bounds handling
            // }

            // if (_dots[_currentCoordinates.Row, _currentCoordinates.Col] != null &&
            //     _dots[newCoordinates.Row, newCoordinates.Col] != null &&
            //     _dots[_currentCoordinates.Row, _currentCoordinates.Col].Color ==
            //     _dots[newCoordinates.Row, newCoordinates.Col].Color)
            // {
            //     if (_line.Count > 1 && _line[_line.Count - 1] == _dots[newCoordinates.Row, newCoordinates.Col]) //to restrict choosing previous dot
            //     {
            //         return;
            //     }
            //     _currentCoordinates = newCoordinates;
            //     _line.Add(_dots[_currentCoordinates.Row, _currentCoordinates.Col]); //adding selected dot to line of dots
            // }
            
            ConnectionHandling(CreateNewCoordinates(action));
        }

        public Coordinate CreateNewCoordinates(Action action)
        {
            Coordinate coordinateDiff = new Coordinate(0, 0); //to convert the action into coordinate shift form
            switch (action) //converting
            {
                case Action.None:
                    return new Coordinate(-1, -1); //special exit case for next method
                case Action.Up:
                    coordinateDiff.Row = -1;
                    break;
                case Action.Left:
                    coordinateDiff.Col = -1;
                    break;
                case Action.Down:
                    coordinateDiff.Row = 1;
                    break;
                case Action.Right:
                    coordinateDiff.Col = 1;
                    break;
                case Action.Cancel:
                    // _line.Clear();
                    return new Coordinate(-3, -3); //special exit case for next method
                case Action.End: //this case is made to finish connecting
                    // if (_line.Count <= 1) //do nothing if there is only 1 dot in connection line
                    // {
                    //     return new Coordinate(-1, -1); //special exit case for next method
                    // }
                    // RemoveConnectedDots();
                    // Score += _line.Count;
                    // _line.Clear(); //make line list empty for future usage
                    // while (true) //make it rain
                    // {
                    //     if (!this.Gravity())
                    //     {
                    //         break;
                    //     }
                    // }
                    // this.AddNewDots(); //for next game cycle
                    return new Coordinate(-2, -2); //special exit case for next method
            }

            Coordinate newCoordinates = new Coordinate();
            newCoordinates.Row = _currentCoordinates.Row + coordinateDiff.Row;
            newCoordinates.Col = _currentCoordinates.Col + coordinateDiff.Col;
            if (newCoordinates.Row < 0 || newCoordinates.Row > this.Rows - 1 || newCoordinates.Col < 0 ||
                newCoordinates.Col > this.Cols - 1)
            {
                return new Coordinate(-1, -1); //out of bounds handling
            }
            return newCoordinates;
        }

        public void ConnectionHandling(Coordinate newCoordinates)
        {
            if (newCoordinates.Row == -1 && newCoordinates.Col == -1) //special exit case
            {
                return;
            }

            if (newCoordinates.Row == -2 && newCoordinates.Col == -2) //END signal
            {
                if (_line.Count <= 1) //do nothing if there is only 1 dot in connection line
                {
                    return;
                }
                RemoveConnectedDots();
                Score += _line.Count;
                _line.Clear(); //make line list empty for future usage
                while (true) //make it rain
                {
                    if (!this.Gravity())
                    {
                        break;
                    }
                }
                this.AddNewDots(); //for next game cycle
                return;
            }

            if (newCoordinates.Row == -3 && newCoordinates.Col == -3) //CANCEL signal
            {
                _line.Clear();
                return;
            }
            
            if (_dots[_currentCoordinates.Row, _currentCoordinates.Col] != null &&
                _dots[newCoordinates.Row, newCoordinates.Col] != null &&
                _dots[_currentCoordinates.Row, _currentCoordinates.Col].Color ==
                _dots[newCoordinates.Row, newCoordinates.Col].Color)
            {
                if (_line.Count > 1 && _line[_line.Count - 1] == _dots[newCoordinates.Row, newCoordinates.Col]) //to restrict choosing previous dot
                {
                    return;
                }

                if (_line.Count >= 1)
                {
                    int diffX = Math.Abs(_currentCoordinates.Col - newCoordinates.Col);
                    int diffY = Math.Abs(_currentCoordinates.Row - newCoordinates.Row);
                    if (diffX + diffY != 1)
                    {
                        return;
                    }
                }
                _currentCoordinates = newCoordinates;
                _line.Add(_dots[_currentCoordinates.Row, _currentCoordinates.Col]); //adding selected dot to line of dots
            }
        }
        
        public bool Gravity()
        {
            for (int r = 0; r < this.Rows - 1; r++) //cycle through the field except last row
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    if (_dots[r, c] != null && _dots[r + 1, c] == null) //if there is and empty space under a certain dot, this dot should be moved to that empty space
                    {
                        _dots[r + 1, c] = _dots[r, c];
                        _dots[r, c] = null;
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddNewDots()
        {
            for (int r = this.Rows - 1; r >= 0; r--)
            {
                for (int c = this.Cols - 1; c >= 0; c--)
                {
                    if (_dots[r, c] == null)
                    {
                        var colorValues = Enum.GetValues (typeof (Color)); //gets array of Color Enum values, excepts None value
                        _dots[r, c] = new Dot((Color)colorValues.GetValue(new Random().Next(colorValues.Length - 1))); //creates a dot with a random color on current position
                    }
                }
            }
        }

        public void Exit()
        {
            this.GameRunning = false;
        }

        public List<Dot> GetLine()
        {
            return this._line;
        }

        public void Timer() //ends game when time is up
        {
            SecondsLeft = _time - (int)(DateTime.Now - _startTime).TotalSeconds;
            if (SecondsLeft <= 0)
            {
                this.GameRunning = false;
            }
        }
    }
}