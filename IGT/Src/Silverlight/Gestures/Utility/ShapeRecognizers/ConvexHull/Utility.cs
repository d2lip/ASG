
using System.Windows;
namespace ShapeRecognizers.ConvexHull
{
    public static class Utility
    {
        public static bool Equals(this Point self, Point point)
        {
            return (self.X == point.X && self.Y == point.Y);
        }

        public static bool Less(this Point self, Point point)
        {
            return (self.X < point.X || self.X == point.X && self.Y < point.Y);
        }

        // Twice the signed area of the triangle (p0, p1, p2)
        public static double Area(Point p0, Point p1, Point p2)
        {
            return (p0.X * (p1.Y - p2.Y) + p1.X * (p2.Y - p0.Y) + p2.X * (p0.Y - p1.Y));
        }
    }
}
