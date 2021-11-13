using System;

namespace MyLab.Iterators
{
    /// <summary>
    /// Basic implementation of an image iterator, requires a ByteImage on which to apply the given
    /// functors. Iterations happen from top to bottom, left to right
    /// </summary>
    internal class ImageIterator : IImageIterator
    {
        private ReadOnlyByteImage[] sources;
        private ByteImage target;

        // The dimensions of the "bounding box"
        private Vector2 startingPoint;
        private Vector2 size;

        public ImageIterator(ReadOnlyByteImage[] sourceImages, ByteImage targetImage, Vector2 startPos, Vector2 areaSize)
        {
            sources = sourceImages;
            target = targetImage;
            startingPoint = startPos;
            size = areaSize;
        }

        public void ForEachPixel(Func<ReadOnlyByteImage[], int, int, Color> functor)
        {
            for (int y = startingPoint.Y; y < startingPoint.Y + size.Y; y++)
            {
                for (int x = startingPoint.X; x < startingPoint.X + size.X; x++)
                {
                    Color color = functor(sources, x, y);
                    target.SetPixel(color, x, y);
                }
            }
        }
    }
}