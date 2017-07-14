using System;
using System.Collections.Generic;
using BattleshipBot.Gameboards;
using BattleshipBot.Ships;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    public class AdmiralAckbot : IBattleshipsBot
    {
        private IGridSquare lastTarget;

        private GameBoard ourBoard;
        private EnemyBoard enemyBoard;

        private Random rng;

        public AdmiralAckbot()
        {
            ourBoard = new GameBoard();
            enemyBoard = new EnemyBoard();
            rng = new Random();
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            lastTarget = null; // Forget all our history when we start a new game
        
            List<Ship> allianceFleet = new List<Ship>();
            allianceFleet.Add(new Carrier());
            allianceFleet.Add(new Battleship());
            allianceFleet.Add(new Destroyer());
            allianceFleet.Add(new Frigate());
            allianceFleet.Add(new PatrolBoat());

            foreach (Ship ship in allianceFleet)
            {
                ourBoard.AddShipToRandomPosition(ship);
            }

            List<IShipPosition> shipPositions = new List<IShipPosition>();

            foreach (Ship ship in allianceFleet)
            {
                shipPositions.Add(ship.GetPosition());
            }

            return shipPositions;
        }

        public string DrawBoard()
        {
            return enemyBoard.ToString();
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastTarget = nextTarget;
            return nextTarget;
        }

        private IGridSquare GetNextTarget()
        {
            return PickRandomTarget();
            /*
            if (lastTarget == null)
            {
                return new GridSquare('A', 1);
            }

            var row = lastTarget.Row;
            var col = lastTarget.Column + 1;
            if (lastTarget.Column != 10)
            {
                return new GridSquare(row, col);
            }

            row = (char)(row + 1);
            if (row > 'J')
            {
                row = 'A';
            }
            col = 1;
            return new GridSquare(row, col);
            */
        }

        /*
         * We can cover the whole board by only aiming at half of the squares
         * (In a chess-board pattern)
         * Pick even columns in rows A, C, E...
         * Pick odd columns in rows B, D, F...
         */
        private IGridSquare PickRandomTarget()
        {
            int row = rng.Next(10);
            int column = rng.Next(5) * 2;

            if (row % 2 == 0)
                column++;

            int firstX = column;
            int firstY = row;

            int firstColumnInRow = column;

            Console.WriteLine("Initial attempt at (" + (column + 1) + ", " + GameBoard.GridRefs.ToCharArray()[row] + ")");

            while (enemyBoard.IsTestedSquare(column, row))
            {
                column = (column + 2) % 10;

                Console.WriteLine("Shifting to tile (" + (column + 1) + ", " + GameBoard.GridRefs.ToCharArray()[row] + ")");

                if (column == firstColumnInRow)
                {
                    row = (row + 1) % 10;
                    column = (column + 1) % 10;
                    firstColumnInRow = column;

                    Console.WriteLine("Shifting down to tile (" + (column + 1) + ", " + GameBoard.GridRefs.ToCharArray()[row] + ")");
                }

                //Complete loop, therefore shift to other coloured tiles
                //This will eventually be redundant, as matches will end before all options explored
                //But in the half-finished state, we need to avoid infinite loops
                if (column == firstX && row == firstY)
                {
                    column = (column + 1) % 10;

                    Console.WriteLine("Shifting to next colour tile (" + (column + 1) + ", " + GameBoard.GridRefs.ToCharArray()[row] + ")");
                }
            }

            IGridSquare gridSquare = new GridSquare(GameBoard.GridRefs.ToCharArray()[row], column + 1);
            
            //TODO: Remove before submitting!
            enemyBoard.UpdateBoard(gridSquare, false);

            return gridSquare;
        }


        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            // Ignore whether we're successful
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            // Ignore what our opponent does
        }

        public string Name => "Admiral Ackbot";
    }
}
