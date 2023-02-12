using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public Sprite tile;
    public int worldSize = 100;

    public float surfaceValue = 0.7f;
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float heightMultiplier = 4;
    public float heightAddition = 25;

    public float seed;
    public float saveSeed;
    private float saveSeed2;

    public Texture2D noiseTexture;
    public Texture2D texture;

    private void Start() {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
    }
    /*시드값 조절시 인스펙터 노이즈가 바뀐다*/
    private void OnValidate()
    {
        if (saveSeed != saveSeed2) { saveSeed = saveSeed2;  }
        /* 시드가 바꿔서 충족
         * 세이브 시드를 바꿔서 충족
         */
        //GenerateTerrain();
        GenerateNoiseTexture();
        //GenerateTexture();
    }
    void resetTerrain()
    {
        print(transform.childCount);
        if (transform.childCount == 0) return; 
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void GenerateTerrain()
    {
        if (saveSeed != 0 && saveSeed != seed)
            return;
        resetTerrain();
        for (int x = 0; x < worldSize; x++)
        {
            float Xcoord = (x + seed) * terrainFreq;
            float Ycoord = seed * terrainFreq;
            float height = Mathf.PerlinNoise(Xcoord, Ycoord) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {

                if (noiseTexture.GetPixel(x, y).r < surfaceValue)
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
        saveSeed2 = seed;
    }
    private void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++) {
            for (int y = 0; y < noiseTexture.height; y++) {
                float Xcoord = (x + seed) * terrainFreq;
                float Ycoord = (y + seed) * terrainFreq;
                float v = Mathf.PerlinNoise(Xcoord, Ycoord);
                #region what is perlinNoise
                /* x = 1, y = 1, seed = -10000, noiseFreq = 0.05
                 * (-9999 * 0.05, -9999 * 0.05)
                 * (-499.95, -499.95)
                 */
                #endregion what is perlinNoised
                noiseTexture.SetPixel(x, y, new UnityEngine.Color(v,v,v));
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
             * 01234 0 01234 1 01234 2 01234 3 01234 4 총 25번 출력, 이런 식이야
            if (a >= 200) return;
            */
            float v = Mathf.PerlinNoise((x + seed) * terrainFreq, (y + seed) * terrainFreq);
             #region what is perlinNoise
             /* x = 1, y = 1, seed = -10000, noiseFreq = 0.05
              * (-9999 * 0.05, -9999 * 0.05)
              * (-499.95, -499.95)
              */
             #endregion what is perlinNoise
             texture.SetPixel(x, y, new UnityEngine.Color(v, v, v));
        }
        texture.Apply();
    }
}

