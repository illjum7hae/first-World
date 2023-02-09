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
   
    public float heightMultiplier;

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
   =>   GenerateNoiseTexture();
    #endregion seed
    public float noiseFreq = 0.05f;
    public Texture2D noiseTexture;

   
    private void Start() {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
    }
    
    private void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * noiseFreq, seed * noiseFreq) * heightMultiplier;
            for (int y = 0; y < worldSize; y++)
            {
                if (noiseTexture.GetPixel(x, y).r < 0.5f)
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
                noiseTexture.SetPixel(x, y, new Color(v,v,v));
            }
        }
        noiseTexture.Apply();
    }
}

