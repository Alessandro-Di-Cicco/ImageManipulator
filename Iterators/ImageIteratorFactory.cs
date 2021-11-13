namespace MyLab.Iterators
{
    /// <summary>
    /// Factory that allows to build ImageIterator objects
    /// </summary>
    public class ImageIteratorFactory
    {
        // Sets the behaviour of the CreateIterator() method
        private const bool useAsync = true;

        /// <param name="sourceImages"></param>
        /// <param name="target"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <returns>An IImageIterator built with the given input parameters</returns>
        // This uses sync/async iterators depending on the value of the variable
        public IImageIterator CreateIterator(ReadOnlyByteImage[] sourceImages, ByteImage target, Vector2 startPos, Vector2 targetPos)
        {
            return useAsync
                ? new DistributedImageIterator(sourceImages, target, startPos, targetPos)
                : new ImageIterator(sourceImages, target, startPos, targetPos);
        }

        /// <param name="sourceImages"></param>
        /// <param name="target"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <returns>An asynchronous image iterator built with the given inputs</returns>
        public IImageIterator CreateAsyncIterator(ReadOnlyByteImage[] sourceImages, ByteImage target, Vector2 startPos, Vector2 targetPos)
            => new DistributedImageIterator(sourceImages, target, startPos, targetPos);

        /// <summary>
        /// Builds an image iterator that is always synchronous
        /// that
        /// </summary>
        /// <param name="sourceImages"></param>
        /// <param name="target"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <returns>A synchronous image iterator built with the given inputs</returns>
        public IImageIterator CreateSyncIterator(ReadOnlyByteImage[] sourceImages, ByteImage target, Vector2 startPos, Vector2 targetPos)
            => new ImageIterator(sourceImages, target, startPos, targetPos);
    }
}