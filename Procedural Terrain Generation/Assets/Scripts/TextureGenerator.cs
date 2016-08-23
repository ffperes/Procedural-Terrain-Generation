using UnityEngine;
using System.Collections;

public static class TextureGenerator {

    // This will create a texture from an one-dimensional array colour map
    public static Texture2D TextureFromColourMap(Color [] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        // This fix the bluring effect of the texture
        texture.filterMode = FilterMode.Point; // instead of bilinear
        // To fix the border of the map
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    // another method to get an texture based on a 2D height map
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0); // for the first dimension
        int height = heightMap.GetLength(1); // for the second dimension

        // Create a colour array to store all the colour of the pixels.
        // For eficiency, all values will be added to an one-dimensional array
        Color[] colourMapContainer = new Color[width * height];

        //Then I need to loop through noiseMap array to extrat the value
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // This technique is used to turn values from a two-dimensional array
                // into an one-dimensional array
                // in the lerp function, the first 2 parameters are the colours set as minimum and
                // maximum values to lerp through, and the percentage will be 0 and 1, which is the same
                // as the range of the noiseMap
                colourMapContainer[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return TextureFromColourMap (colourMapContainer, width, height);
    }
}
