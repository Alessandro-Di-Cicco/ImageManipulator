using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Reduces the numbers of bits of each pixel for every pixel below the given luminance threshold
    /// </summary>
    public class PosterizeDark : Posterize
    {
        private float luminanceThreshold;

        public override string FilterName => "Posterize dark";

        public float LuminanceThreshold
        {
            get => luminanceThreshold;
            set {
                if (value < 0 || value > 255)
                    throw new ArgumentOutOfRangeException();
                luminanceThreshold = value;
            }
        }

        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            Color pixel = sources[0].GetColor(x, y);
            float luminance = 0.2126f * pixel.R + 0.7152f * pixel.G + 0.0722f * pixel.B;
            
            if (luminance > LuminanceThreshold) return pixel;

            return base.Application(sources, x, y);
        }
    }
}