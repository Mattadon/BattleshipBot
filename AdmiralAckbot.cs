using System;
using System.Collections.Generic;
using BattleshipBot.Gameboards;
using BattleshipBot.Ships;
using Battleships.Player.Interface;

namespace BattleshipBot
{
    public enum BotStates
    {
        Searching,
        Attacking
    }

    public class AdmiralAckbot : IBattleshipsBot
    {
        private IGridSquare lastTarget;

        private BotStates state;

        private GameBoard ourBoard;
        private EnemyBoard enemyBoard;

        //The first hit on the previous ship attacked
        private EnemyShip previousShipAttacked;

        private Random rng;

        public AdmiralAckbot()
        {
            state = BotStates.Searching;
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

        public GameBoard GetBoard()
        {
            return ourBoard;
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            lastTarget = nextTarget;
            return nextTarget;
        }

        private IGridSquare GetNextTarget()
        {
            if (state == BotStates.Searching)
            {
                return PickRandomTarget();
            }
            else
            {
                return AttackShip();
            }
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

            while (enemyBoard.IsTestedSquare(column, row))
            {
                column = (column + 2) % 10;

                if (column == firstColumnInRow)
                {
                    row = (row + 1) % 10;
                    column = (column + 1) % 10;
                    firstColumnInRow = column;
                }

                //Complete loop, therefore shift to other coloured tiles
                //This will eventually be redundant, as matches will end before all options explored
                //But in the half-finished state, we need to avoid infinite loops
                if (column == firstX && row == firstY)
                {
                    column = (column + 1) % 10;
                }
            }

            IGridSquare gridSquare = new GridSquare(GameBoard.GridRefs.ToCharArray()[row], column + 1);

            return gridSquare;
        }

        private IGridSquare AttackShip()
        {
            if (previousShipAttacked.HasUnexploredPointsAroundSeed())
            {
                return previousShipAttacked.GetNextTarget();
            }
            else
            {
                state = BotStates.Searching;
                return PickRandomTarget();
            }
        }


        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            if (state == BotStates.Searching && wasHit)
            {
                state = BotStates.Attacking;
                previousShipAttacked = new EnemyShip(square);

                Console.WriteLine("FOUND A SHIP");
            }
            else if (state == BotStates.Attacking)
            {
                previousShipAttacked.UpdateBasedOnShotResult(square, wasHit);    
            }

            enemyBoard.UpdateBoard(square, wasHit);
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            // Ignore what our opponent does
        }

        public string Name => "Admiral Ackbot";
    }
}
