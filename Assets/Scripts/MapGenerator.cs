using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh,
    }

    public DrawMode _drawMode = DrawMode.ColorMap;

    
    
    [Range(0,6)]
    public int _levelOfDetail;

    
    public const int kMapChunkSize = 241;
    
    // [SerializeField]
    // private int _mapWidth;
    // [SerializeField]
    // private int _mapHeight;
    [SerializeField]
    private float _noiseScale;
    
    public int _octaves;
    
    [Range(0,1)]
    public float _persistance;
    public float _lacunarity;
    
    
    [SerializeField]
    public bool _autoUpdate = false;
    [SerializeField]
    public int _seed;
    [SerializeField]
    public Vector2 _offset;

    [SerializeField] 
    private float _heightMultiplier = 1.0f;

    [SerializeField] private AnimationCurve _meshHeightCurve;

    [SerializeField]
    private TerrainType[] _regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            kMapChunkSize, kMapChunkSize, _noiseScale
            ,_seed,
            _octaves,_persistance,_lacunarity,
            _offset);

        Color[] colorMap = new Color[kMapChunkSize * kMapChunkSize];
        for (int y = 0;y < kMapChunkSize;y++)
        {
            for (int x = 0;x < kMapChunkSize;x++)
            {
                float currentHeight = noiseMap[x,y];
                for (int i = 0;i < _regions.Length;i++)
                {
                    if (currentHeight <= _regions[i].height)
                    {
                        colorMap[y * kMapChunkSize + x] = _regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (_drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (_drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap,kMapChunkSize,kMapChunkSize));
        }
        else if (_drawMode == DrawMode.Mesh)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap,_heightMultiplier,_meshHeightCurve,_levelOfDetail);
            Texture2D texture = TextureGenerator.TextureFromColorMap(colorMap, kMapChunkSize, kMapChunkSize);
            display.DrawMesh(meshData,texture);
        }
    }


    private void OnValidate()
    {
        if (_lacunarity < 1)
        {
            _lacunarity = 1;
        }

        if (_octaves < 0)
        {
            _octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

