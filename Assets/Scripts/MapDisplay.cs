using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField]
    private Renderer _textureRenderer;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        
        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        for (int y = 0;y < height;y++)
        {
            for (int x = 0;x < width;x++)
            {
                float v = noiseMap[x, y];
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, v);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        //texture.filterMode = FilterMode.Point;
        
        _textureRenderer.sharedMaterial.mainTexture = texture;
        _textureRenderer.transform.localScale = new Vector3(width,1,height);
    }
}
