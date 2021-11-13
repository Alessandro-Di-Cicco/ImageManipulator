using System;

namespace MyLab
{
    /// <summary>
    /// Manages images stored as bytes, that have three bytes per pixel, allowing
    /// read-only operations
    /// </summary>
    public class ReadOnlyByteImage
    {
        protected byte[] imageData;
        public int Width { get; }
        public int Height { get; }

        public ReadOnlyByteImage(byte[] data, int width, int height)
        {
            imageData = data;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Returns the index of a pixel within the array
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        // The image is stored row by row
        protected int GetIndex(int x, int y) => (y * Width + x) * 3;

        public byte GetR(int x, int y) => imageData[GetIndex(x, y)];
        public byte GetG(int x, int y) => imageData[GetIndex(x, y) + 1];
        public byte GetB(int x, int y) => imageData[GetIndex(x, y) + 2];

        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The pixel at the given coordinates</returns>
        public Color GetColor(int x, int y)
        {
            int index = GetIndex(x, y);
            return new Color(imageData[index], imageData[index + 1], imageData[index + 2]);
        }

        /// <summary> Returns a copy of the image as a raw byte array </summary>
        public byte[] GetCopy()
        {
            byte[] data = new byte[imageData.Length];
            Array.Copy(imageData, data, data.Length);
            return data;
        }
    }
}