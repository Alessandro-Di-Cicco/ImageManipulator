using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Adaptation of the Vertical Stripes filter that makes the lines negate the pixels
    /// </summary>
    public class VerticalStripesNegative : VerticalStripesLight
    {
        private float negativeScale;

        public float NegativeScale
        {
            get => negativeScale; set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                negativeScale = value;
            }
        }

        protected override Color StripePixel(Color pixel)
        {
            pixel.R = (byte)(255 - pixel.R * NegativeScale);
            pixel.G = (byte)(255 - pixel.G * NegativeScale);
            pixel.B = (byte)(255 - pixel.B * NegativeScale);

            return pixel;
        }
    }
}