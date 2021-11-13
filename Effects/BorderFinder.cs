using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Filter that returns a mask based on the contrast of a pixel with its neighbors.
    /// Requires the image to mask and a blurred version of it
    /// </summary>
    public class BorderFinder : Filter
    {
        public override string FilterName => "Border finder";
        public override int SourcesCount => 2;

        private float colorMultiplier;
        /// <summary>
        /// The amount of amplification applied to the outlines
        /// </summary>
        /// <value></value>
        public float ColorMultiplier { get => colorMultiplier; set => colorMultiplier = value; }

        private float minLuminance = 150;
        /// <summary>
        /// The minimum luminance of outline pixels, output pixels that have a luminance below this threshold
        /// are replaced by black
        /// </summary>
        public float MinLuminance
        {
            get => minLuminance;
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Luminance threshold cannot be a negative value");
                if (value >= maxLuminance)
                    throw new ArgumentOutOfRangeException("min luminance must be lower than max luminance");

                minLuminance = value;
            }
        }

        private float maxLuminance = 180;
        /// <summary>
        /// The maximum luminance of outline pixels, output pixels that have a luminance above this threshold
        /// are replaced by black
        /// </summary>
        /// <value></value>
        public float MaxLuminance
        {
            get => maxLuminance;
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Luminance threshold cannot be a negative value");
                if (value <= minLuminance)
                    throw new ArgumentOutOfRangeException("Max luminance must be greater than min luminance");

                maxLuminance = value;
            }
        }

        // Subtracts the blurred pixel from each corresponding pixel in the original image
        protected override Color Application(ReadOnlyByteImage[] images, int x, int y)
        {
            Color color = new Color();
            color.R = (byte) Math.Clamp((images[0].GetR(x, y) - images[1].GetR(x, y)) * ColorMultiplier, 0, 255);
            color.G = (byte) Math.Clamp((images[0].GetG(x, y) - images[1].GetG(x, y)) * ColorMultiplier, 0, 255);
            color.B = (byte) Math.Clamp((images[0].GetB(x, y) - images[1].GetB(x, y)) * ColorMultiplier, 0, 255);

            double luminance = (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);

            if (luminance <= MinLuminance || luminance > MaxLuminance)
                return new Color();
            return color;
        }
    }
}