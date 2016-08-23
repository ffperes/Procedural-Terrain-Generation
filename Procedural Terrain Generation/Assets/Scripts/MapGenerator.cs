using UnityEngine;
using System.Collections;

// This script will be instantiate in the scene by unity and therefore needs
// to inherit from monobehaviour class
public class MapGenerator : MonoBehaviour {

    // Create a enumerator to determine which mode it will be displayed
    public enum DisplayMode { NoiseMap, ColourMap, Mesh};
    public DisplayMode displayMode;

    public int mapWidth;
    public int mapHeight;
    public float scale;
    public int numberOfOctaves;
    [Range(0, 1)] // set the persistance to range between 0 and 1
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    // a boolean to update the noise map when the generateMap is pressed again
    public bool updateNoiseMap;
    public TypesOfTerrain[] biomes;
    public float meshHeightValueMultiplier;
    public AnimationCurve meshHeightCurve;

    public void mapGen()
    {
        float[,] noiseMap = NoiseGenerator.MapNoiseGenerator(mapWidth, mapHeight, scale, numberOfOctaves, 
                                                             persistance, lacunarity, seed, offset);
        Color[] colourMap = new Color[mapWidth * mapHeight];
        for(int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeightValue = noiseMap[x, y];
                for(int j = 0; j < biomes.Length; j++)
                {
                    if(currentHeightValue <= biomes[j].height)
                    {
                        colourMap[y * mapWidth + x] = biomes[j].colour;
                        break; // we have found our biome and dont need to continue
                    }
                } 
            }
        }
        // Reference to map displayer class
        MapDisplayer dis = FindObjectOfType<MapDisplayer>();
        if(displayMode == DisplayMode.NoiseMap)
        {
            dis.DrawTextureToScreen(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(displayMode == DisplayMode.ColourMap)
        {
            dis.DrawTextureToScreen(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
        else if(displayMode == DisplayMode.Mesh)
        {
            dis.DrawMeshToScreen(MeshCreator.TerrainMeshCreator(noiseMap, meshHeightValueMultiplier, meshHeightCurve), 
                                                TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
        
    }

    // To impose limits to the values in the inspector, we can camp some of the values
    // to be in a certain range
    // The function OnValidate is called automatically whenever one variable is changed in the inspector

    void OnValidate()
    {
        if(mapWidth < 10)
        {
            mapWidth = 10;
        }
        if(mapHeight < 10)
        {
            mapHeight = 10;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (numberOfOctaves < 1)
        {
            numberOfOctaves = 1;
        }
    }	
}

[System.Serializable] // to be seem in the inspector
public struct TypesOfTerrain
{
    public string nameOfBiome;
    public float height;
    public Color colour;
}
