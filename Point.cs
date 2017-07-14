using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipBot
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
        }

        public bool Equals(Point other)
        {
            bool xSame = X == other.X;
            bool ySame = Y == other.Y;
            return xSame && ySame;
        }

        public Point Plus(Point other)
        {
            return new Point(X + other.X, Y + other.Y); 
        }
    }
}
