namespace DotNet101.Tests
{
    public struct PointStruct
    {
        public PointStruct(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static PointStruct operator +(PointStruct point1, PointStruct point2)
        {
            return new PointStruct(point1.X+point2.X, point1.Y+point2.Y);
        }

        public static PointStruct operator *(PointStruct point1, int multiplier)
        {
            return new PointStruct(point1.X * multiplier, point1.Y * multiplier);
        }

        public static bool operator ==(PointStruct point1, PointStruct point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(PointStruct point1, PointStruct point2)
        {
            return !point1.Equals(point2);
        }

        public bool InBox(Rectangle box)
        {
            return box.BottomLeft.X <= X
                   && box.TopRight.X >= X
                   && box.BottomLeft.Y <= Y
                   && box.TopRight.Y >= Y;
        }
    }
}