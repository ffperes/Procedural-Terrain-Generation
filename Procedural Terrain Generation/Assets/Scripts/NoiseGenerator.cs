using UnityEngine;
using System.Collections;

// It will be static because it does not need to create multiple instances of this class
// dont need to inherit from monobehaviour because it will not be applied to any object in the scene


    
public static class NoiseGenerator  {

    // a method to generate a noise map and return a grid o values to be returned between 0 and 1

    public static float [,] MapNoiseGenerator (int mapWidth, int mapHeight, 
        float scale, int numberOctaves, float persistance, float lacunarity, int seed, Vector2 offset)
    {
        float[,] mapNoise = new float[mapWidth, mapHeight];

        System.Random pseudoRandomNumberGenerator = new System.Random(seed);
        // In order to sampling from different locations, and array of vector2 is used
        Vector2[] octaveOffsets = new Vector2[numberOctaves];
        for(int i = 0; i < numberOctaves; i++)
        {

            // we also could scroll through the noise by provading our own offset value 
            float offsetX = pseudoRandomNumberGenerator.Next(-100000, 100000) + offset.x;
            float offsetY = pseudoRandomNumberGenerator.Next(-100000, 100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // an error handler in case of scale is set to 0 (impossible division by zero)

        if(scale <= 0)
        {
            scale = 0.000001f;
        }

        float maxNoiseValue = float.MinValue; // float is set to the minimum possible value
        float minNoiseValue = float.MaxValue; // same as above but set to the maximum value

        // when we scale the mapNoise, the values are calculated from the top right corner
        // to fix this problem we calculate the half width and half height and apply those 
        // values to coordinateSample x and y
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        // nested for loop to iterate within the bi-dimmensional array
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < numberOctaves; i++)
                {

                    // accessing sample coordinates
                    // adding a scale for the noise in order to not get rounded interger values
                    // the higher the frequency, more distant the sample points will be and therefore, more
                    // rapidly tha values will change
                    float coordinateSampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float coordinateSampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; 
                    // generate a 2D perlin noise value
                    // by multipling by 2 and subtracting by 1 we will
                    // inscrease the range from 0 to 1 to -1 to 1, since the PerlinNoise function only deliveries
                    // values in the range of 0 to 1
                    float perlinValue = Mathf.PerlinNoise(coordinateSampleX, coordinateSampleY) * 2 -1;
                    // apply the values to the noise map
                    noiseHeight += perlinValue * amplitude;

                    // at the end of each octave
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseValue)
                {
                    maxNoiseValue = noiseHeight;
                } else if(noiseHeight < minNoiseValue)
                {
                    minNoiseValue = noiseHeight;
                }
                // it will apply the noiseHeight to the noiseMap
                mapNoise[x, y] = noiseHeight;               
            }
        }

        // Now, we need to normalize the values to get back again values from 0 to 1
        // to do that, we need to keep track of the minimum and maxium values
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // the inverse lerp method returns a value between 0 and 1
                // where the same value of minNoiseValue will return zero and
                // same value of maxNoiseValue will return 1
                mapNoise[x, y] = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, mapNoise[x, y]);
            }
        }

        return mapNoise;
    }
}
