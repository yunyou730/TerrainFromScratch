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

    [SerializeField]
    private int _mapWidth;
    [SerializeField]
    private int _mapHeight;
    [SerializeField]
    private float _noiseScale;
    
    public int _octaves;
    
    [Range(0,1)]
    public float _persistance;
    public float _lacunarity;
    
    
    [SerializeField]
    public bool _autoUpdate = false;

    public int _seed;
    public Vector2 _offset;

    [SerializeField]
    private TerrainType[] _regions;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            _mapWidth, _mapHeight, _noiseScale
            ,_seed,
            _octaves,_persistance,_lacunarity,
            _offset);

        Color[] colorMap = new Color[_mapWidth * _mapHeight];
        for (int y = 0;y < _mapHeight;y++)
        {
            for (int x = 0;x < _mapWidth;x++)
            {
                float currentHeight = noiseMap[x,y];
                for (int i = 0;i < _regions.Length;i++)
                {
                    if (currentHeight <= _regions[i].height)
                    {
                        colorMap[y * _mapWidth + x] = _regions[i].color;
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
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap,_mapWidth,_mapHeight));
        }
        else if (_drawMode == DrawMode.Mesh)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap);
            Texture2D texture = TextureGenerator.TextureFromColorMap(colorMap, _mapWidth, _mapHeight);
            display.DrawMesh(meshData,texture);
        }
    }


    private void OnValidate()
    {
        if (_mapWidth < 1)
        {
            _mapWidth = 1;
        }
        if (_mapHeight < 1)
        {
            _mapHeight = 1;
        }

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

