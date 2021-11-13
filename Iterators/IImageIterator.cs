using System;

namespace MyLab.Iterators
{
    /// <summary>
    /// A generic image iterator, provides a single method to apply a functor on every pixel
    /// </summary>
    public interface IImageIterator
    {
        /// <summary>
        /// Calls the functor for every pixel
        /// </summary>
        /// <param name="functor">The functor to call, the first parameter is an array of input images,
        /// The second and third are the x and y of the current pixel, and the fourth is the color of said pixel</param>
        void ForEachPixel(Func<ReadOnlyByteImage[], int, int, Color> functor);
    }
}