using System;
using ImageMagick;
using MyLab.Iterators;

namespace MyLab.Effects
{
    /// <summary>
    /// Base type for filters that can be applied to ByteImages
    /// </summary>
    public abstract class Filter
    {
        private ImageIteratorFactory factory;
        /// <summary> When true, the filter is always applied with a sync iterator </summary>
        protected bool forceSync;

        public abstract string FilterName { get; }
        public abstract int SourcesCount { get; }

        public Filter() => factory = new ImageIteratorFactory();

        /// <summary>
        /// Applies the filter on the given input images
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public ByteImage GetFilteredBytes(ReadOnlyByteImage[] sources)
        {
            if (sources.Length != SourcesCount)
                throw new ArgumentException($"The {FilterName} filter requires {SourcesCount} sources, not {sources.Length}");

            ByteImage output = new ByteImage(sources[0]);

            Console.WriteLine($"Running {FilterName}");

            // Run initialization code
            OnApplication(sources);

            // Pick appropriate iterator, explicitly sync if required, otherwise it's left to the factory to decide
            IImageIterator iterator = !forceSync
                ? factory.CreateIterator(sources, output, new Vector2(0, 0), new Vector2(sources[0].Width, sources[0].Height))
                : factory.CreateSyncIterator(sources, output, new Vector2(0, 0), new Vector2(sources[0].Width, sources[0].Height));
            iterator.ForEachPixel(Application);

            return output;
        }

        /// <summary> Overridable, called before an application starts iterating </summary>
        protected virtual void OnApplication(ReadOnlyByteImage[] sources) {}

        /// <param name="sources"></param>
        /// <returns>The filtered image, as an IMagickImage</returns>
        public IMagickImage<byte> GetFilteredImage(ReadOnlyByteImage[] sources) => GetFilteredBytes(sources).CreateImage();

        /// <summary>
        /// The functor applied to every pixel in the image
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract Color Application(ReadOnlyByteImage[] sources, int x, int y);
    }
}