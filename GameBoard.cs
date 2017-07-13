using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipBot.Ships;

namespace BattleshipBot
{
    class GameBoard
    {
        public static string GridRefs = "ABCDEFGHIJ";
        private bool[,] grid;

        Random rng = new Random();

        public GameBoard()
        {
            grid = new bool[10, 10];
        }

        public void AddShipToRandomPosition(Ship ship)
        {
            bool placedShip = false;
            while (!placedShip)
            {
                int vertical = rng.Next(2);
                if (vertical == 1)
                {
                    placedShip = TryAddShipVertical(ship);
                }
                else
                {
                    placedShip = TryAddShipHorizontal(ship);
                }
            }
        }

        private bool TryAddShipVertical(Ship ship)
        {
            int testX = rng.Next(10);
            int testY = rng.Next(10 - ship.GetSize());

            if (CanShipFitVertical(testX, testY, ship))
            {
                PlaceShip(testX, testY, ship, Orientation.Vertical);
                UpdateGrid(testX, testY, ship);
                return true;
            }
            else
            {
                Console.WriteLine("Cannae Do it Cap'n");
            }

            return false;
        }

        private bool TryAddShipHorizontal(Ship ship)
        {
            int testX = rng.Next(10 - ship.GetSize());
            int testY = rng.Next(10);

            if (CanShipFitHorizontal(testX, testY, ship))
            {
                PlaceShip(testX, testY, ship, Orientation.Horizontal);
                UpdateGrid(testX, testY, ship);
                return true;
            }
            else
            {
                Console.WriteLine("Cannae Do it Cap'n");
            }

            return false;
        }

        private bool CanShipFitVertical(int x, int y, Ship ship)
        {
            for (int j = y; j < y + ship.GetSize(); j++)
            {
                if (grid[x, j])
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanShipFitHorizontal(int x, int y, Ship ship)
        {
            for (int i = x; i < x + ship.GetSize(); i++)
            {
                if (grid[i, y])
                {
                    return false;
                }
            }
            return true;
        }

        private void PlaceShip(int x, int y, Ship ship, Orientation orientation)
        {
            ship.SetPosition(x, y, orientation);
        }

        private void UpdateGrid(int x, int y, Ship ship)
        {
            if (ship.GetOrientation() == Orientation.Horizontal)
            {
                for (int i = x - 1; i <= x + ship.GetSize(); i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (PointOnGrid(i, j))
                        {
                            grid[i, j] = true;
                        }
                    }
                }
            }
            else
            {
                for (int j = y - 1; j <= y + ship.GetSize(); j++)
                {
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        if (PointOnGrid(i, j))
                        {
                            grid[i, j] = true;
                        }
                    }
                }
            }
        }

        private bool PointOnGrid(int x, int y)
        {
            bool xInGrid = x >= 0 && x < 10;
            bool yInGrid = y >= 0 && y < 10;
            return xInGrid && yInGrid;
        }

        public override string ToString()
        {
            StringBuilder gridString = new StringBuilder();

            gridString.Append("[ ][1][2][3][4][5][6][7][8][9][0]\n");
            for (int j = 0; j < 10; j++)
            {
                gridString.Append("[" + GridRefs.ToCharArray()[j] + "]");
                for (int i = 0; i < 10; i++)
                {
                    gridString.Append("[" + (grid[i,j] ? 'X' : ' ') + "]");
                }
                gridString.Append("\n");
            }

            return gridString.ToString();
        }
    }
}
