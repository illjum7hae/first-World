using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    #region value
    public Sprite tile;
    public int worldSize = 100;
    public float noiseFreq = 0.05f;
    public float seed;
    private float saveSeed;
    public float acessSaveSeed
    { get { return saveSeed; } }

    public float displaySavedSeed;
    public Texture2D noiseTexture;
    #endregion value

    private void Update()
    {
        displaySavedSeed = saveSeed;
    }
    private void Start() {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
    }
    private void OnValidate()
    {
        GenerateNoiseTexture();
    }
    
    private void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (noiseTexture.GetPixel(x, y).r < 0.5f)
                {
                    GameObject newTile = new GameObject(name = "tile");
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = tile;
                    newTile.transform.position = new Vector2(x, y);
                }
            }
        }
        saveSeed = seed;
    }
    private void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++) {
            for (int y = 0; y < noiseTexture.height; y++) {
                float v = Mathf.PerlinNoise((x + seed) * noiseFreq, (y + seed) * noiseFreq);
                noiseTexture.SetPixel(x, y, new Color(v,v,v));
            }
        }
        noiseTexture.Apply();
    }
}

