using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipBot.Gameboards;
using Battleships.Player.Interface;

namespace BattleshipBot.Ships
{
    class EnemyShip
    {
        private readonly Point seed;

        private readonly int maxSize;
        private int sizeDetected;

        /*
         * Where x is the seed:
         *        ...
         *         o
         * o o o o X o o o o
         *         o
         *        ... (etc up and down)
         * We will eliminate points from this list as we shoot around the ship
         */
        private readonly List<Point> possibleTilesAroundSeed;

        public EnemyShip(IGridSquare seedSquare, EnemyBoard enemyBoard)
        {
            maxSize = enemyBoard.GetActiveShipMaxSize();
            sizeDetected = 1;

            seed = Utils.ConvertGridSquareToPoint(seedSquare);

            bool addDown = true;
            bool addRight = true;
            bool addUp = true;
            bool addLeft = true;

            possibleTilesAroundSeed = new List<Point>();
            for(int dist = 1; dist <= 4; dist++)
            {
                Point down = seed.Plus(new Point(0, dist));
                if (IsPointOnGrid(down) && enemyBoard.IsTestedSquare(down))
                    addDown = false;

                if (addDown && IsPointOnGrid(down))
                    possibleTilesAroundSeed.Add(down);

                Point right = seed.Plus(new Point(dist, 0));
                if (IsPointOnGrid(right) && enemyBoard.IsTestedSquare(right))
                    addRight = false;

                if (addRight && IsPointOnGrid(right))
                    possibleTilesAroundSeed.Add(right);

                Point up = seed.Plus(new Point(0, -dist));
                if (IsPointOnGrid(up) && enemyBoard.IsTestedSquare(up))
                    addUp = false;

                if (addUp && IsPointOnGrid(up))
                    possibleTilesAroundSeed.Add(up);

                Point left = seed.Plus(new Point(-dist, 0));
                if (IsPointOnGrid(left) && enemyBoard.IsTestedSquare(left))
                    addLeft = false;

                if (addLeft && IsPointOnGrid(left))
                    possibleTilesAroundSeed.Add(left);
            }
        }

        protected bool IsPointOnGrid(Point p)
        {
            bool xInGrid = p.X >= 0 && p.X < 10;
            bool yInGrid = p.Y >= 0 && p.Y < 10;
            return xInGrid && yInGrid;
        }

        public IGridSquare GetNextTarget()
        {
            Point nextPoint = possibleTilesAroundSeed[0];

            IGridSquare square = Utils.ConvertPointToGridSquare(nextPoint);

            possibleTilesAroundSeed.RemoveAt(0);

            return square;
        }

        public int GetSizeOfDestroyedShip()
        {
            return sizeDetected;
        }

        public bool IsNotDestroyed()
        {
            bool hitLessThanMaxSize = sizeDetected < maxSize;
            bool allOptionsNotExplored = possibleTilesAroundSeed.Count > 0;

            return hitLessThanMaxSize && allOptionsNotExplored;
        }

        public void UpdateBasedOnShotResult(IGridSquare shot, bool wasHit)
        {
            Point shotPoint = Utils.ConvertGridSquareToPoint(shot);

            if (!wasHit)
            {
                possibleTilesAroundSeed.RemoveAll(point => ShouldRemovePointsBeyondMiss(point, shotPoint));
            }
            else
            {
                possibleTilesAroundSeed.RemoveAll(point => ShouldRemovePointsNoInShipDirection(point, shotPoint));
                sizeDetected++;
            }
        }

        private bool ShouldRemovePointsBeyondMiss(Point testPoint, Point missPoint)
        {
            Console.Write("Removing points:");
            //Remove point if beyond the miss point
            //Therefore, absolute values the same and magnitude the same
            if (missPoint.X == testPoint.X) //Test on Y direction
            {
                int testRealtiveY = testPoint.Y - seed.Y;
                int missRelativeY = missPoint.Y - seed.Y;
                bool testPointFurtherOut = Math.Abs(testRealtiveY) > Math.Abs(missRelativeY);
                bool testPointSameDirection = Math.Sign(testRealtiveY) == Math.Sign(missRelativeY);
                if (testPointFurtherOut && testPointSameDirection)
                {
                    Console.Write("(" + (testPoint.X + 1) + ", " + GameBoard.GridRefs.ToCharArray()[testPoint.Y] + ")\n");
                    return true;
                }
            }
            else if (missPoint.Y == testPoint.Y)
            {
                int testRelativeX = testPoint.X - seed.X;
                int missRelativeX = missPoint.X - seed.X;
                bool testPointFurtherOut = Math.Abs(testRelativeX) > Math.Abs(missRelativeX);
                bool testPointSameDirection = Math.Sign(testRelativeX) == Math.Sign(missRelativeX);
                if (testPointFurtherOut && testPointSameDirection)
                {
                    Console.Write("(" + (testPoint.X + 1) + ", " + GameBoard.GridRefs.ToCharArray()[testPoint.Y] + ")\n");
                    return true;
                }
            }
            Console.WriteLine();

            return false;
        }

        private bool ShouldRemovePointsNoInShipDirection(Point testPoint, Point hitPoint)
        {
            if (hitPoint.X == seed.X) //Then ship is vertical
            {
                return testPoint.X != seed.X;
            }
            else
            {
                return testPoint.Y != seed.Y;
            }
        }
    }
}
