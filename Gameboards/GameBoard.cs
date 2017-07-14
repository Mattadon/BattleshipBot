using System;
using System.Text;
using BattleshipBot.Ships;
using Battleships.Player.Interface;

namespace BattleshipBot.Gameboards
{
    public class GameBoard
    {
        public static string GridRefs = "ABCDEFGHIJ";
        private bool[,] grid;

        Random rng = new Random();

        public GameBoard()
        {
            grid = new bool[10, 10];
        }

        public GameBoard(bool[,] board)
        {
            grid = board;
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
            Point test = new Point(rng.Next(10), rng.Next(10 - ship.GetSize()));

            if (CanShipFitVertical(test, ship))
            {
                PlaceShip(test, ship, Orientation.Vertical);
                UpdateGrid(test, ship);
                return true;
            }

            return false;
        }

        private bool TryAddShipHorizontal(Ship ship)
        {
            Point test = new Point(rng.Next(10 - ship.GetSize()), rng.Next(10));

            if (CanShipFitHorizontal(test, ship))
            {
                PlaceShip(test, ship, Orientation.Horizontal);
                UpdateGrid(test, ship);
                return true;
            }

            return false;
        }

        private bool CanShipFitVertical(Point head, Ship ship)
        {
            for (int j = head.Y; j < head.Y + ship.GetSize(); j++)
            {
                if (grid[head.X, j])
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanShipFitHorizontal(Point head, Ship ship)
        {
            for (int i = head.X; i < head.X + ship.GetSize(); i++)
            {
                if (grid[i, head.Y])
                {
                    return false;
                }
            }
            return true;
        }

        private void PlaceShip(Point head, Ship ship, Orientation orientation)
        {
            ship.SetPosition(head, orientation);
        }

        private void UpdateGrid(Point head, Ship ship)
        {
            if (ship.GetOrientation() == Orientation.Horizontal)
            {
                for (int i = head.X - 1; i <= head.X + ship.GetSize(); i++)
                {
                    for (int j = head.Y - 1; j <= head.Y + 1; j++)
                    {
                        if (IsPointOnGrid(new Point(i , j)))
                        {
                            grid[i, j] = true;
                        }
                    }
                }
            }
            else
            {
                for (int j = head.Y - 1; j <= head.Y + ship.GetSize(); j++)
                {
                    for (int i = head.X - 1; i <= head.X + 1; i++)
                    {
                        if (IsPointOnGrid(new Point(i, j)))
                        {
                            grid[i, j] = true;
                        }
                    }
                }
            }
        }

        protected bool IsPointOnGrid(Point p)
        {
            bool xInGrid = p.X >= 0 && p.X < 10;
            bool yInGrid = p.Y >= 0 && p.Y < 10;
            return xInGrid && yInGrid;
        }

        public bool TargetIsShip(IGridSquare square)
        {
            Point target = Utils.ConvertGridSquareToPoint(square);

            return grid[target.X, target.Y];
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
                    gridString.Append("[" + (grid[i, j] ? 'X' : ' ') + "]");
                }
                gridString.Append("\n");
            }

            return gridString.ToString();
        }
    }
}
