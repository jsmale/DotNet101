using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet101.Tests
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void InitializeClass()
        {
            var point = new PointClass(5, 7);

            Assert.AreEqual(5, point.X, "X did not match");
            Assert.AreEqual(7, point.Y, "Y did not match");
        }

        [TestMethod]
        public void ClassEquality()
        {
            var point = new PointClass(5, 7);
            var point2 = new PointClass(5, 7);

            Assert.AreEqual(point, point2);
        }

        [TestMethod]
        public void ClassAssignment()
        {
            var point = new PointClass(5, 7);
            var point2 = point;

            Assert.AreEqual(point, point2);

            point2.X = 10;

            Assert.AreEqual(10, point.X);
        }

        [TestMethod]
        public void StructEquality()
        {
            var point = new PointStruct(5, 7);
            var point2 = new PointStruct(5, 7);

            Assert.AreEqual(point, point2);
        }

        [TestMethod]
        public void StructAssignment()
        {
            var point = new PointStruct(5, 7);
            var point2 = point;

            Assert.AreEqual(point, point2);

            point2.X = 10;

            Assert.AreEqual(10, point2.X);
            Assert.AreEqual(5, point.X);
        }

        [TestMethod]
        public void OutParameters()
        {
            var result = Parse("4.2");
            Assert.IsTrue(result.Success, "Could not parse decimal");
            Assert.AreEqual(4.2m, result.Value, "Decimal value is incorrect");
        }

        struct DecimalParseResult
        {
            public DecimalParseResult(decimal value, bool success)
            {
                Value = value;
                Success = success;
            }

            public decimal Value { get; }
            public bool Success { get; }
        }

        private DecimalParseResult Parse(string s)
        {
            decimal value;
            var result = decimal.TryParse("4.2", out value);
            return new DecimalParseResult(value, result);
        }

        [TestMethod]
        public void TupleTest()
        {
            var result = ParseTuple("4.2");
            Assert.IsTrue(result.Item1, "Could not parse decimal");
            Assert.AreEqual(4.2m, result.Item2, "Decimal value is incorrect");
        }

        private Tuple<bool, decimal> ParseTuple(string s)
        {
            decimal value;
            var result = decimal.TryParse(s, out value);
            return new Tuple<bool, decimal>(result, value);
        }

        [TestMethod]
        public void PointAddition()
        {
            var point1 = new PointStruct(5, 7);
            var point2 = new PointStruct(3, 5);
            {
                var point4 = new PointStruct(4, 5);

                Assert.AreEqual(4, point4.X);
                Assert.AreEqual(3, point2.X);
            }
            
            PointStruct point3 = point1 + point2;
            
            Assert.AreEqual(8, point3.X);
            Assert.AreEqual(12, point3.Y);
        }

        [TestMethod]
        public void PointMultiply()
        {
            var point1 = new PointStruct(5, 7);
            PointStruct point3 = point1 * 2;

            Assert.AreEqual(10, point3.X);
            Assert.AreEqual(14, point3.Y);
        }

        [TestMethod]
        public void PointEqualsOperator()
        {
            var point = new PointStruct(5, 7);
            var point2 = point;

            Assert.AreEqual(point, point2);
            Assert.IsTrue(point == point2);
            Assert.IsFalse(point != point2);

            point2.X = 8;
            Assert.IsTrue(point != point2);
        }

        [TestMethod]
        public void PointMethods()
        {
            var point = new PointStruct(5, 7);
            var box = new Rectangle(
                new PointStruct(2, 4),
                new PointStruct(7, 9)
                );

            var result = point.InBox(box);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RectangleArea()
        {
            IShape box = new Rectangle(
                new PointStruct(2, 4),
                new PointStruct(7, 9)
                );

            Assert.AreEqual(25, box.Area);
        }

        [TestMethod]
        public void SquareArea()
        {
            IShape box = new Square(new PointStruct(2, 4), 7);

            Assert.AreEqual(49, box.Area);
        }

        [TestMethod]
        public void SquareInBox()
        {
            Rectangle box = new Square(new PointStruct(2, 4), 7);

            Assert.IsTrue(new PointStruct(4, 6).InBox(box));
        }

        [TestMethod]
        public void SquareWidthEqualsHight()
        {
            IHasWidthAndHeight box = new Square(new PointStruct(2, 4), 7);

            Assert.AreEqual(box.Width, box.Height);
        }

        [TestMethod]
        public void JsonPoint()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new PointStruct(5, 6));

            Assert.AreEqual("{\"X\":5,\"Y\":6}", json);
        }
    }
}