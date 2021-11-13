using System;
using System.Threading.Tasks;

namespace MyLab.Iterators
{
    /// <summary>
    /// Implementation of IImageIterator that performs in parallel applications.
    /// It leverages simple ImageIterator objects to distribute the load, running each
    /// one of them on a separate task.
    /// </summary>
    internal class DistributedImageIterator : IImageIterator
    {
        private ReadOnlyByteImage[] sources;
        private ByteImage target;
        private Vector2 startingPoint;
        private Vector2 size;

        public DistributedImageIterator(ReadOnlyByteImage[] sourceImages, ByteImage targetImage, Vector2 startPos, Vector2 areaSize)
        {
            sources = sourceImages;
            target = targetImage;
            startingPoint = startPos;
            size = areaSize;
        }

        /*
        * This method simply waits for all the asynchronous operations to run, hiding
        * any implementation details from interface users
        */
        public void ForEachPixel(Func<ReadOnlyByteImage[], int, int, Color> functor)
        {
            Task.Run(() => ForEachPixelAsync(functor)).Wait();
        }

        /// <summary>
        /// Splits the load of operations among 4 tasks, by dividing the boundaries in 4
        /// equal parts and assigning them to 4 simple ImageIterator objects, run in parallel
        /// </summary>
        /// <param name="functor"></param>
        /// <returns></returns>
        private async Task ForEachPixelAsync(Func<ReadOnlyByteImage[], int, int, Color> functor)
        {
            int halfWidth = size.X / 2;
            int halfHeight = size.Y / 2;

            int otherWidth = size.X % 2 != 0 ? halfWidth + 1 : halfWidth;
            int otherHeight = size.Y % 2 != 0 ? halfHeight + 1 : halfHeight;

            // Creation of the 4 iterators
            ImageIterator first = new ImageIterator(sources, target, new Vector2(startingPoint.X, startingPoint.Y), new Vector2(halfWidth, halfHeight));
            ImageIterator second = new ImageIterator(sources, target, new Vector2(halfWidth + startingPoint.X, startingPoint.Y), new Vector2(otherWidth, halfHeight));
            ImageIterator third = new ImageIterator(sources, target, new Vector2(startingPoint.X, halfHeight + startingPoint.Y), new Vector2(halfWidth, otherHeight));
            ImageIterator fourth = new ImageIterator(sources, target, new Vector2(halfWidth + startingPoint.X, halfHeight + startingPoint.Y), new Vector2(otherWidth, otherHeight));            

            // Starting all the iterators
            Task t1 = Task.Run(() => first.ForEachPixel(functor));
            Task t2 = Task.Run(() => second.ForEachPixel(functor));
            Task t3 = Task.Run(() => third.ForEachPixel(functor));
            Task t4 = Task.Run(() => fourth.ForEachPixel(functor));

            // Awaiting all the iterators
            await t1;
            await t2;
            await t3;
            await t4;
        }
    }
}