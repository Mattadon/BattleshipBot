using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Battleships.Player.Interface;

namespace BattleshipBot.Gameboards
{
    public enum GridResult
    {
        Unknown = ' ',
        Hit = 'H',
        Miss = 'M'
    }

    /*
     * A model of what we perceive to be the enemy board
     */
    public class EnemyBoard
    {
        private char[,] grid;

        private List<int> activeEnemyShipSizes;

        public EnemyBoard()
        {
            grid = new char[10,10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = (char)GridResult.Unknown;
                }
            }

            activeEnemyShipSizes = new List<int> {5, 4, 3, 3, 2};
        }

        public void UpdateBoard(IGridSquare square, bool wasHit)
        {
            int gridX = square.Column - 1;
            int gridY = (int) (square.Row - 'A');

            grid[gridX, gridY] = wasHit ? (char) GridResult.Hit : (char) GridResult.Miss;
        }

        public bool IsTestedSquare(Point point)
        {
            return grid[point.X, point.Y] != (char)GridResult.Unknown;
        }

        public override string ToString()
        {
            StringBuilder gridString = new StringBuilder();

            gridString.Append("[ ][1][2][3][4][5][6][7][8][9][0]\n");
            for (int j = 0; j < 10; j++)
            {
                gridString.Append("[" + GameBoard.GridRefs.ToCharArray()[j] + "]");
                for (int i = 0; i < 10; i++)
                {
                    gridString.Append("[" + grid[i,j] + "]");
                }
                gridString.Append("\n");
            }

            return gridString.ToString();
        }

        public int GetActiveShipMaxSize()
        {
            return activeEnemyShipSizes[0];
        }

        public void RemoveEnemyShip(int size)
        {
            activeEnemyShipSizes.Remove(size);
        }
    }
}
