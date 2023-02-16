using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public TileAtlas tileAtlas;

    [Header("Trees")]
    public int treeChance = 10;
    public int minTreeHeight = 4;
    public int maxTreeHeight = 6;

    [Header("Generation Setting")]
    public int chunkSize = 16;
    public int worldSize = 100;
    public bool generateCaves = true;
    public int dirtLayerHeight = 5;
    public float surfaceValue = 0.7f;
    public float heightMultiplier = 4;
    public float heightAddition = 25;

    [Header("Noise Setting")]
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float seed;
    public float saveSeed;
    private float saveSeed2;
    public Texture2D caveNoiseTexture;

    [Header("Ore Settings")]
    public OreClass[] ores;

    public GameObject[] worldChunks;
    private List<Vector2> worldTiles = new();

    
    private void Start() {
        seed = Random.Range(-10000, 10000);
        caveNoiseTexture = new(worldSize, worldSize);
        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);
        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].size = Mathf.Clamp(ores[i].size, 0, 1);
            if (caveNoiseTexture == null)
                ores[i].spreadTextrue = new(worldSize, worldSize);
            GenerateNoiseTexture(ores[i].rarity, ores[i].size, ores[i].spreadTextrue);
        }
        CreateChunks();
        GenerateTerrain();
        //Invoke("GenerateTerrain", 1f);
        //Invoke("CreateChunks", 5f);  
    }
    /*시드값 조절시 인스펙터 노이즈가 바뀐다*/
    private void OnValidate() {
        caveNoiseTexture = new(worldSize, worldSize);
        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);
        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].size = Mathf.Clamp(ores[i].size, 0, 1);
            ores[i].spreadTextrue = new(worldSize, worldSize);
            GenerateNoiseTexture(ores[i].rarity, ores[i].size, ores[i].spreadTextrue);
        }
        if (worldSize <= 1) { worldSize = 1; }
        if (saveSeed != saveSeed2) { saveSeed = saveSeed2; }
        
    }

    void DestoryTerrain()
    {
        if (transform.childCount <= 0) return; 
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
  
    public void CreateChunks()
    {
        int numChunks = Mathf.RoundToInt((float)worldSize / chunkSize)+1;
        print("worldSize / chunkSize " + (float)worldSize / chunkSize +" numChunks " + numChunks);
        worldChunks = new GameObject[numChunks];

        for (int j = 0; j < numChunks; j++)
        {
            GameObject newChunk = new();
            newChunk.transform.parent = transform;
            newChunk.name = j.ToString();
            worldChunks[j] = newChunk;
        }
    }

    private void GenerateTerrain()
    {
        if (saveSeed != 0 && saveSeed != seed)
            return; 
        for (int x = 0; x < worldSize; x++)
        {
          float Xcoord = (x + seed) * terrainFreq;
           float Ycoord = seed * terrainFreq;
            float height = Mathf.PerlinNoise(Xcoord, Ycoord) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - dirtLayerHeight)
                {
                    if (ores[0].spreadTextrue.GetPixel(x, y).r > 0.5f)
                        tileSprite = tileAtlas.coal.tileSprite;
                    else if (ores[1].spreadTextrue.GetPixel(x, y).r > 0.5f)
                        tileSprite = tileAtlas.iron.tileSprite;
                    else if (ores[2].spreadTextrue.GetPixel(x, y).r > 0.5f)
                        tileSprite = tileAtlas.gold.tileSprite;
                    else if (ores[3].spreadTextrue.GetPixel(x, y).r > 0.5f)
                        tileSprite = tileAtlas.diamond.tileSprite;
                    else
                        tileSprite = tileAtlas.stone.tileSprite;
                }
                else if (y <= height - 1)

                    tileSprite = tileAtlas.dirt.tileSprite;

                else if (y >= height - 1)
                {
                    tileSprite = tileAtlas.grass.tileSprite;
                }
                else { tileSprite = tileAtlas.grass.tileSprite; }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                        PlaceTile(tileSprite, x, y);
                } else
                    PlaceTile(tileSprite, x, y);

                if (y > height - 1)
                {
                    int t = Random.Range(0, treeChance);
                    if (t == 1)
                    {
                        //generate a Tree
                        if (worldTiles.Contains(new Vector2(x, y)))
                            GenerateTree(x, y + 1);
                    }
                }
            }
        }
        saveSeed = seed;
        saveSeed2 = seed;
    }

    private void GenerateNoiseTexture(float frequency, float limit, Texture2D noiseTexture)
    {
        for (int x = 0; x < noiseTexture.width; x++) {
            for (int y = 0; y < noiseTexture.height; y++) {
                float Xcoord = (x + seed) * frequency;
                float Ycoord = (y + seed) * frequency;
                float v = Mathf.PerlinNoise(Xcoord, Ycoord);
                //if (frequency == diamondRarity)
                //Debug.Log("v value at (" + x + ", " + y + ") is " + v);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else 
                    noiseTexture.SetPixel(x, y , Color.black);
            }
        } noiseTexture.Apply();
    }
    
    void GenerateTree(float x, float y)
    {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);
        for (float i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.treeRoot.tileSprite, x, y);
            if (i != 0)
            PlaceTile(tileAtlas.log.tileSprite, x, y + i);
        }
        for (int i = 0; i < 3; i++)
        {
            PlaceTile(tileAtlas.leaf.tileSprite, x, y + treeHeight + i);
                if (i != 2)
            PlaceTile(tileAtlas.leaf.tileSprite, x - 1, y + treeHeight + i);
            if (i != 2)
                PlaceTile(tileAtlas.leaf.tileSprite, x + 1, y + treeHeight + i);
        }
    }

    void PlaceTile(Sprite tileSprite, float x, float y)
    {
        GameObject newTile = new();

        int chunkCoord = Mathf.RoundToInt((float)x / chunkSize);

        newTile.transform.parent = worldChunks[chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x, y) + Vector2.one *0.5f;

        worldTiles.Add(newTile.transform.position - Vector3.one *0.5f);
    }
}

//public int sizeTexture;
//private void GenerateTexture()
//{
//    texture = new Texture2D(sizeTexture, sizeTexture);
//    for (int count = 0; count < sizeTexture * sizeTexture; count++)
//    {
//        int x = count % sizeTexture;
//        int y = count / sizeTexture;

//        /*% and /
//         * print($"{count}%{sizeTexture}={x}, {count}/{sizeTexture}={y}");
//         * 01234 0 01234 1 01234 2 01234 3 01234 4 총 25번 출력, 이런 식이야
//        if (a >= 200) return;
//        */
//        float v = Mathf.PerlinNoise((x + seed) * terrainFreq, (y + seed) * terrainFreq);
//         #region what is perlinNoise
//         /* x = 1, y = 1, seed = -10000, noiseFreq = 0.05
//          * (-9999 * 0.05, -9999 * 0.05)
//          * (-499.95, -499.95)
//          */
//         #endregion what is perlinNoise
//         texture.SetPixel(x, y, new UnityEngine.Color(v, v, v));
//    }
//    texture.Apply();
//}