namespace MyLab
{
    /// <summary>
    /// Simple struct to contain a point in 2D space
    /// </summary>
    public struct Vector2
    {
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}