using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Blur filter, replaces every pixel with the average of its neighboring pixels within
    /// an n*n window. This filter can run asynchonously
    /// </summary>
    public class Blur : Filter
    {
        private int blurRadius;

        public override string FilterName => "Blur";
        public override int SourcesCount => 1;
        public int BlurRadius
        {
            get => blurRadius; set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Blur radius must be a positive integer");
                blurRadius = value;
            }
        }

        public Blur(int radius = 1) : base() => BlurRadius = radius;

        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            int[] sums = new int[3];
            // It's float to avoid integer division at the end
            float counted = 0;

            // Rightmost columns out of bounds is managed in the loop condition
            for (int xOff = -blurRadius; xOff <= blurRadius && x + xOff < sources[0].Width; xOff++)
            {
                // Leftmost columns out of bounds
                if (x + xOff < 0) continue;

                // Bottom rows out of bounds is managed in the loop condition
                for (int yOff = -blurRadius; yOff <= blurRadius && y + yOff < sources[0].Height; yOff++)
                {
                    // Starting rows out of bounds
                    if (y + yOff < 0) continue;

                    sums[0] += sources[0].GetR(x + xOff, y + yOff);
                    sums[1] += sources[0].GetG(x + xOff, y + yOff);
                    sums[2] += sources[0].GetB(x + xOff, y + yOff);
                    counted++;
                }
            }

            Color color = new Color();

            color.R = (byte)(sums[0] / counted);
            color.G = (byte)(sums[1] / counted);
            color.B = (byte)(sums[2] / counted);

            return color;
        }
    }
}