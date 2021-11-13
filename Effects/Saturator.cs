using System;
using ImageMagick;

namespace MyLab.Effects
{
    /// <summary>
    /// Alters the saturation of the image
    /// </summary>
    public class Saturator : Filter
    {
        public override string FilterName => "Saturation";
        public override int SourcesCount => 1;

        public float Scale { get; set; } = 20;

        /// <summary>
        /// Uses data structures provided by the ImageMagick library to convert from
        /// RGB to HSV and vice versa
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            Color output = new Color();
            Color pixel = sources[0].GetColor(x, y);

            ColorHSV hsv = ColorHSV.FromMagickColor(new MagickColor(pixel.R, pixel.G, pixel.B));
            hsv.Saturation = Math.Clamp(hsv.Saturation * Scale, 0, 1);
            IMagickColor<byte> rgb = hsv.ToMagickColor();
            output.R = rgb.R;
            output.G = rgb.G;
            output.B = rgb.B;

            return output;
        }
    }
}