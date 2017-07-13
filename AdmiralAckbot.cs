using System;
using System.Collections.Generic;
using BattleshipBot.Ships;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    public class AdmiralAckbot : IBattleshipsBot
    {
        private IGridSquare lastTarget;

        GameBoard board = new GameBoard();

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
                board.AddShipToRandomPosition(ship);
            }

            List<IShipPosition> shipPositions = new List<IShipPosition>();

            foreach (Ship ship in allianceFleet)
            {
                shipPositions.Add(ship.GetPosition());
                Console.WriteLine(ship);
            }

            return shipPositions;
        }

        public string DrawBoard()
        {
            return board.ToString();
        }

        private static ShipPosition GetShipPosition(char startRow, int startColumn, char endRow, int endColumn)
        {
            return new ShipPosition(new GridSquare(startRow, startColumn), new GridSquare(endRow, endColumn));
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastTarget = nextTarget;
            return nextTarget;
        }

        private IGridSquare GetNextTarget()
        {
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
