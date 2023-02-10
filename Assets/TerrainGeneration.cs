using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public Sprite tile;
    public int worldSize = 100;

    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float heightMultiplier = 4;
    public float heightAddition = 25;

    public float seed;
    #region seed
    /*��Ʈ�� �õ� = �õ�(��������) + ���� �õ�(�����Ұ�)��
     *acess�� �����Ϳ��� �����Ϸ��� �������
     */
    private float saveSeed;
    public float acessSaveSeed { get { return saveSeed; } }
    public float displaySavedSeed;

    //���� �õ�� ����� �ٲ��� ���� �õ带 ���� ������
    private void Update()
   =>   displaySavedSeed = saveSeed;

    /*�õ尪 ������ �ν����� ����� �ٲ��*/
    private void OnValidate()
    {
        GenerateNoiseTexture();
        GenerateTexture();
    }
    #endregion seed
    public float noiseFreq = 0.05f;
    public Texture2D noiseTexture;
    public Texture2D texture;


    private void Start() {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
    }
    
    private void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * noiseFreq, seed * noiseFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
                
                //if (noiseTexture.GetPixel(x, y).r < 0.5f)
                {
                    GameObject newTile = new GameObject(name = "tile");
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = tile;
                    newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
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
                #region what is perlinNoise
                /* x = 1, y = 1, seed = -10000, noiseFreq = 0.05
                 * (-9999 * 0.05, -9999 * 0.05)
                 * (-499.95, -499.95)
                 */
                #endregion what is perlinNoise
                noiseTexture.SetPixel(x, y, new Color(v,v,v));
            }
        }
        noiseTexture.Apply();
    }
    public int sizeTexture;
    private void GenerateTexture()
    {
        texture = new Texture2D(sizeTexture, sizeTexture);
        for (int count = 0; count < sizeTexture * sizeTexture; count++)
        {
            int x = count % sizeTexture;
            int y = count / sizeTexture;

            /*% and /
             * print($"{count}%{sizeTexture}={x}, {count}/{sizeTexture}={y}");
             * 01234 0 01234 1 01234 2 01234 3 01234 4 �� 25�� ���, �̷� ���̾�
            if (a >= 200) return;
            */
            float v = Mathf.PerlinNoise((x + seed) * noiseFreq, (y + seed) * noiseFreq);
             #region what is perlinNoise
             /* x = 1, y = 1, seed = -10000, noiseFreq = 0.05
              * (-9999 * 0.05, -9999 * 0.05)
              * (-499.95, -499.95)
              */
             #endregion what is perlinNoise
             texture.SetPixel(x, y, new Color(v, v, v));
        }
        texture.Apply();
    }
}

