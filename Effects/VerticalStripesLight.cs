using System;
using System.Collections.Generic;

namespace MyLab.Effects
{
    public class VerticalStripesLight : Filter
    {
        public override string FilterName => "Vertical stripes";
        public override int SourcesCount => 2;

        public int LineThickness {get; set;} = 30;
        public int LineNumber {get; set;} = 8;
        public int StopThreshold {get; set;} = 30;
        public int StopWindowCount {get; set;} = 6;
        public float StripeColorScale {get; set;} = 1.3f;

        private List<int> xCoords;

        public VerticalStripesLight() => forceSync = true;

        protected override void OnApplication(ReadOnlyByteImage[] sources)
        {
            Random generator = new Random();
            xCoords = new List<int>();
            for (int i = 0; i < LineNumber; i++)
            {
                int stripeWidth = generator.Next(1, LineThickness);
                int xPos = generator.Next(sources[0].Width - stripeWidth);
                for (int j = 0; j < stripeWidth; j++)
                    xCoords.Add(xPos + j);
            }
        }

        protected override Color Application(ReadOnlyByteImage[] sources, int x, int y)
        {
            Color pixel = sources[0].GetColor(x, y);

            int colIndex;
            if ((colIndex = OnLine(x)) == -1) return pixel;

            if (ObstacleAt(sources[1], x, y))
            {
                xCoords.RemoveAt(colIndex);
                return pixel;
            }
                        
            pixel = StripePixel(pixel);
            return pixel;
        }

        protected virtual Color StripePixel(Color pixel)
        {
            pixel.R = (byte) Math.Clamp(pixel.R * StripeColorScale, 0, 255);
            pixel.G = (byte) Math.Clamp(pixel.G * StripeColorScale, 0, 255);
            pixel.B = (byte) Math.Clamp(pixel.B * StripeColorScale, 0, 255);

            return pixel;
        }

        private bool ObstacleAt(ReadOnlyByteImage image, int x, int y)
        {
            Color pixel = image.GetColor(x, y);
            return pixel.R >= StopThreshold || pixel.G >= StopThreshold || pixel.B >= StopThreshold;
        }

        private int OnLine(int x)
        {
            return xCoords.FindIndex(xCoord => x == xCoord);
        }
    }
}