using ImageMagick;

namespace MyLab
{
    /// <summary>
    /// An extension of the ReadOnlyByteImage that provides write-operations
    /// </summary>
    public class ByteImage : ReadOnlyByteImage
    {
        public ByteImage(byte[] data, int width, int height) : base(data, width, height) {}

        /// <summary> Copies the original read only image </summary>
        public ByteImage(ReadOnlyByteImage source) : base(source.GetCopy(), source.Width, source.Height) { }

        public void SetPixel(Color color, int x, int y)
        {
            int baseIndex = GetIndex(x, y);

            imageData[baseIndex] = color.R;
            imageData[baseIndex + 1] = color.G;
            imageData[baseIndex + 2] = color.B;
        }

        public void WriteImage(IMagickImage<byte> image) => image.GetPixels().SetPixels(imageData);

        /// <returns>The represented image as a IMagickImage</returns>
        public IMagickImage<byte> CreateImage()
        {
            MagickImageFactory imageFactory = new MagickImageFactory();
            IMagickImage<byte> image = imageFactory.Create(MagickColor.FromRgb(255, 255, 255), Width, Height);
            WriteImage(image);
            return image;
        }
    }
}