namespace MyLab.Effects
{
    /// <summary>
    /// Filters an image by removing all the pixels below a certain threshold
    /// </summary>
    public class HighPassFilter : Filter
    {
        public override string FilterName => "High Pass filter";
        public override int SourcesCount => 1;

        public float Threshold { get; set; } = 150;

        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            Color pixel = sources[0].GetColor(x, y);
            double luminance = (0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);

            if (luminance < Threshold)
                return new Color();
            return pixel;
        }
    }
}