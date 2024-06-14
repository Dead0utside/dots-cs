using System;

namespace Dots.Core
{
    [Serializable]
    public struct Coordinate //struct to handle coordinates easier
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}