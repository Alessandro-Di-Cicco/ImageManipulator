using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Applies a posterization effect by reducing the number of bits per pixel
    /// </summary>
    public class Posterize : Filter
    {
        public override string FilterName => "Posterize";
        public override int SourcesCount => 1;

        private int posterizeLevel = 4;
        /// <summary>
        /// The number of bits to remove from the pixels (from each color of a pixel) [1 to 8]
        /// </summary>
        /// <value></value>
        public int PosterizeLevel
        {
            get => posterizeLevel;
            // Removing either 0 or all 8 bits doesn't make sense
            set {
                if (value < 1 || value > 7)
                    throw new ArgumentOutOfRangeException();
                posterizeLevel = value;
            }
        }

        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            Color pixel = sources[0].GetColor(x, y);
            pixel.R = (byte) ((pixel.R >> PosterizeLevel) << PosterizeLevel);
            pixel.G = (byte) ((pixel.G >> PosterizeLevel) << PosterizeLevel);
            pixel.B = (byte) ((pixel.B >> PosterizeLevel) << PosterizeLevel);
            return pixel;
        }
    }
}