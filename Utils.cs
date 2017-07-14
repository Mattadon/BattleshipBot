using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipBot.Gameboards;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    public class Utils
    {
        public static Point ConvertGridSquareToPoint(IGridSquare square)
        {
            int absX = square.Column - 1;
            int absY = (int)(square.Row - 'A');

            return new Point(absX, absY);
        }

        public static IGridSquare ConvertPointToGridSquare(Point point)
        {
            return new GridSquare(GameBoard.GridRefs.ToCharArray()[point.Y], point.X + 1);
        }
    }
}
