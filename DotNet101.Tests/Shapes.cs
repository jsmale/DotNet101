namespace DotNet101.Tests
{
    public interface IShape
    {
        int Area { get; }
    }

    public interface IHasWidthAndHeight
    {
        int Width { get; }
        int Height { get; }
    }

    public abstract class Shape : IShape
    {
        public abstract int Area { get; }
    }

    public class Rectangle : Shape, IHasWidthAndHeight
    {
        private readonly PointStruct topRight;
        private readonly PointStruct bottomLeft;

        public Rectangle(PointStruct bottomLeft, PointStruct topRight)
        {
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
        }

        public override int Area => Width * Height;

        public PointStruct TopRight => topRight;

        public PointStruct BottomLeft => bottomLeft;

        public int Width => (topRight.Y - bottomLeft.Y);

        public int Height => (topRight.X - bottomLeft.X);
    }

    public class Square : Rectangle
    {
        public Square(PointStruct bottomLeft, int width) 
            : base(bottomLeft, new PointStruct(bottomLeft.X+width, bottomLeft.Y+width))
        {
        }
    }
}
