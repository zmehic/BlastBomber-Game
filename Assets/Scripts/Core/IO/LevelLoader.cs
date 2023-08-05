using UnityEngine;

namespace BlastBomberV2.Core.IO
{
    public static class LevelLoader
    {
        public static Color[,] LoadLevel(TextAsset imageAsset)
        {
            var tekstura = new Texture2D(1, 1);

            tekstura.LoadImage(imageAsset.bytes);

            var pixels = tekstura.GetPixels();
            var pixelArray = new Color[tekstura.width, tekstura.height];

            for (var y = 0; y < tekstura.height; y++)
            {
                for (var x = 0; x < tekstura.width; x++)
                {
                    var pixel = pixels[y * tekstura.width + x];
                    pixelArray[x, y] = pixel;
                }

                
            }
            return pixelArray;
        }
    }
}
