using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
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

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            _mapWidth, _mapHeight, _noiseScale
            ,_seed,
            _octaves,_persistance,_lacunarity,
            _offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
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
