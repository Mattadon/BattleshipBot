using System.Xml.Serialization;
using BattleshipBot.Gameboards;
using Battleships.Player.Interface;

namespace BattleshipBot.Ships
{
    public enum Orientation
    {
        Horizontal,
        Vertical,
        Unknown
    }

    abstract public class Ship
    {
        private readonly int size;

        private Point head;
        private Orientation orientation;

        protected Ship(int shipSize)
        {
            size = shipSize;
        }

        public void SetPosition(Point point, Orientation shipOrientation)
        {
            head = point;
            orientation = shipOrientation;
        }

        public ShipPosition GetPosition()
        {
            Point end = new Point(head);

            if (orientation == Orientation.Horizontal)
                end.X = head.X + size - 1;
            else
                end.Y = head.Y + size - 1;

            ShipPosition pos = new ShipPosition(Utils.ConvertPointToGridSquare(head), Utils.ConvertPointToGridSquare(end));
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
            Point end = new Point(head);

            if (orientation == Orientation.Horizontal)
                end.X = head.X + size - 1;
            else
                end.Y = head.Y + size - 1;

            return "SHIP SIZE: " + size + " AT (" + (head.X + 1) + "," + ConvertPosToChar(head.Y) + ") TO (" + (end.X + 1) + "," + ConvertPosToChar(end.Y) + ")";
        }
    }
}
