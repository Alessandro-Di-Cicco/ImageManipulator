using System;
using System.Diagnostics;
using ImageMagick;
using MyLab.Effects;

#nullable enable
namespace MyLab
{
    /// <summary>
    /// Entry point, used to apply the filters
    /// </summary>
    class Core
    {
        // The main contains an example of usage. It loads an image called castle.jpg from
        // a Castle folder on the desktop
        static void Main(string[] args)
        {
            string filename = "Castle/castle";

            // Loads the image as a ReadOnlyByteImage
            ReadOnlyByteImage originalReadOnly;
            using (MagickImage original = LoadImage($"{filename}.jpg"))
            {
                originalReadOnly = new ReadOnlyByteImage(original.GetPixels().ToArray(), original.Width, original.Height);
            }
            Stopwatch total = new Stopwatch();
            total.Start();

            // Applies blur with a radius of 1
            Blur blur = new Blur(1);
            ByteImage blurredBytes = blur.GetFilteredBytes(new[] {originalReadOnly});
            SaveImage(blurredBytes, $"{filename}_blurred[x{blur.BlurRadius}].jpg");

            // Border finding filter
            BorderFinder borders = new BorderFinder() {ColorMultiplier = 4, MinLuminance = 25, MaxLuminance = 250};
            ByteImage borderBytes = borders.GetFilteredBytes(new[] {originalReadOnly, blurredBytes});
            SaveImage(borderBytes, $"{filename}_outline.jpg");

            // Detail enhancer
            DetailPusher pusher = new DetailPusher(.25f);
            ByteImage pushedBytes = pusher.GetFilteredBytes(new[] {originalReadOnly, borderBytes});
            SaveImage(pushedBytes, $"{filename}_enhanced.jpg");

            // Saturation change
            Saturator saturator = new Saturator {Scale = 1.2f};
            ByteImage saturatedBytes = saturator.GetFilteredBytes(new[] {pushedBytes});
            SaveImage(saturatedBytes, $"{filename}_saturated.jpg");

            // Posterize Dark filter: posterizes dark areas of the image
            PosterizeDark posterize = new PosterizeDark() {PosterizeLevel = 6, LuminanceThreshold = 80};
            ByteImage posterizedBytes = posterize.GetFilteredBytes(new[] {saturatedBytes});
            SaveImage(posterizedBytes, $"{filename}_posterized.jpg");

            // Vertical Stripes filter
            VerticalStripesLight striper = new VerticalStripesLight() {LineThickness = 70, LineNumber = 9, StripeColorScale = 1.25f };
            ByteImage stripedBytes = striper.GetFilteredBytes(new[] {posterizedBytes, borderBytes});
            SaveImage(stripedBytes, $"{filename}_posterized_striped.jpg");

            // Vertical Stripes Negative filter
            VerticalStripesNegative negStriper = new VerticalStripesNegative() {LineThickness = 100, LineNumber = 7, NegativeScale = 1f};
            ByteImage negatedStripedBytes = negStriper.GetFilteredBytes(new[] {posterizedBytes, borderBytes});
            SaveImage(negatedStripedBytes, $"{filename}_posterized_striped_neg.jpg");

            Console.WriteLine($"TOTAL TIME: {total.ElapsedMilliseconds}ms");
        }
        
        static MagickImage LoadImage(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + filename;
            MagickImage image = new MagickImage(path);
            return image;
        }

        static void SaveImage(ByteImage image, string filename) => image.CreateImage().Write(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{filename}");
    }
}