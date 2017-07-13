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
        
            Carrier monCalCarrier = new Carrier();
            Battleship monCalCCruiser = new Battleship();
            Destroyer starDestroyer = new Destroyer();
            Frigate hammerHeadFrigate = new Frigate();
            PatrolBoat corellianCorvette = new PatrolBoat();

            board.AddShipToRandomPosition(monCalCarrier);
            board.AddShipToRandomPosition(monCalCCruiser);
            board.AddShipToRandomPosition(starDestroyer);
            board.AddShipToRandomPosition(hammerHeadFrigate);
            board.AddShipToRandomPosition(corellianCorvette);

            return new List<IShipPosition>
            {
                monCalCarrier.GetPosition(),
                monCalCCruiser.GetPosition(),
                starDestroyer.GetPosition(),
                hammerHeadFrigate.GetPosition(),
                corellianCorvette.GetPosition()
            };
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
