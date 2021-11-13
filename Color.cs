namespace MyLab
{
    /// <summary>
    /// Simple struct to contain an RGB triple
    /// </summary>
    public struct Color
    {
        public Color(byte r, byte g, byte b) {
            R = r;
            G = g;
            B = b;
        }

        public byte R;
        public byte G;
        public byte B;
    }
}