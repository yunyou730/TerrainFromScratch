using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField]
    private Renderer _textureRenderer;

    public void DrawTexture(Texture2D texture)
    {
        _textureRenderer.sharedMaterial.mainTexture = texture;
        _textureRenderer.transform.localScale = new Vector3(texture.width, 1.0f, texture.height);
    }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Texture2D texture = TextureGenerator.TextureFromHeightMap(noiseMap);

        //texture.filterMode = FilterMode.Point;
        
        _textureRenderer.sharedMaterial.mainTexture = texture;
        _textureRenderer.transform.localScale = new Vector3(width,1,height);
    }
}
