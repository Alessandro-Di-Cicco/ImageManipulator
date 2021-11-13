using System;

namespace MyLab.Effects
{
    /// <summary>
    /// Enhances the pixels of the first input image using the values of the pixels in the second
    /// </summary>
    public class DetailPusher : Filter
    {
        public override string FilterName => "Detail pusher";
        public override int SourcesCount => 2;

        private float pushMultiplier;
        /// <summary>
        /// The amount of influence that the pixels in the second image have over the enhancement
        /// </summary>
        /// <value></value>
        public float PushMultiplier { get => pushMultiplier; set => pushMultiplier = value; }

        public DetailPusher(float pushStrength = 0.04f) => PushMultiplier = pushStrength;

        protected override Color Application(ReadOnlyByteImage[] images, int x, int y)
        {
            Color originalColor = images[0].GetColor(x, y);
            Color borderColor = images[1].GetColor(x, y);

            originalColor.R = (byte) Math.Clamp(originalColor.R + borderColor.R * PushMultiplier, 0, 255);
            originalColor.G = (byte) Math.Clamp(originalColor.G + borderColor.G * PushMultiplier, 0, 255);
            originalColor.B = (byte) Math.Clamp(originalColor.B + borderColor.B * PushMultiplier, 0, 255);

            return originalColor;
        }
    }
}