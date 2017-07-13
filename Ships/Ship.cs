using System.Xml.Serialization;
using Battleships.Player.Interface;

namespace BattleshipBot.Ships
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    abstract  class Ship
    {
        private readonly int size;

        private int headX;
        private int headY;
        private Orientation orientation;

        protected Ship(int shipSize)
        {
            size = shipSize;
        }

        public void SetPosition(int x, int y, Orientation shipOrientation)
        {
            headX = x;
            headY = y;
            orientation = shipOrientation;
        }

        public ShipPosition GetPosition()
        {
            int endX = headX;
            int endY = headY;

            if (orientation == Orientation.Horizontal)
                endX = headX + size - 1;
            else
                endY = headY + size - 1;

            ShipPosition pos = new ShipPosition(new GridSquare(ConvertPosToChar(headY), headX + 1), new GridSquare(ConvertPosToChar(endY), endX + 1));
            return pos;
        }

        private char ConvertPosToChar(int pos)
        {
            return GameBoard.GridRefs.ToCharArray()[pos];
        }

        public int GetSize()
        {
            return size;
        }

        public Orientation GetOrientation()
        {
            return orientation;
        }

        public override string ToString()
        {
            int endX = headX;
            int endY = headY;

            if (orientation == Orientation.Horizontal)
                endX = headX + size - 1;
            else
                endY = headY + size - 1;

            return "SHIP SIZE: " + size + " AT (" + (headX + 1) + "," + ConvertPosToChar(headY) + ") TO (" + (endX + 1) + "," + ConvertPosToChar(endY) + ")";
        }
    }
}
